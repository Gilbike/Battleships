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
    _texture = BattleshipGame.Instance.GetRandomOceanTile();

    State = FieldState.Empty;
  }

  public void Render(SpriteBatch batch) {
    batch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, new Vector2((float)_grid.FieldSize / _texture.Width), SpriteEffects.None, 0f);
    if (State == FieldState.Ship) {
      Vector2 scale = new Vector2((float)_grid.FieldSize / _texture.Width);
      batch.Draw(BattleshipGame.Instance.ShipBody, _position + new Vector2(8 * scale.X, 8 * scale.Y), null, Color.White, MathHelper.ToRadians(90f), new Vector2(8, 8), scale, SpriteEffects.None, 0f);
    }
  }

  public void Update() { }
}