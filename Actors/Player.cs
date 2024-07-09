using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Player : Node2D
{
	[Export] private WaiterLocation[] WaiterLocations;
	private int step = 0;
	private int Step { get => step; set => step = Math.Clamp(value, 0, WaiterLocations.Length - 1); }
	private Area2D collider;
	private Tray tray;
	private Food newFood;
	private int hatCounter = 0;
	private TaskGenerator Task;


	public override void _Ready()
	{
		// Set up variables and signals
		Position = WaiterLocations[Step].GlobalPosition;
		collider = GetNode<Area2D>("FoodCollider");
		collider.AreaEntered += OnAreaEntered;
		tray = GetNode<Tray>("%Tray");
		Task = GetNode<TaskGenerator>("/root/Game/TaskGenerator");
	}

    public override void _Process(double delta)
    {
        if (tray.TrayItems.ContainsValue(true))
				{
					
			tray.Visible = true;
				}
				else
				{
					tray.Visible = false;
				}
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
		// Flip the sprite to face the conveyors
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;


		// If Waiter is at table
		if (Step == WaiterLocations.Length - 1)
		{
			// Flip the sprite to face the table
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;

			// If the waiter is holding foods
			if (tray.TrayItems.ContainsValue(true))
			{
				// Award Points
				switch (tray.TrayItems.Count(x => x.Value is true))
				{
					case 1:
						GameInstance.AwardPoints(10);
						break;

					case 2:
						GameInstance.AwardPoints(20);
						break;
					case 3:
						GameInstance.AwardPoints(100);
						// Grow Hat
						HatTopper topper = new()
						{
							GlobalPosition = new(216, 143 - (hatCounter * 2))
						};
						GetTree().Root.AddChild(topper);
						hatCounter++;
						// Create a new Task when full meal is delivered.
						Task.WriteTray(Task.NewTask().TrayItems);
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
				GetNode<Label>("/root/Game/Score").Text = GameInstance.Score.ToString();


				// Reset
				// Clear the tray
				foreach (string foodName in tray.TrayItems.Keys)
				{
					tray.TrayItems[foodName] = false;
				}
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "default";
				tray.Visible = false;
			}
		}

		// If Waiter is at counter
		if (@event.IsActionPressed("MoveUp") && Step == 0)
		{
			// Flip the sprite to face the counter
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			Tray counter = GetNode<Tray>("/root/Game/Conveyors/Counter/Tray");
		
			
			// Logic for storing items on the counter.
			if (tray.TrayItems.ContainsValue(true)) // If the Waiter is holding food
			{
				List<string> movedItems = new();

				// Find which items can be moved and move them
				List<string> moveable = tray.TrayItems.Where(v => v.Value is true).Select(k => k.Key).ToList();
				counter.AddToTray(moveable, out movedItems);

				// remove moved items
				tray.RemoveFromTray(movedItems);

				// Update animation and tray visibility
				if (!tray.TrayItems.ContainsValue(true)) 
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "default";
				  tray.Visible = false;
				}
			}

			// Logic for retrieving items from the counter.
			else if (!tray.TrayItems.ContainsValue(true)) // If there are no items on the tray.
			{
				List<string> movedItems = new();

				// Find which items can be moved and move them
				List<string> moveable = counter.TrayItems.Where(v => v.Value is true).Select(k => k.Key).ToList();
				tray.AddToTray(moveable, out movedItems);

				// remove moved items
				counter.RemoveFromTray(movedItems);

				// Update animation and tray visibility
				if (tray.TrayItems.ContainsValue(true)) 
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "carry";
				  tray.Visible = true;
				}
			}
		}

	}
	private async void OnAreaEntered(Node2D body)
	{
		Debug.Assert(body.GetParent().GetType() == typeof(Food), "Body must be of type Food");
		newFood = (Food)body.GetParent();


		if (tray.TrayItems.ContainsValue(false)) // If we have space on our tray
		{
			// is the newFood already on the tray?
			// If yes, skip the rest of the function.
			if (tray.TrayItems[newFood.foodName]) return;

			// Only run if food collected is unique to the tray.
			// Disable collider to prevent further funny business.
			collider.AreaEntered -= OnAreaEntered;

			// Add newFood to the tray.
			tray.AddToTray(new(){newFood.foodName}, out _);

			// Delete the food item object. We don't need the physical actor anymore.
			newFood.QueueFree();

			// Wait for the next process frame then reconnect to AreaEntered Signal
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			collider.AreaEntered += OnAreaEntered;

			// Flair
			GetNode<AudioStreamPlayer2D>("Pickup").Play();
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "carry";
		}



	}
}
