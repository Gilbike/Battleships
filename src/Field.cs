using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battleships;

enum FieldState {
  Empty,
  Ship,
  Hit
}

public class Field : BaseObject {
  private Texture2D _texture;
  private Vector2 _position;

  private FieldState _state;

  public Field(Vector2 position, Texture2D texture) {
    _position = position;
    _texture = texture;

    _state = FieldState.Empty;
  }

  public void Render(SpriteBatch batch) {
    batch.Draw(_texture, _position, Color.White);
  }

  public void Update() { }
}