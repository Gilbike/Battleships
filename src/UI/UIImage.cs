using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.UI;

public class UIImage : UIElement {
  private Vector2 scale;
  private Texture2D image;

  public UIImage(Vector2 position, Vector2 size, Texture2D image) : base(position) {
    this.image = image;
    this.scale = new Vector2(size.X / image.Width, size.Y / image.Height);
  }

  public override void Render() {
    BattleshipGame.Instance.Batch.Draw(image, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
  }
}