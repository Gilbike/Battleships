using System;

namespace Battleships;

public class OpponentAI {
  private Grid _playerGrid;
  private Random random;
  private int _attackTick;

  public OpponentAI(Grid playerGrid) {
    random = new Random();
    _playerGrid = playerGrid;
  }

  public void Update() {

  }

  public bool AttackPlayer() {
    int selectedField = random.Next(100);
    return _playerGrid.AttackField(selectedField);
  }
}