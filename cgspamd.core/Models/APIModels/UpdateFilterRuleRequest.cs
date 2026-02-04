using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class UpdateFilterRuleRequest
    {
        public required int Id { get; set; }
        public required string Value { get; set; }
        public required string Comment { get; set; }
    }
}
