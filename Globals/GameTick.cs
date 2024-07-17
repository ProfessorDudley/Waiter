using Godot;
using System;

public partial class GameTick : Timer
{
  public override void _Ready()
  {
    // Confirm setup for timer.
    ProcessCallback = TimerProcessCallback.Idle;
    WaitTime = 2.0f;
    OneShot = false;
    Autostart = false;

    // Set up timer callback to speed up the conveyors.
    Timeout += SpeedUp;
  }

/// <summary>
/// If the tick rate is faster slower then 1 second, increase the rate by 0.05 seconds.
/// </summary>
  private void SpeedUp()
  {
    if (!(WaitTime <= 1.25f))
    {
      WaitTime = Math.Round(WaitTime -= 0.025f, 3);
    }
  }

}
