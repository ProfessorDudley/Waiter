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
	private Tray Tray;
	private Food newFood;
	private int hatCounter = 0;
	private TaskGenerator Task;
	private int DeliveryAttempts = 0;


	public override void _Ready()
	{
		// Set up variables and signals
		Position = WaiterLocations[Step].GlobalPosition;
		collider = GetNode<Area2D>("FoodCollider");
		collider.AreaEntered += OnAreaEntered;
		Tray = GetNode<Tray>("%Tray");
		Task = GetNode<TaskGenerator>("/root/Game/TaskGenerator");
	}

	public override void _Process(double delta)
	{
		if (Tray.TrayItems.ContainsValue(true))
		{

			Tray.Visible = true;
		}
		else
		{
			Tray.Visible = false;
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
			if (Tray.TrayItems.ContainsValue(true))
			{

				DeliveryAttempts++;
				GD.Print($"Delivery Attempts: {DeliveryAttempts}");

				// If what is on the tray matches what is requested
				if (Tray.TrayItems.OrderBy(x => x.Key).SequenceEqual(Task.TrayItems.OrderBy(x => x.Key)))
				{
					GameInstance.AwardPoints(1000);
					// Grow Hat
					HatTopper topper = new()
					{
						GlobalPosition = new(216, 143 - (hatCounter * 2))
					};
					GetTree().Root.AddChild(topper);
					hatCounter++;
					// Clear task when order is delivered.
					Task.WriteTray(new(){
							{"drink", false},
							{"plate", false},
							{"bowl", false},
						});
						DeliveryAttempts = 0;
						GD.Print("Delivery Attempts Reset to 0 due to perfect meal");
				}
				else
				{
					// Award Points
					switch (Tray.TrayItems.Count(x => x.Value is true))
					{
						case 1:
							GameInstance.AwardPoints(10);
							break;

						case 2:
							GameInstance.AwardPoints(20);
							break;
						case 3:
							GameInstance.AwardPoints(100);
							// Create a new Task when full meal is delivered.
							Task.WriteTray(Task.NewTask().TrayItems);
						DeliveryAttempts = 0;
						GD.Print("Delivery Attempts Reset to 0 due to New Task");
							break;
						default:
							GD.PrintErr("This should never happen!");
							break;
					}
					if (DeliveryAttempts > 2)
					{
						// Clear task if failed delivery in 3 attempts.
						Task.WriteTray(new(){
							{"drink", false},
							{"plate", false},
							{"bowl", false},
						});
						
						DeliveryAttempts = 0;
						GD.Print("Delivery Attempts Reset to 0 due to timeout");
					}
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
				foreach (string foodName in Tray.TrayItems.Keys)
				{
					Tray.TrayItems[foodName] = false;
				}
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "default";
				Tray.Visible = false;
			}
		}

		// If Waiter is at counter
		if (@event.IsActionPressed("MoveUp") && Step == 0)
		{
			// Flip the sprite to face the counter
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			Tray counter = GetNode<Tray>("/root/Game/Conveyors/Counter/Tray");


			// Logic for storing items on the counter.
			if (Tray.TrayItems.ContainsValue(true)) // If the Waiter is holding food
			{
				List<string> movedItems = new();

				// Find which items can be moved and move them
				List<string> moveable = Tray.TrayItems.Where(v => v.Value is true).Select(k => k.Key).ToList();
				counter.AddToTray(moveable, out movedItems);

				// remove moved items
				Tray.RemoveFromTray(movedItems);

				// Update animation and tray visibility
				if (!Tray.TrayItems.ContainsValue(true))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "default";
					Tray.Visible = false;
				}
			}

			// Logic for retrieving items from the counter.
			else if (!Tray.TrayItems.ContainsValue(true)) // If there are no items on the tray.
			{
				List<string> movedItems = new();

				// Find which items can be moved and move them
				List<string> moveable = counter.TrayItems.Where(v => v.Value is true).Select(k => k.Key).ToList();
				Tray.AddToTray(moveable, out movedItems);

				// remove moved items
				counter.RemoveFromTray(movedItems);

				// Update animation and tray visibility
				if (Tray.TrayItems.ContainsValue(true))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "carry";
					Tray.Visible = true;
				}
			}
		}

	}
	private async void OnAreaEntered(Node2D body)
	{
		Debug.Assert(body.GetParent().GetType() == typeof(Food), "Body must be of type Food");
		newFood = (Food)body.GetParent();


		if (Tray.TrayItems.ContainsValue(false)) // If we have space on our tray
		{
			// is the newFood already on the tray?
			// If yes, skip the rest of the function.
			if (Tray.TrayItems[newFood.foodName]) return;

			// Only run if food collected is unique to the tray.
			// Disable collider to prevent further funny business.
			collider.AreaEntered -= OnAreaEntered;

			// Add newFood to the tray.
			Tray.AddToTray(new() { newFood.foodName }, out _);

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
