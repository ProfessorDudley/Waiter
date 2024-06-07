using Godot;
using System;

public partial class Player : Node2D
{
  [Export] private Marker2D[] conveyors;
  private int step = 0;
	private int Step { get => step; set => step = Math.Clamp(value, 0, conveyors.Length-1); }


	public override void _Ready()
	{
		Position = conveyors[Step].GlobalPosition;
	}

	public override void _Input(InputEvent @event) {
	  if (@event.IsActionPressed("MoveUp") && Step != 0)
	  {
	  Step -= 1;
	  Position = conveyors[Step].GlobalPosition;
		GetNode<AudioStreamPlayer2D>("Steps").Play();
	  }
	  else if (@event.IsActionPressed("MoveDown") && Step != conveyors.Length-1)
	  {
		Step += 1;
		Position = conveyors[Step].GlobalPosition;
		GetNode<AudioStreamPlayer2D>("Steps").Play();
	  }
		if (Step == conveyors.Length-1)
		{
			GetNode<Sprite2D>("Sprite2D").Scale = new Vector2(-0.678f, 0.678f);
		}
		else
		{
			GetNode<Sprite2D>("Sprite2D").Scale = new Vector2(0.678f, 0.678f);
		}
	}
}
