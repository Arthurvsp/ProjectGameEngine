using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace rpgtowerdefense {
    internal class Projectile {

        public static List<Projectile> projectiles = new List<Projectile>();

        private Vector2 position;
        private int speed = 1000;
        public int radius = 18;
        private Dir direction;
        private bool collided = false;

        public Projectile(Vector2 newPos, Dir newDir) {
            position = newPos;
            direction = newDir;
        }

        public Vector2 Position {
            get { 
                return position;
            }
        }

        public bool Collided {
            get { return collided; }
            set { collided = value; }
        }

        public void Update(GameTime gameTime) { 
           float dt = ((float) gameTime.ElapsedGameTime.TotalSeconds);

            switch (direction) {
                case Dir.D:
                    position.X += speed * dt;
                    break;
                case Dir.A:
                    position.X -= speed * dt;
                    break;
                case Dir.S:
                    position.Y += speed * dt;
                    break;
                case Dir.W:
                    position.Y -= speed * dt;
                    break;
            }
        }
    }
}
