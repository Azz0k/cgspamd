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
        public static bool IsEqualWithWildcard(string senderDomain, string domainPattern)
        {
            string regexPattern = $"^{Regex.Escape(domainPattern).Replace(@"\*", ".*")}$";
            Regex regex = new(regexPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(senderDomain);

        }
        public static bool isAddUserRequestValid(AddUserRequest request)
        {
            if (request.FullName.Trim().Length == 0) return false;
            if (request.Password.Length < 8) return false;
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
        public static bool isEmailValid(string? email)
        {
            if (email == null) return false;
            if (email.Length == 0)   return false;
            if (email.IndexOf('@') == -1) return false;
            if (email.IndexOf('.') == -1) return false;
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
        public static bool EnsureFileExists(string file)
        {
            FileInfo fileInfo = new(file);
            if (!fileInfo.Exists)
            {
                return false;
            }
            return true;
        }
        public static string? ReadAsciiLine(BinaryReader br)
        {
            using var ms = new MemoryStream();
            while (true)
            {
                int b;
                try { b = br.ReadByte(); }
                catch { return null; }

                if (b == '\n') break;
                if (b == '\r') continue;
                ms.WriteByte((byte)b);
            }
            return Encoding.ASCII.GetString(ms.ToArray());
        }
        public static string? GetRecipient(string line)
        {
            string pattern = @".*<(.*)>";
            if (line.StartsWith("R W "))
            {
                Match regexMatch = Regex.Match(line, pattern);
                if (regexMatch.Success)
                {
                    string recipient = regexMatch.Groups[1].Value;
                    return recipient.Trim();
                }
            }
            return null;
        }
        public static string? GetSender(string line)
        {
            string pattern = @".*<(.*)>";
            if (line.StartsWith("P I "))
            {
                Match regexMatch = Regex.Match(line, pattern);
                if (regexMatch.Success)
                {
                    string sender = regexMatch.Groups[1].Value;
                    return sender.Trim();
                }
            }
            return null;
        }
        public static string GetDomaInEmail(string email)
        {
            return email.Substring(email.IndexOf('@') + 1);
        }

    }
}
