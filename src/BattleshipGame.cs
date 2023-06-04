using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Battleships.Resources;
using Battleships.UI;
using System;

namespace Battleships;

public class BattleshipGame : Game {
  public static Random random = new Random();
  private static BattleshipGame _instance;
  private static Color bgColor = new Color(60, 72, 107);

  public static BattleshipGame Instance {
    get {
      if (_instance == null) {
        _instance = new BattleshipGame();
      }
      return _instance;
    }
  }

  private GraphicsDeviceManager graphics;
  private SpriteBatch batch;

  public SpriteBatch Batch => batch;

  private Match currentMatch;

  private GameOverHUD gameOverHUD;

  private BattleshipGame() {
    graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    Input.Init();

    ResourceManager.Create(Content);

    batch = new SpriteBatch(GraphicsDevice);
    Window.Title = "Battleships";

    StartNewGame();

    base.Initialize();
  }

  private void StartNewGame() {
    currentMatch = new Match();
    currentMatch.OnMatchEnd += OnGameEnded;
    gameOverHUD = null;
  }

  private void OnGameEnded(string winner) {
    gameOverHUD = new GameOverHUD(winner);
    gameOverHUD.NewGameRequested += StartNewGame;
    gameOverHUD.ExitRequested += Exit;
    UIManager.Screen = gameOverHUD;
    currentMatch = null;
  }

  protected override void Update(GameTime gameTime) {
    Input.Update(Mouse.GetState());
    base.Update(gameTime);
  }

  protected override void Draw(GameTime time) {
    GraphicsDevice.Clear(bgColor);

    batch.Begin(samplerState: SamplerState.PointClamp);
    if (currentMatch != null) {
      currentMatch.Render();
    }
    UIManager.Render();
    batch.End();

    base.Draw(time);
  }
}