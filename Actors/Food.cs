using Godot;
using System;

public partial class Food : Node2D
{
	public double Speed = 50.0;

	public override void _PhysicsProcess(double delta)
	{
	  Vector2 update = GlobalPosition;
	  update.X += (float)(delta * Speed);
	  GlobalPosition = update;
	}
}
