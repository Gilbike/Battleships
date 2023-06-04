using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public static class Input {
  private static bool isLeftMousePressed = false;
  private static Vector2 lastMousePosition = new Vector2(-1);
  private static int lastScrollPosition = 0;

  public static Action<float, float> OnLeftMouseClicked;
  public static Action<float, float> OnMouseMoved;
  public static Action<ScrollDirection> OnMouseScrolled;
  public static Action<Keys> OnKeyPressed;

  public static Vector2 MouseLocation => lastMousePosition;

  public static void Init() {
    BattleshipGame.Instance.Window.KeyDown += delegate (object sender, InputKeyEventArgs args) {
      OnKeyPressed?.Invoke(args.Key);
    };
  }

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

    if (lastScrollPosition > mouse.ScrollWheelValue) {
      OnMouseScrolled?.Invoke(ScrollDirection.Down);
      lastScrollPosition = mouse.ScrollWheelValue;
    } else if (lastScrollPosition < mouse.ScrollWheelValue) {
      OnMouseScrolled?.Invoke(ScrollDirection.Up);
      lastScrollPosition = mouse.ScrollWheelValue;
    }
  }
}

public enum ScrollDirection {
  Up,
  Down
}