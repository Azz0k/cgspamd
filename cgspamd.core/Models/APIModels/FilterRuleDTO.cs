using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models.APIModels
{
    public class FilterRuleDTO
    {
        public int Id { get; set; }
        public required string Value { get; set; }
        public required string Comment { get; set; }
        public required int Type { get; set; }
        public required string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public required string CreatedByUserName { get; set; }
        public string? UpdatedByUserName { get; set; }

    }
}
