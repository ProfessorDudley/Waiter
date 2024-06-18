using Godot;
using System;
using System.Diagnostics;

public partial class Player : Node2D
{
	[Export] private WaiterLocation[] WaiterLocations;
	private int step = 0;
	private int Step { get => step; set => step = Math.Clamp(value, 0, WaiterLocations.Length - 1); }
	private Area2D collider;
	public bool isHoldingFood = false;
	private Food food;


	public override void _Ready()
	{
		// Set up variables and signals
		Position = WaiterLocations[Step].GlobalPosition;
		collider = GetNode<Area2D>("FoodCollider");
		collider.AreaEntered += OnAreaEntered;
		
	}

	public override void _Input(InputEvent @event)
	{
		// Handle Player movement input
		if (@event.IsActionPressed("MoveUp") && Step != 0)
		{
			// Move Up
			Step -= 1;
			Position = WaiterLocations[Step].GlobalPosition;
			GetNode<AudioStreamPlayer2D>("Steps").Play();
			WaiterLocations[step].OnConveyorOverlap(this);
		}
		else if (@event.IsActionPressed("MoveDown") && Step != WaiterLocations.Length - 1)
		{
			// Move Down
			Step += 1;
			Position = WaiterLocations[Step].GlobalPosition;
			GetNode<AudioStreamPlayer2D>("Steps").Play();
			WaiterLocations[step].OnConveyorOverlap(this);
		}

		// If Waiter is at table
		if (Step == WaiterLocations.Length - 1)
		{
			// Flip the sprite to face the table
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;

			if (isHoldingFood)
			{
			isHoldingFood = false;
			collider.AreaEntered += OnAreaEntered;
			food.QueueFree();
			GetNode<AudioStreamPlayer2D>("AddPoints").Play();
			GameInstance.AwardPoints();
			GetNode<Label>("%Score").Text = GameInstance.Score.ToString();
			}
		}
		else
		{
			// Flip the sprite to face the conveyors
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
		}
	}

	private void OnAreaEntered(Node2D body)
	{
		// Handle attaching food to waiter.
		Debug.Assert(body.GetParent().GetType() == typeof(Food), "Body must be of type Food");
		food = (Food)body.GetParent();
		// Need to defer Reparent call because physics apparently?
		food.CallDeferred("reparent", GetNode<Marker2D>("FoodRoot"), false);
		food.Position = new(0, 0);
		food.Speed = 0;
		isHoldingFood = true;
		food.isAttached = true;
		GetNode<AudioStreamPlayer2D>("Pickup").Play();
		collider.AreaEntered -= OnAreaEntered;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "carry";
	}
}
