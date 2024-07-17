using Godot;
public partial class SaveGame : Node
{
  public static void SaveScore(int highscore)
  {
	using var saveGame = FileAccess.Open("user://savegame.data", FileAccess.ModeFlags.Write);
	saveGame.StoreVar(highscore);
  }

  public static int LoadScore()
  {
    if (!FileAccess.FileExists("user://savegame.data"))
    {
      return 0;
    }
	using var saveGame = FileAccess.Open("user://savegame.data", FileAccess.ModeFlags.Read);
  
	return (int)saveGame.GetVar();
  }
}
