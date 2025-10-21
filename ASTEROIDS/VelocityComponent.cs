using ASTEROIDS;
using System.Numerics;

class VelocityComponent
{
    public Vector2 Velocity;

    public VelocityComponent(float x = 0f, float y = 0f)
    {
        Velocity = new Vector2(x, y);
    }

    public void Move(PositionComponent position, float deltaTime)
    {
        position.Position += Velocity * deltaTime;
        position.WrapAround();
    }
}
