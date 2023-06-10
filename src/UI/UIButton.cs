using System;
using Battleships.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.UI;

public class UIButton : UIElement {
  private readonly Color foreground = new Color(51, 51, 51);
  private readonly Color foregroundHover = new Color(112, 105, 93);

  private string Text { get; set; }
  private Vector2 size;

  private Vector2 labelPosition;

  private bool hovered = false;

  public Action OnButtonPressed;

  public UIButton(Vector2 position, Vector2 size, string text) : base(position) {
    Text = text;
    this.size = size;
    Vector2 textDimensions = ResourceManager.Font.MeasureString(text);
    labelPosition = new Vector2(position.X + size.X / 2 - textDimensions.X / 2 + 2, position.Y + size.Y / 2 - textDimensions.Y / 2 + 3);
    Input.OnMouseMoved += CheckForHover;
    Input.OnLeftMouseClicked += CheckForClick;
  }

  private void CheckForHover(float x, float y) {
    hovered = x >= position.X && y >= position.Y && x <= position.X + size.X && y <= position.Y + size.Y;
  }

  private void CheckForClick(float x, float y) {
    if (hovered && Active) {
      OnButtonPressed?.Invoke();
    }
  }

  public override void Render() {
    var batch = BattleshipGame.Instance.Batch;
    batch.Draw(ResourceManager.UISurface, position, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
    Color foregroundColor = !hovered ? foreground : foregroundHover;
    batch.Draw(ResourceManager.UISurface, position + new Vector2(2), null, foregroundColor, 0f, Vector2.Zero, size - new Vector2(4), SpriteEffects.None, 0f);
    batch.DrawString(ResourceManager.Font, Text, labelPosition, Color.White);
  }
}