using System;
using Microsoft.Xna.Framework;

namespace Battleships.UI;

public class SettingsMenu : UIScreen {
  private static int totalHeight = 35 + 3 + 35 + 2 + 30;

  private UIButton backButton;
  private UIToggle soundToggle;

  public Action BackRequested;

  public SettingsMenu() {
    Vector2 settingsTitleSize = Resources.ResourceManager.Font.MeasureString("Settings");
    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(settingsTitleSize.X / 2, settingsTitleSize.Y / 2 + totalHeight / 2), "Settings"));
    Elements.Add(new UILabel(UIManager.ScreenCenter - new Vector2(150, settingsTitleSize.Y / 2 + (totalHeight / 2 - 38)), "Sounds"));

    backButton = new UIButton(UIManager.ScreenCenter - new Vector2(100, settingsTitleSize.Y / 2 - (totalHeight / 2) + 30), new Vector2(200, 30), "Back");
    backButton.OnButtonPressed += delegate () { BackRequested?.Invoke(); };

    soundToggle = new UIToggle(UIManager.ScreenCenter - new Vector2(-115, settingsTitleSize.Y / 2 - (totalHeight / 2) + 67), new Vector2(35, 35));
    soundToggle.Enabled = Settings.SettingsManager.EnableSounds;
    soundToggle.OnToggle += delegate () {
      Settings.SettingsManager.EnableSounds = soundToggle.Enabled;
    };

    Elements.Add(backButton);
    Elements.Add(soundToggle);
  }
}