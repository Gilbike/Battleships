using Microsoft.Xna.Framework;

namespace Battleships.UI;

public static class UIManager {
  public static UIScreen Screen { get; set; }
  public static readonly Vector2 ScreenCenter = new Vector2(BattleshipGame.Instance.GraphicsDevice.Viewport.Width / 2, BattleshipGame.Instance.GraphicsDevice.Viewport.Height / 2);

  public static void Render() {
    if (Screen != null) {
      Screen.Render();
    }
  }
}