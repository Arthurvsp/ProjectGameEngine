using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace rpgtowerdefense {
    internal class Player {

        private Vector2 position = new Vector2(500, 300);
        private int speed = 800;
        private Dir direction = Dir.S;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        public bool dead = false;

        public SpriteAnimation anim;

        public SpriteAnimation[] animations = new SpriteAnimation[4];

        public Vector2 Position {
            get { 
                return position;
            }
        }

        public void setX(float newX) { 
            position.X = newX;
        }   

        public void setY(float newY) { 
            position.Y = newY;
        }

        public void Update(GameTime gameTime) { 
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            isMoving = false;

            if (kState.IsKeyDown(Keys.D)) { 
                direction = Dir.D;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.A)) {
                direction = Dir.A;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.W)) {
                direction = Dir.W;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.S)) {
                direction = Dir.S;
                isMoving = true;
            }

            if(kState.IsKeyDown(Keys.Space))
                isMoving = false;

            if(dead)
                isMoving = false;

            if (isMoving) {
                switch (direction) {
                    case Dir.D:
                        if(position.X < 1275)
                            position.X += speed * dt;
                        break;
                    case Dir.A:
                        if (position.X > 225)
                            position.X -= speed * dt;
                        break;
                    case Dir.S:
                        if (position.Y < 1250)
                            position.Y += speed * dt;
                        break;
                    case Dir.W:
                        if (position.Y > 200)
                            position.Y -= speed * dt;
                        break;
                }
            }

            anim = animations[(int)direction];


            anim.Position = new Vector2(position.X - 48, position.Y - 48);

            if (kState.IsKeyDown(Keys.Space))
                anim.setFrame(0);
            else if (isMoving)
                anim.Update(gameTime);
            else
                anim.setFrame(1);

            if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
                Projectile.projectiles.Add(new Projectile(position, direction));

            kStateOld = kState;
        }
    }
}
