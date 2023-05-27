using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battleships;

public enum FieldState {
  Empty,
  Ship,
  Hit,
  ShipHit,
  ShipSunk,
}

static class FieldColors {
  public static Color BaseColor = new Color(21, 152, 149);
  public static Color ShipColor = new Color(237, 237, 237);
  public static Color MissColor = new Color(232, 170, 66);
  public static Color HitColor = new Color(244, 80, 80);
  public static Color SunkColor = new Color(94, 59, 77);
}

public class Field : BaseObject {
  private Texture2D _texture;
  private Vector2 _position;
  private Grid _grid;

  public FieldState State { get; set; }
  public int ShipID { get; set; }

  public Field(Grid grid, Vector2 position, Texture2D texture) {
    _grid = grid;
    _position = position;
    _texture = texture;

    State = FieldState.Empty;
  }

  public void Render(SpriteBatch batch) {
    Color renderColor = FieldColors.BaseColor;
    switch (State) {
      case FieldState.Ship:
        if (!_grid.Encoded)
          renderColor = FieldColors.ShipColor;
        break;
      case FieldState.Hit:
        renderColor = FieldColors.MissColor;
        break;
      case FieldState.ShipHit:
        renderColor = FieldColors.HitColor;
        break;
      case FieldState.ShipSunk:
        renderColor = FieldColors.SunkColor;
        break;
    }
    batch.Draw(_texture, _position, renderColor);
  }

  public void Update() { }
}