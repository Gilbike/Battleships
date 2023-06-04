using System.Collections.Generic;

namespace Battleships.UI;

public class UIScreen {
  public List<UIElement> Elements = new List<UIElement>();

  public void Render() {
    foreach (UIElement element in Elements) {
      element.Render();
    }
  }
}