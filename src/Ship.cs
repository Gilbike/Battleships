using Microsoft.Xna.Framework;

namespace Battleships;

public enum ShipOrientation {
  Horizontal,
  Vertical
}

public class Ship {
  private int hitCount = 0;

  public int Size { get; private set; }
  public bool Alive => hitCount == Size;

  private Vector2[] locations;

  public Ship(int size) {
    this.Size = size;
    locations = new Vector2[size];
  }

  public Vector2[] Place(Vector2 location, ShipOrientation orientation) {
    Vector2 offset = new Vector2(1 - (int)orientation, (int)orientation);
    locations[0] = location;
    locations[1] = location + offset;
    locations[2] = location + offset * 2;
    return locations;
  }
}