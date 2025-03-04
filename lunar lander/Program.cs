using Raylib_cs;
using System;
using System.Numerics;

namespace lunar_lander
{
    internal class Ship
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool EngineOn;
        public float Fuel;
        private float EnginePower = 200f;
        private float FuelConsumption = 20f;

        public Ship(Vector2 startPosition)
        {
            Position = startPosition;
            Velocity = new Vector2(0, 0);
            Fuel = 100f;
        }

        public void Update(float deltaTime)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up) && Fuel > 0)
            {
                EngineOn = true;
                Velocity.Y -= EnginePower * deltaTime;
                Fuel -= FuelConsumption * deltaTime;
            }
            else
            {
                EngineOn = false;
            }

            Velocity.Y += 50.0f * deltaTime; 
            Position += Velocity * deltaTime;
        }

        public void Draw()
        {
            Raylib.DrawTriangle(new Vector2(Position.X, Position.Y - 10),
                                new Vector2(Position.X - 10, Position.Y + 10),
                                new Vector2(Position.X + 10, Position.Y + 10),
                                Color.White);

            if (EngineOn)
            {
                float flameWidth = 10; 
                float flameHeight = 20; 
                Raylib.DrawTriangle(new Vector2(Position.X, Position.Y + 10),
                                    new Vector2(Position.X - flameWidth / 2, Position.Y + 10 + flameHeight), 
                                    new Vector2(Position.X + flameWidth / 2, Position.Y + 10 + flameHeight), 
                                    Color.Yellow); 

                float innerFlameWidth = 6; 
                float innerFlameHeight = 14; 
                Raylib.DrawTriangle(new Vector2(Position.X, Position.Y + 10),
                                    new Vector2(Position.X - innerFlameWidth / 2, Position.Y + 10 + innerFlameHeight), 
                                    new Vector2(Position.X + innerFlameWidth / 2, Position.Y + 10 + innerFlameHeight), 
                                    Color.Orange); 
            }

            Raylib.DrawText($"Fuel: {Fuel:F1}", 10, 10, 20, Color.White);
        }
    }

    internal class Lander
    {
        private Ship player;
        private bool gameOver = false;
        private bool gameWon = false;
        private bool explosionActive = false;
        private float explosionTime = 0f;
        private const int ScreenWidth = 800;
        private const int ScreenHeight = 600;
        private float GroundY = 550;

        static void Main(string[] args)
        {
            Lander game = new Lander();
            game.Init();
            game.GameLoop();
        }

        void Init()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Lunar Lander");
            Raylib.SetTargetFPS(60);
            player = new Ship(new Vector2(ScreenWidth / 2, 100));
        }

        void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }
            Raylib.CloseWindow();
        }

        void Update()
        {
            if (gameOver || gameWon) return;

            float deltaTime = Raylib.GetFrameTime();
            player.Update(deltaTime);

            if (player.Position.Y >= GroundY)
            {
                if (player.Velocity.Y > 50.0f)
                {
                    gameOver = true; 
                    explosionActive = true; 
                    explosionTime = 0f; 
                }
                else
                {
                    gameWon = true; 
                }

                player.Velocity = new Vector2(0, 0);
                player.Position.Y = GroundY;
            }
        }

        void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            if (explosionActive)
            {
                Raylib.DrawCircle((int)player.Position.X, (int)(player.Position.Y - 10), 40, Color.Orange); 
                Raylib.DrawCircle((int)player.Position.X, (int)(player.Position.Y - 10), 25, Color.Yellow); 

                explosionTime += Raylib.GetFrameTime();
                if (explosionTime > 1f) 
                {
                    gameOver = true;
                    explosionActive = false;
                }
            }

            Raylib.DrawRectangle(0, (int)GroundY, ScreenWidth, ScreenHeight - (int)GroundY, Color.Green);

            if (gameOver)
            {
                Raylib.DrawText("Game Over!", ScreenWidth / 2 - 50, ScreenHeight / 2, 30, Color.Red);
            }
            else if (gameWon)
            {
                Raylib.DrawText("You Win!", ScreenWidth / 2 - 50, ScreenHeight / 2, 30, Color.Green);
            }
            else
            {
                player.Draw();
            }

            Raylib.EndDrawing();
        }
    }
}
