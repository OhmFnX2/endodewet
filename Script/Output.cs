using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Godot;
using System.Text;

public class Output : TextEdit
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    //private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwsyz";
    private FileDialog fileDialogLoad;
    private TextEdit input;
    private LineEdit key;
    private byte[] GetOutByte; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fileDialogLoad = GetNode<FileDialog>("/root/Control/Panel/FileDialog");
        input = (TextEdit)GetNode("/root/Control/Panel/TextEdit");
        key = (LineEdit)GetNode("/root/Control/Panel/LineEdit");
    }

    private void _on_Button2_pressed()
    {
        int? keySize = null;
        var key1 = CreateMd5Hash(key.Text);

        var aes = Aes.Create();

        if (keySize.HasValue &&
            keySize.Value > 0)
        {
            aes.KeySize = keySize.Value;
        }
        else if (aes.LegalKeySizes?.Length > 0)
        {
            aes.KeySize = aes.LegalKeySizes
                .Max(n => n.MaxSize);
        }

        var encryptor = aes.CreateEncryptor(key1, aes.IV);
        var memoryStream = new MemoryStream();
        var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(Encoding.Default.GetString(input.Get("GetDataByte") as byte[]));
        }

        var iv = aes.IV;
        var bytes = memoryStream.ToArray();
        var result = new byte[iv.Length + bytes.Length];

        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(bytes, 0, result, iv.Length, bytes.Length);
        
        GetOutByte = result;
        Text = Convert.ToBase64String(result);
    }
    
    private static byte[] CreateMd5Hash(string input)
    {
        var md5 = MD5.Create();

        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        
        GD.Print(sb.ToString());
    
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private void _on_Button3_pressed()
    {       
        var key1 = CreateMd5Hash(key.Text);
        var fullCipher = Convert.FromBase64String(Encoding.Default.GetString(input.Get("GetDataByte") as byte[]));
        var iv = new byte[16];
        var cipher = new byte[fullCipher.Length - iv.Length];
        int? keySize = null;
        try
        {
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            string result;

            var aes = Aes.Create();

            if (keySize.HasValue &&
                keySize.Value > 0)
            {
                aes.KeySize = keySize.Value;
            }
            else if (aes.LegalKeySizes?.Length > 0)
            {
                aes.KeySize = aes.LegalKeySizes
                    .Max(n => n.MaxSize);
            }

            var decryptor = aes.CreateDecryptor(key1, iv);
            var memoryStream = new MemoryStream(cipher);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            using (var streamReader = new StreamReader(cryptoStream))
            {
                result = streamReader.ReadToEnd();
            }

            GetOutByte = result.ToUTF8();
            Text = result;
        }
        catch (Exception _)
        {
            GetOutByte = "***Key Not True***".ToUTF8();
            Text = "***Key Not True***";
        }
    }

    private void _on_FileDialog_file_selected(string path)
    {
        var file = new Godot.File();
        file.Open(path, Godot.File.ModeFlags.Write);
        file.StoreBuffer(GetOutByte);
        file.Close();
    }

    private void _on_Button5_pressed()
    {
        fileDialogLoad.SetMode(FileDialog.ModeEnum.SaveFile);
        fileDialogLoad.Popup_();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
