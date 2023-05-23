using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class ShipPlacer {
  private readonly List<int> SHIPS = new List<int>() { 5, 4, 3, 3, 2 };

  private Grid _grid;

  // for player placement
  private List<int> unplacedShips;
  private int unplacedShipIndex = 0;
  private Ship currentShip;
  private List<Field> lastFields = new List<Field>();
  private ShipOrientation orientation = ShipOrientation.Horizontal;

  public Action onPlacementDone;

  // for ai placement
  private bool leftSideBig = false;
  private bool rightSideBig = false;

  public ShipPlacer(Grid grid) {
    _grid = grid;
  }

  public void StartPlayerPlacement() {
    unplacedShips = SHIPS;
    currentShip = new Ship(unplacedShips[unplacedShipIndex]);
  }

  public void PlaceRandom() {
    Random random = new Random();
    foreach (int size in SHIPS.OrderBy(x => random.Next()).ToList()) {
      Vector2[] proposedLocations;
      Ship ship;
      do {
        int startField = random.Next(100);
        Vector2 placeLocation = _grid.GetLocationVectorFromIndex(startField);

        if (size > 3) {
          bool tryLeftSide = random.Next(100) > 50 ? true : false;
          if (tryLeftSide && !leftSideBig) {
            System.Console.WriteLine("left " + size);
            leftSideBig = true;
            placeLocation.X = random.Next(5, 10);
          } else {
            if (!rightSideBig) {
              System.Console.WriteLine("right " + size);
              rightSideBig = true;
              placeLocation.X = random.Next(0, 5);
            } else {
              System.Console.WriteLine("left " + size);
              leftSideBig = true;
              placeLocation.X = random.Next(5, 10);
            }
          }
        }

        ShipOrientation shipOrientation = (ShipOrientation)random.Next(2);
        ship = new Ship(size);
        proposedLocations = ship.Place(placeLocation, shipOrientation);
      } while (!isPlacementValid(proposedLocations));
      _grid.PlaceShip(ship);
    }
  }

  public bool isPlacementValid(Vector2[] locations) {
    foreach (Vector2 location in locations) {
      if (location.X > 9 || location.Y > 9) // outside of grid
        return false;
      Field field = _grid.GetField(_grid.GetIndexFromLocationVector(location));
      if (field.State == FieldState.Ship)
        return false;
    }
    return true;
  }

  private bool isRPressed = false;
  private bool isLeftMousePressed = false;
  private int lastWheelValue = 0;
  public void Update() {
    if (currentShip == null)
      return;
    MouseState mState = BattleshipGame.Instance.mouseState;
    KeyboardState kState = BattleshipGame.Instance.keyboardState;
    Vector4 dimensions = _grid.GetDimensions();
    if (mState.X < dimensions.X || mState.X > dimensions.Z || mState.Y < dimensions.Y || mState.Y > dimensions.W)
      return;

    bool isPlacementValid = true;

    // Change held ship size
    if (lastWheelValue > mState.ScrollWheelValue && unplacedShipIndex + 1 < unplacedShips.Count) {
      unplacedShipIndex += 1;
      foreach (Field field in lastFields)
        field.State = FieldState.Empty;
      currentShip = new Ship(unplacedShips[unplacedShipIndex]);
    } else if (lastWheelValue < mState.ScrollWheelValue && unplacedShipIndex > 0) {
      unplacedShipIndex -= 1;
      foreach (Field field in lastFields)
        field.State = FieldState.Empty;
      currentShip = new Ship(unplacedShips[unplacedShipIndex]);
    }
    lastWheelValue = mState.ScrollWheelValue;

    // Rotate held ship
    if (kState.IsKeyDown(Keys.R) && !isRPressed) {
      orientation = (ShipOrientation)1 - (int)orientation;
      isRPressed = true;
    } else if (kState.IsKeyUp(Keys.R) && isRPressed) {
      isRPressed = false;
    }

    // Set held ship location
    foreach (Field field in lastFields)
      field.State = FieldState.Empty;
    lastFields.Clear();

    Vector2 baseLocation = _grid.GetHoveredField();
    if (baseLocation == new Vector2(-1, -1)) return;
    Vector2[] shipLocations = currentShip.Place(baseLocation, orientation);
    foreach (Vector2 location in shipLocations) {
      if (location.X > 9 || location.Y > 9) { // outside of grid
        isPlacementValid = false;
        continue;
      }
      Field field = _grid.GetField(_grid.GetIndexFromLocationVector(location));
      if (field.State == FieldState.Ship) {
        isPlacementValid = false;
      } else {
        field.State = FieldState.Ship;
        lastFields.Add(field);
      }
    }

    // Place held ship
    if (mState.LeftButton == ButtonState.Pressed && !isLeftMousePressed) {
      isLeftMousePressed = true;
      if (!isPlacementValid) return;
      unplacedShips.RemoveAt(unplacedShipIndex);
      if (unplacedShips.Count == 0) {
        onPlacementDone?.Invoke();
        currentShip = null;
        return;
      }
      _grid.PlaceShip(currentShip);
      unplacedShipIndex = 0;
      currentShip = new Ship(unplacedShips[unplacedShipIndex]);
      lastFields.Clear();
    } else if (mState.LeftButton == ButtonState.Released && isLeftMousePressed)
      isLeftMousePressed = false;
  }
}