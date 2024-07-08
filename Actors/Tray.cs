using Godot;
using System.Collections.Generic;

public partial class Tray : Node2D
{
    private Dictionary<string, bool> trayItems = new()
    {
      {"drink", false},
      {"plate", false },
      {"bowl", false },
    };

    public Dictionary<string, bool> TrayItems { get => trayItems; protected set => trayItems = value; }

    public override void _Process(double delta)
    {
      foreach (string foodName in TrayItems.Keys)
      {
        GetNode<AnimatedSprite2D>($"{foodName}").Modulate = new Color(1, 1, 1, TrayItems[foodName] ? 1 : 0.24f);
      }
    }

    public List<string> AddToTray(List<string> foodNames, out List<string> movedItems)
  {
    movedItems = new();
    foreach (string foodName in foodNames)
    {
      if (trayItems[foodName] == false) 
      {
        trayItems[foodName] = true;
        movedItems.Add(foodName);
      }
    }
    return movedItems;
    
  }
  public void RemoveFromTray(List<string> foodNames)
  {
    foreach (string foodName in foodNames)
    {
      if (trayItems[foodName] == true)
      {
        trayItems[foodName] = false;
      }
    }
  }
}
