using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships;

public class Match {
  private SpriteBatch _batch;

  private Grid _playerGrid;
  private Grid _opponentGrid;

  private ShipPlacer _placer;

  private MatchState matchState = MatchState.Placement;

  private int OnTurn; // 1 - localplayer, 0 - opponent

  public Match() {
    _batch = BattleshipGame.Instance.Batch;
    _playerGrid = new Grid(new Vector2(10, 10), 305, false);
    _opponentGrid = new Grid(new Vector2(335, 10), 450, true);
    _placer = new ShipPlacer(_playerGrid);
    _placer.StartPlayerPlacement();
    _placer.onPlacementDone += onPlacementDone;
  }

  public void Update() {
    if (matchState == MatchState.Placement)
      _placer.Update();
    else if (matchState == MatchState.Battle) {

    }
  }

  public void onPlacementDone() {
    matchState = MatchState.Battle;
    _placer = null;
    OnTurn = 1;
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