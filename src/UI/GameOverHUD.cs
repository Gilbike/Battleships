using System;
using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class GameOverHUD : UIScreen {
  private UILabel label;
  private UIButton restartButton;
  private UIButton exitButton;

  public Action NewGameRequested;
  public Action ExitRequested;

  public GameOverHUD(string winner) {
    Vector2 buttonSize = new Vector2(200, 35);
    Vector2 gameOverSize = Resources.ResourceManager.Font.MeasureString("GameOver");
    Vector2 winnerSize = Resources.ResourceManager.Font.MeasureString($"{winner} wins!");

    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(gameOverSize.X / 2, gameOverSize.Y / 2 + 70), "GameOver"));
    label = new UILabel(UIManager.ScreenCenter - new Vector2(winnerSize.X / 2, winnerSize.Y / 2 + 35), $"{winner} wins!");
    restartButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 10), buttonSize, "New Game");
    exitButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 50), buttonSize, "Exit");

    Elements.Add(label);
    Elements.Add(restartButton);
    Elements.Add(exitButton);

    restartButton.OnButtonPressed += delegate () { NewGameRequested?.Invoke(); };
    exitButton.OnButtonPressed += delegate () { ExitRequested?.Invoke(); };
  }
}