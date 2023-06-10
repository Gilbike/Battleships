using Microsoft.Xna.Framework;

namespace Battleships.UI;

public static class UIManager {
  private static UIScreen _screen;
  public static UIScreen Screen {
    get { return _screen; }
    set {
      if (_screen != null) {
        _screen.Disable();
      }
      _screen = value;
      _screen.Enable();
    }
  }
  public static readonly Vector2 ScreenCenter = new Vector2(BattleshipGame.Instance.GraphicsDevice.Viewport.Width / 2, BattleshipGame.Instance.GraphicsDevice.Viewport.Height / 2);

  public static void Render() {
    if (Screen != null) {
      Screen.Render();
    }
  }
}