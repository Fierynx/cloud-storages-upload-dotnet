using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileCloudUpload
{
    public class FileCloudOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}