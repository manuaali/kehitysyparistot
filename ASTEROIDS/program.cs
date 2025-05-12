using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

Raylib.InitWindow(800, 600, "Asteroids");
Raylib.SetTargetFPS(60);

Texture2D shipTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\playerShip2_blue.png");
Texture2D bulletTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\laserBlue01.png");
Texture2D asteroidTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\meteorBrown_big1.png");
Texture2D smallAsteroidTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\meteorBrown_med1.png");
Texture2D ufoTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\ufoRed.png");
Texture2D ufoBulletTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\laserRed07.png");
Texture2D explosionTexture = Raylib.LoadTexture("C:\\Tiedostot\\kehitysymparistot\\kehitysyparistot\\ASTEROIDS\\Images\\rajahdys.png");

Player player = new Player(shipTexture);
List<Bullet> bullets = new();
List<Asteroid> asteroids = new();
List<Ufo> ufos = new();

int wave = 1;
bool ufoSpawnedThisWave = false;
bool playerDead = false;
float explosionTimer = 0f;

void StartWave(int count)
{
    for (int i = 0; i < count; i++)
    {
        Vector2 spawn = new Vector2(
            Raylib.GetRandomValue(0, Raylib.GetScreenWidth()),
            Raylib.GetRandomValue(0, Raylib.GetScreenHeight())
        );

        Asteroid newAsteroid = new Asteroid(spawn, asteroidTexture);
        newAsteroid.IsSmall = false;
        asteroids.Add(newAsteroid);
    }
}

void ResetGame()
{
    player.Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
    bullets.Clear();
    asteroids.Clear();
    ufos.Clear();
    wave = 1;
    ufoSpawnedThisWave = false;
    StartWave(wave);
}

StartWave(wave);

while (!Raylib.WindowShouldClose())
{
    if (!playerDead)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            Vector2 bulletSpawn = player.Position;
            bullets.Add(new Bullet(bulletSpawn, player.Rotation, bulletTexture));
        }

        if (!ufoSpawnedThisWave && wave >= 3)
        {
            Vector2 spawn = new Vector2(
                Raylib.GetRandomValue(0, Raylib.GetScreenWidth()),
                Raylib.GetRandomValue(0, Raylib.GetScreenHeight())
            );
            ufos.Add(new Ufo(spawn, ufoTexture, ufoBulletTexture));
            ufoSpawnedThisWave = true;
        }

        player.Update();
    }
    else
    {
        explosionTimer -= Raylib.GetFrameTime();
        if (explosionTimer <= 0f)
        {
            ResetGame();
            playerDead = false;
        }
    }

    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        bullets[i].Update();
        if (!bullets[i].Active)
            bullets.RemoveAt(i);
    }

    foreach (var asteroid in asteroids)
        asteroid.Update();

    foreach (var ufo in ufos)
        ufo.Update();

    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        Bullet bullet = bullets[i];

        for (int j = asteroids.Count - 1; j >= 0; j--)
        {
            Asteroid asteroid = asteroids[j];
            float distance = Vector2.Distance(bullet.Position, asteroid.Position);
            float collisionRadius = asteroid.IsSmall ? 20f : 40f;

            if (distance < collisionRadius)
            {
                asteroids.RemoveAt(j);
                bullets.RemoveAt(i);

                if (!asteroid.IsSmall)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        float angle = Raylib.GetRandomValue(0, 360);
                        float radians = MathF.PI / 180 * angle;
                        Vector2 velocity = new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * 2f;

                        Asteroid smallAsteroid = new Asteroid(asteroid.Position, smallAsteroidTexture);
                        smallAsteroid.Velocity = velocity;
                        smallAsteroid.Speed = 2f;
                        smallAsteroid.IsSmall = true;

                        asteroids.Add(smallAsteroid);
                    }
                }
                break;
            }
        }
    }

    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        Bullet bullet = bullets[i];
        for (int j = ufos.Count - 1; j >= 0; j--)
        {
            Ufo ufo = ufos[j];
            float distance = Vector2.Distance(bullet.Position, ufo.Position);
            if (distance < 30f)
            {
                ufos.RemoveAt(j);
                bullets.RemoveAt(i);
                break;
            }
        }
    }

    if (!playerDead)
    {
        foreach (var asteroid in asteroids)
        {
            if (Vector2.Distance(player.Position, asteroid.Position) < (asteroid.IsSmall ? 20f : 40f))
            {
                playerDead = true;
                explosionTimer = 2f;
                break;
            }
        }

        foreach (var ufo in ufos)
        {
            for (int i = ufo.Bullets.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(player.Position, ufo.Bullets[i].Position) < 20f)
                {
                    playerDead = true;
                    explosionTimer = 2f;
                    break;
                }
            }
        }
    }

    if (asteroids.Count == 0 && ufos.Count == 0)
    {
        wave++;
        StartWave(wave);
        ufoSpawnedThisWave = false;
    }

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    if (!playerDead)
        player.Draw();
    else
    {
        Raylib.DrawTextureEx(
            explosionTexture,
            player.Position - new Vector2(explosionTexture.Width, explosionTexture.Height) / 2,
            0f,
            1f,
            Color.White
        );
    }

    foreach (var bullet in bullets)
        bullet.Draw();

    foreach (var asteroid in asteroids)
        asteroid.Draw();

    foreach (var ufo in ufos)
        ufo.Draw(); 

    Raylib.DrawText($"Wave: {wave}", 10, 10, 24, Color.White);

    Raylib.EndDrawing();
}

Raylib.UnloadTexture(shipTexture);
Raylib.UnloadTexture(bulletTexture);
Raylib.UnloadTexture(asteroidTexture);
Raylib.UnloadTexture(smallAsteroidTexture);
Raylib.UnloadTexture(ufoTexture);
Raylib.UnloadTexture(ufoBulletTexture);
Raylib.UnloadTexture(explosionTexture);
Raylib.CloseWindow();
