using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class BattleshipGame : Game {
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _batch;

  private Match currentMatch;

  public BattleshipGame() {
    _graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    _batch = new SpriteBatch(GraphicsDevice);
    Window.Title = "Battleships";

    currentMatch = new Match(GraphicsDevice, _batch);

    base.Initialize();
  }

  protected override void Draw(GameTime time) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _batch.Begin();
    currentMatch.Render();
    _batch.End();

    base.Draw(time);
  }
}