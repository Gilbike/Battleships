using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class Grid : BaseObject {
  private const int FieldGap = 2;

  private Vector2 _position;
  private int _size;
  private bool _encoded;

  public bool Encoded => _encoded;

  private int _fieldSize;
  private Texture2D _fieldTexture;

  private Field[] _fields = new Field[10 * 10];

  private Ship[] _ships = new Ship[5];

  public Grid(Vector2 position, int size, bool encoded) {
    _position = position;
    _size = size;
    _encoded = encoded;
    _fieldSize = (size - (FieldGap - 1) * 10) / 10;

    CreateFieldTexture();
    CreateFields();
  }

  private void CreateFieldTexture() {
    _fieldTexture = new Texture2D(BattleshipGame.Instance.GraphicsDevice, _fieldSize, _fieldSize);
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
        _fields[col + row * 10] = new Field(this, new Vector2(_position.X + col * _fieldSize + colGap, _position.Y + row * _fieldSize + rowGap), _fieldTexture);
      }
    }
  }

  public Field GetField(int index) {
    return _fields[index];
  }

  public int GetIndexFromLocationVector(Vector2 location) {
    return (int)location.Y * 10 + (int)location.X;
  }

  public Vector2 GetLocationVectorFromIndex(int index) {
    return new Vector2(index / 10, index % 10);
  }

  public Vector4 GetDimensions() {
    return new Vector4(_position.X, _position.Y, _position.X + _size, _position.Y + _size);
  }

  public Vector2 GetHoveredField() {
    int x = BattleshipGame.Instance.mouseState.X;
    int y = BattleshipGame.Instance.mouseState.Y;
    for (int row = 0; row < 10; row++) {
      if (y < _position.Y + row * _fieldSize + (row - 1) * FieldGap || y > _position.Y + (row + 1) * _fieldSize + row * FieldGap)
        continue;

      for (int col = 0; col < 10; col++) {
        if (x < _position.X + col * _fieldSize + (col - 1) * FieldGap || x > _position.X + (col + 1) * _fieldSize + col * FieldGap)
          continue;

        return new Vector2(col, row);
      }
    }
    return new Vector2(-1, -1);
  }

  public void PlaceShip(Ship ship) {
    for (int i = 0; i < _ships.Length; i++) {
      if (_ships[i] != null)
        continue;

      _ships[i] = ship;
      Vector2[] locations = ship.GetLocations();
      foreach (Vector2 location in locations) {
        int index = GetIndexFromLocationVector(location);
        _fields[index].State = FieldState.Ship;
        _fields[index].ShipID = i;
      }
      break;
    }
  }

  public void Render(SpriteBatch batch) {
    foreach (Field field in _fields) {
      field.Render(batch);
    }
  }

  public void Update() { }
}