using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public static class Input {
  private static bool isLeftMousePressed = false;
  private static Vector2 lastMousePosition = new Vector2(-1);

  public static Action<float, float> OnLeftMouseClicked;
  public static Action<float, float> OnMouseMoved;

  public static void Update(MouseState mouse) {
    if (!BattleshipGame.Instance.IsActive) {
      return;
    }

    if (mouse.LeftButton == ButtonState.Pressed && !isLeftMousePressed) {
      OnLeftMouseClicked?.Invoke(mouse.X, mouse.Y);
      isLeftMousePressed = true;
    } else if (mouse.LeftButton == ButtonState.Released && isLeftMousePressed) {
      isLeftMousePressed = false;
    }

    if (mouse.X != lastMousePosition.X || mouse.Y != lastMousePosition.Y) {
      OnMouseMoved?.Invoke(mouse.X, mouse.Y);
      lastMousePosition = new Vector2(mouse.X, mouse.Y);
    }
  }
}