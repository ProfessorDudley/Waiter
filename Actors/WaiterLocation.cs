using Godot;
using System.Collections.Generic;

public partial class WaiterLocation : Marker2D
{
  private List<Food> _foods  = new();

  // [Export] private bool isTable;
  public List<Food> Foods
  {
    get
    {
      return _foods;
    }
    set
    {
      _foods = value;
      foreach (Food food in _foods)
      {
        GetNode<AnimatedSprite2D>($"/root/Game/SlotsRoot/{food.foodName}").Modulate = new Color(1, 1, 1, 1);
      }

    }
  }

  public void EmptyCounter()
  {
    foreach (Food food in _foods)
    {
      GetNode<AnimatedSprite2D>($"/root/Game/SlotsRoot/{food.foodName}").Modulate = new Color(1, 1, 1, 1);
      Foods.Clear();
    }
  }
}