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
        partType = ShipPieceType.Default,
        texture = BattleshipGame.Instance.ShipBody
      };
    }
    if (isFirstFront) {
      parts[0].partType = ShipPieceType.Front;
      parts[0].texture = BattleshipGame.Instance.ShipFront;
    } else {
      parts[parts.Length - 1].partType = ShipPieceType.Front;
      parts[parts.Length - 1].texture = BattleshipGame.Instance.ShipFront;
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
  public ShipPieceType partType;
  public Texture2D texture;
}

public enum ShipPieceType {
  Default,
  Front,
  Back,
  CommandCenter
}