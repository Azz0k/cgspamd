using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class AddFilterRuleRequest
    {
        public required string Value { get; set; }
        public required string Comment { get; set; }
        public required int Type { get; set; }
    }
}
