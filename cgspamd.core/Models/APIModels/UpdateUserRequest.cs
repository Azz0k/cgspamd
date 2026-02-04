using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class UpdateUserRequest
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public string? Password { get; set; }
        public required string FullName { get; set; }
        public required bool Enabled { get; set; }
        public required bool IsAdmin { get; set; }
    }
}
