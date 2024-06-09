using Godot;
using System;
using System.Diagnostics;

public partial class WaiterLocation : Marker2D, IConveyor
{
  [Export] private bool isTable;

  public void OnConveyorOverlap(Node body)
  {
    Debug.Assert(body.GetType() == typeof(Player), "body must be the player");
  }

}

interface IConveyor
{
  void OnConveyorOverlap(Node body);
}