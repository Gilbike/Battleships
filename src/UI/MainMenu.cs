using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.UI;

public class MainMenu : UIScreen {
  private UIButton startButton;
  private UIButton exitButton;

  public Action StartRequested;
  public Action ExitRequested;

  public MainMenu() {
    Vector2 buttonSize = new Vector2(200, 35);
    Vector2 gameTitleSize = Resources.ResourceManager.Font.MeasureString("Battleships");

    UILabel title = new UILabel(UIManager.ScreenCenter - new Vector2(gameTitleSize.X / 2, gameTitleSize.Y / 2 + 70), "Battleships");
    startButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 10), buttonSize, "Start Game");
    exitButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 50), buttonSize, "Exit");

    startButton.OnButtonPressed += delegate () { StartRequested?.Invoke(); };
    exitButton.OnButtonPressed += delegate () { ExitRequested?.Invoke(); };

    Vector2 screenSize = new Vector2(UIManager.ScreenCenter.X * 2, UIManager.ScreenCenter.Y * 2);
    RenderTarget2D background = new RenderTarget2D(BattleshipGame.Instance.GraphicsDevice, (int)screenSize.X, (int)screenSize.Y);
    int widthFit = (int)screenSize.X / 16;
    int heightFit = (int)screenSize.Y / 16;
    BattleshipGame.Instance.GraphicsDevice.SetRenderTarget(background);
    BattleshipGame.Instance.Batch.Begin();
    for (int row = 0; row < heightFit; row++) {
      for (int col = 0; col < widthFit; col++) {
        BattleshipGame.Instance.Batch.Draw(Resources.ResourceManager.GetRandomOceanTile(), new Vector2(col * 16, row * 16), Color.White);
      }
    }
    BattleshipGame.Instance.Batch.End();
    BattleshipGame.Instance.GraphicsDevice.SetRenderTarget(null);

    Elements.Add(new UIImage(Vector2.Zero, screenSize, background));

    Elements.Add(title);
    Elements.Add(startButton);
    Elements.Add(exitButton);
  }
}