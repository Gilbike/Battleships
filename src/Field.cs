using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battleships;

public enum FieldState {
  Empty,
  Ship,
  Hit,
  ShipHit,
}

public class Field : BaseObject {
  private Texture2D _texture;
  private Vector2 _position;

  public FieldState State { get; set; }

  public Field(Vector2 position, Texture2D texture) {
    _position = position;
    _texture = texture;

    State = FieldState.Empty;
  }

  public void Render(SpriteBatch batch) {
    Color renderColor = Color.White;
    switch (State) {
      case FieldState.Ship:
        renderColor = Color.GreenYellow;
        break;
      case FieldState.Hit:
        renderColor = Color.Orange;
        break;
      case FieldState.ShipHit:
        renderColor = Color.Red;
        break;
    }
    batch.Draw(_texture, _position, renderColor);
  }

  public void Update() { }
}