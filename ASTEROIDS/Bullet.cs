using ASTEROIDS;
using Raylib_cs;
using System.Numerics;

class Bullet
{
    public PositionComponent Position;
    public VelocityComponent Velocity;
    private RenderComponent Render;
    public bool Active = true;

    public Bullet(Vector2 spawnPos, float rotation, Texture2D texture, float speed = 400f)
    {
        Position = new PositionComponent(spawnPos.X, spawnPos.Y);
        Render = new RenderComponent(texture);

        Vector2 direction = Utils.DirectionFromRotation(rotation);
        Velocity = new VelocityComponent(direction.X * speed, direction.Y * speed);
    }

    public void Update()
    {
        Velocity.Move(Position, Raylib.GetFrameTime());

        // Poistetaan bullet, jos se menee ruudun ulkopuolelle
        if (Position.Position.X < 0 || Position.Position.X > Raylib.GetScreenWidth() ||
            Position.Position.Y < 0 || Position.Position.Y > Raylib.GetScreenHeight())
        {
            Active = false;
        }
    }

    public void Draw()
    {
        Render.Draw(Position.Position);
    }
}
