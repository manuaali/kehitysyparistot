using Raylib_cs;
using System.Numerics;

public class Player
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public Texture2D Texture;
    public float Speed = 0.1f;
    public float MaxSpeed = 3f;

    public Player(Texture2D texture)
    {
        Texture = texture;
        Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        Velocity = Vector2.Zero;
        Rotation = 0f;
    }

    public void Update()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Left))
            Rotation -= 3f;
        if (Raylib.IsKeyDown(KeyboardKey.Right))
            Rotation += 3f;

        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            float radians = MathF.PI / 180 * (Rotation - 90f);
            Vector2 thrust = new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * Speed;
            Velocity += thrust;

            if (Velocity.Length() > MaxSpeed)
            {
                Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
            }
        }

        Position += Velocity;

        Position = Utils.WrapPosition(Position, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }

    public void Draw()
    {
        Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        Raylib.DrawTexturePro(
            Texture,
            new Rectangle(0, 0, Texture.Width, Texture.Height),
            new Rectangle(Position.X, Position.Y, Texture.Width, Texture.Height),
            origin,
            Rotation,
            Color.White
        );
    }
}


