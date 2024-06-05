using Godot;
using System;

public partial class Player : Node2D
{
  [Export] public Conveyor[] conveyors;
  private int step = 0;

	public int Step { get => step; set => step = Math.Clamp(value, 0, conveyors.Length-1); }


	public override void _Ready()
	{
		Position = conveyors[Step].GetNode<Marker2D>("Marker2D").GlobalPosition;
	}

	public override void _Input(InputEvent @event) {
	  if (@event.IsActionPressed("MoveUp"))
	  {
	  
	  GD.Print("MoveUp");
	  Step -= 1;
		Marker2D marker = conveyors[step].GetNode<Marker2D>("Marker2D");
		GD.Print(marker.GlobalPosition);
	  Position = conveyors[Step].GetChild<Marker2D>(0).GlobalPosition;
	  GD.Print(Step);

	  }
	  else if (@event.IsActionPressed("MoveDown"))
	  {
		GD.Print("MoveDown");
		Step += 1;
		Position = conveyors[Step].GetChild<Marker2D>(0).GlobalPosition;
		GD.Print(Step);
	  }
	}
}
