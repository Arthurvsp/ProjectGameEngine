using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;

namespace rpgtowerdefense {

    enum Dir { 
        S, 
        W, 
        A, 
        D
    }

    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerSprite;
        Texture2D magoDown;
        Texture2D magoUp;
        Texture2D magoRight;
        Texture2D magoLeft;

        Texture2D background;
        Texture2D fireBall;
        Texture2D skull;

        Player player = new Player();

        Camera camera;


        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {

            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();

            this.camera = new Camera(_graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("assets/player/mago");
            magoDown = Content.Load<Texture2D>("assets/player/magoDown");
            magoUp = Content.Load<Texture2D>("assets/player/magoUp");
            magoRight = Content.Load<Texture2D>("assets/player/magoRight");
            magoLeft = Content.Load<Texture2D>("assets/player/magoLeft");

            background = Content.Load<Texture2D>("assets/background");
            fireBall = Content.Load<Texture2D>("assets/boladefogo");
            skull = Content.Load<Texture2D>("assets/caveira");

            player.animations[0] = new SpriteAnimation(magoDown, 4, 8);
            player.animations[1] = new SpriteAnimation(magoUp, 4, 8);
            player.animations[2] = new SpriteAnimation(magoLeft, 4, 8);
            player.animations[3] = new SpriteAnimation(magoRight, 4, 8);

            player.anim = player.animations[0];

            

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            if(!player.dead)
                Controller.Update(gameTime, skull);

            this.camera.Position = player.Position;
            this.camera.Update(gameTime);

            foreach (Projectile proj in Projectile.projectiles) { 
                proj.Update(gameTime);
            }

            foreach(Enemy e in Enemy.enemies){ 
                e.Update(gameTime, player.Position, player.dead);
                int sum = 32 + e.radius;
                if (Vector2.Distance(player.Position, e.Position) < sum) { 
                    player.dead = true;
                }
            }

            foreach (Projectile proj in Projectile.projectiles) {
                foreach (Enemy enemy in Enemy.enemies) {
                    int sum = proj.radius + enemy.radius;
                    if (Vector2.Distance(proj.Position, enemy.Position) < sum) { 
                        proj.Collided = true;
                        enemy.Dead = true;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Dead);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(this.camera);

            _spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
            foreach (Enemy e in Enemy.enemies) {
                e.anim.Draw(_spriteBatch);
            }
            foreach (Projectile proj in Projectile.projectiles) {
                _spriteBatch.Draw(fireBall, new Vector2(proj.Position.X - 48, proj.Position.Y - 48), Color.White);
            }

            if(!player.dead)
                player.anim.Draw(_spriteBatch);
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
