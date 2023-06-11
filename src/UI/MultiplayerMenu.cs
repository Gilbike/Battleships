using System;
using Battleships.Multiplayer.ClientSide;
using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class MultiplayerMenu : UIScreen {
  private static int totalHeight = 35 + 2 + 35 + 30 + 2 + 35 + 30 + 2 + 30 + 2 + 30;

  private UIButton backButton;
  private UIButton startButton;
  private UIEditbox ipInput;
  private UIEditbox nameInput;

  public Action BackRequested;

  public MultiplayerMenu() {
    Vector2 settingsTitleSize = Resources.ResourceManager.Font.MeasureString("Multiplayer");
    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(settingsTitleSize.X / 2, settingsTitleSize.Y / 2 + totalHeight / 2), "Multiplayer"));

    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(150, settingsTitleSize.Y / 2 + totalHeight / 2) + new Vector2(0, 37), "Server IP Address"));
    ipInput = new UIEditbox(UIManager.ScreenCenter - new Vector2(150, settingsTitleSize.Y / 2 + totalHeight / 2) + new Vector2(0, 68), new Vector2(300, 30));
    ipInput.Text = "127.0.0.1";

    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(150, settingsTitleSize.Y / 2 + totalHeight / 2) + new Vector2(0, 104), "Your name"));
    nameInput = new UIEditbox(UIManager.ScreenCenter - new Vector2(150, settingsTitleSize.Y / 2 + totalHeight / 2) + new Vector2(0, 132), new Vector2(300, 30));

    startButton = new UIButton(UIManager.ScreenCenter - new Vector2(100, settingsTitleSize.Y / 2 - (totalHeight / 2) + 62), new Vector2(200, 30), "Connect");
    startButton.OnButtonPressed += delegate () { MultiplayerManager.client = new Client(nameInput.Text); };

    backButton = new UIButton(UIManager.ScreenCenter - new Vector2(100, settingsTitleSize.Y / 2 - (totalHeight / 2) + 30), new Vector2(200, 30), "Back");
    backButton.OnButtonPressed += delegate () { BackRequested?.Invoke(); };

    Elements.Add(ipInput);
    Elements.Add(nameInput);
    Elements.Add(startButton);
    Elements.Add(backButton);
  }
}