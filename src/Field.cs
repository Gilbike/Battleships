using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Battleships;

public class Field {
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
      if (Sunken || !_grid.Encoded) {
        Vector2 scale = new Vector2((float)_grid.FieldSize / _texture.Width);
        batch.Draw(Part.texture, _position + new Vector2(8 * scale.X, 8 * scale.Y), null, Color.White, MathHelper.ToRadians(Part.rotation), new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }

      if (Attacked && !Sunken) {
        Vector2 scale = new Vector2((float)(_grid.FieldSize - 5) / _texture.Width);
        Vector2 offset = new Vector2(5f / 2);
        batch.Draw(BattleshipGame.Instance.Fire, _position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      } else if (Attacked && Sunken) {
        Vector2 scale = new Vector2((float)(_grid.FieldSize - 5) / _texture.Width);
        Vector2 offset = new Vector2(5f / 2);
        batch.Draw(BattleshipGame.Instance.Sunken, _position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }
    } else {
      if (Attacked) {
        Vector2 scale = new Vector2(MathF.Round((float)(_grid.FieldSize - 15) / _texture.Width));
        Vector2 offset = new Vector2(15f / 2);
        batch.Draw(BattleshipGame.Instance.MissedAttack, _position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }
    }
  }

  public void Update() { }
}