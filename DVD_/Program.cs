using Raylib_cs;
using System.Numerics;

namespace DVD_
{
    class Program
    {
        static void Main()
        {
            Raylib.InitWindow(1200, 800, "DVD Logo");
            Raylib.SetTargetFPS(60);

            Vector2 position = new Vector2(100, 100);
            Vector2 direction = new Vector2(1, 1); 
            float speed = 1000.0f; 

            while (!Raylib.WindowShouldClose())
            {
                float frameTime = Raylib.GetFrameTime();
                position += direction * speed * frameTime;

                if (position.X > Raylib.GetScreenWidth() - 60 || position.X < 0) 
                {
                    direction.X *= -1; 
                }
                if (position.Y > Raylib.GetScreenHeight() - 40 || position.Y < 0) 
                {
                    direction.Y *= -1; 
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawText("DVD", (int)position.X, (int)position.Y, 40, Color.Green);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
