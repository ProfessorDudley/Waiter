using Godot;
using System;

public partial class Conveyors : Node2D
{
  private float[] Belts = { 60.0f, 94.0f, 128.0f };
  private readonly Random random = new();

  public override void _Ready()
  {
    GameTick tick = GetNode<GameTick>("/root/GameTick");
    tick.Timeout += SpawnFood;

  }

  /// <summary>
  /// Selects from the list of conveyor belts and spawns a food actor at the start.
  /// </summary>
  private void SpawnFood()
  {
    Food instance = (Food)GD.Load<PackedScene>("res://Actors/Food.tscn").Instantiate();
    instance.GlobalPosition = new Vector2(10.0f, Belts[random.Next(0, 3)]);
    instance.AddToGroup("Foods");
    GetTree().Root.AddChild(instance);
  }
}