using cgspamd.core.Applications;
using cgspamd.core.Enums;
using cgspamd.helper.Models;
using cgspamd.helper.Services;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Text;
using static cgspamd.core.Utils.Utils;

namespace cgspamd.helper.Applications
{
    internal class HelperApplication
    {
        private ConsoleOutputService _console;
        private AppSettings appSettings;
        private FilterRulesApplication app;
        public HelperApplication(ConsoleOutputService console, AppSettings appSettings, FilterRulesApplication application)
        {
            _console = console;
            this.appSettings = appSettings;
            app = application;
        }
        public async Task ProcessMessageAsync(string message, CancellationTokenSource source)
        {
            string[] messageParts = message.Split();
            string lineNumber = messageParts[0];
            string command = messageParts[1].ToLowerInvariant();
            switch (command)
            {
                case "quit":
                    source.Cancel();
                    _console.PrintGoodMessage(lineNumber);
                    Environment.Exit(0);
                    break;
                case "intf":
                    _console.Print($"{lineNumber} INTF 3");
                    break;
                case "file":
                    if (messageParts.Length != 3)
                    {
                        _console.PrintGoodMessage(lineNumber);
                        _console.PrintLogMessage("Error: wrong INTF format!");
                        return;
                    }
                    string fileName = messageParts[2];
                    var file = Path.Combine(appSettings.baseDir, fileName);
                    if (await EnsureMessageAllowed(file))
                    {
                        _console.PrintGoodMessage(lineNumber);
                    }
                    else
                    {
                        _console.PrintBadMessage(lineNumber);
                    }
                    break;
                default:
                    _console.PrintGoodMessage(lineNumber);
                    _console.PrintLogMessage($"Error: command {command} is not implemented");
                    break;
            }
        }
        private async Task<bool> EnsureMessageAllowed(string file)
        {
            if (!EnsureFileExists(file)) return true;
            try
            {
                using FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var br = new BinaryReader(fs, Encoding.ASCII, leaveOpen: true);
                string? line = null;
                while ((line = ReadAsciiLine(br)) != null)
                {
                    if (line == "") break;
                    if (await app.IsEmailExcludedAsync(GetRecipient(line)))
                    {
                        return true;
                    }
                    string? sender = GetSender(line);
                    bool? status = await GetEmailListStatusAsync(sender);
                    if (status.HasValue)
                    {
                        return status.Value;
                    }
                }
                var eml = MsgReader.Mime.Message.Load(fs);
                string textBody = "";
                string htmlBody = "";
                if (eml.TextBody != null && eml.TextBody.ContentType.CharSet != null)
                {
                    Encoding CodePage = Encoding.GetEncoding(eml.TextBody.ContentType.CharSet);
                    textBody = CodePage.GetString(eml.TextBody.Body);
                }
                if (eml.HtmlBody != null && eml.HtmlBody.ContentType.CharSet != null)
                {
                    Encoding CodePage = Encoding.GetEncoding(eml.HtmlBody.ContentType.CharSet);
                    htmlBody = CodePage.GetString(eml.HtmlBody.Body);
                }
                return !await ContainsProhibitedContentAsync(textBody, htmlBody); 
            }
            catch
            {
                _console.PrintLogMessage($"cannot process message {file}");
                return true;
            }
        }
        public async Task<bool?> GetEmailListStatusAsync(string? email)
        {
            if (!isEmailValid(email)) return null;
            string domain = GetDomaInEmail(email!);
            var whiteListAddress = app.IsEmailListedAsync(email!, FilterRulesType.whiteListSenderAddresses);
            var whiteListDomains = app.IsDomainListedAsync(domain, FilterRulesType.whiteListSenderDomains);
            var blackListAddress = app.IsEmailListedAsync(email!, FilterRulesType.whiteListSenderAddresses);
            var blackListDomains = app.IsDomainListedAsync(domain, FilterRulesType.whiteListSenderDomains);
            await Task.WhenAll(whiteListAddress, whiteListDomains, blackListAddress, blackListDomains);
            bool white = whiteListAddress.Result || whiteListDomains.Result;
            bool black = whiteListAddress.Result || whiteListDomains.Result;
            if (white) return true;
            if (black) return false;
            return null;
        }
        public async Task<bool> ContainsProhibitedContentAsync(string textBody, string htmlBody)
        {
            if (textBody == "" && htmlBody == "") return false;
            var textBodyContainsProhibitedText = app.ContainProhibitedTextAsync(textBody);
            var textBodyContainsProhibitedRegex = app.ContainProhibitedRegexAsync(textBody);
            var htmlBodyContainsProhibitedText = app.ContainProhibitedTextAsync(htmlBody);
            var htmlBodyContainsProhibitedRegex = app.ContainProhibitedRegexAsync(htmlBody);
            await Task.WhenAll(
                textBodyContainsProhibitedText,
                textBodyContainsProhibitedRegex,
                htmlBodyContainsProhibitedText,
                htmlBodyContainsProhibitedRegex
                );
            return 
                textBodyContainsProhibitedText.Result  ||
                textBodyContainsProhibitedRegex.Result ||
                htmlBodyContainsProhibitedText.Result  ||
                htmlBodyContainsProhibitedRegex.Result;
        }
    }
}
