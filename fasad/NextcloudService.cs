using CliWrap;
using fasad.Models;

namespace fasad;

public class NextcloudService
{
    private readonly HttpClient _client;

    public NextcloudService(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> CreateArende(string arendenummer)
    {
        var msg1 = new HttpRequestMessage(new HttpMethod("MKCOL"), $"files/admin/arende/{arendenummer}");
        var msg2 = new HttpRequestMessage(new HttpMethod("MKCOL"), $"files/admin/arende/{arendenummer}/arbetshandlingar");
        var msg3 = new HttpRequestMessage(new HttpMethod("MKCOL"), $"files/admin/arende/{arendenummer}/allmänna_handlingar");

        var res1 = await _client.SendAsync(msg1);
        if (!res1.IsSuccessStatusCode)
        {
            var body = await res1.Content.ReadAsStringAsync();
            Console.WriteLine($"ERROR-1: {body}");
            return false;
        }
        
        var res2 = await _client.SendAsync(msg2);
        if (!res2.IsSuccessStatusCode)
        {
            var body = await res1.Content.ReadAsStringAsync();
            Console.WriteLine($"ERROR-2: {body}");
            return false;
        }
        
        var res3 = await _client.SendAsync(msg3);
        if (!res3.IsSuccessStatusCode)
        {
            var body = await res1.Content.ReadAsStringAsync();
            Console.WriteLine($"ERROR-3: {body}");
            return false;
        }

        return true;
    }

    public async Task<bool> UploadDocument(UploadRequest request)
    {
        var res = await _client.PutAsync($"files/admin/arende/{request.Arendenummer}/{request.HandlingsTyp}/{request.FileName}", new ByteArrayContent(request.GetBytes()));
        
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"ERROR: {body}");
            return false;
        }

        return true;
    }

    public async Task<bool> ConvertToAllmanHandling(ConvertRequest request)
    {
        var res = await _client.GetAsync($"files/admin/arende/{request.Arendenummer}/arbetshandlingar/{request.FileName}");
        var docxBytes = await res.Content.ReadAsByteArrayAsync();
        Console.WriteLine($"RES: {docxBytes.Length}");

        var tempDir = Directory.CreateTempSubdirectory().FullName;
        Console.WriteLine($"tempDir: {tempDir}");
        
        var docxPath = Path.Join(tempDir, request.FileName);
        await File.WriteAllBytesAsync(docxPath, docxBytes);

        var fileNameWithoutExtension = request.FileName.TrimEnd(".docx".ToCharArray());
        var pdfPath = Path.Join(tempDir, $"{fileNameWithoutExtension}.pdf");
        
        var result = await Cli.Wrap("pandoc")
            .WithArguments([docxPath, "-o", pdfPath])
            .ExecuteAsync();
        
        Console.WriteLine($"result: {result.ExitCode}");
        
        return true;
    }
}