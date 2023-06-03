using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public enum ShipOrientation {
  Horizontal,
  Vertical
}

public class Ship {
  private static Random random = new Random();
  private int hitCount = 0;
  private bool _placed = false;
  private bool isFirstFront;

  public int Size { get; private set; }
  public bool Alive => hitCount != Size;

  private ShipPart[] parts;

  public Ship(int size) {
    this.Size = size;
    parts = new ShipPart[size];
    isFirstFront = random.Next(100) > 50;
  }

  public ShipPart[] Place(Vector2 location, ShipOrientation orientation) {
    _placed = true;
    Vector2 offset = new Vector2(1 - (int)orientation, (int)orientation);
    for (int i = 0; i < Size; i++) {
      parts[i] = new ShipPart {
        location = location + offset * i,
        texture = BattleshipGame.Instance.ShipBody,
        rotation = orientation == ShipOrientation.Horizontal ? 90f : 0f
      };
    }
    if (isFirstFront) {
      parts[0].texture = BattleshipGame.Instance.ShipFront;
      parts[0].rotation *= -1;
      parts[parts.Length - 1].texture = BattleshipGame.Instance.ShipBack;
      parts[parts.Length - 1].rotation *= -1;
    } else {
      parts[parts.Length - 1].texture = BattleshipGame.Instance.ShipFront;
      parts[0].texture = BattleshipGame.Instance.ShipBack;
      if (orientation == ShipOrientation.Vertical) {
        parts[parts.Length - 1].rotation = 180f;
        parts[0].rotation = 180f;
      }
    }
    return parts;
  }

  public Vector2[] GetLocations() {
    if (!_placed)
      return new Vector2[0];
    Vector2[] locations = new Vector2[parts.Length];
    int i = 0;
    parts.ToList().ForEach(part => {
      locations[i] = part.location;
      i++;
    });
    return locations;
  }

  public bool Hit() {
    hitCount++;
    return !Alive;
  }
}

public class ShipPart {
  public Vector2 location;
  public Texture2D texture;
  public float rotation;
}

public enum ShipPieceType {
  Default,
  Front,
  Back,
  CommandCenter
}