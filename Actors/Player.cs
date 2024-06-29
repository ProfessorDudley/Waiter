using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Player : Node2D
{
	[Export] private WaiterLocation[] WaiterLocations;
	private int step = 0;
	private int Step { get => step; set => step = Math.Clamp(value, 0, WaiterLocations.Length - 1); }
	private Area2D collider;
	private Food food;
	private readonly List<Food> Tray = new();
	private int hatCounter = 0;


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
		}
		else if (@event.IsActionPressed("MoveDown") && Step != WaiterLocations.Length - 1)
		{
			// Move Down
			Step += 1;
			Position = WaiterLocations[Step].GlobalPosition;
			GetNode<AudioStreamPlayer2D>("Steps").Play();
		}




		// If Waiter is at table
		if (Step == WaiterLocations.Length - 1)
		{
			// Flip the sprite to face the table
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;



			// If the waiter is holding foods
			if (Tray.Count > 0)
			{
				// Award Points
				switch (Tray.Count)
				{
					case 1:
						GameInstance.AwardPoints(10);
						break;

					case 2:
						GameInstance.AwardPoints(10);
						break;
					case 3:
						GameInstance.AwardPoints(50);
						// Grow Hat
						HatTopper topper = new()
						{
							GlobalPosition = new(216, 143 - (hatCounter * 2))
						};
						GetTree().Root.AddChild(topper);
						hatCounter++;
						break;
					default:
						GD.PrintErr("This should never happen!");
						break;
				}
				// Create Dollar Particle
				Particle particle = new()
				{
					GlobalPosition = new(200, 152),
				};
				GetTree().Root.AddChild(particle);
				GetNode<AudioStreamPlayer2D>("AddPoints").Play();
				// Update Score
				GetNode<Label>("%Score").Text = GameInstance.Score.ToString();

				// Clear the tray
				foreach (Food food in Tray)
				{
					food.QueueFree();
				}
				Tray.Clear();

				// Reset
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "default";
			}
		}
		else
		{
			// Flip the sprite to face the conveyors
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
		}
	}

	private async void OnAreaEntered(Node2D body)
	{
		Debug.Assert(body.GetParent().GetType() == typeof(Food), "Body must be of type Food");
		food = (Food)body.GetParent();


		if (Tray.Count < 3)
		{
			// Handle attaching food to waiter.
			foreach (Food f in Tray)
			{
				// Check each food on the tray and compare it to the 
				// new food item. If it matches, return from the function.
				if (food.foodName == f.foodName) return;
			}
			collider.AreaEntered -= OnAreaEntered;
			Tray.Add(food);
			food.Attach();
			// Need to defer Reparent call because physics apparently?
			food.CallDeferred("reparent", GetNode<Marker2D>("FoodRoot"), false);
			food.Position = new(0, 0);

			// Wait for the next process frame then reconnect to AreaEntered Signal
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			collider.AreaEntered += OnAreaEntered;

			// Flair
			GetNode<AudioStreamPlayer2D>("Pickup").Play();
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "carry";
		}



	}
}
