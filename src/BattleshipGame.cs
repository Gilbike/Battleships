using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Battleships;

public class BattleshipGame : Game {
  private static BattleshipGame _instance;

  private Color bgColor = new Color(60, 72, 107);
  private Random random = new Random();

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
  private List<Texture2D> oceanTiles = new List<Texture2D>();
  public Texture2D ShipBody;
  public Texture2D ShipFront;
  public Texture2D ShipBack;
  public Texture2D ShipCenter;
  public Texture2D MissedAttack;
  public Texture2D Fire;
  public Texture2D Sunken;

  private Match currentMatch;

  private BattleshipGame() {
    _graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    LoadResources();

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
    GraphicsDevice.Clear(bgColor);

    _batch.Begin(samplerState: SamplerState.PointClamp);
    if (currentMatch != null) {
      currentMatch.Render();
    } else {
      EndScreen.Render();
    }
    _batch.End();

    base.Draw(time);
  }

  private void LoadResources() {
    UIFont = Content.Load<SpriteFont>("Content/fonts/UI");
    SoundEffects.Add("deploy", Content.Load<SoundEffect>("Content/sounds/deploy"));
    SoundEffects.Add("fire", Content.Load<SoundEffect>("Content/sounds/fire"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean1"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean2"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean3"));
    ShipBody = Content.Load<Texture2D>("Content/sprites/shipbody");
    ShipFront = Content.Load<Texture2D>("Content/sprites/shipfront");
    ShipBack = Content.Load<Texture2D>("Content/sprites/shipback");
    ShipCenter = Content.Load<Texture2D>("Content/sprites/shipcenter");
    MissedAttack = Content.Load<Texture2D>("Content/sprites/miss");
    Fire = Content.Load<Texture2D>("Content/sprites/fire");
    Sunken = Content.Load<Texture2D>("Content/sprites/skull");
    EndScreen.LoadContent();
  }

  public Texture2D GetRandomOceanTile() {
    return oceanTiles[random.Next(oceanTiles.Count)];
  }
}