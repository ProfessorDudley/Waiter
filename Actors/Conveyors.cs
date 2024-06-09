using Godot;
using System;

public partial class Conveyors : Node2D
{
  [Export] private float[] Belts = { 224f, 296f, 370f };
  public Timer timer;
  private readonly Random random = new();

  public override void _Ready()
  {
    timer = GetNode<Timer>("SpawnTimer");
    timer.Timeout += SpawnFood;

  }

  private void SpawnFood()
  {
    Food instance = (Food)GD.Load<PackedScene>("res://Actors/Food.tscn").Instantiate();
    instance.Position = new Vector2(310, Belts[random.Next(0, 3)]);
    GetTree().Root.AddChild(instance);
    if (!(timer.WaitTime <= 1.0))
    {
      timer.WaitTime = Math.Round(timer.WaitTime -= 0.05, 2);
    }
    GD.Print(timer.WaitTime);
  }
}