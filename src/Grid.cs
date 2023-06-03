using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Grid {
  private Vector2 _position;
  private int _size;
  private bool _encoded;

  public bool Encoded => _encoded;

  private int _fieldSize;
  private Texture2D _fieldTexture;

  private Field[] _fields = new Field[10 * 10];

  private Ship[] _ships = new Ship[5];

  public Action onFleetDestroyed;

  public int FieldSize => _fieldSize;

  public Grid(Vector2 position, int size, bool encoded) {
    _position = position;
    _size = size;
    _encoded = encoded;
    _fieldSize = size / 10;

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
        _fields[col + row * 10] = new Field(this, new Vector2(_position.X + col * _fieldSize, _position.Y + row * _fieldSize), _fieldTexture);
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
      if (y < _position.Y + row * _fieldSize || y > _position.Y + (row + 1) * _fieldSize)
        continue;

      for (int col = 0; col < 10; col++) {
        if (x < _position.X + col * _fieldSize || x > _position.X + (col + 1) * _fieldSize)
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
      ShipPart[] locations = ship.GetParts();
      foreach (ShipPart location in locations) {
        int index = GetIndexFromLocationVector(location.location);
        _fields[index].Part = location;
        _fields[index].ShipID = i;
      }
      break;
    }
  }

  public bool AttackField(int index) {
    Field attackedField = _fields[index];
    if (attackedField.Part == null) {
      attackedField.Attacked = true;

    } else if (attackedField.Part != null) {
      attackedField.Attacked = true;
      bool shipDied = _ships[attackedField.ShipID].Hit();
      if (shipDied) {
        foreach (Vector2 location in _ships[attackedField.ShipID].GetLocations()) {
          _fields[GetIndexFromLocationVector(location)].Sunken = true;
        }
        if (!isFleetAlive()) {
          onFleetDestroyed?.Invoke();
        }
      }
      return true;
    }
    return false;
  }

  public bool AttackField(Vector2 location) {
    return AttackField(GetIndexFromLocationVector(location));
  }

  public bool isFleetAlive() {
    foreach (Ship ship in _ships) {
      if (ship.Alive) return true;
    }
    return false;
  }

  public int[] GetNeighbourFields(int index) {
    int[] neighbours = new int[4] { -1, -1, -1, -1 };

    if (index / 10 != 0) { // not on top border
      neighbours[0] = (index / 10 - 1) * 10 + index % 10;
    }
    if (index % 10 != 9) { // not on right border
      neighbours[1] = index + 1;
    }
    if (index / 10 != 9) { // not on bottom border
      neighbours[2] = (index / 10 + 1) * 10 + index % 10;
    }
    if (index % 10 != 0) { // not on left border
      neighbours[3] = index - 1;
    }

    return neighbours;
  }

  public int[] GetNeighbourFields(Vector2 location) {
    return GetNeighbourFields(GetIndexFromLocationVector(location));
  }

  public void Render(SpriteBatch batch) {
    foreach (Field field in _fields) {
      field.Render(batch);
    }
  }

  public void Update() { }
}