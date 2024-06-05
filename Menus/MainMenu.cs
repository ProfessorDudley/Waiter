using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    private Button PlayButton;
    private Button ExitButton;

    public override void _Ready()
    {
        // Setup buttons and their events
        PlayButton = GetNode<Button>("VBoxContainer/PlayButton");
        PlayButton.Pressed += OnPressedPlay;

        ExitButton = GetNode<Button>("VBoxContainer/ExitButton"); 
        ExitButton.Pressed += OnPressedExit;
    }

    private void OnPressedExit()
    {
        GetTree().Quit();
    }


    private void OnPressedPlay()
    {
        GetTree().ChangeSceneToFile("res://scenes/playground.tscn");
    }

}
