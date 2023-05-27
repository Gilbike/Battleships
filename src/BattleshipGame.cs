using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Battleships;

public class BattleshipGame : Game {
  private static BattleshipGame _instance;

  public static BattleshipGame Instance {
    get {
      if (_instance == null) {
        _instance = new BattleshipGame();
      }
      return _instance;
    }
  }

  private GraphicsDeviceManager _graphics;
  private SpriteBatch _batch;

  public SpriteBatch Batch => _batch;
  public MouseState mouseState { get; private set; }
  public KeyboardState keyboardState { get; private set; }

  public SpriteFont UIFont { get; private set; }
  public Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();

  private Match currentMatch;

  private BattleshipGame() {
    _graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    _batch = new SpriteBatch(GraphicsDevice);
    Window.Title = "Battleships";
    EndScreen.Initialize();
    EndScreen.OnRestartClick += OnRestartClick;
    EndScreen.OnQuitClick += OnQuitClick;

    StartNewGame();

    base.Initialize();
  }

  private void StartNewGame() {
    currentMatch = new Match();
    currentMatch.OnMatchEnd += OnGameEnded;
  }

  private void OnGameEnded(string winner) {
    EndScreen.SetText($"{winner} wins!");
    currentMatch = null;
  }

  private void OnRestartClick() {
    StartNewGame();
  }

  private void OnQuitClick() {
    Exit();
  }

  protected override void Update(GameTime gameTime) {
    mouseState = Mouse.GetState();
    keyboardState = Keyboard.GetState();
    if (currentMatch != null) {
      currentMatch.Update();
    } else {
      EndScreen.Update();
    }
    base.Update(gameTime);
  }

  protected override void Draw(GameTime time) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _batch.Begin();
    if (currentMatch != null) {
      currentMatch.Render();
    } else {
      EndScreen.Render();
    }
    _batch.End();

    base.Draw(time);
  }

  protected override void LoadContent() {
    UIFont = Content.Load<SpriteFont>("Content/fonts/UI");
    SoundEffects.Add("deploy", Content.Load<SoundEffect>("Content/sounds/deploy"));
    SoundEffects.Add("fire", Content.Load<SoundEffect>("Content/sounds/fire"));
    EndScreen.LoadContent();
    base.LoadContent();
  }
}