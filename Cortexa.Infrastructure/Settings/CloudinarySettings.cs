using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Settings
{
    public class CloudinarySettings
    {
        public required string CloudName { get; set; }
        public required string ApiKey { get; set; }
        public required string ApiSecret { get; set; }
    }
}
