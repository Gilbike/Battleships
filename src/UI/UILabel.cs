using Battleships.Resources;
using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class UILabel : UIElement {
  public string Text { get; set; }

  public UILabel(Vector2 position, string text) : base(position) {
    Text = text;
  }

  public override void Render() {
    BattleshipGame.Instance.Batch.DrawString(ResourceManager.Font, Text, position, Color.White);
  }
}