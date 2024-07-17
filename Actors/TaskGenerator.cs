using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TaskGenerator : Tray
{

  public Tray NewTask()
	{
		Random random = new();
		Tray Task = new();
do
{
	foreach (string foodName in Task.TrayItems.Keys)
		{
			Task.TrayItems[foodName] = Convert.ToBoolean(random.Next(2));
		}
} while (!Task.TrayItems.Any(x => x.Value is true));
		
		return Task;
	}

  public void WriteTray(Dictionary<string, bool> Tray)
  {
    TrayItems = Tray;
  }
}
