using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Ship {
  private static Random random = new Random();
  private static Dictionary<int, int> centerIndexes = new Dictionary<int, int>() {
    {5, 2},
    {4, 2},
    {3, 1},
    {2, -1}
  };

  private int hitCount = 0;
  private bool placed = false;
  private bool isFirstFront;

  public int Size { get; private set; }
  public bool Alive => hitCount < Size;

  private ShipPart[] parts;

  public Ship(int size) {
    this.Size = size;
    parts = new ShipPart[size];
    isFirstFront = random.Next(100) > 50;
  }

  public ShipPart[] Place(Vector2 location, ShipOrientation orientation) {
    placed = true;
    Vector2 offset = new Vector2(1 - (int)orientation, (int)orientation);
    for (int i = 0; i < Size; i++) {
      parts[i] = new ShipPart {
        location = location + offset * i,
        texture = BattleshipGame.Instance.ShipBody,
        rotation = orientation == ShipOrientation.Horizontal ? isFirstFront ? -90f : 90f : isFirstFront ? 0f : 180f
      };
    }

    // set front and back sprite
    if (isFirstFront) {
      parts[0].texture = BattleshipGame.Instance.ShipFront;
      parts[parts.Length - 1].texture = BattleshipGame.Instance.ShipBack;
    } else {
      parts[parts.Length - 1].texture = BattleshipGame.Instance.ShipFront;
      parts[0].texture = BattleshipGame.Instance.ShipBack;
    }

    // set command center sprite
    if (centerIndexes[Size] != -1) {
      int index = isFirstFront ? centerIndexes[Size] : (Size - 1) - centerIndexes[Size];
      parts[index].texture = BattleshipGame.Instance.ShipCenter;
    }

    return parts;
  }

  public Vector2[] GetLocations() {
    if (!placed) {
      return new Vector2[0];
    }
    Vector2[] locations = new Vector2[parts.Length];
    int i = 0;
    parts.ToList().ForEach(part => {
      locations[i] = part.location;
      i++;
    });
    return locations;
  }

  public ShipPart[] GetParts() {
    if (!placed) {
      return new ShipPart[0];
    }
    return parts;
  }

  public bool Hit() {
    hitCount++;
    return !Alive;
  }
}

public enum ShipOrientation {
  Horizontal,
  Vertical
}

public class ShipPart {
  public Vector2 location;
  public Texture2D texture;
  public float rotation;
}
