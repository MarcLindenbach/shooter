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
        LaserManager lasers;
        KeyboardState currentKeyboardState;
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
        double elapsedTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

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

            lasers = new LaserManager();
            Texture2D laserTexture = Content.Load<Texture2D>("Graphics\\laser");
            lasers.Initialise(laserTexture, GraphicsDevice.Viewport.TitleSafeArea.Width);

            enemies = new List<Enemy>();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            scoreFont = Content.Load<SpriteFont>("Graphics\\Score");
            messageFont = Content.Load<SpriteFont>("Graphics\\Message");
            state = GameState.PAUSED;
            Reset();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgLayer1.Initialize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            bgLayer2.Initialize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);
            mainBackground = Content.Load<Texture2D>("Graphics/mainbackground");
        }

        protected override void UnloadContent()
        {
        }

        private void UpdateGameState()
        {
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && state != GameState.PLAYING)
            {
                if (state == GameState.GAME_OVER)
                {
                    Reset();
                }
                state = GameState.PLAYING;
            }

            if (currentKeyboardState.IsKeyDown(Keys.P))
            {
                state = GameState.PAUSED;
            }

            if (player.Health <= 0)
            {
                player.Health = 0;
                state = GameState.GAME_OVER;
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            enemyCount = startEnemyCount + (int)(elapsedTime / 10);

            while (enemies.Count < enemyCount)
            {
                Enemy enemy = new Enemy();
                enemy.Initialize(this, random);
                enemies.Add(enemy);
            }
            enemies.ForEach(enemy => enemy.Update(gameTime));
        }

        public void UpdateLasers(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                lasers.ShootLaser(new Vector2(player.Position.X + (player.Width / 2), player.Position.Y + (player.Height / 2) - (lasers.Height / 2)));
            }
            lasers.Update(gameTime);
        }

        public void HandleEnemyCollisions(GameTime gameTime)
        {
            enemies.ForEach(enemy =>
            {
                lasers.Lasers.RemoveAll(laser =>
                {
                    if (laser.Intersects(enemy))
                    {
                        score += 100;
                        enemy.Reset();
                        return true;
                    }
                    return false;
                });

                if (player.Intersects(enemy))
                {
                    player.Health -= 5;
                    enemy.Reset();
                }
            });
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            UpdateGameState();
            if (state != GameState.PLAYING) return;

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePlayer(gameTime);
            UpdateEnemies(gameTime);
            UpdateLasers(gameTime);
            HandleEnemyCollisions(gameTime);

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

            player.Position.X = MathHelper.Clamp(
                player.Position.X, 0, 
                GraphicsDevice.Viewport.Width - player.PlayerAnimation.FrameWidth * scale);
            player.Position.Y = MathHelper.Clamp(
                player.Position.Y, 0, 
                GraphicsDevice.Viewport.Height - player.PlayerAnimation.FrameHeight * scale);

            player.Update(gameTime);
        }

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
                spriteBatch.DrawString(messageFont, "Game Paused, press <enter> to continue", new Vector2(5, (GraphicsDevice.Viewport.Height / 2) - 32), Color.Black);
            }
            if (state == GameState.GAME_OVER)
            {
                spriteBatch.DrawString(messageFont, String.Format("Game Over, Score {0:0} press <enter> to restart", score), new Vector2(5, (GraphicsDevice.Viewport.Height / 2) - 32), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void Reset()
        {
            enemyCount = startEnemyCount;
            score = 0;
            elapsedTime = 0;
            enemies.Clear(); 
            player.Health = 100;
            player.Position = new Vector2(
                GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
        }
    }
}