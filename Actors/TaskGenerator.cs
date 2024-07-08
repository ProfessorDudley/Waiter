using Godot;
using System;
using System.Collections.Generic;

public partial class TaskGenerator : Tray
{

  public Tray NewTask()
	{
		Random random = new();
		Tray task = new();

		foreach (string foodName in task.TrayItems.Keys)
		{
			task.TrayItems[foodName] = Convert.ToBoolean(random.Next(2));
		}
		return task;
	}

  public void WriteTray(Dictionary<string, bool> Tray)
  {
    TrayItems = Tray;
  }
}
