using Microsoft.Xna.Framework;

namespace Battleships;

public enum ShipOrientation {
  Horizontal,
  Vertical
}

public class Ship {
  private int hitCount = 0;
  private bool _placed = false;

  public int Size { get; private set; }
  public bool Alive => hitCount != Size;

  private Vector2[] locations;

  public Ship(int size) {
    this.Size = size;
    locations = new Vector2[size];
  }

  public Vector2[] Place(Vector2 location, ShipOrientation orientation) {
    _placed = true;
    Vector2 offset = new Vector2(1 - (int)orientation, (int)orientation);
    for (int i = 0; i < Size; i++) {
      locations[i] = location + offset * i;
    }
    return locations;
  }

  public Vector2[] GetLocations() {
    if (!_placed)
      return new Vector2[0];
    return locations;
  }

  public bool Hit() {
    hitCount++;
    return !Alive;
  }
}