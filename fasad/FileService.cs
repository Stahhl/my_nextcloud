namespace fasad;

public class FileService
{
    const string FileName = "input.txt";
    public readonly byte[] Bytes;

    public FileService()
    {
        if (!File.Exists(FileName)) throw new FileNotFoundException(FileName);

        Bytes = File.ReadAllBytes(FileName);
        
        Console.WriteLine("READ BYTES");
    }
}