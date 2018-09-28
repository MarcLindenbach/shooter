using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace shooter.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        List<Enemy> enemies;
        Lasers lasers;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        float playerMoveSpeed;
        Texture2D mainBackground;
        Rectangle rectBackground;
        float scale = 1f;
        int startEnemyCount = 4;
        int enemyCount = 0;
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;
        Random random = new Random();
        SpriteFont scoreFont;
        SpriteFont messageFont;
        GameState state;
        int score;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            float xPos = GraphicsDevice.Viewport.TitleSafeArea.X;
            float yPos = GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2;
            Vector2 playerPos = new Vector2(xPos, yPos);
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Graphics\\shipAnimation");
            playerAnimation.Initialize(
                playerTexture,
                Vector2.Zero,
                115,
                69,
                8,
                60,
                Color.White,
                scale,
                true);
            Vector2 playerPosition = new Vector2(
                GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);
            playerMoveSpeed = 8.0f;

            lasers = new Lasers();
            Texture2D laserTexture = Content.Load<Texture2D>("Graphics\\laser");
            lasers.Initialise(laserTexture, GraphicsDevice.Viewport.TitleSafeArea.Width);

            enemies = new List<Enemy>();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            scoreFont = Content.Load<SpriteFont>("Graphics\\Score");
            messageFont = Content.Load<SpriteFont>("Graphics\\Message");
            state = GameState.PAUSED;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgLayer1.Initialize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            bgLayer2.Initialize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);
            mainBackground = Content.Load<Texture2D>("Graphics/mainbackground");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            if (state == GameState.PAUSED || state == GameState.GAME_OVER)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (state == GameState.GAME_OVER)
                    {
                        enemyCount = startEnemyCount;
                        score = 0;
                        enemies = new List<Enemy>();
                        player.Health = 100;
                        player.Position = new Vector2(
                            GraphicsDevice.Viewport.TitleSafeArea.X,
                            GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                    }
                    state = GameState.PLAYING;
                }
            }

            if (state != GameState.PLAYING) return;

            UpdatePlayer(gameTime);
            lasers.Update(gameTime);
            enemies.ForEach(enemy => enemy.Update(gameTime));

            bgLayer1.Update(gameTime);
            bgLayer2.Update(gameTime);
            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime) 
        {
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S)) 
            {
                player.Position.Y += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                lasers.ShootLaser(new Vector2(player.Position.X + (player.Width / 2), player.Position.Y + (player.Height / 2) - (lasers.Height / 2)));
            }
            if (currentKeyboardState.IsKeyDown(Keys.P))
            {
                state = GameState.PAUSED;
            }

            player.Position.X = MathHelper.Clamp(
                player.Position.X, 0, 
                GraphicsDevice.Viewport.Width - player.PlayerAnimation.FrameWidth * scale);
            player.Position.Y = MathHelper.Clamp(
                player.Position.Y, 0, 
                GraphicsDevice.Viewport.Height - player.PlayerAnimation.FrameHeight * scale);

            enemyCount = startEnemyCount + ((int)gameTime.TotalGameTime.TotalSeconds / 10);

            while (enemies.Count < enemyCount)
            {
                Enemy enemy = new Enemy();
                enemy.Initialize(this, random);
                enemies.Add(enemy);
            }

            // Check for collisions with mines
            enemies.ForEach(enemy =>
            {
                lasers.Positions.RemoveAll(laser =>
                {
                    if (!(laser.X > enemy.Right ||
                          laser.X + lasers.Width < enemy.Left ||
                          laser.Y > enemy.Bottom ||
                          laser.Y + lasers.Height < enemy.Top))
                    {
                        score += 100;
                        enemy.Explode();
                        return true;
                    }
                    return false;
                });

                if (!(player.Left > enemy.Right || 
                      player.Right < enemy.Left ||
                      player.Top > enemy.Bottom ||
                      player.Bottom < enemy.Top))
                {
                    player.Health -= 5;
                    enemy.Explode();
                }
            });
            if (player.Health <= 0)
            {
                player.Health = 0;
                state = GameState.GAME_OVER;
            }
            player.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            Texture2D rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            Rectangle coords = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y,
                player.PlayerAnimation.FrameWidth,
                player.PlayerAnimation.FrameHeight);

            player.Draw(spriteBatch);
            lasers.Draw(spriteBatch);
            enemies.ForEach(enemy =>
            {
                Rectangle enemyCoords = new Rectangle(
                    (int)enemy.Position.X,
                    (int)enemy.Position.Y,
                    enemy.EnemyAnimation.FrameWidth,
                    enemy.EnemyAnimation.FrameHeight);
                enemy.Draw(spriteBatch);
            });
            spriteBatch.DrawString(scoreFont, String.Format("Score {0:0}", score), new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(scoreFont, String.Format("Health {0:0}", player.Health), new Vector2(5, 20), Color.Black);

            if (state == GameState.PAUSED)
            {
                spriteBatch.DrawString(messageFont, "Game Paused, press <space> to continue", new Vector2(5, (GraphicsDevice.Viewport.Height / 2) - 32), Color.Black);
            }
            if (state == GameState.GAME_OVER)
            {
                spriteBatch.DrawString(messageFont, String.Format("Game Over, Score {0:0} press <space> to restart", score), new Vector2(5, (GraphicsDevice.Viewport.Height / 2) - 32), Color.Black);
            }
            spriteBatch.End();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}