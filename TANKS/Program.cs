using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

Raylib.InitWindow(800, 600, "Tank Game");
Raylib.InitAudioDevice();
Raylib.SetTargetFPS(60);

// Tekstuurit ja äänet 
Texture2D tank1Texture = Raylib.LoadTexture("Images/Tank1.png");
Texture2D tank2Texture = Raylib.LoadTexture("Images/Tank2.png");
Texture2D explosionTexture = Raylib.LoadTexture("Images/explosion.png");

Sound shootSound = Raylib.LoadSound("Images/shoot.wav");
Sound explosionSound = Raylib.LoadSound("Images/explosion.wav");

// Pelin tila 
bool inMenu = true;
bool explosionActive = false;
float explosionTimer = 0f;
float explosionDuration = 2.0f;
Vector2 explosionPosition = new Vector2();

// Pelaajat ja kenttä 
Tank tank1 = new Tank(100, 250, tank1Texture);
Tank tank2 = new Tank(600, 250, tank2Texture);
List<Bullet> bullets = new List<Bullet>();
Rectangle wall = new Rectangle(350, 200, 100, 200);

void ResetGame()
{
    tank1.ResetPosition(100, 250);
    tank2.ResetPosition(600, 250);
    bullets.Clear();
    explosionActive = false;
    explosionTimer = 0f;
}

while (!Raylib.WindowShouldClose())
{
    if (inMenu)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Raylib.DrawText("TANK GAME", 250, 100, 40, Color.White);

        Rectangle startButton = new Rectangle(300, 250, 200, 60);
        Rectangle quitButton = new Rectangle(300, 350, 200, 60);

        Vector2 mouse = Raylib.GetMousePosition();

        if (Raylib.CheckCollisionPointRec(mouse, startButton))
        {
            Raylib.DrawRectangleRec(startButton, Color.Gray);
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                inMenu = false;
        }
        else Raylib.DrawRectangleRec(startButton, Color.DarkGray);

        Raylib.DrawText("START", 355, 265, 30, Color.White);

        if (Raylib.CheckCollisionPointRec(mouse, quitButton))
        {
            Raylib.DrawRectangleRec(quitButton, Color.Gray);
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Raylib.CloseWindow();
        }
        else Raylib.DrawRectangleRec(quitButton, Color.DarkGray);

        Raylib.DrawText("QUIT", 370, 365, 30, Color.White);

        Raylib.EndDrawing();
        continue;
    }

    if (!explosionActive)
    {
        tank1.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.F, bullets, wall, tank2, shootSound);
        tank2.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.M, bullets, wall, tank1, shootSound);

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].Update();

            if (bullets[i].Shooter == tank1 && Raylib.CheckCollisionRecs(bullets[i].rect, tank1.rect))
                continue;
            if (bullets[i].Shooter == tank2 && Raylib.CheckCollisionRecs(bullets[i].rect, tank2.rect))
                continue;

            if (bullets[i].Shooter != tank1 && Raylib.CheckCollisionRecs(bullets[i].rect, tank1.rect))
            {
                tank2.Score++;
                Raylib.PlaySound(explosionSound);
                explosionActive = true;
                explosionTimer = 0f;
                explosionPosition = new Vector2(tank1.rect.X, tank1.rect.Y);
                bullets.Clear();
                continue;
            }

            if (bullets[i].Shooter != tank2 && Raylib.CheckCollisionRecs(bullets[i].rect, tank2.rect))
            {
                tank1.Score++;
                Raylib.PlaySound(explosionSound);
                explosionActive = true;
                explosionTimer = 0f;
                explosionPosition = new Vector2(tank2.rect.X, tank2.rect.Y);
                bullets.Clear();
                continue;
            }

            if (Raylib.CheckCollisionRecs(bullets[i].rect, wall) || bullets[i].OutOfBounds(800, 600))
                bullets.RemoveAt(i);
        }
    }
    else
    {
        explosionTimer += Raylib.GetFrameTime();
        if (explosionTimer >= explosionDuration)
            ResetGame();
    }

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    tank1.Draw();
    tank2.Draw();

    Raylib.DrawRectangleRec(wall, Color.DarkGray);

    foreach (var bullet in bullets)
        bullet.Draw();

    if (explosionActive)
    {
        float progress = explosionTimer / explosionDuration;
        float scale = 1.0f + progress * 1.3f;
        int alpha = (int)(255 * (1.0f - progress));

        float explosionX = explosionPosition.X + tank1Texture.Width / 2 - (explosionTexture.Width * scale) / 2;
        float explosionY = explosionPosition.Y + tank1Texture.Height / 2 - (explosionTexture.Height * scale) / 2;

        Raylib.DrawTexturePro(
            explosionTexture,
            new Rectangle(0, 0, explosionTexture.Width, explosionTexture.Height),
            new Rectangle(explosionX, explosionY, explosionTexture.Width * scale, explosionTexture.Height * scale),
            new Vector2(0, 0),
            0,
            new Color(255, 255, 255, alpha)
        );
    }

    Raylib.DrawText($"Tank 1: {tank1.Score}", 10, 10, 20, Color.White);
    Raylib.DrawText($"Tank 2: {tank2.Score}", 700, 10, 20, Color.White);

    Raylib.EndDrawing();
}

Raylib.UnloadTexture(tank1Texture);
Raylib.UnloadTexture(tank2Texture);
Raylib.UnloadTexture(explosionTexture);
Raylib.UnloadSound(shootSound);
Raylib.UnloadSound(explosionSound);
Raylib.CloseAudioDevice();
Raylib.CloseWindow();


class Tank
{
    public Rectangle rect;
    private Texture2D texture;
    private Vector2 direction = new Vector2(0, -1);
    public int Score = 0;

    public Tank(float x, float y, Texture2D texture)
    {
        this.rect = new Rectangle(x, y, texture.Width, texture.Height);
        this.texture = texture;
    }

    public void ResetPosition(float x, float y)
    {
        rect.X = x;
        rect.Y = y;
    }

    public void Update(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right, KeyboardKey shoot,
                       List<Bullet> bullets, Rectangle wall, Tank opponent, Sound shootSound)
    {
        float speed = 2.0f;
        Rectangle newRect = rect;

        if (Raylib.IsKeyDown(up)) { newRect.Y -= speed; direction = new Vector2(0, -1); }
        if (Raylib.IsKeyDown(down)) { newRect.Y += speed; direction = new Vector2(0, 1); }
        if (Raylib.IsKeyDown(left)) { newRect.X -= speed; direction = new Vector2(-1, 0); }
        if (Raylib.IsKeyDown(right)) { newRect.X += speed; direction = new Vector2(1, 0); }

        if (!Raylib.CheckCollisionRecs(newRect, wall) && !Raylib.CheckCollisionRecs(newRect, opponent.rect))
            rect = newRect;

        if (Raylib.IsKeyPressed(shoot))
        {
            Vector2 barrelEnd = new Vector2(rect.X + rect.Width / 2 + direction.X * 20, rect.Y + rect.Height / 2 + direction.Y * 20);
            bullets.Add(new Bullet(barrelEnd.X - 5, barrelEnd.Y - 5, direction, this));

            Raylib.PlaySound(shootSound);
        }
    }

    public void Draw()
    {
        float rotation = MathF.Atan2(direction.Y, direction.X) * (180.0f / MathF.PI) + 90.0f;

        Raylib.DrawTexturePro(
            texture,
            new Rectangle(0, 0, texture.Width, texture.Height),
            new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width, rect.Height),
            new Vector2(rect.Width / 2, rect.Height / 2),
            rotation,
            Color.White
        );
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
        Raylib.DrawRectangleRec(rect, Color.DarkPurple);
    }

    public bool OutOfBounds(int width, int height)
    {
        return rect.X < 0 || rect.X > width || rect.Y < 0 || rect.Y > height;
    }
}
