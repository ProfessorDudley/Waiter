using Godot;
using Godot.NativeInterop;
using System;

public partial class Particle : CpuParticles2D
{
  public override void _Ready()
  {
	Emitting = true;
	Amount = 1;
	OneShot = true;
	Texture = GD.Load<Texture2D>("res://Assets/Images/Dollar.png");
	Direction = new(0, -1);
	Spread = 0;
	Gravity = new(0, 0);
	InitialVelocityMin = 20;
	InitialVelocityMax = 20;
	ColorRamp = new Gradient
	{
	  Offsets = new float[] { 0.0f, 0.4f, 0.8f, 1.0f },
	  Colors = new Color[] {
		new(1, 1, 1, 1),
		new(1, 1, 1, 1),
		new(1, 1, 1, 0),
		new(1, 1, 1, 0),
	  }
	};

	Timer timer = new Timer
	{
	  WaitTime = 1.0,
	  OneShot = true,
	  Autostart = true,
	};
	AddChild(timer);
	timer.Timeout += () =>
	{
	  QueueFree();
	};
	timer.Start();
  }
}
