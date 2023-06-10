using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battleships.Resources;

namespace Battleships.UI;

public class MainMenu : UIScreen {
  private UIButton startButton;
  private UIButton multiplayerButton;
  private UIButton settingsButton;
  private UIButton exitButton;

  public Action StartRequested;
  public Action SettingsRequested;
  public Action ExitRequested;

  public MainMenu() {
    Vector2 buttonSize = new Vector2(200, 35);
    Vector2 gameTitleSize = Resources.ResourceManager.Font.MeasureString("Battleships");

    UILabel title = new UILabel(UIManager.ScreenCenter - new Vector2(gameTitleSize.X / 2, gameTitleSize.Y / 2 + 70), "Battleships");
    startButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 10), buttonSize, "Singleplayer");
    multiplayerButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 50), buttonSize, "Multiplayer");
    settingsButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 90), buttonSize, "Settings");
    exitButton = new UIButton(UIManager.ScreenCenter - new Vector2(buttonSize.X / 2, buttonSize.Y / 2 - 130), buttonSize, "Exit");

    startButton.OnButtonPressed += delegate () { StartRequested?.Invoke(); };
    settingsButton.OnButtonPressed += delegate () { SettingsRequested?.Invoke(); };
    exitButton.OnButtonPressed += delegate () { ExitRequested?.Invoke(); };

    Vector2 screenSize = new Vector2(UIManager.ScreenCenter.X * 2, UIManager.ScreenCenter.Y * 2);
    RenderTarget2D background = new RenderTarget2D(BattleshipGame.Instance.GraphicsDevice, (int)screenSize.X, (int)screenSize.Y);
    int widthFit = (int)screenSize.X / 16;
    int heightFit = (int)screenSize.Y / 16;

    int shipCount = BattleshipGame.random.Next(8, 26);
    BattleshipGame.Instance.GraphicsDevice.SetRenderTarget(background);
    BattleshipGame.Instance.Batch.Begin();
    for (int row = 0; row < heightFit; row++) {
      for (int col = 0; col < widthFit; col++) {
        BattleshipGame.Instance.Batch.Draw(ResourceManager.GetRandomOceanTile(), new Vector2(col * 16, row * 16), Color.White);
      }
    }
    for (int i = 0; i < shipCount; i++) {
      int shipSize = BattleshipGame.random.Next(2, 6);
      ShipOrientation orientation = (ShipOrientation)BattleshipGame.random.Next(2);
      Vector2 basePosition = new Vector2(BattleshipGame.random.Next(widthFit), BattleshipGame.random.Next(heightFit));
      bool startWithBack = BattleshipGame.random.Next(100) > 50;

      Texture2D firstSprite = startWithBack ? ResourceManager.ShipBack : ResourceManager.ShipFront;
      Texture2D lastSprite = startWithBack ? ResourceManager.ShipFront : ResourceManager.ShipBack;

      float rotation = orientation == ShipOrientation.Horizontal ? startWithBack ? 90f : -90f : startWithBack ? 180f : 0;
      BattleshipGame.Instance.Batch.Draw(firstSprite, new Vector2(basePosition.X * 16, basePosition.Y * 16), null, Color.White, MathHelper.ToRadians(rotation), Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
      for (int offset = 1; offset < shipSize; offset++) {
        int commandCenterIndex = !startWithBack ? Ship.centerIndexes[shipSize] : (shipSize - 1) - Ship.centerIndexes[shipSize];
        Vector2 offsetVector = new Vector2(offset * (1 - (int)orientation) * 16, offset * (int)orientation * 16);
        Texture2D sprite = offset + 1 == shipSize ? lastSprite : offset == commandCenterIndex ? ResourceManager.ShipCenter : ResourceManager.ShipBody;
        BattleshipGame.Instance.Batch.Draw(sprite, new Vector2(basePosition.X * 16, basePosition.Y * 16) + offsetVector, null, Color.White, MathHelper.ToRadians(rotation), Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
      }
    }
    BattleshipGame.Instance.Batch.End();
    BattleshipGame.Instance.GraphicsDevice.SetRenderTarget(null);

    Elements.Add(new UIImage(Vector2.Zero, screenSize, background));

    Elements.Add(title);
    Elements.Add(startButton);
    Elements.Add(multiplayerButton);
    Elements.Add(settingsButton);
    Elements.Add(exitButton);
  }
}