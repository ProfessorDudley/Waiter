using Godot;
using System;

public partial class HatTopper : Sprite2D
{
    public override void _Ready()
    {
      Texture = GD.Load<Texture2D>("res://Assets/Images/HatTopper.png");
    }
}
