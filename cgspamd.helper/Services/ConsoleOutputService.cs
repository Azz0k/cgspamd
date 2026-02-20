using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using cgspamd.helper.Models;

namespace cgspamd.helper.Services
{
    internal class ConsoleOutputService
    {
        private string me = "cgspamd";
        AppSettings appSettings;
        public ConsoleOutputService(AppSettings appSettings)
        {
            this.appSettings = appSettings; 
        }
        public void PrintLogMessage(string message)
        {
            Print($"* {me} {message}");
        }
        public void PrintGoodMessage(string lineNumber)
        {
            Print($"{lineNumber} OK");
        }
        public void PrintBadMessage(string lineNumber)
        {
            Print($"{lineNumber} {appSettings.BadMessage}");
        }
        public void PrintStartupMessage()
        {
            PrintLogMessage("INFO: cgspamd free Version: 0.1");
            PrintLogMessage("INFO: Initialization completed");
        }
        public void Print(string message)
        {
            Console.WriteLine(message);
            Console.Out.Flush();
        }
    }
}
