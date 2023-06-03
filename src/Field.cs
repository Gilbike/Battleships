using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battleships;

public class Field : BaseObject {
  private Texture2D _texture;
  private Vector2 _position;
  private Grid _grid;

  public int ShipID { get; set; }

  public ShipPart Part { get; set; }
  public bool Attacked { get; set; }
  public bool Sunken { get; set; }

  public Field(Grid grid, Vector2 position, Texture2D texture) {
    _grid = grid;
    _position = position;
    _texture = BattleshipGame.Instance.GetRandomOceanTile();

    Attacked = false;
    Sunken = false;
  }

  public void Render(SpriteBatch batch) {
    batch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, new Vector2((float)_grid.FieldSize / _texture.Width), SpriteEffects.None, 0f);
    if (Part != null) {
      Vector2 scale = new Vector2((float)_grid.FieldSize / _texture.Width);
      batch.Draw(Part.texture, _position + new Vector2(8 * scale.X, 8 * scale.Y), null, Color.White, MathHelper.ToRadians(Part.rotation), new Vector2(8, 8), scale, SpriteEffects.None, 0f);
    }
  }

  public void Update() { }
}