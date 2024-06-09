using Godot;
using System;

public partial class GameOverScreen : Control
{
  private Button MenuButton;

    public override void _Ready()
    {
        // Setup buttons and their events
        MenuButton = GetNode<Button>("VBoxContainer/Button");
        MenuButton.Pressed += OnPressedButton;
    }


    private void OnPressedButton()
    {
        GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
    }
}
