using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Match {
  // From Game class
  private SpriteBatch _batch;

  private Grid _playerGrid;
  private Grid _opponentGrid;

  private MatchState matchState = MatchState.Placement;

  public Match(GraphicsDevice device, SpriteBatch batch) {
    _batch = batch;

    //
    _playerGrid = new Grid(device, new Vector2(10, 10), 305, false);
    _opponentGrid = new Grid(device, new Vector2(335, 10), 450, true);
  }

  public void Update() {

  }

  public void Render() {
    _playerGrid.Render(_batch);
    if (matchState != MatchState.Placement)
      _opponentGrid.Render(_batch);
  }
}

enum MatchState {
  Placement,
  Battle
}