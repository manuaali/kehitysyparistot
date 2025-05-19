using System.Numerics;

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
}