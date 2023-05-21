using System;
using System.Threading.Tasks;

namespace Battleships;

public class OpponentAI {
  private Grid _playerGrid;
  private Random random;

  public OpponentAI(Grid playerGrid) {
    random = new Random();
    _playerGrid = playerGrid;
  }

  public void Update() { }

  public async Task<bool> AttackPlayer() {
    await Task.Delay(random.Next(500, 1750));
    int selectedField = random.Next(100);
    bool result = _playerGrid.AttackField(selectedField);
    return await Task.Run<bool>(delegate () {
      return result;
    });
  }
}