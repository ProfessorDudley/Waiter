using Godot;
using System;
using System.Diagnostics;

public partial class Player : Node2D
{
	[Export] private WaiterLocation[] WaiterLocations;
	private int step = 0;
	private int Step { get => step; set => step = Math.Clamp(value, 0, WaiterLocations.Length - 1); }
	private Area2D collider;


	public override void _Ready()
	{
		Position = WaiterLocations[Step].GlobalPosition;
		collider = GetNode<Area2D>("Sprite2D/FoodCollider");
		GD.Print(collider.Name);
        collider.AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Node2D body)
	{
					Debug.Assert(body.GetParent().GetType() == typeof(Food), "Body must be of type Food");
					Food food = (Food)body.GetParent();
					// Need to defer Reparent call because physics apparently?
					food.CallDeferred("reparent", GetNode<Marker2D>("Sprite2D/FoodRoot"), false);
					food.Position = new(0, 0);
					food.Speed = 0;
	}	

	public override void _Input(InputEvent @event)
	{
		// Handle Player movement input
		if (@event.IsActionPressed("MoveUp") && Step != 0)
		{
			Step -= 1;
			Position = WaiterLocations[Step].GlobalPosition;
			GetNode<AudioStreamPlayer2D>("Steps").Play();
			WaiterLocations[step].OnConveyorOverlap(this);
		}
		else if (@event.IsActionPressed("MoveDown") && Step != WaiterLocations.Length - 1)
		{
			Step += 1;
			Position = WaiterLocations[Step].GlobalPosition;
			GetNode<AudioStreamPlayer2D>("Steps").Play();
			WaiterLocations[step].OnConveyorOverlap(this);
		}

		// Handle setting player facing direction depending on which belt they are at.
		if (Step == WaiterLocations.Length - 1)
		{
			GetNode<Sprite2D>("Sprite2D").Scale = new Vector2(-0.678f, 0.678f);
		}
		else
		{
			GetNode<Sprite2D>("Sprite2D").Scale = new Vector2(0.678f, 0.678f);
		}
	}
}
