using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Godot;
using System.Text;
using File = System.IO.File;

public class Input : TextEdit
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private FileDialog fileDialogLoad;
    public byte[] GetDataByte = {};

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetDataByte = Text.ToUTF8();
        fileDialogLoad = GetNode<FileDialog>("/root/Control/Panel/FileDialog");
    }

    private void _on_Button4_pressed()
    {
        fileDialogLoad.SetMode(FileDialog.ModeEnum.OpenFile);
        fileDialogLoad.Popup_();
    }

    private void _on_FileDialog_file_selected(string path)
    {
        Text = "";
        var content = File.ReadAllBytes(path);
        Text = Encoding.UTF8.GetString(content);
        GetDataByte = content;
    }

    private void _on_TextEdit_text_changed()
    {
        GetDataByte = Text.ToUTF8();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
