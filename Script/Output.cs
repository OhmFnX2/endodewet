using Godot;
using System.Text;

public class Output : TextEdit
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private TextEdit input;
    private LineEdit key;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        input = (TextEdit)GetNode("/root/Control/Panel/TextEdit");
        key = (LineEdit)GetNode("/root/Control/Panel/LineEdit");
    }

    private void _on_Button2_pressed()
    {
        var crypto = new Crypto();
        var key1 = new CryptoKey();
        byte[] plainText = input.Text.ToUTF8();

        key1.LoadFromString(key.Text, true);

        var encrypted = crypto.Encrypt(key1, plainText);

        this.Text = Encoding.UTF8.GetString(encrypted);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
