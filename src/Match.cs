using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battleships.Resources;
using Battleships.UI;

namespace Battleships;

public class Match {
  private SpriteBatch batch;

  private Grid playerGrid;
  private Grid opponentGrid;

  private ShipPlacer placer;
  private OpponentAI oppenent;

  private MatchHUD hud;

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

    hud = new MatchHUD();
    hud.SetLabel("Prepare for battle! Place your ships");

    UIManager.Screen = hud;
  }

  private void OnClick(float x, float y) {
    if (turningSide == 0) {
      return;
    }
    AttackOpponent();
  }

  public void onPlacementDone() {
    matchState = MatchState.Battle;
    placer = null;
    turningSide = 1;
    Input.OnLeftMouseClicked += OnClick;
    hud.SetLabel($"Player's turn");
  }

  public async void AttackOpponent() {
    Vector2 clickedField = opponentGrid.GetHoveredField();
    if (clickedField == new Vector2(-1) || opponentGrid.GetField(opponentGrid.GetIndexFromLocationVector(clickedField)).Attacked) {
      return;
    }
    bool didHitShip = opponentGrid.AttackField(clickedField);
    if (Settings.SettingsManager.EnableSounds) {
      ResourceManager.SoundEffects["fire"].Play();
    }
    if (!didHitShip) {
      turningSide = 0;
      hud.SetLabel("AI's turn");
      bool didHitPlayer = false;
      do {
        didHitPlayer = await oppenent.AttackPlayer();
      } while (didHitPlayer);
      turningSide = 1;
      hud.SetLabel("Player's turn");
    }
  }

  private void EndGame(int losingSide) {
    matchState = MatchState.End;
    loser = losingSide;

    Input.OnLeftMouseClicked -= OnClick;

    OnMatchEnd?.Invoke((loser == 1 ? "AI" : "Player"));
  }

  public void Render() {
    playerGrid.Render(batch);
    if (matchState == MatchState.Battle) {
      opponentGrid.Render(batch);
    }
  }
}

enum MatchState {
  Placement,
  Battle,
  End
}