using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Battleships.Resources;

namespace Battleships;

public static class EndScreen {
  private static Color backgroundColor = new Color(18, 18, 18, 190);
  private static Color restartButtonColor = new Color(8, 131, 149, 220);
  private static Color restartButtonColorHover = new Color(10, 77, 104, 220);
  private static Color quitButtonColor = new Color(237, 43, 42, 220);
  private static Color quitButtonColorHover = new Color(210, 19, 18, 220);

  private static Texture2D rectangle;
  private static SpriteFont font;

  private static Vector2 fullScreenScale;
  private static Vector2 screenCenter;

  private static string displayText;
  private static Vector2 displayTextLocation;

  private static Vector2 restartButtonPosition;
  private static Vector2 restartButtonSize;
  private static Vector2 exitButtonPosition;
  private static Vector2 exitButtonSize;

  private static Vector2 restartTextSize;
  private static Vector2 exitTextSize;

  public static Action OnRestartClick;
  public static Action OnQuitClick;

  public static void Initialize() {
    rectangle = new Texture2D(BattleshipGame.Instance.GraphicsDevice, 1, 1);
    rectangle.SetData(new Color[] { Color.White });

    fullScreenScale = new Vector2(BattleshipGame.Instance.GraphicsDevice.Viewport.Width, BattleshipGame.Instance.GraphicsDevice.Viewport.Height);
    screenCenter = new Vector2(fullScreenScale.X / 2, fullScreenScale.Y / 2);

    restartButtonPosition = new Vector2(screenCenter.X - 100, screenCenter.Y + 60);
    restartButtonSize = new Vector2(200, 40);

    exitButtonPosition = new Vector2(screenCenter.X - 100, screenCenter.Y + 110);
    exitButtonSize = new Vector2(200, 40);
  }

  public static void LoadContent() {
    font = ResourceManager.Font;
    restartTextSize = font.MeasureString("New Game");
    exitTextSize = font.MeasureString("Exit");
  }

  public static void SetText(string text) {
    displayText = text;
    Vector2 size = font.MeasureString(text);
    displayTextLocation = new Vector2(screenCenter.X - size.X / 2, screenCenter.Y - size.Y / 2);
  }

  private static bool isInRect(Vector2 location, Vector2 size) {
    MouseState state = BattleshipGame.Instance.mouseState;
    return (state.X >= location.X && state.X <= location.X + size.X && state.Y >= location.Y && state.Y <= location.Y + size.Y);
  }

  private static bool leftClicked = false;
  public static void Update() {
    MouseState state = BattleshipGame.Instance.mouseState;
    if (state.LeftButton == ButtonState.Pressed && !leftClicked) {
      if (isInRect(restartButtonPosition, restartButtonSize)) {
        OnRestartClick?.Invoke();
      } else if (isInRect(exitButtonPosition, exitButtonSize)) {
        OnQuitClick?.Invoke();
      }
      leftClicked = true;
    } else if (state.LeftButton == ButtonState.Released && leftClicked) {
      leftClicked = false;
    }
  }

  public static void Render() {
    BattleshipGame.Instance.Batch.Draw(rectangle, Vector2.Zero, null, backgroundColor, 0f, Vector2.Zero, fullScreenScale, SpriteEffects.None, 1f);
    BattleshipGame.Instance.Batch.DrawString(font, displayText, displayTextLocation, Color.White);

    {
      // Restart button
      Color restartDrawColor = restartButtonColor;
      if (isInRect(restartButtonPosition, restartButtonSize)) {
        restartDrawColor = restartButtonColorHover;
      }
      BattleshipGame.Instance.Batch.Draw(rectangle, restartButtonPosition, null, restartDrawColor, 0, Vector2.Zero, restartButtonSize, SpriteEffects.None, 0);
      BattleshipGame.Instance.Batch.DrawString(font, "New Game", new Vector2(restartButtonPosition.X + restartButtonSize.X / 2 - restartTextSize.X / 2, restartButtonPosition.Y + restartButtonSize.Y / 2 - restartTextSize.Y / 2), Color.White);
    }

    {
      // Exit button
      Color exitDrawColor = quitButtonColor;
      if (isInRect(exitButtonPosition, exitButtonSize)) {
        exitDrawColor = quitButtonColorHover;
      }
      BattleshipGame.Instance.Batch.Draw(rectangle, exitButtonPosition, null, exitDrawColor, 0, Vector2.Zero, exitButtonSize, SpriteEffects.None, 0);
      BattleshipGame.Instance.Batch.DrawString(font, "Exit", new Vector2(exitButtonPosition.X + exitButtonSize.X / 2 - exitTextSize.X / 2, exitButtonPosition.Y + exitButtonSize.Y / 2 - exitTextSize.Y / 2), Color.White);
    }
  }
}