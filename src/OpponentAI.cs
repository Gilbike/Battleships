using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battleships;

public class OpponentAI {
  private Grid _playerGrid;
  private Random random;

  private LastAttackResult _lastResult = new LastAttackResult();

  private LineAttack lineAttack = new LineAttack();

  private List<int> attackedFields = new List<int>();

  private int attacksSinceCheck = 0;
  private int[] quarters = new int[4];

  public OpponentAI(Grid playerGrid) {
    random = new Random();
    _playerGrid = playerGrid;
  }

  private void CalculateQuarters() {
    for (int i = 0; i < 4; i++) {
      int quarter = 0;

      int minRow = i < 2 ? 0 : 5;
      int maxRow = i < 2 ? 5 : 10;

      int minCol = i % 2 == 0 ? 0 : 5;
      int maxCol = i % 2 == 0 ? 5 : 10;

      for (int row = minRow; row < maxRow; row++) {
        for (int col = minCol; col < maxCol; col++) {
          int index = _playerGrid.GetIndexFromLocationVector(new Vector2(col, row));
          if (attackedFields.IndexOf(index) != -1) {
            quarter++;
          }
        }
      }
      System.Console.WriteLine("quarter " + i + " got " + quarter + " attacks");
      quarters[i] = quarter;
    }
  }

  public async Task<bool> AttackPlayer() {
    await Task.Delay(random.Next(500, 1750));

    int selectedField;
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
          if (neighbours[lineAttack.direction] == -1) { // hit border
            selectedField = random.Next(100);
            lineAttack = new LineAttack();
          }
        }
      }
    } while (attackedFields.IndexOf(selectedField) != -1);

    if (!lineAttack.enabled && selectedDirection == -1 && attacksSinceCheck >= 5) {
      CalculateQuarters();
      int laggingRegionIndex = quarters.ToList().IndexOf(quarters.Min());

      int minRow = laggingRegionIndex < 2 ? 0 : 5;
      int maxRow = laggingRegionIndex < 2 ? 5 : 10;

      int minCol = laggingRegionIndex % 2 == 0 ? 0 : 5;
      int maxCol = laggingRegionIndex % 2 == 0 ? 5 : 10;

      selectedField = _playerGrid.GetIndexFromLocationVector(new Vector2(random.Next(minCol, maxCol), random.Next(minRow, maxRow)));

      attacksSinceCheck = 0;
      System.Console.WriteLine("random lagging area " + laggingRegionIndex);
    }

    result = _playerGrid.AttackField(selectedField);
    attackedFields.Add(selectedField);

    if (_lastResult.successful && !result) { // last one was successful but this one was not
      if (lineAttack.enabled) {
        lineAttack = new LineAttack();
      } else if (!lineAttack.enabled && selectedDirection != -1) {
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

    attacksSinceCheck++;

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