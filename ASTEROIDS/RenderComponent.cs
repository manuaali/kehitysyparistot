using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASTEROIDS
{
    class RenderComponent
    {
        private Texture2D Texture;

        public RenderComponent(Texture2D texture)
        {
            Texture = texture;
        }

        public void Draw(Vector2 position, float rotation = 0f)
        {
            Raylib.DrawTextureEx(Texture, position - new Vector2(Texture.Width, Texture.Height) / 2, rotation, 1f, Color.White);
        }
    }
}
