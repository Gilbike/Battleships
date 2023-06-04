using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class MatchHUD : UIScreen {
  private UILabel label;

  public MatchHUD() {
    label = new UILabel(new Vector2(10, 320), "");
    Elements.Add(label);
  }

  public void SetLabel(string text) {
    label.Text = text;
  }
}