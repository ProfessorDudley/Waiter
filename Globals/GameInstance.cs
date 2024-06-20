using Godot;
using System;

public partial class GameInstance : Node
{
  public static int Score = 0;
  private static int HighScore = 0;


  public override void _Ready()
  {
	GD.PrintRich("[b]GameInstance.cs Loaded[/b]");
	GetNode<Label>("%HighScore").Text = HighScore.ToString();

  }

  public static void AwardPoints()
  {
	Score += 10;
	GD.PrintRich(Score);
  }

  public void GameOver()
  {
	GetNode<AnimatedSprite2D>("/root/Game/Player/AnimatedSprite2D").Animation = "fail";
	if (Score > HighScore) HighScore = Score;
	Node instance = GD.Load<PackedScene>("res://Menus/GameOverScreen.tscn").Instantiate();
	GetTree().Root.AddChild(instance);
	GetNode<GameTick>("/root/GameTick").Stop();
	GetNode<Player>("/root/Game/Player").SetProcessInput(false);
	foreach (var node in GetTree().GetNodesInGroup("Foods"))
	{
	  node.QueueFree();
	}
  }
}

