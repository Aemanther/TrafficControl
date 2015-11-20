using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrafficControl
{
    class Intersection
    {
        public Vector2 Position;
        public Rectangle Area;

        public List<bool> Lights = new List<bool>();

        public void Initialize(Vector2 position)
        {
            Position = position;
            Area = new Rectangle((int)Position.X, (int)Position.Y, 200, 200);
            Lights.Add(false);
            Lights.Add(false);
            Lights.Add(false);
            Lights.Add(false);
        }
    }
}
