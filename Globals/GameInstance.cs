using Godot;
using System;

public partial class GameInstance : Node
{
  private static int Score = 0;
  private static int HighScore = 0;
  public override void _Ready()
  {
    GD.PrintRich("[b]GameInstance.cs Loaded[/b]");
  }

  public static void AwardPoints()
  {
    Score += 10;
    GD.PrintRich(Score);
  }
}
 