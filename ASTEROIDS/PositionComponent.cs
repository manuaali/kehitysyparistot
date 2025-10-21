using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASTEROIDS
{
    class PositionComponent
    {
        public Vector2 Position;

        public PositionComponent(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void WrapAround()
        {
            if (Position.X < 0) Position.X = Raylib.GetScreenWidth();
            if (Position.X > Raylib.GetScreenWidth()) Position.X = 0;
            if (Position.Y < 0) Position.Y = Raylib.GetScreenHeight();
            if (Position.Y > Raylib.GetScreenHeight()) Position.Y = 0;
        }
    }
}
