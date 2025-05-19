using Raylib_cs;
using System.Numerics;

public class Bullet
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Texture2D Texture;
    public bool Active = true;
    public float Speed = 8f;
    public float Rotation;

    private float lifetime = 4f;
    private float age = 0f;

    // Pelaajan ammuksia varten
    public Bullet(Vector2 startPos, float rotation, Texture2D texture)
    {
        Texture = texture;
        Position = startPos;
        Rotation = rotation;

        float radians = MathF.PI / 180 * (rotation - 90f);
        Velocity = new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * Speed;
    }

    // UFO:n ammuksia varten
    public Bullet(Vector2 startPos, float rotation, Texture2D texture, float speed, Vector2 velocity)
    {
        Texture = texture;
        Position = startPos;
        Rotation = rotation;
        Speed = speed;
        Velocity = velocity * Speed;
    }

    public void Update()
    {
        Position += Velocity;

        age += Raylib.GetFrameTime();
        if (age >= lifetime)
        {
            Active = false;
        }

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