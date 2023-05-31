using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace ASFEnhance.Profile;
public class MultipartFormContent : MultipartContent
{
    private const string formData = "form-data";

    public MultipartFormContent()
        : base(formData)
    {
    }

    public MultipartFormContent(string boundary)
        : base(formData, boundary)
    {
    }

    public void Add(HttpContent content, string name)
    {
        AddInternal(content, name, null);
    }

    public void Add(HttpContent content, string name, string fileName)
    {
        AddInternal(content, name, fileName);
    }

    private void AddInternal(HttpContent content, string name, string? fileName)
    {
        if (content.Headers.ContentDisposition == null)
        {
            var header = new ContentDispositionHeaderValue(formData)
            {
                Name = name,
                FileName = fileName,
                FileNameStar = fileName,
            };
        }
        base.Add(content);
    }
}
