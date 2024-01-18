using System.ComponentModel;

namespace fasad.Models;

public class UploadRequest
{
    public required string Arendenummer { get; set; }
    [DefaultValue("arbetshandlingar")]
    public required string HandlingsTyp { get; set; }
    public required string FileName { get; set; }
    [DefaultValue("SGVsbG8gV29ybGQh")]
    public required string Base64Content { get; set; }

    public byte[] GetBytes()
    {
        if (Base64Content == FileName)
        {
            return File.ReadAllBytes(Path.Join("Input", FileName));
        }
        
        return Convert.FromBase64String(Base64Content);
    }
}