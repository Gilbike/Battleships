using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class ShipPlacer {
  private readonly int[] SHIPS = new int[] { 5, 4, 3, 3, 2 };

  private Grid _grid;

  private int[] unplacedShips;
  private Ship currentShip;
  private List<Field> lastFields = new List<Field>();
  private ShipOrientation orientation = ShipOrientation.Horizontal;

  public ShipPlacer(Grid grid) {
    _grid = grid;
  }

  public void StartPlayerPlacement() {
    unplacedShips = SHIPS;
    currentShip = new Ship(unplacedShips[0]);
  }

  private bool isRPressed = false;
  public void Update() {
    MouseState mState = BattleshipGame.Instance.mouseState;
    KeyboardState kState = BattleshipGame.Instance.keyboardState;
    Vector4 dimensions = _grid.GetDimensions();
    if (mState.X < dimensions.X || mState.X > dimensions.Z || mState.Y < dimensions.Y || mState.Y > dimensions.W)
      return;

    if (kState.IsKeyDown(Keys.R) && !isRPressed) {
      orientation = (ShipOrientation)1 - (int)orientation;
      isRPressed = true;
    } else if (kState.IsKeyUp(Keys.R) && isRPressed) {
      isRPressed = false;
    }

    foreach (Field field in lastFields)
      field.State = FieldState.Empty;

    Vector2 baseLocation = _grid.GetHoveredField(mState.X, mState.Y);
    if (baseLocation == new Vector2(-1, -1)) return;
    Vector2[] shipLocations = currentShip.Place(baseLocation, orientation);
    foreach (Vector2 location in shipLocations) {
      if (location.X > 9 || location.Y > 9) continue;
      Field field = _grid.GetField(_grid.GetIndexFromLocationVector(location));
      field.State = FieldState.Ship;
      lastFields.Add(field);
    }
  }
}