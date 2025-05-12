using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using System.Reflection.Metadata;

public class ArtilleryGame
{
    private static List<Tank> players;
    private static Terrain terrain;
    private static Projectile? currentProjectile;
    private static int currentPlayerIndex = 0;

    public static void Main()
    {
        Raylib.InitWindow(800, 600, "Artillery Game");
        Raylib.SetTargetFPS(60);

        players = new List<Tank>
        {
            new Tank(new Vector2(100, 500), Color.Red, true),
            new Tank(new Vector2(600, 500), Color.Blue, false)
        };

        terrain = new Terrain();

        while (!Raylib.WindowShouldClose())
        {
            UpdateGame();
            DrawGame();
        }

        Raylib.CloseWindow();
    }

    private static void UpdateGame()
    {
        if (currentProjectile != null)
        {
            currentProjectile.Update();
            if (terrain.CheckCollision(currentProjectile) || CheckPlayerCollision())
            {
                currentProjectile = null;
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // vuoron vaihto
            }
        }
        else
        {
            if (Raylib.IsKeyDown(KeyboardKey.Left))
                players[currentPlayerIndex].AdjustAngle(-1);
            if (Raylib.IsKeyDown(KeyboardKey.Right))
                players[currentPlayerIndex].AdjustAngle(1);

            if (Raylib.IsKeyPressed(KeyboardKey.Up))
                players[currentPlayerIndex].AdjustPower(1);
            if (Raylib.IsKeyPressed(KeyboardKey.Down))
                players[currentPlayerIndex].AdjustPower(-1);

            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                currentProjectile = players[currentPlayerIndex].Shoot();
            }
        }
    }

    private static void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        terrain.Draw();
        foreach (var player in players)
            player.Draw();

        currentProjectile?.Draw();

        Raylib.DrawText($"Power: {players[currentPlayerIndex].Power}", 10, 10, 20, Color.Black);

        Raylib.EndDrawing();
    }

    private static bool CheckPlayerCollision()
    {
        foreach (var player in players)
        {
            if (currentProjectile != null && player.CheckHit(currentProjectile))
            {
                Console.WriteLine("osuma");
                return true;
            }
        }
        return false;
    }
}

public class Tank
{
    public Vector2 Position;
    public Color Color;
    private int angle = 45;
    public int Power { get; private set; } = 5; 
    private bool facingRight;

    public Tank(Vector2 position, Color color, bool facingRight)
    {
        Position = position;
        Color = color;
        this.facingRight = facingRight;
    }

    public void Draw()
    {
        Raylib.DrawRectangle((int)Position.X - 20, (int)Position.Y - 10, 40, 20, Color);
        Vector2 barrelEnd = Position + new Vector2(
            25 * (float)Math.Cos(angle * Math.PI / 180) * (facingRight ? 1 : -1),
            -25 * (float)Math.Sin(angle * Math.PI / 180));
        Raylib.DrawLine((int)Position.X, (int)Position.Y, (int)barrelEnd.X, (int)barrelEnd.Y, Color.Black);
    }

    public void AdjustAngle(int delta)
    {
        angle = Math.Clamp(angle + delta, 0, 90);
    }

    public void AdjustPower(int delta)
    {
        Power = Math.Clamp(Power + delta, 1, 10); 
    }

    public Projectile Shoot()
    {
        Vector2 projectileStart = Position + new Vector2(
            25 * (float)Math.Cos(angle * Math.PI / 180) * (facingRight ? 1 : -1),
            -25 * (float)Math.Sin(angle * Math.PI / 180));
        return new Projectile(projectileStart, angle, Power, facingRight);
    }

    public bool CheckHit(Projectile projectile)
    {
        return Vector2.Distance(projectile.Position, Position) < 20;
    }
}

public class Terrain
{
    private const int SegmentCount = 10;
    private const int SegmentWidth = 800 / SegmentCount;
    private int[] heights;

    public Terrain()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        heights = new int[SegmentCount];
        Random rand = new Random();

        for (int i = 0; i < SegmentCount; i++)
        {
            heights[i] = 200 + rand.Next(-100, 50);
        }
    }

    public void Draw()
    {
        for (int i = 0; i < SegmentCount; i++)
        {
            int x = i * SegmentWidth;
            int y = 600 - heights[i];
            Raylib.DrawRectangle(x, y, SegmentWidth, heights[i], Color.DarkGreen);
            Raylib.DrawRectangleLines(x, y, SegmentWidth, heights[i], Color.Black);
        }
    }

    public bool CheckCollision(Projectile projectile)
    {
        int segmentIndex = Math.Clamp((int)(projectile.Position.X /SegmentWidth), 0, SegmentCount - 1);
        return projectile.Position.Y >= (600 - heights[segmentIndex]);
    }

    public Vector2 GetTankPosition(int index)
    {
        int x = index * SegmentWidth + SegmentWidth / 2;
        int segmentIndex = Math.Clamp(index, 0, SegmentCount - 1);
        int y = 600 - heights[segmentIndex] - 10;
        return new Vector2(x, y);
    }
}



public class Projectile
{
    public Vector2 Position;
    private Vector2 velocity;

    public Projectile(Vector2 startPosition, int angle, int power, bool facingRight)
    {
        Position = startPosition;
        velocity = new Vector2(
            power * 2.0f * (float)Math.Cos(angle * Math.PI / 180) * (facingRight ? 1 : -1), 
            -power * 2.0f * (float)Math.Sin(angle * Math.PI / 180)
        );
    }

    public void Update()
    {
        Position += velocity;
        velocity.Y += 0.3f; // graviyt
    }

    public void Draw()
    {
        Raylib.DrawCircle((int)Position.X, (int)Position.Y, 5, Color.Black);
    }
}
