using cgspamd.core.Models.APIModels;
using System;
using System.Collections.Generic;
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
    }
}
