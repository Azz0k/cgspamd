using cgspamd.core.Enums;
using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace cgspamd.core.Utils
{
    public static class Utils
    {
        public static bool isAddUserRequestValid(AddUserRequest request)
        {
            if (request.FullName.Trim().Length == 0) return false;
            if (request.Password.Length<8) return false;
            string pattern = @"[a-z\.]+";
            var match = Regex.Match(request.UserName, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
        public static bool isUpdateUserRequestValid(UpdateUserRequest request)
        {
            if (request.Id <= 0) return false;
            if (request.FullName.Trim().Length == 0) return false;
            if (request.Password != null && request.Password.Length < 8) return false;
            string pattern = @"[a-z\.]+";
            var match = Regex.Match(request.UserName, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
        public static bool isAddRuleRequestValid(AddFilterRuleRequest request)
        {
            return Enum.IsDefined(typeof(FilterRulesType), request.Type);
        }
        public static bool isUpdateRuleRequestValid(UpdateFilterRuleRequest request)
        {
            if (request.Id <= 0) return false;
            return true;
        }
        public static string GenerateNowDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        public static int GetId(HttpContext context)
        {
            return int.Parse(context.User.Claims.First(x => x.Type == "Id").Value);
        }
    }
}
