using System.Numerics;
using Raylib_cs;

public static class Utils
{
    public static Vector2 WrapPosition(Vector2 position, int screenWidth, int screenHeight)
    {
        if (position.X < 0) position.X = screenWidth;
        else if (position.X > screenWidth) position.X = 0;

        if (position.Y < 0) position.Y = screenHeight;
        else if (position.Y > screenHeight) position.Y = 0;

        return position;
    }
    public static Vector2 DirectionFromRotation(float rotation)
    {
        // Raylibin kulma nollassa osoittaa ylös, joten vähennetään PI/2
        return new Vector2((float)Math.Cos(rotation - MathF.PI / 2), (float)Math.Sin(rotation - MathF.PI / 2));
    }

    public static Random rnd = new Random();
    public static Vector2 GetRandomDirection(float speed)
    {
        float angle = Raylib.GetRandomValue(0, 360);
        float radians = angle * Raylib.DEG2RAD;
        return new Vector2(MathF.Cos(radians), MathF.Sin(radians)) * speed;
    }
}
