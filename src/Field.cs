using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Battleships;

public class Field {
  private Texture2D texture;
  private Vector2 position;
  private Grid grid;

  public int ShipID { get; set; }

  public ShipPart Part { get; set; }
  public bool Attacked { get; set; }
  public bool Sunken { get; set; }

  public Field(Grid grid, Vector2 position, Texture2D texture) {
    this.grid = grid;
    this.position = position;
    texture = BattleshipGame.Instance.GetRandomOceanTile();

    Attacked = false;
    Sunken = false;
  }

  public void Render(SpriteBatch batch) {
    batch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, new Vector2((float)grid.FieldSize / texture.Width), SpriteEffects.None, 0f);
    if (Part != null) {
      if (Sunken || !grid.Encoded) {
        Vector2 scale = new Vector2((float)grid.FieldSize / texture.Width);
        batch.Draw(Part.texture, position + new Vector2(8 * scale.X, 8 * scale.Y), null, Color.White, MathHelper.ToRadians(Part.rotation), new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }

      if (Attacked && !Sunken) {
        Vector2 scale = new Vector2((float)(grid.FieldSize - 5) / texture.Width);
        Vector2 offset = new Vector2(5f / 2);
        batch.Draw(BattleshipGame.Instance.Fire, position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      } else if (Attacked && Sunken) {
        Vector2 scale = new Vector2((float)(grid.FieldSize - 5) / texture.Width);
        Vector2 offset = new Vector2(5f / 2);
        batch.Draw(BattleshipGame.Instance.Sunken, position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }
    } else {
      if (Attacked) {
        Vector2 scale = new Vector2(MathF.Round((float)(grid.FieldSize - 15) / texture.Width));
        Vector2 offset = new Vector2(15f / 2);
        batch.Draw(BattleshipGame.Instance.MissedAttack, position + new Vector2(8 * scale.X, 8 * scale.Y) + offset, null, Color.White, 0f, new Vector2(8, 8), scale, SpriteEffects.None, 0f);
      }
    }
  }
}