using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

Raylib.InitWindow(800, 600, "Asteroids");
Raylib.SetTargetFPS(60);

Texture2D shipTexture = Raylib.LoadTexture("Images\\playerShip2_blue.png");
Texture2D bulletTexture = Raylib.LoadTexture("Images\\laserBlue01.png");
Texture2D asteroidTexture = Raylib.LoadTexture("Images\\meteorBrown_big1.png");
Texture2D smallAsteroidTexture = Raylib.LoadTexture("Images\\meteorBrown_med1.png");
Texture2D ufoTexture = Raylib.LoadTexture("Images\\ufoRed.png");
Texture2D ufoBulletTexture = Raylib.LoadTexture("Images\\laserRed07.png");
Texture2D explosionTexture = Raylib.LoadTexture("Images\\rajahdys.png");

// Pelaaja
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
        float spawnX = Raylib.GetRandomValue(0, Raylib.GetScreenWidth());
        float spawnY = Raylib.GetRandomValue(0, Raylib.GetScreenHeight());

        Asteroid newAsteroid = new Asteroid(new Vector2(spawnX, spawnY), asteroidTexture, false);
        asteroids.Add(newAsteroid);
    }
}

void ResetGame()
{
    player.Position.Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
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
    float deltaTime = Raylib.GetFrameTime();

    if (!playerDead)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            bullets.Add(new Bullet(player.Position.Position, player.Rotation, bulletTexture));
        }

        if (!ufoSpawnedThisWave && wave >= 3)
        {
            float spawnX = Raylib.GetRandomValue(0, Raylib.GetScreenWidth());
            float spawnY = Raylib.GetRandomValue(0, Raylib.GetScreenHeight());

            ufos.Add(new Ufo(new Vector2(spawnX, spawnY), ufoTexture, ufoBulletTexture));
            ufoSpawnedThisWave = true;
        }

        player.Update();
    }
    else
    {
        explosionTimer -= deltaTime;
        if (explosionTimer <= 0f)
        {
            ResetGame();
            playerDead = false;
        }
    }

    // Päivitä luodit
    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        bullets[i].Update();
        if (!bullets[i].Active)
            bullets.RemoveAt(i);
    }

    // Päivitä asteroidit
    foreach (var asteroid in asteroids)
        asteroid.Update();

    // Päivitä UFOt
    foreach (var ufo in ufos)
        ufo.Update(deltaTime);

    // Asteroidien törmäys ja tuho
    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        Bullet bullet = bullets[i];
        for (int j = asteroids.Count - 1; j >= 0; j--)
        {
            Asteroid asteroid = asteroids[j];
            if (Raylib.CheckCollisionPointCircle(bullet.Position.Position, asteroid.Position.Position, asteroid.GetRadius()))
            {
                asteroids.RemoveAt(j);
                bullets.RemoveAt(i);

                if (!asteroid.IsSmall)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        Asteroid smallAsteroid = new Asteroid(
                            asteroid.Position.Position,   
                            smallAsteroidTexture,         
                            true                          
                        );

                        smallAsteroid.Velocity.Velocity = Utils.GetRandomDirection(2f);

                        asteroids.Add(smallAsteroid);
                    }
                }
                break;
            }
        }
    }

    // UFOjen törmäys
    for (int i = bullets.Count - 1; i >= 0; i--)
    {
        Bullet bullet = bullets[i];
        for (int j = ufos.Count - 1; j >= 0; j--)
        {
            Ufo ufo = ufos[j];
            if (Raylib.CheckCollisionPointCircle(bullet.Position.Position, ufo.Position.Position, 20f))
            {
                ufos.RemoveAt(j);
                bullets.RemoveAt(i);
                break;
            }
        }
    }

    // Pelaajan törmäykset
    if (!playerDead)
    {
        foreach (var asteroid in asteroids)
        {
            if (Raylib.CheckCollisionPointCircle(player.Position.Position, asteroid.Position.Position, asteroid.GetRadius()))
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
                if (Raylib.CheckCollisionPointCircle(player.Position.Position, ufo.Bullets[i].Position.Position, 20f))
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

    // Piirrä kaikki
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    if (!playerDead)
        player.Draw();
    else
    {
        Raylib.DrawTextureEx(
            explosionTexture,
            player.Position.Position - new Vector2(explosionTexture.Width, explosionTexture.Height) / 2,
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

// Unload textures
Raylib.UnloadTexture(shipTexture);
Raylib.UnloadTexture(bulletTexture);
Raylib.UnloadTexture(asteroidTexture);
Raylib.UnloadTexture(smallAsteroidTexture);
Raylib.UnloadTexture(ufoTexture);
Raylib.UnloadTexture(ufoBulletTexture);
Raylib.UnloadTexture(explosionTexture);
Raylib.CloseWindow();
