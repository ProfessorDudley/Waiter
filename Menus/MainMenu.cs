using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    private Button PlayButton;
    private Button ExitButton;

    public override void _Ready()
    {
        // Setup buttons and their events
        PlayButton = GetNode<Button>("%PlayButton");
        PlayButton.Pressed += OnPressedPlay;

        ExitButton = GetNode<Button>("%ExitButton"); 
        ExitButton.Pressed += OnPressedExit;
    }

    private void OnPressedExit()
    {
        GetTree().Quit();
    }


    private void OnPressedPlay()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

}
