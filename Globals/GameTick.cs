using Godot;
using System;

public partial class GameTick : Timer
{
  public override void _Ready()
  {
    // Confirm setup for timer.
    ProcessCallback = TimerProcessCallback.Idle;
    WaitTime = 2.5;
    OneShot = false;
    Autostart = true;

    // Set up timer callback to speed up the conveyors.
    Timeout += SpeedUp;
  }

/// <summary>
/// If the tick rate is faster slower then 1 second, increase the rate by 0.05 seconds.
/// </summary>
  private void SpeedUp()
  {
    if (!(WaitTime <= 0.9f))
    {
      WaitTime = Math.Round(WaitTime -= 0.05f, 2);
    }

    GD.Print(WaitTime);
  }

}
