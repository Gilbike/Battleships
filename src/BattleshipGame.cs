using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

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

  private Match currentMatch;

  private BattleshipGame() {
    _graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    _batch = new SpriteBatch(GraphicsDevice);
    Window.Title = "Battleships";

    currentMatch = new Match();

    base.Initialize();
  }

  protected override void Update(GameTime gameTime) {
    mouseState = Mouse.GetState();
    keyboardState = Keyboard.GetState();
    currentMatch.Update();
    base.Update(gameTime);
  }

  protected override void Draw(GameTime time) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _batch.Begin();
    currentMatch.Render();
    _batch.End();

    base.Draw(time);
  }
}