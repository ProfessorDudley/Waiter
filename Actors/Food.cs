using Godot;
using System;

public partial class Food : Node2D
{
	public double Speed = 50.0;
	private GameInstance game;
	private Player player;

	public override void _Ready()
	{
		game = GetNode<GameInstance>("/root/Game");
		
	}

	public override void _Process(double delta)
	{
	  Vector2 update = GlobalPosition;
	  update.X += (float)(delta * Speed);
	  GlobalPosition = update;
		if (GlobalPosition.X > 610)
		{
			game.GameOver();
		}
	}
}
