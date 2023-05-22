using System;
using System.Threading.Tasks;

namespace Battleships;

public class OpponentAI {
  private Grid _playerGrid;
  private Random random;

  private LastAttackResult _lastResult;

  public OpponentAI(Grid playerGrid) {
    random = new Random();
    _playerGrid = playerGrid;
  }

  public void Update() { }

  public async Task<bool> AttackPlayer() {
    await Task.Delay(random.Next(500, 1750));
    int selectedField;
    bool result = false;

    if (_lastResult != null && _lastResult.successful) { // select neighbour field when hit successful
      int[] neighbours = _playerGrid.GetNeighbourFields(_lastResult.location);
      int selectedDirection = -1;
      do {
        selectedDirection = random.Next(neighbours.Length);
      } while (neighbours[selectedDirection] == -1);
      selectedField = neighbours[selectedDirection];
    } else {
      selectedField = random.Next(100);
    }

    result = _playerGrid.AttackField(selectedField);
    _lastResult = new LastAttackResult { location = selectedField, successful = result };

    return await Task.Run<bool>(delegate () {
      return result;
    });
  }
}

class LastAttackResult {
  public int location;
  public bool successful;
}