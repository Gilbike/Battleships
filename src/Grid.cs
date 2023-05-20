using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Grid : BaseObject {
  private const int FieldGap = 2;

  private Vector2 _position;
  private int _size;
  private bool _encoded;

  private int _fieldSize;
  private Texture2D _fieldTexture;

  private Field[] _fields = new Field[10 * 10];

  public Grid(GraphicsDevice device, Vector2 position, int size, bool encoded) {
    _position = position;
    _size = size;
    _encoded = encoded;
    _fieldSize = (size - (FieldGap - 1) * 10) / 10;

    CreateFieldTexture(device);
    CreateFields();
  }

  private void CreateFieldTexture(GraphicsDevice device) {
    _fieldTexture = new Texture2D(device, _fieldSize, _fieldSize);
    Color[] data = new Color[_fieldSize * _fieldSize];
    for (int i = 0; i < _fieldSize * _fieldSize; i++) {
      data[i] = Color.White;
    }
    _fieldTexture.SetData(data);
  }

  private void CreateFields() {
    for (int row = 0; row < 10; row++) {
      for (int col = 0; col < 10; col++) {
        int rowGap = (row - 1) * FieldGap;
        int colGap = (col - 1) * FieldGap;
        _fields[col + row * 10] = new Field(new Vector2(_position.X + col * _fieldSize + colGap, _position.Y + row * _fieldSize + rowGap), _fieldTexture);
      }
    }

    _fields[0].State = FieldState.Ship;
    _fields[10].State = FieldState.Ship;
    _fields[20].State = FieldState.Ship;
    _fields[1].State = FieldState.Hit;
    _fields[30].State = FieldState.ShipHit;
  }

  public void Render(SpriteBatch batch) {
    foreach (Field field in _fields) {
      field.Render(batch);
    }
  }

  public void Update() { }
}