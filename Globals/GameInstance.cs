using Godot;
using System;

public partial class GameInstance : Node
{
  private static int Score = 0;


  public override void _Ready()
  {
    GD.PrintRich("[b]GameInstance.cs Loaded[/b]");

  }

  // public override void _Input(InputEvent @event)
  // {
  //     if (@event.IsActionPressed("ui_accept"))
  //     {
  //       GameOver();
  //     }
  // }

  public static void AwardPoints()
  {
    Score += 10;
    GD.PrintRich(Score);
  }

  public void GameOver()
  {
    Node instance = GD.Load<PackedScene>("res://Menus/GameOverScreen.tscn").Instantiate();
    GetTree().Root.AddChild(instance);
    GetNode<Conveyors>("/root/Game/Conveyors").timer.Stop();
    GetNode<Player>("/root/Game/Player").SetProcessInput(false);
    foreach (var node in GetTree().GetNodesInGroup("Foods"))
    {
      node.QueueFree();
    }
  }
}

