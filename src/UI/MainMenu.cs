using System;
using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class MainMenu : UIScreen {
  private UIButton startButton;
  private UIButton exitButton;

  public Action StartRequested;
  public Action ExitRequested;

  public MainMenu() {
    Vector2 buttonSize = new Vector2(200, 35);
    Vector2 gameTitleSize = Resources.ResourceManager.Font.MeasureString("Battleships");

    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(gameTitleSize.X / 2, gameTitleSize.Y / 2 + 70), "Battleships"));
    startButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 10), buttonSize, "Start Game");
    exitButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 50), buttonSize, "Exit");

    Elements.Add(startButton);
    Elements.Add(exitButton);

    startButton.OnButtonPressed += delegate () { StartRequested?.Invoke(); };
    exitButton.OnButtonPressed += delegate () { ExitRequested?.Invoke(); };
  }
}