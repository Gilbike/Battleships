using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Resources;

public static class ResourceManager {
  public static SpriteFont Font { get; private set; }

  public static Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
  private static List<Texture2D> oceanTiles = new List<Texture2D>();

  // textures
  public static Texture2D ShipBody;
  public static Texture2D ShipFront;
  public static Texture2D ShipBack;
  public static Texture2D ShipCenter;
  public static Texture2D MissedAttack;
  public static Texture2D Fire;
  public static Texture2D Sunken;

  public static void Create(ContentManager Content) {
    Font = Content.Load<SpriteFont>("Content/fonts/UI");
    SoundEffects.Add("deploy", Content.Load<SoundEffect>("Content/sounds/deploy"));
    SoundEffects.Add("fire", Content.Load<SoundEffect>("Content/sounds/fire"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean1"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean2"));
    oceanTiles.Add(Content.Load<Texture2D>("Content/sprites/ocean3"));
    ShipBody = Content.Load<Texture2D>("Content/sprites/shipbody");
    ShipFront = Content.Load<Texture2D>("Content/sprites/shipfront");
    ShipBack = Content.Load<Texture2D>("Content/sprites/shipback");
    ShipCenter = Content.Load<Texture2D>("Content/sprites/shipcenter");
    MissedAttack = Content.Load<Texture2D>("Content/sprites/miss");
    Fire = Content.Load<Texture2D>("Content/sprites/fire");
    Sunken = Content.Load<Texture2D>("Content/sprites/skull");
  }

  public static Texture2D GetRandomOceanTile() {
    return oceanTiles[BattleshipGame.random.Next(oceanTiles.Count)];
  }
}