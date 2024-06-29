using Godot;
using System;

public partial class Food : Node2D
{
	GameTick tick;
	private readonly Random random = new();
	private bool isAttached;
	// public string foodName;

	public override void _Ready()
	{
		tick = GetNode<GameTick>("/root/GameTick");
	tick.Timeout += AdvanceStep;
	random.Next(0,3);
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = new[] {"bowl", "drink", "plate",}[random.Next(0,3)];
	}


  public void AdvanceStep()
	{		
		if (isAttached || GlobalPosition.X <= 118.0f) 
		{
			// Move the food a little to the right
			Position += new Vector2(26.0f, 0.0f);
		} else {
			GetNode<GameInstance>("/root/Game").GameOver();
		}
	}

	public void Attach()
	{
		isAttached = true;
		GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		tick.Timeout -= AdvanceStep;
	}
}
