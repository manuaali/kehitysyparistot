using ASTEROIDS;
using Raylib_cs;
using System.Numerics;

class Asteroid
{
    public PositionComponent Position;
    public VelocityComponent Velocity;
    private RenderComponent Render;
    public bool IsSmall;

    private float Speed = 100f;

    public Asteroid(Vector2 spawnPos, Texture2D texture, bool isSmall)
    {
        Position = new PositionComponent(spawnPos.X, spawnPos.Y);
        Render = new RenderComponent(texture);
        IsSmall = isSmall;

        Vector2 direction = Utils.GetRandomDirection(1f);
        Velocity = new VelocityComponent(direction.X * Speed, direction.Y * Speed);
    }

    public void Update()
    {
        Velocity.Move(Position, Raylib.GetFrameTime());
    }

    public void Draw()
    {
        Render.Draw(Position.Position);
    }

    public float GetRadius() => RenderTextureRadius();

    private float RenderTextureRadius()
    {
        return IsSmall ? 20f : 40f; 
    }
}
