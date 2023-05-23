using System;
using System.Threading.Tasks;

namespace Battleships;

public class OpponentAI {
  private Grid _playerGrid;
  private Random random;

  private LastAttackResult _lastResult = new LastAttackResult();

  private LineAttack lineAttack = new LineAttack();

  private List<int> attackedFields = new List<int>();
  public OpponentAI(Grid playerGrid) {
    random = new Random();
    _playerGrid = playerGrid;
  }

  public void Update() { }

  public async Task<bool> AttackPlayer() {
    await Task.Delay(random.Next(500, 1750));
    int selectedField = random.Next(100);
    bool result = false;

    int selectedDirection = -1;

    do {
      selectedField = random.Next(100);

    if (_lastResult.successful || lineAttack.enabled) { // select neighbour field when hit successful
      int[] neighbours = _playerGrid.GetNeighbourFields(_lastResult.location);
      if (!lineAttack.enabled || lineAttack.searchingForDirection) {
        do {
          selectedDirection = random.Next(neighbours.Length);
        } while (neighbours[selectedDirection] == -1);
        selectedField = neighbours[selectedDirection];
        if (neighbours[selectedDirection] == -1) { // hit border
          selectedField = random.Next(100);
          lineAttack = new LineAttack();
        }
      } else if (lineAttack.enabled && !lineAttack.searchingForDirection) {
        selectedField = neighbours[lineAttack.direction];
          if (neighbours[selectedDirection] == -1) { // hit border
            selectedField = random.Next(100);
            lineAttack = new LineAttack();
      }
    }
      }
    } while (attackedFields.IndexOf(selectedField) != -1);

    result = _playerGrid.AttackField(selectedField);
    attackedFields.Add(selectedField);

    if (_lastResult.successful && !result) { // last one was successful but this one was not
      if (lineAttack.enabled) {
        System.Console.WriteLine();
        lineAttack = new LineAttack();
      } else {
        lineAttack.enabled = true;
        lineAttack.searchedDirections[selectedDirection] = true;
        selectedField = _lastResult.location;
      }
    } else if (_lastResult.successful && result) {
      if (!lineAttack.enabled && selectedDirection != -1) {
        lineAttack.enabled = true;
        lineAttack.searchingForDirection = false;
        lineAttack.direction = selectedDirection;
      } else if (lineAttack.enabled && lineAttack.searchingForDirection) {
        lineAttack.searchingForDirection = false;
        lineAttack.direction = selectedDirection;
      }
    }

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

class LineAttack {
  public bool enabled = false;
  public bool searchingForDirection = true;
  public bool[] searchedDirections = new bool[4];
  public int direction;
}