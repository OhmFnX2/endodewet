using Godot;
using System;
using System.Linq;

public class Key : Godot.LineEdit
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private static Random random = new Random();

	public static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwsyz";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());
	}


	private void _on_Button_pressed()
	{
		this.Text = RandomString(32);
	}

}
