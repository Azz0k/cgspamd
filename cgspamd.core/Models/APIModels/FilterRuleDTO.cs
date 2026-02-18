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

        public static explicit operator FilterRuleDTO(FilterRule v)
        {
            return new() 
            {
                Id = v.Id,
                Value = v.Value,
                Comment = v.Comment,
                Type = v.Type,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                CreatedByUserName = v.CreatedByUser.UserName,
                UpdatedByUserName = v.UpdatedByUser?.UserName
            };
        }
    }
}
