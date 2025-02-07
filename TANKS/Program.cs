using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

Raylib.InitWindow(800, 600, "Tank Game");
Raylib.SetTargetFPS(60);

Tank tank1 = new Tank(100, 250, Color.Pink);
Tank tank2 = new Tank(600, 250, Color.Orange);

List<Bullet> bullets = new List<Bullet>();

Rectangle wall = new Rectangle(350, 200, 100, 200);

void ResetGame()
{
    tank1.ResetPosition(100, 250);
    tank2.ResetPosition(600, 250);
    bullets.Clear();
}

while (!Raylib.WindowShouldClose())
{
    tank1.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.F, bullets, wall, tank2);
    tank2.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.M, bullets, wall, tank1);

    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        bullets[i].Update();

        if (bullets[i].Shooter == tank1 && Raylib.CheckCollisionRecs(bullets[i].rect, tank1.rect))
        {
            continue;
        }
        if (bullets[i].Shooter == tank2 && Raylib.CheckCollisionRecs(bullets[i].rect, tank2.rect))
        {
            continue;
        }

        if (bullets[i].Shooter != tank1 && Raylib.CheckCollisionRecs(bullets[i].rect, tank1.rect))
        {
            tank2.Score++;
            bullets.RemoveAt(i);
            ResetGame();
            continue;
        }
        if (bullets[i].Shooter != tank2 && Raylib.CheckCollisionRecs(bullets[i].rect, tank2.rect))
        {
            tank1.Score++;
            bullets.RemoveAt(i);
            ResetGame();
            continue;
        }

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

    Raylib.DrawText($"Tank 1: {tank1.Score}", 10, 10, 20, Color.White);
    Raylib.DrawText($"Tank 2: {tank2.Score}", 700, 10, 20, Color.White);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();


class Tank
{
    public Rectangle rect;
    private Color color;
    private Vector2 direction = new Vector2(1, 0); 
    public int Score = 0;
    private float startX, startY;

    public Tank(float x, float y, Color color)
    {
        this.rect = new Rectangle(x, y, 40, 40);
        this.color = color;
        this.startX = x;
        this.startY = y;
    }

    public void ResetPosition(float x, float y)
    {
        rect.X = x;
        rect.Y = y;
    }

    public void Update(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right, KeyboardKey shoot, List<Bullet> bullets, Rectangle wall, Tank opponent)
    {
        float speed = 4.0f;
        Rectangle newRect = rect;

        if (Raylib.IsKeyDown(up)) { newRect.Y -= speed; direction = new Vector2(0, -1); }
        if (Raylib.IsKeyDown(down)) { newRect.Y += speed; direction = new Vector2(0, 1); }
        if (Raylib.IsKeyDown(left)) { newRect.X -= speed; direction = new Vector2(-1, 0); }
        if (Raylib.IsKeyDown(right)) { newRect.X += speed; direction = new Vector2(1, 0); }

        // Tarkista törmäys seinään tai toiseen tankkiin ennen päivittämistä
        if (!Raylib.CheckCollisionRecs(newRect, wall) && !Raylib.CheckCollisionRecs(newRect, opponent.rect))
        {
            rect = newRect;
        }

        if (Raylib.IsKeyPressed(shoot))
        {
            Vector2 barrelEnd = new Vector2(rect.X + rect.Width / 2 + direction.X * 20, rect.Y + rect.Height / 2 + direction.Y * 20);
            bullets.Add(new Bullet(barrelEnd.X - 5, barrelEnd.Y - 5, direction, this));
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(rect, color);
        Vector2 barrelEnd = new Vector2(rect.X + rect.Width / 2 + direction.X * 20, rect.Y + rect.Height / 2 + direction.Y * 20);
        Raylib.DrawRectangle((int)barrelEnd.X - 5, (int)barrelEnd.Y - 5, 10, 20, color);
    }
}


class Bullet
{
    public Rectangle rect;
    private Vector2 direction;
    private float speed = 6.0f;
    public Tank Shooter { get; }

    public Bullet(float x, float y, Vector2 direction, Tank shooter)
    {
        this.rect = new Rectangle(x, y, 10, 10);
        this.direction = direction;
        this.Shooter = shooter;
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
