using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleships;

public class Match {
  private SpriteBatch batch;

  private Grid playerGrid;
  private Grid opponentGrid;

  private ShipPlacer placer;
  private OpponentAI oppenent;

  private MatchState matchState = MatchState.Placement;
  private int turningSide; // 1 - localplayer, 0 - opponent
  private int loser;

  public Action<string> OnMatchEnd;

  public Match() {
    batch = BattleshipGame.Instance.Batch;
    playerGrid = new Grid(new Vector2(10, 10), 305, false);
    playerGrid.onFleetDestroyed += delegate () { EndGame(1); };
    opponentGrid = new Grid(new Vector2(335, 10), 450, true);
    opponentGrid.onFleetDestroyed += delegate () { EndGame(0); };

    placer = new ShipPlacer(playerGrid);
    placer.StartPlayerPlacement();
    placer.onPlacementDone += onPlacementDone;

    ShipPlacer _aiPlacer = new ShipPlacer(opponentGrid);
    _aiPlacer.PlaceRandom();

    oppenent = new OpponentAI(playerGrid);
  }

  private bool isLeftClicked = false;
  public void Update() {
    if (matchState == MatchState.Placement) {
      placer.Update();
    } else if (matchState == MatchState.Battle) {
      MouseState state = BattleshipGame.Instance.mouseState;
      if (state.LeftButton == ButtonState.Pressed && !isLeftClicked) {
        if (turningSide == 0) {
          return;
        }
        Vector4 opponentGridSize = opponentGrid.GetDimensions();
        if (state.X < opponentGridSize.X || state.X > opponentGridSize.Z || state.Y < opponentGridSize.Y || state.Y > opponentGridSize.W) {
          return;
        }
        AttackOpponent();
        isLeftClicked = true;
      } else if (state.LeftButton == ButtonState.Released && isLeftClicked) {
        isLeftClicked = false;
      }
    }
  }

  public void onPlacementDone() {
    matchState = MatchState.Battle;
    placer = null;
    turningSide = 1;
  }

  public async void AttackOpponent() {
    Vector2 clickedField = opponentGrid.GetHoveredField();
    bool didHitShip = opponentGrid.AttackField(clickedField);
    BattleshipGame.Instance.SoundEffects["fire"].Play();
    if (!didHitShip) {
      turningSide = 0;
      bool didHitPlayer = false;
      do {
        didHitPlayer = await oppenent.AttackPlayer();
      } while (didHitPlayer);
      turningSide = 1;
    }
  }

  private void EndGame(int losingSide) {
    matchState = MatchState.End;
    loser = losingSide;

    OnMatchEnd?.Invoke((loser == 1 ? "AI" : "Player"));
  }

  public void Render() {
    playerGrid.Render(batch);
    if (matchState == MatchState.Battle) {
      opponentGrid.Render(batch);
      batch.DrawString(BattleshipGame.Instance.UIFont, $"{(turningSide == 1 ? "Player" : "AI")}'s turn", new Vector2(10, 320), Color.White);
    }
  }
}

enum MatchState {
  Placement,
  Battle,
  End
}