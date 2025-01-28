using Raylib_cs;
using System.Numerics;

namespace DVD_
{
    class Program
    {
        static void Main()
        {
            Raylib.InitWindow(1000, 800, "DVD Logo");
            Raylib.SetTargetFPS(60);

            Vector2 position = new Vector2(100, 100);
            Vector2 direction = new Vector2(1, 1);
            float speed = 200.0f;
            int fontSize = 40;

            string text = "DVD";
            int textWidth = Raylib.MeasureText(text, fontSize);
            int textHeight = fontSize;

            while (!Raylib.WindowShouldClose())
            {
                float frameTime = Raylib.GetFrameTime();
                position += direction * speed * frameTime;

                if (position.X > Raylib.GetScreenWidth() - textWidth || position.X < 0)
                {
                    direction.X *= -1;
                }
                if (position.Y > Raylib.GetScreenHeight() - textHeight || position.Y < 0)
                {
                    direction.Y *= -1;
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawText(text, (int)position.X, (int)position.Y, fontSize, Color.Yellow);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}

