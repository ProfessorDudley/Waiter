using Godot;
using System;
public partial class Conveyors : Node2D
{
  [Export] private float[] Belts = { 60.0f, 94.0f, 128.0f };
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
    instance.GlobalPosition = new Vector2(14.0f, Belts[random.Next(0, 3)]);
    instance.AddToGroup("Foods");
    GetTree().Root.AddChild(instance);
    
    timer.Timeout += instance.AdvanceStep;
    if (!(timer.WaitTime <= 1.0f))
    {
      timer.WaitTime = Math.Round(timer.WaitTime -= 0.05f, 2);
    }
    GD.Print(timer.WaitTime);
  }
}

public static class FoodSpawner
{

}