using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

public class Ufo
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Texture2D Texture;
    public bool Active = true;

    public List<UfoBullet> Bullets = new List<UfoBullet>();
    private Texture2D bulletTexture;

    private float fireTimer = 0f;
    private float fireInterval;

    public Ufo(Vector2 spawnPos, Texture2D texture, Texture2D bulletTex)
    {
        Texture = texture;
        bulletTexture = bulletTex;
        Position = spawnPos;

        float angle = Raylib.GetRandomValue(0, 360);
        float radians = MathF.PI / 180 * angle;
        float speed = 2.5f;
        Velocity = new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * speed;

        fireInterval = Raylib.GetRandomValue(1500, 3000) / 1000f;
    }

    public void Update()
    {
        Position += Velocity;
        Position = Utils.WrapPosition(Position, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        fireTimer += Raylib.GetFrameTime();
        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
            fireInterval = Raylib.GetRandomValue(1500, 3000) / 1000f; // satunnaista uusi väli joka laukauksen jälkeen
        }

        for (int i = Bullets.Count - 1; i >= 0; i--)
        {
            Bullets[i].Update();
            if (!Bullets[i].Active)
            {
                Bullets.RemoveAt(i);
            }
        }
    }

    private void Fire()
    {
        float angle = Raylib.GetRandomValue(0, 360);
        float radians = MathF.PI / 180 * angle;
        Vector2 direction = new Vector2(MathF.Cos(radians), MathF.Sin(radians));

        Bullets.Add(new UfoBullet(Position, direction, bulletTexture));
    }

    public void Draw()
    {
        Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        Raylib.DrawTexturePro(
            Texture,
            new Rectangle(0, 0, Texture.Width, Texture.Height),
            new Rectangle(Position.X, Position.Y, Texture.Width, Texture.Height),
            origin,
            0f,
            Color.White
        );

        foreach (var bullet in Bullets)
            bullet.Draw();
    }
}