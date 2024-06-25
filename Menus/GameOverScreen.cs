using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
  private Button MenuButton;

    public override void _Ready()
    {
        GetNode<Label>("%Score").Text = $"Score: {GameInstance.Score}";
        // Setup buttons and their events
        MenuButton = GetNode<Button>("%Button");
        MenuButton.Pressed += OnPressedButton;
    }


    private void OnPressedButton()
    {
        GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
        GameInstance.Score = 0;
        QueueFree();
    }
}
