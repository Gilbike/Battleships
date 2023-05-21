using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class ShipPlacer {
  private readonly int[] SHIPS = new int[] { 5, 4, 3, 3, 2 };

  private Grid _grid;

  private int[] unplacedShips;
  private Ship currentShip;

  public ShipPlacer(Grid grid) {
    _grid = grid;
  }

  public void StartPlayerPlacement() {
    unplacedShips = SHIPS;
    currentShip = new Ship(unplacedShips[0]);
  }

  public void Update() {
    MouseState state = BattleshipGame.Instance.mouseState;
    Vector4 dimensions = _grid.GetDimensions();
    if (state.X < dimensions.X || state.X > dimensions.Z || state.Y < dimensions.Y || state.Y > dimensions.W)
      return;

    Vector2 baseLocation = _grid.GetHoveredField(state.X, state.Y);
    Vector2[] shipLocations = currentShip.Place(baseLocation, ShipOrientation.Horizontal);
    foreach (Vector2 location in shipLocations) {
      if (location.X > 9 || location.Y > 9) continue;
      _grid.GetField(_grid.GetIndexFromLocationVector(location)).State = FieldState.Ship;
    }
  }
}