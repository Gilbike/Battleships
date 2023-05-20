using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

interface BaseObject {
  void Update();
  void Render(SpriteBatch batch);
}