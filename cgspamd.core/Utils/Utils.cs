using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace cgspamd.core.Utils
{
    public static class Utils
    {
#warning "TODO: Implement function."
        public static bool isAddUserRequestValid(AddUserRequest request)
        {
            return true;
        }
        public static bool isUpdateUserRequestValid(UpdateUserRequest request)
        {
            if (request.Id <= 0) return false;
            return true;
        }
        public static bool isAddRuleRequestValid(AddFilterRuleRequest request)
        {
            return true;
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
            return int.Parse(context.User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        }
    }
}
