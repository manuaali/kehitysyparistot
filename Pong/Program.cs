using System;
using System.Numerics;
using Raylib_cs;
class Pong
{
    static void Main()
    {
        int screenWidth = 800;
        int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "Pong");

        Rectangle player1 = new Rectangle(50, screenHeight / 2 - 50, 10, 100);
        Rectangle player2 = new Rectangle(screenWidth - 60, screenHeight / 2 - 50, 10, 100);
        int playerSpeed = 5;

        Rectangle ball = new Rectangle(screenWidth / 2 - 5, screenHeight / 2 - 5, 10, 10);
        Vector2 ballSpeed = new Vector2(4, 4);

        int player1Score = 0;
        int player2Score = 0;

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsKeyDown(KeyboardKey.W) && player1.Y > 0)
                player1.Y -= playerSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.S) && player1.Y < screenHeight - player1.Height)
                player1.Y += playerSpeed;

            if (Raylib.IsKeyDown(KeyboardKey.Up) && player2.Y > 0)
                player2.Y -= playerSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Down) && player2.Y < screenHeight - player2.Height)
                player2.Y += playerSpeed;

            ball.X += ballSpeed.X;
            ball.Y += ballSpeed.Y;

            if (ball.Y <= 0 || ball.Y + ball.Height >= screenHeight)
                ballSpeed.Y *= -1;

            if (Raylib.CheckCollisionRecs(ball, player1) || Raylib.CheckCollisionRecs(ball, player2))
                ballSpeed.X *= -1;

            if (ball.X <= 0)
            {
                player2Score++;
                ResetBall(ref ball, screenWidth, screenHeight);
            }

            if (ball.X + ball.Width >= screenWidth)
            {
                player1Score++;
                ResetBall(ref ball, screenWidth, screenHeight);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DarkBlue);

            Raylib.DrawRectangleRec(player1, Color.LightGray);
            Raylib.DrawRectangleRec(player2, Color.Orange);

            Raylib.DrawRectangleRec(ball, Color.White);

            Raylib.DrawText(player1Score.ToString(), screenWidth / 4, 20, 40, Color.LightGray);
            Raylib.DrawText(player2Score.ToString(), screenWidth * 3 / 4, 20, 40, Color.Orange);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static void ResetBall(ref Rectangle ball, int screenWidth, int screenHeight)
    {
        ball.X = screenWidth / 2 - ball.Width / 2;
        ball.Y = screenHeight / 2 - ball.Height / 2;
    }
}
