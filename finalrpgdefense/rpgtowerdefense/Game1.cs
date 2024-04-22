using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;
using static System.Formats.Asn1.AsnWriter;

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

        private bool playSelected = true; // Play selecionado inicialmente
        private KeyboardState previousKeyboardState;

        Texture2D playerSprite;
        Texture2D magoDown;
        Texture2D magoUp;
        Texture2D magoRight;
        Texture2D magoLeft;

        Texture2D background;
        Texture2D backgroundcreditos;
        Texture2D fireBall;
        Texture2D skull;

        Player player = new Player();

        Camera camera;

        Texture2D backgroundpontos;
        SpriteFont gameFont;

        int score = 0;
        bool inicio = false;



        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize() {

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            this.camera = new Camera(_graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            backgroundpontos = Content.Load<Texture2D>("assets/Button");

            playerSprite = Content.Load<Texture2D>("assets/player/mago");
            magoDown = Content.Load<Texture2D>("assets/player/magoDown");
            magoUp = Content.Load<Texture2D>("assets/player/magoUp");
            magoRight = Content.Load<Texture2D>("assets/player/magoRight");
            magoLeft = Content.Load<Texture2D>("assets/player/magoLeft");

            background = Content.Load<Texture2D>("assets/background");

            fireBall = Content.Load<Texture2D>("assets/boladefogo");
            skull = Content.Load<Texture2D>("assets/caveira");

            gameFont = Content.Load<SpriteFont>("assets/galleryFont");

            player.animations[0] = new SpriteAnimation(magoDown, 4, 8);
            player.animations[1] = new SpriteAnimation(magoUp, 4, 8);
            player.animations[2] = new SpriteAnimation(magoLeft, 4, 8);
            player.animations[3] = new SpriteAnimation(magoRight, 4, 8);

            player.anim = player.animations[0];



        }

        protected override void Update(GameTime gameTime) {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W)) {
                playSelected = !playSelected;
            }

            if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S)) {
                playSelected = !playSelected;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter)) {
                if (playSelected) {
                    inicio = true;
                }
                else {
                    Exit();
                }
            }

            previousKeyboardState = keyboardState;




            player.Update(gameTime);



            this.camera.Position = player.Position;
            this.camera.Update(gameTime);



            if (inicio) {

                if (!player.dead)
                    Controller.Update(gameTime, skull);

                foreach (Projectile proj in Projectile.projectiles) {
                    proj.Update(gameTime);
                }

                foreach (Enemy e in Enemy.enemies) {
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
                            if (!player.dead)
                                score++;
                        }
                    }
                }
                if (!player.dead) {
                    Projectile.projectiles.RemoveAll(p => p.Collided);
                    Enemy.enemies.RemoveAll(e => e.Dead);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector2 textPosition = player.Position + new Vector2(-600, -300);
            Vector2 creditosPosition = player.Position + new Vector2(-600, -300);



            _spriteBatch.Begin(this.camera);

            if (!inicio) {

                _spriteBatch.DrawString(gameFont, "W para subir", new Vector2(700, 200) + creditosPosition, Color.White);
                _spriteBatch.DrawString(gameFont, "S para descer", new Vector2(700, 300) + creditosPosition, Color.White);
                if (playSelected) {
                    _spriteBatch.DrawString(gameFont, "Play", new Vector2(500, 200) + creditosPosition, Color.Yellow);
                    _spriteBatch.DrawString(gameFont, "Exit", new Vector2(500, 300) + creditosPosition, Color.White);
                }
                else {
                    _spriteBatch.DrawString(gameFont, "Play", new Vector2(500, 200) + creditosPosition, Color.White);
                    _spriteBatch.DrawString(gameFont, "Exit", new Vector2(500, 300) + creditosPosition, Color.Yellow);
                }
            }

            if (!player.dead && inicio) {

                _spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
                _spriteBatch.Draw(backgroundpontos, textPosition + new Vector2(-3, 0), Color.White);
                _spriteBatch.DrawString(gameFont, "pontos = " + score.ToString(), textPosition, Color.Blue);
                foreach (Enemy e in Enemy.enemies) {
                    e.anim.Draw(_spriteBatch);
                }
                foreach (Projectile proj in Projectile.projectiles) {
                    _spriteBatch.Draw(fireBall, new Vector2(proj.Position.X - 48, proj.Position.Y - 48), Color.White);

                }

                player.anim.Draw(_spriteBatch);

            }


            if (player.dead) {


                _spriteBatch.DrawString(gameFont, "OBRIGADO POR JOGAR NOSSO JOGO", creditosPosition + new Vector2(200, 100), Color.White);
                _spriteBatch.DrawString(gameFont, "CRIADORES: Hugo Ferreira, Rodrigo Guedes", creditosPosition + new Vector2(200, 200), Color.White);
                _spriteBatch.DrawString(gameFont, "Arthur Victor, Dyelson Motta", creditosPosition + new Vector2(200, 250), Color.White);
                _spriteBatch.DrawString(gameFont, "Jonas Rodrigues, Mateus Batista", creditosPosition + new Vector2(200, 300), Color.White);
                _spriteBatch.DrawString(gameFont, "sua pontuacao = " + score, creditosPosition + new Vector2(200, 400), Color.White);


            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
