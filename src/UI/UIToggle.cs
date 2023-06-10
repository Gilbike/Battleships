using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battleships.Resources;

namespace Battleships.UI;

public class UIToggle : UIElement {
  private readonly Color foreground = new Color(51, 51, 51);
  private readonly Color foregroundHover = new Color(112, 105, 93);

  private Vector2 size;

  public bool Enabled { get; set; }

  public Action OnToggle;

  public UIToggle(Vector2 position, Vector2 size) : base(position) {
    this.size = size;
    Enabled = false;
    Input.OnLeftMouseClicked += OnClick;
  }

  private void OnClick(float x, float y) {
    if (x >= position.X && y >= position.Y && x <= position.X + size.X && y <= position.Y + size.Y && Active) {
      Enabled = !Enabled;
      OnToggle?.Invoke();
    }
  }

  public override void Render() {
    var batch = BattleshipGame.Instance.Batch;
    batch.Draw(ResourceManager.UISurface, position, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
    Color foregroundColor = !Enabled ? foreground : foregroundHover;
    batch.Draw(ResourceManager.UISurface, position + new Vector2(2), null, foregroundColor, 0f, Vector2.Zero, size - new Vector2(4), SpriteEffects.None, 0f);
  }
}