using cgspamd.core.Context;
using cgspamd.core.Models;
using cgspamd.core.Models.APIModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static cgspamd.core.Utils.Utils;

namespace cgspamd.core.Application
{
    public class FilterRulesApplication
    {
        private StoreDbContext db;
        public FilterRulesApplication(StoreDbContext storeDbContext) 
        {
            db = storeDbContext;
        }

        public async Task<List<FilterRuleDTO>> GetAllRecordsAsync()
        {
            return await db.FilterRules.Select(x => new FilterRuleDTO()
            {
                Id = x.Id,
                Value = x.Value,
                Comment = x.Comment,
                Type = x.Type,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                CreatedByUserName = x.CreatedByUser.UserName,
                UpdatedByUserName = x.UpdatedByUser == null ? null : x.UpdatedByUser.UserName
            }).ToListAsync();
        }
        public async Task<(int, FilterRuleDTO?)> AddAsync(int userId, AddFilterRuleRequest request)
        {
            if (!isAddRuleRequestValid(request))
            {
                return (400, null);
            }
            FilterRule? existingRule = await db.FilterRules.FirstOrDefaultAsync(rule => rule.Value == request.Value && rule.Type == request.Type);
            if (existingRule != null)
            {
                return (409, null);
            }
            User? user = await db.Users.FindAsync(userId);
            if (user == null)
            {
                return (403, null);
            }
            FilterRule newRule = new() 
            {
                Value = request.Value,
                Comment = request.Comment,
                Type = request.Type,
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                CreatedAt = GenerateNowDate()
            };
            await db.FilterRules.AddAsync(newRule);
            await db.SaveChangesAsync();
            return (201, (FilterRuleDTO) newRule);
        }
        public async Task<(int,FilterRuleDTO?)> UpdateAsync(int userId, UpdateFilterRuleRequest request)
        {

            if (!isUpdateRuleRequestValid(request))
            {
                return (400, null);
            }
            FilterRule? existingRule = await db.FilterRules.FindAsync(request.Id);
            if (existingRule == null)
            {
                return (404,null);
            }
            User? user = await db.Users.FindAsync(userId);
            if (user == null)
            {
                return (403, null);
            }
            existingRule.Value = request.Value;
            existingRule.Comment = request.Comment;
            existingRule.UpdatedByUser = user;
            existingRule.UpdatedByUserId = user.Id;
            existingRule.UpdatedAt = GenerateNowDate();
            await db.SaveChangesAsync();
            return (200, (FilterRuleDTO) existingRule);
        }
        public  async Task<int> DeleteAsync(int id) 
        {
            if (id <= 0) return 400;
            FilterRule? rule = await db.FilterRules.FindAsync(id);
            if (rule == null) return 404;
            db.FilterRules.Remove(rule);
            await db.SaveChangesAsync();
            return 204;
        }
    }
}
