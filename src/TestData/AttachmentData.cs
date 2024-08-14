using Cts.Domain.Entities.Attachments;
using Cts.TestData.Identity;

namespace Cts.TestData;

public static class AttachmentData
{
    private static IEnumerable<Attachment> AttachmentSeedItems => new List<Attachment>
    {
        new(new Guid("20000000-0000-0000-0000-000000000001")) // 0
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            FileName = "FileOne.png",
            FileExtension = ".png",
            Size = 1,
            UploadedDate = DateTimeOffset.Now.AddDays(-2),
            UploadedBy = UserData.GetUsers.ElementAt(1),
            IsImage = true,
        },
        new(new Guid("20000000-0000-0000-0000-000000000002")) // 1
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            FileName = "File Two.svg",
            FileExtension = ".svg",
            Size = 10,
            UploadedDate = DateTimeOffset.Now.AddDays(-1),
            UploadedBy = UserData.GetUsers.ElementAt(1),
            IsImage = false,
        },
        new(new Guid("20000000-0000-0000-0000-000000000003")) // 2
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            FileName = "File-Three-💻.pdf",
            FileExtension = ".pdf",
            Size = 100000,
            UploadedBy = UserData.GetUsers.ElementAt(2),
            UploadedDate = DateTimeOffset.Now.AddDays(-1),
        },
        new(new Guid("20000000-0000-0000-0000-000000000004")) // 3
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            FileName = "File-Four-Empty-File.png",
            FileExtension = ".png",
            Size = 1000,
            UploadedDate = DateTimeOffset.Now.AddDays(-3),
            UploadedBy = UserData.GetUsers.ElementAt(0),
            IsImage = true,
        },
        new(new Guid("20000000-0000-0000-0000-000000000005")) // 4
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            FileName = "File-Five-Attachment-Deleted.pdf",
            FileExtension = ".pdf",
            Size = 1000,
            UploadedDate = DateTimeOffset.Now.AddDays(-3),
            UploadedBy = UserData.GetUsers.ElementAt(0),
        },
        new(new Guid("20000000-0000-0000-0000-000000000006")) // 5
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(3),
            FileName = "File-Six-Complaint-Deleted.pdf",
            FileExtension = ".pdf",
            Size = 1000,
            UploadedDate = DateTimeOffset.Now.AddDays(-3),
            UploadedBy = UserData.GetUsers.ElementAt(0),
        },
        new(new Guid("20000000-0000-0000-0000-000000000007")) // 6
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(1),
            FileName = "File-Seven-Attachment-Deleted.pdf",
            FileExtension = ".pdf",
            Size = 1000,
            UploadedDate = DateTimeOffset.Now.AddDays(-3),
            UploadedBy = UserData.GetUsers.ElementAt(0),
        },
    };

    private static IEnumerable<Attachment>? _attachments;

    public static IEnumerable<Attachment> GetAttachments
    {
        get
        {
            if (_attachments is not null) return _attachments;

            _attachments = AttachmentSeedItems.ToList();
            _attachments.ElementAt(4).SetDeleted("20000000-0000-0000-0000-000000000002");
            _attachments.ElementAt(6).SetDeleted("20000000-0000-0000-0000-000000000002");
            return _attachments;
        }
    }

    public static void ClearData()
    {
        _attachments = null;
        _attachmentFiles = null;
    }

    private static IEnumerable<AttachmentFile> AttachmentFilesSeedItems
    {
        get
        {
            const string attachmentsPath = "UserFiles/Attachments/20";
            const string thumbnailsPath = "UserFiles/Thumbnails/20";
            return new List<AttachmentFile>
            {
                new("20000000-0000-0000-0000-000000000001.png", attachmentsPath, EncodedPngFile),
                new("20000000-0000-0000-0000-000000000001.png", thumbnailsPath, EncodedPngFile),
                new("20000000-0000-0000-0000-000000000002.svg", attachmentsPath, EncodedSvgFile),
                new("20000000-0000-0000-0000-000000000003.pdf", attachmentsPath, EncodedPdfFile),
                new("20000000-0000-0000-0000-000000000004.png", attachmentsPath, Base64EncodedFile: null),
                new("20000000-0000-0000-0000-000000000004.png", thumbnailsPath, Base64EncodedFile: null),
                new("20000000-0000-0000-0000-000000000005.pdf", attachmentsPath, Base64EncodedFile: null),
                new("20000000-0000-0000-0000-000000000006.pdf", attachmentsPath, EncodedPdfFile),
                new("20000000-0000-0000-0000-000000000007.pdf", attachmentsPath, EncodedPdfFile),
            };
        }
    }

    private static IEnumerable<AttachmentFile>? _attachmentFiles;

    public static IEnumerable<AttachmentFile> GetAttachmentFiles()
    {
        if (_attachmentFiles is not null) return _attachmentFiles;
        _attachmentFiles = AttachmentFilesSeedItems;
        return _attachmentFiles;
    }

    #region Encoded binary data

    private const string EncodedPngFile =
        "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";

    private const string EncodedPdfFile =
        "JVBERi0xLjIKMSAwIG9iago8PD4+CnN0cmVhbQpCVC9GMSAyNCBUZiAxMCA4IFREIChIZWxsbyB3b3JsZCEpJyBFVAplbmRzdHJlYW0KZW5kb2JqCjQgMCBvYmoKPDwvVHlwZSAvUGFnZS9QYXJlbnQgMiAwIFIvQ29udGVudHMgMSAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9LaWRzIFs0IDAgUl0vQ291bnQgMS9UeXBlIC9QYWdlcy9NZWRpYUJveCBbMCAwIDI1MCA1MF0+PgplbmRvYmoKMyAwIG9iago8PC9QYWdlcyAyIDAgUi9UeXBlIC9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAzIDAgUj4+CiUlRU9G";

    private const string EncodedSvgFile =
        "PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyMSIgaGVpZ2h0PSIyMSIgdmlld0JveD0iMCAwIDIxIDIxIj48dGl0bGU+TWljcm9zb2Z0IExvZ288L3RpdGxlPjxyZWN0IHg9IjEiIHk9IjEiIHdpZHRoPSI5IiBoZWlnaHQ9IjkiIGZpbGw9IiNmMjUwMjIiLz48cmVjdCB4PSIxIiB5PSIxMSIgd2lkdGg9IjkiIGhlaWdodD0iOSIgZmlsbD0iIzAwYTRlZiIvPjxyZWN0IHg9IjExIiB5PSIxIiB3aWR0aD0iOSIgaGVpZ2h0PSI5IiBmaWxsPSIjN2ZiYTAwIi8+PHJlY3QgeD0iMTEiIHk9IjExIiB3aWR0aD0iOSIgaGVpZ2h0PSI5IiBmaWxsPSIjZmZiOTAwIi8+PC9zdmc+";

    #endregion
}

public record AttachmentFile(string FileName, string Path, string? Base64EncodedFile);
