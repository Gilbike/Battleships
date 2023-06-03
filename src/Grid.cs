using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Grid {
  private Vector2 position;
  private int size;

  private Texture2D fieldTexture;

  private Field[] fields = new Field[10 * 10];
  private Ship[] ships = new Ship[5];

  public bool Encoded { get; private set; }
  public int FieldSize { get; private set; }

  public Action onFleetDestroyed;

  public Grid(Vector2 position, int size, bool encoded) {
    this.position = position;
    this.size = size;
    Encoded = encoded;
    FieldSize = size / 10;

    CreateFieldTexture();
    CreateFields();
  }

  private void CreateFieldTexture() {
    fieldTexture = new Texture2D(BattleshipGame.Instance.GraphicsDevice, FieldSize, FieldSize);
    Color[] data = new Color[FieldSize * FieldSize];
    for (int i = 0; i < FieldSize * FieldSize; i++) {
      data[i] = Color.White;
    }
    fieldTexture.SetData(data);
  }

  private void CreateFields() {
    for (int row = 0; row < 10; row++) {
      for (int col = 0; col < 10; col++) {
        fields[col + row * 10] = new Field(this, new Vector2(position.X + col * FieldSize, position.Y + row * FieldSize), fieldTexture);
      }
    }
  }

  public Field GetField(int index) {
    return fields[index];
  }

  public int GetIndexFromLocationVector(Vector2 location) {
    return (int)location.Y * 10 + (int)location.X;
  }

  public Vector2 GetLocationVectorFromIndex(int index) {
    return new Vector2(index / 10, index % 10);
  }

  public Vector4 GetDimensions() {
    return new Vector4(position.X, position.Y, position.X + size, position.Y + size);
  }

  public Vector2 GetHoveredField() {
    int x = BattleshipGame.Instance.mouseState.X;
    int y = BattleshipGame.Instance.mouseState.Y;
    for (int row = 0; row < 10; row++) {
      if (y < position.Y + row * FieldSize || y > position.Y + (row + 1) * FieldSize) {
        continue;
      }

      for (int col = 0; col < 10; col++) {
        if (x < position.X + col * FieldSize || x > position.X + (col + 1) * FieldSize) {
          continue;
        }

        return new Vector2(col, row);
      }
    }
    return new Vector2(-1, -1);
  }

  public void PlaceShip(Ship ship) {
    for (int i = 0; i < ships.Length; i++) {
      if (ships[i] != null) {
        continue;
      }

      ships[i] = ship;
      ShipPart[] locations = ship.GetParts();
      foreach (ShipPart location in locations) {
        int index = GetIndexFromLocationVector(location.location);
        fields[index].Part = location;
        fields[index].ShipID = i;
      }
      break;
    }
  }

  public bool AttackField(int index) {
    Field attackedField = fields[index];
    if (attackedField.Part == null) {
      attackedField.Attacked = true;
    } else if (attackedField.Part != null) {
      attackedField.Attacked = true;
      bool shipDied = ships[attackedField.ShipID].Hit();
      if (shipDied) {
        foreach (Vector2 location in ships[attackedField.ShipID].GetLocations()) {
          fields[GetIndexFromLocationVector(location)].Sunken = true;
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
    foreach (Ship ship in ships) {
      if (ship.Alive) {
        return true;
      }
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
    foreach (Field field in fields) {
      field.Render(batch);
    }
  }
}