using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextCloudUpload
{
    public class NextCloudOptions
    {
        public string BaseUrl { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}