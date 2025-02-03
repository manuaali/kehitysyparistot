using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

Raylib.InitWindow(800, 600, "Tankki peli");
Raylib.SetTargetFPS(60);

Tank tank1 = new Tank(100, 250, Color.Blue);
Tank tank2 = new Tank(600, 250, Color.Red);

List<Bullet> bullets = new List<Bullet>();

Rectangle wall = new Rectangle(350, 200, 100, 200);

while (!Raylib.WindowShouldClose())
{
    tank1.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.F, bullets);
    tank2.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.M, bullets);

    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        bullets[i].Update();

        if (Raylib.CheckCollisionRecs(bullets[i].rect, wall) || bullets[i].OutOfBounds(800, 600))
        {
            bullets.RemoveAt(i);
        }
    }

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Green);

    tank1.Draw();
    tank2.Draw();

    Raylib.DrawRectangleRec(wall, Color.DarkGray);

    foreach (var bullet in bullets)
    {
        bullet.Draw();
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

class Tank
{
    public Rectangle rect;
    private Color color;
    private Vector2 direction = new Vector2(1, 0);

    public Tank(float x, float y, Color color)
    {
        this.rect = new Rectangle(x, y, 40, 40);
        this.color = color;
    }

    public void Update(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right, KeyboardKey shoot, List<Bullet> bullets)
    {
        float speed = 4.0f;

        if (Raylib.IsKeyDown(up)) { rect.Y -= speed; direction = new Vector2(0, -1); }
        if (Raylib.IsKeyDown(down)) { rect.Y += speed; direction = new Vector2(0, 1); }
        if (Raylib.IsKeyDown(left)) { rect.X -= speed; direction = new Vector2(-1, 0); }
        if (Raylib.IsKeyDown(right)) { rect.X += speed; direction = new Vector2(1, 0); }

        if (Raylib.IsKeyPressed(shoot))
        {
            bullets.Add(new Bullet(rect.X + 15, rect.Y + 15, direction));
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(rect, color);
    }
}


class Bullet
{
    public Rectangle rect;
    private Vector2 direction;
    private float speed = 6.0f;

    public Bullet(float x, float y, Vector2 direction)
    {
        this.rect = new Rectangle(x, y, 10, 10);
        this.direction = direction;
    }

    public void Update()
    {
        rect.X += direction.X * speed;
        rect.Y += direction.Y * speed;
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(rect, Color.Black);
    }

    public bool OutOfBounds(int width, int height)
    {
        return rect.X < 0 || rect.X > width || rect.Y < 0 || rect.Y > height;
    }
}