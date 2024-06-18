using Godot;
using System;

public partial class Food : Node2D
{
	public double Speed = 50.0;
	private GameInstance game;
	private Player player;

	public bool isAttached;

	public override void _Ready()
	{
		game = GetNode<GameInstance>("/root/Game");
	}


  public void AdvanceStep()
	{
		GD.Print("Advance!");
		GD.Print(GlobalPosition.X);
		
		if (isAttached || GlobalPosition.X <= 118.0f) 
		{
			Position += new Vector2(26.0f, 0.0f);
		} else {
			game.GameOver();
		}
	}
}
