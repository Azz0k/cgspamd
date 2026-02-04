using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public bool Enabled { get; set; }
        public bool IsAdmin { get; set; }
    }
}
