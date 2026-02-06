using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class UserLoginRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
