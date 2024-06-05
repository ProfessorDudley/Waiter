using Godot;
using System;

public partial class GameInstance : Node
{
  public int score;
  public GameInstance(int score)
  {
    GD.Print($"Score: {score}"); 
  }
}
