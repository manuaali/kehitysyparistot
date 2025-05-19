using Raylib_cs;
using System.Numerics;

public class Asteroid
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Texture2D Texture;
    public float Rotation;
    public float Speed = 2f;
    public bool IsSmall = false;

    public Asteroid(Vector2 spawnPos, Texture2D texture)
    {
        Texture = texture;
        Position = spawnPos;

        float angle = Raylib.GetRandomValue(0, 360);
        float radians = MathF.PI / 180 * angle;
        Velocity = new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * Speed;

        Rotation = Raylib.GetRandomValue(0, 360);
    }

    public void Update()
    {
        Position += Velocity;

        Position = Utils.WrapPosition(Position, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }


    public void Draw()
    {
        Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

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