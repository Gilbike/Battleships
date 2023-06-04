namespace Battleships.UI;

public static class UIManager {
  public static UIScreen Screen { get; set; }

  public static void Render() {
    if (Screen != null) {
      Screen.Render();
    }
  }
}