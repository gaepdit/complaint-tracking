using System.IO;

namespace ComplaintTracking
{
    public static class FileTypes
    {
        public const string CsvContentType = "text/csv";
        public const string ZipContentType = "application/zip";

        public static bool FileUploadAllowed(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".txt":
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".svg":
                case ".pdf":
                case ".html":
                case ".htm":
                case ".csv":
                case ".xlsx":
                case ".xls":
                case ".rtf":
                case ".docx":
                case ".doc":
                case ".ppt":
                case ".pptx":
                    return true;
            }

            return false;
        }

        public static bool FilenameImpliesImage(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".bmp":
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return true;
            }

            return false;
        }

        public static string GetContentType(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".txt":
                    return "text/plain";

                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                case ".xls":
                    return "application/ms-excel";

                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                case ".doc":
                    return "application/msword";

                case ".pdf":
                    return "application/pdf";

                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                case ".bmp":
                    return "image/bmp";

                case ".gif":
                    return "image/gif";

                case ".svg":
                    return "image/svg+xml";

                case ".html":
                case ".htm":
                    return "text/html";

                case ".rtf":
                    return "application/rtf";

                case ".zip":
                    return ZipContentType;

                case ".csv":
                    return CsvContentType;

                case ".ppt":
                    return "application/vnd.ms-powerpoint";

                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";

            }

            return "application/octet-stream";
        }
    }
}