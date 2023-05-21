using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class Match {
  private SpriteBatch _batch;

  private Grid _playerGrid;
  private Grid _opponentGrid;

  private ShipPlacer _placer;
  private OpponentAI oppenent;

  private MatchState matchState = MatchState.Placement;

  private int OnTurn; // 1 - localplayer, 0 - opponent

  private int loser;

  public Match() {
    _batch = BattleshipGame.Instance.Batch;
    _playerGrid = new Grid(new Vector2(10, 10), 305, false);
    _playerGrid.onFleetDestroyed += delegate () { EndGame(1); };
    _opponentGrid = new Grid(new Vector2(335, 10), 450, true);
    _opponentGrid.onFleetDestroyed += delegate () { EndGame(0); };

    _placer = new ShipPlacer(_playerGrid);
    _placer.StartPlayerPlacement();
    _placer.onPlacementDone += onPlacementDone;

    ShipPlacer _aiPlacer = new ShipPlacer(_opponentGrid);
    _aiPlacer.PlaceRandom();

    oppenent = new OpponentAI(_playerGrid);
  }

  private bool isLeftClicked = false;
  public void Update() {
    if (matchState == MatchState.Placement)
      _placer.Update();
    else if (matchState == MatchState.Battle) {
      MouseState state = BattleshipGame.Instance.mouseState;
      if (state.LeftButton == ButtonState.Pressed && !isLeftClicked) {
        if (OnTurn == 0)
          return;
        Vector4 opponentGridSize = _opponentGrid.GetDimensions();
        if (state.X < opponentGridSize.X || state.X > opponentGridSize.Z || state.Y < opponentGridSize.Y || state.Y > opponentGridSize.W)
          return;
        AttackOpponent();
        isLeftClicked = true;
      } else if (state.LeftButton == ButtonState.Released && isLeftClicked)
        isLeftClicked = false;
    }
  }

  public void onPlacementDone() {
    matchState = MatchState.Battle;
    _placer = null;
    OnTurn = 1;
  }

  public async void AttackOpponent() {
    Vector2 clickedField = _opponentGrid.GetHoveredField();
    bool didHitShip = _opponentGrid.AttackField(clickedField);
    if (!didHitShip) {
      OnTurn = 0;
      bool didHitPlayer = false;
      do {
        didHitPlayer = await oppenent.AttackPlayer();
      } while (didHitPlayer);
      OnTurn = 1;
    }
  }

  private void EndGame(int losingSide) {
    matchState = MatchState.End;
    loser = losingSide;
  }

  public void Render() {
    _playerGrid.Render(_batch);
    if (matchState == MatchState.Battle) {
      _opponentGrid.Render(_batch);
      _batch.DrawString(BattleshipGame.Instance.UIFont, $"{(OnTurn == 1 ? "Player" : "AI")}'s turn", new Vector2(10, 320), Color.Black);
    } else if (matchState == MatchState.End) {
      _batch.DrawString(BattleshipGame.Instance.UIFont, $"{(loser == 1 ? "AI" : "Player")} wins", new Vector2(10, 320), Color.Black);
    }
  }
}

enum MatchState {
  Placement,
  Battle,
  End
}