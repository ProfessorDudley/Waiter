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
		GetNode<GameTick>("/root/GameTick").Start();

	}

	public static void AwardPoints(int points = 10)
	{
		Score += points;
		GD.Print($"Added {points} points!");
		GD.Print($"Points: {Score}");
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

