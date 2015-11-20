using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrafficControl
{
    class Car
    {
        //Local variables
        public bool Active;
        public bool Moving;
        bool Turning;
        int CurrentStreet;
        public int CurrentSlot;
        public int NextStreet;
        public int NextSlot;
        int TargetStreet;
        int TargetSlot;
        float Speed;
        float DistanceLeft;
        float Turntime;
        float RotationInc;
        string Direction;
        string NewHeading;

        public Vector2 Position;
        Vector2 Origin;
        Vector2 TurnMove;
        float Rotation;
        Texture2D CarSprite;


        public void Initialize(Texture2D sprite, int startingStreet, int startingSlot, int targetStreet, int targetSlot, bool startVertical, Vector2 streetStart)
        {
            CarSprite = sprite;

            Origin = new Vector2(30f, 15f);

            Speed = 100f;
            DistanceLeft = 60f;

            CurrentStreet = startingStreet;
            CurrentSlot = startingSlot;
            NextStreet = startingStreet;
            NextSlot = startingSlot;

            TargetStreet = targetStreet;
            TargetSlot = targetSlot;

            if (startVertical)
            {
                
            }
            else
            {
                Position.X = streetStart.X - 30f;
                Position.Y = streetStart.Y - 25f;
                Rotation = 0f;
                Direction = "East";
            }

            NewHeading = "Straight";
            Active = true;
            Moving = true;
        }

        public void Update(TimeSpan deltaGameTime)
        {
            if (Moving)
            {
                if (NewHeading.Equals("Straight"))
                {
                    float distance = Speed * (float)deltaGameTime.TotalSeconds;

                    DistanceLeft -= distance;

                    if (DistanceLeft < 0)
                    {
                        distance += DistanceLeft;
                        Moving = false;
                        if (CurrentSlot == 6)
                        {
                            Active = false;
                        }
                        CurrentSlot = NextSlot;
                        FindNextSlot();
                    }

                    switch (Direction)
                    {
                        case "East":
                            Position.X += distance;
                            break;
                        case "South":
                            Position.Y += distance;
                            break;
                    }
                }
                else if (NewHeading.Equals("RightTurn"))
                {
                    Turntime -= (float)deltaGameTime.TotalSeconds;
                    RotationInc = 1.57f * ((1.5f - Turntime) / 1.5f);

                    TurnMove.X = 75 * (float)Math.Cos(1.57f - RotationInc);
                    TurnMove.Y = 75 - 75 * (float)Math.Sin(1.57f - RotationInc);

                    if (Turntime < 0)
                    {
                        Rotation += 1.57f;
                        Position.X += 75f;
                        Position.Y += 75f;
                        Turning = false;
                        Moving = false;
                        CurrentSlot = NextSlot;
                        FindNextSlot();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(CarSprite, Position, Color.White);
            if (Turning)
            {
                spriteBatch.Draw(CarSprite, new Vector2(Position.X + TurnMove.X, Position.Y + TurnMove.Y), null, Color.White, Rotation + RotationInc, Origin, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(CarSprite, Position, null, Color.White, Rotation, new Vector2(30f, 15f), 1f, SpriteEffects.None, 0f);
            }
        }

        public void Go()
        {
            switch (NewHeading)
            {
                case "Straight":
                    DistanceLeft = 75f;
                    break;
                case "StraightInt":
                    DistanceLeft = 200f;
                    NewHeading = "Straight";
                    break;
                case "RightTurn":
                    Turntime = 1.5f;
                    Turning = true;
                    break;
            }
            Moving = true;
        }

        private void FindNextSlot()
        {
            if (CurrentSlot % 4 == 0)
            {
                NextSlot = CurrentSlot + 2;
                NewHeading = "Straight";
            }
            else if (CurrentSlot % 4 == 2)
            {
                if (CurrentSlot == 6)
                {
                    NextSlot = CurrentSlot + 2;
                    NewHeading = "Straight";
                }
                else
                {
                    Random rand = new Random();
                    switch (rand.Next(0, 2))
                    {
                        case 0:
                            NextSlot = CurrentSlot + 2;
                            NewHeading = "StraightInt";
                            break;
                        case 1:
                            NextSlot = CurrentSlot + 2;
                            NextStreet = CurrentStreet + 1;
                            NewHeading = "RightTurn";
                            Direction = "South";
                            break;
                    }
                }

            }
        }
    }
}
