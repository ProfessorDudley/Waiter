using Godot;
using System;

public partial class Food : Node2D
{
	GameTick tick;
	private bool isAttached;
	public bool IsAttached
	{
		get
		{
			return isAttached;
		}
		set
		{
			tick.Timeout -= AdvanceStep;
			isAttached = value;
		}
	}

	public override void _Ready()
	{
		tick = GetNode<GameTick>("/root/GameTick");
    tick.Timeout += AdvanceStep;
	}


  public void AdvanceStep()
	{		
		if (IsAttached || GlobalPosition.X <= 118.0f) 
		{
			// Move the food a little to the right
			Position += new Vector2(26.0f, 0.0f);
		} else {
			GetNode<GameInstance>("/root/Game").GameOver();
		}
	}
}
