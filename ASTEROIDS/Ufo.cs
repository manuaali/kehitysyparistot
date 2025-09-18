using ASTEROIDS;
using Raylib_cs;
using System.Numerics;

class Ufo
{
    public PositionComponent Position;
    public VelocityComponent Velocity;
    private RenderComponent Render;
    private Texture2D BulletTexture;
    public List<Bullet> Bullets = new();

    private float Speed = 120f;
    private float shootTimer;



    public Ufo(Vector2 spawnPos, Texture2D texture, Texture2D bulletTexture)
    {
        Position = new PositionComponent(spawnPos.X, spawnPos.Y);
        Render = new RenderComponent(texture);
        BulletTexture = bulletTexture;

        Vector2 direction = Utils.GetRandomDirection(1f);
        Velocity = new VelocityComponent(direction.X * Speed, direction.Y * Speed);

        ResetShootTimer();
    }

    public void Update(float deltaTime)
    {
        Velocity.Move(Position, deltaTime);

        shootTimer -= deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            ResetShootTimer();
        }

        for (int i = Bullets.Count - 1; i >= 0; i--)
        {
            Bullets[i].Update();
            if (!Bullets[i].Active)
                Bullets.RemoveAt(i);
        }
    }

    public void Draw()
    {
        Render.Draw(Position.Position);
        foreach (var b in Bullets)
            b.Draw();
    }

    private void Shoot()
    {
        float rotation = (float)(Utils.rnd.NextDouble() * 2 * Math.PI);
        Bullets.Add(new Bullet(Position.Position, rotation, BulletTexture));
    }

    private void ResetShootTimer()
    {
        shootTimer = 1.5f + (float)(Utils.rnd.NextDouble() * 1.5f);
    }
}
