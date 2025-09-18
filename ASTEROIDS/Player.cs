using ASTEROIDS;
using Raylib_cs;
using System.Numerics;

class Player
{
    public PositionComponent Position;
    private RenderComponent Render;
    public VelocityComponent Velocity;
    public float Rotation;
    private float Speed = 200f;

    public Player(Texture2D texture)
    {
        Position = new PositionComponent(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        Render = new RenderComponent(texture);
        Velocity = new VelocityComponent();
        Rotation = 0f;
    }

    public void Update()
    {
        // Päivitä rotation
        if (Raylib.IsKeyDown(KeyboardKey.Left)) Rotation -= 3f;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) Rotation += 3f;

        // Päivitä velocity W-näppäimen mukaan
        if (Raylib.IsKeyDown(KeyboardKey.Up))
            Velocity.Velocity = Utils.DirectionFromRotation(Rotation) * Speed;
        else
            Velocity.Velocity = Vector2.Zero;

        // Liikuta positiona velocityn mukaan
        Velocity.Move(Position, Raylib.GetFrameTime());
    }

    public void Draw()
    {
        Render.Draw(Position.Position, Rotation);
    }
}
