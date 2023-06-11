using System;
using System.IO;

namespace Battleships.Settings;

public static class SettingsManager {
  private static bool _enableSounds = true;
  public static bool EnableSounds { get { return _enableSounds; } set { _enableSounds = value; Save(); } }

  public static void Load() {
    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\settings.bss";
    if (!File.Exists(filePath)) {
      File.WriteAllLines(filePath, new string[] {
        "sounds=1"
      });
    }
    string[] fileLines = File.ReadAllLines(filePath);
    foreach (string fileLine in fileLines) {
      string[] line = fileLine.Split('=');
      switch (line[0]) {
        case "sounds":
          _enableSounds = line[1] == "1" ? true : false;
          break;
        default:
          break;
      }
    }
  }

  public static void Save() {
    System.Console.WriteLine("save");
    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\settings.bss";
    File.Delete(filePath);
    File.WriteAllLines(filePath, new string[] {
      $"sounds={(EnableSounds ? "1" : "0")}"
    });
  }
}