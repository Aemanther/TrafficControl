using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrafficControl
{
    class Street
    {
        //Local variables
        public bool IsVertical;
        int StreetNumber;
        public Vector2 StreetStart;

        public void Initialize(int streetNum)
        {
            StreetNumber = streetNum;
            if (StreetNumber % 2 == 0)
            {
                IsVertical = false;
                StreetStart = new Vector2(0f, 250f);
            }
            else
            {
                IsVertical = true;
                StreetStart = new Vector2(150f, 0f);
            }
        }

        public bool IsSlotFree()
        {
            return true;
        }
    }
}
