using Raylib_cs;
using System.Numerics;

public class UfoBullet
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Texture2D Texture;
    public bool Active = true;
    public float Speed = 6f;
    public float Radius => Texture.Width / 2f;


    private float lifetime = 3f;
    private float age = 0f;

    public UfoBullet(Vector2 startPos, Vector2 direction, Texture2D texture)
    {
        Texture = texture;
        Position = startPos;
        Velocity = Vector2.Normalize(direction) * Speed;
    }

    public void Update()
    {
        Position += Velocity;

        age += Raylib.GetFrameTime();
        if (age >= lifetime)
            Active = false;

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
            0f, 
            Color.White
        );
    }
}
