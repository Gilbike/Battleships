using Microsoft.Xna.Framework;

namespace Battleships.UI;

public abstract class UIElement {
  public bool Active = false;
  public Vector2 position { get; private set; }

  public UIElement(Vector2 position) {
    this.position = position;
  }

  public abstract void Render();
}