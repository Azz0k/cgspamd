using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.helper.Models
{
    internal class AppSettings
    {
        public required string baseDir { get; init; }
        public required string ConnectionString { get; init; }
        public required string BadMessage { get; init; }

    }
}
