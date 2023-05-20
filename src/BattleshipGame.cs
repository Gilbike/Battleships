using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class BattleshipGame : Game {
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _batch;

  private Grid grid;

  public BattleshipGame() {
    _graphics = new GraphicsDeviceManager(this);

    IsMouseVisible = true;
  }

  protected override void Initialize() {
    _batch = new SpriteBatch(GraphicsDevice);
    Window.Title = "Battleships";

    grid = new Grid(GraphicsDevice, new Vector2(10, 10), 250, false);

    base.Initialize();
  }

  protected override void Draw(GameTime time) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _batch.Begin();
    grid.Render(_batch);
    _batch.End();

    base.Draw(time);
  }
}