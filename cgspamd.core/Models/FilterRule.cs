using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Models
{
    public class FilterRule
    {
        public int Id { get; set; }
        public required string Value { get; set; }
        public required string Comment { get; set; }
        public required int Type { get; set; }
        public required string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public required int CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public User? UpdatedByUser { get; set;}
    }
}
