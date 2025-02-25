using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ZCLOUD.TaskEv.Components.Pages;
public partial class TaskDocuments
{
    [Parameter]
    public int TaskId { get; set; }

    private List<Document> documents = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDocuments();
    }

    private async Task LoadDocuments()
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        documents = await context.Documents
            .Where(d => d.TaskId == TaskId)
            .ToListAsync();
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        try
        {
            using var context = await ContextFactory.CreateDbContextAsync();

            var file = e.File;
            var uploadPath = Path.Combine(Environment.WebRootPath, "uploads", TaskId.ToString());
            Directory.CreateDirectory(uploadPath);

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.Name);
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = file.OpenReadStream(maxAllowedSize: 10485760); // 10MB limit
            await using var fs = File.Create(filePath);
            await stream.CopyToAsync(fs);

            var document = new Document
            {
                TaskId = TaskId,
                FileName = file.Name,
                FilePath = fileName,
                UploadDate = DateTime.Now
            };

            context.Documents.Add(document);
            await context.SaveChangesAsync();
            documents.Add(document);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Chyba pøi nahrávání: {ex.Message}");
        }
    }

    private async Task DownloadDocument(Document doc)
    {
        var filePath = Path.Combine(Environment.WebRootPath, "uploads", TaskId.ToString(), doc.FilePath);
        if (!File.Exists(filePath))
        {
            await JSRuntime.InvokeVoidAsync("alert", "Soubor nebyl nalezen.");
            return;
        }

        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var base64 = Convert.ToBase64String(fileBytes);

        await JSRuntime.InvokeVoidAsync("downloadFile", doc.FileName, base64);
    }

    private async Task DeleteDocument(Document doc)
    {
        try
        {
            using var context = await ContextFactory.CreateDbContextAsync();

            var filePath = Path.Combine(Environment.WebRootPath, "uploads", TaskId.ToString(), doc.FilePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            context.Documents.Remove(doc);
            await context.SaveChangesAsync();
            documents.Remove(doc);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Chyba pøi mazání: {ex.Message}");
        }
    }
}
