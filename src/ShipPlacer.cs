using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Battleships.Resources;

namespace Battleships;

public class ShipPlacer {
  private readonly List<int> SHIPS = new List<int>() { 5, 4, 3, 3, 2 };

  private Grid grid;

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
    this.grid = grid;
  }

  public void StartPlayerPlacement() {
    unplacedShips = SHIPS;
    currentShip = new Ship(unplacedShips[unplacedShipIndex]);
  }

  public void PlaceRandom() {
    Random random = new Random();
    foreach (int size in SHIPS.OrderBy(x => random.Next()).ToList()) {
      List<Vector2> proposedLocations = new List<Vector2>();
      Ship ship;
      do {
        proposedLocations.Clear();
        int startField = random.Next(100);
        Vector2 placeLocation = grid.GetLocationVectorFromIndex(startField);

        if (size > 3) {
          bool tryLeftSide = random.Next(100) > 50 ? true : false;
          if (tryLeftSide && !leftSideBig) {
            leftSideBig = true;
            placeLocation.X = random.Next(5, 10);
          } else {
            if (!rightSideBig) {
              rightSideBig = true;
              placeLocation.X = random.Next(0, 5);
            } else {
              leftSideBig = true;
              placeLocation.X = random.Next(5, 10);
            }
          }
        }

        ShipOrientation shipOrientation = (ShipOrientation)random.Next(2);
        ship = new Ship(size);
        ship.Place(placeLocation, shipOrientation).ToList().ForEach(part => proposedLocations.Add(part.location));
      } while (!isPlacementValid(proposedLocations.ToArray()));
      grid.PlaceShip(ship);
    }
  }

  public bool isPlacementValid(Vector2[] locations) {
    foreach (Vector2 location in locations) {
      if (location.X > 9 || location.Y > 9) { // outside of grid 
        return false;
      }
      Field field = grid.GetField(grid.GetIndexFromLocationVector(location));
      if (field.Part != null) {
        return false;
      }
    }
    return true;
  }

  private bool isRPressed = false;
  private bool isLeftMousePressed = false;
  private int lastWheelValue = 0;
  public void Update() {
    if (currentShip == null) {
      return;
    }
    MouseState mState = BattleshipGame.Instance.mouseState;
    KeyboardState kState = BattleshipGame.Instance.keyboardState;
    Vector4 dimensions = grid.GetDimensions();
    if (mState.X < dimensions.X || mState.X > dimensions.Z || mState.Y < dimensions.Y || mState.Y > dimensions.W) {
      return;
    }

    bool isPlacementValid = true;

    // Change held ship size
    if (lastWheelValue > mState.ScrollWheelValue && unplacedShipIndex + 1 < unplacedShips.Count) {
      unplacedShipIndex += 1;
      foreach (Field field in lastFields) {
        field.Part = null;
      }
      currentShip = new Ship(unplacedShips[unplacedShipIndex]);
    } else if (lastWheelValue < mState.ScrollWheelValue && unplacedShipIndex > 0) {
      unplacedShipIndex -= 1;
      foreach (Field field in lastFields) {
        field.Part = null;
      }
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
    foreach (Field field in lastFields) {
      field.Part = null;
    }
    lastFields.Clear();

    Vector2 baseLocation = grid.GetHoveredField();
    if (baseLocation == new Vector2(-1, -1)) {
      return;
    }
    ShipPart[] shipLocations = currentShip.Place(baseLocation, orientation);
    foreach (ShipPart part in shipLocations) {
      if (part.location.X > 9 || part.location.Y > 9) { // outside of grid
        isPlacementValid = false;
        continue;
      }
      Field field = grid.GetField(grid.GetIndexFromLocationVector(part.location));
      if (field.Part != null) {
        isPlacementValid = false;
      } else {
        field.Part = part;
        lastFields.Add(field);
      }
    }

    // Place held ship
    if (mState.LeftButton == ButtonState.Pressed && !isLeftMousePressed) {
      isLeftMousePressed = true;
      if (!isPlacementValid) {
        return;
      }
      unplacedShips.RemoveAt(unplacedShipIndex);
      if (unplacedShips.Count == 0) {
        onPlacementDone?.Invoke();
        currentShip = null;
        return;
      }
      ResourceManager.SoundEffects["deploy"].Play();
      grid.PlaceShip(currentShip);
      unplacedShipIndex = 0;
      currentShip = new Ship(unplacedShips[unplacedShipIndex]);
      lastFields.Clear();
    } else if (mState.LeftButton == ButtonState.Released && isLeftMousePressed) {
      isLeftMousePressed = false;
    }
  }
}