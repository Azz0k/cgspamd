using cgspamd.api.Models;
using cgspamd.core.Applications;
using cgspamd.core.Contexts;
using cgspamd.core.Enums;
using cgspamd.core.Models;
using cgspamd.core.Services;
using cgspamd.helper.Applications;
using cgspamd.helper.Models;
using cgspamd.helper.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using static cgspamd.core.Utils.Utils;


namespace cgspamd.tests
{
    public class HelperTests
    {
        private HelperApplication helperApp;
        private StoreDbContext db;
        private string excludedEmail = "ExClUdeD@Email.cOm";
        private string blackListSenderAddresses = "BlAckLisTed@from.foe";
        private string blackListSenderDomains = "blackListed.domain";
        private string whiteListedSenderAddresses = "WhiteListed@blackListed.domain";
        private string whiteListSenderDomains = "whiteListed.domain";
        private string prohibitedText = "fusce id nisl ut iPsum tempus lobortis";
        private AppSettings? appSettings;
        public HelperTests()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var builder = new ConfigurationBuilder().AddJsonFile($"HelperTestsAppSettings.json");
            IConfiguration config = builder.Build();
            appSettings = config.Get<AppSettings>();
            Assert.NotNull(appSettings);
            var serviceProvider = new ServiceCollection()
                .AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                })
                .AddDbContext<StoreDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                })
                .AddScoped<DatabaseService>()
                .AddScoped<FilterRulesApplication>()
                .AddScoped<HelperApplication>()
                .AddSingleton<ConsoleOutputService>()
                .AddSingleton<WorkerService>()
                .AddSingleton<AppSettings>(appSettings)
                .BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            helperApp = scope.ServiceProvider.GetRequiredService<HelperApplication>();
            db = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            db.Database.Migrate();
            SeedData();
        }
        public void SeedData()
        {
            User user = new() { UserName = "Test", FullName = "Test", Hash = "", IsAdmin = true, Enabled = true, Deleted = false};
            db.Users.Add(user);
            db.SaveChanges();
            FilterRule rule = new() {
                Value = excludedEmail,
                Comment = "excluded email", 
                CreatedAt = GenerateNowDate(), 
                CreatedByUser = user, 
                CreatedByUserId = user.Id, 
                Type = (int)FilterRulesType.excludedRecipients 
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
            rule = new()
            {
                Value = blackListSenderAddresses,
                Comment = "blacklisted email",
                CreatedAt = GenerateNowDate(),
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                Type = (int)FilterRulesType.blackListSenderAddresses
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
            rule = new()
            {
                Value = blackListSenderDomains,
                Comment = "blacklisted domain",
                CreatedAt = GenerateNowDate(),
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                Type = (int)FilterRulesType.blackListSenderDomains
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
            rule = new()
            {
                Value = whiteListedSenderAddresses,
                Comment = "whitelisted email",
                CreatedAt = GenerateNowDate(),
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                Type = (int)FilterRulesType.whiteListSenderAddresses
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
            rule = new()
            {
                Value = whiteListSenderDomains,
                Comment = "whitelisted domain",
                CreatedAt = GenerateNowDate(),
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                Type = (int)FilterRulesType.whiteListSenderDomains
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
            rule = new()
            {
                Value = prohibitedText,
                Comment = "prohibited text",
                CreatedAt = GenerateNowDate(),
                CreatedByUser = user,
                CreatedByUserId = user.Id,
                Type = (int)FilterRulesType.prohibitedTextInBody
            };
            db.FilterRules.Add(rule);
            db.SaveChanges();
        }
        [Theory]
        [InlineData("HelperTestFiles/excluded.msg", true)]
        [InlineData("HelperTestFiles/BasePositive/BlackEmail.msg", false)]
        [InlineData("HelperTestFiles/BasePositive/BlackDomain.msg", false)]
        [InlineData("HelperTestFiles/BasePositive/WhiteEmail.msg", true)]
        [InlineData("HelperTestFiles/BasePositive/WhiteDomain.msg", true)]
        [InlineData("HelperTestFiles/prohibitedText.msg", false)]
        public async Task EnsureMessageAllowed_WithVariousInputs_ReturnsExpected(string file, bool expected)
        {
            Assert.NotNull(appSettings);
            file = Path.Combine(appSettings.baseDir, file);
            Assert.Equal(expected, await helperApp.EnsureMessageAllowed(file));
        }
    }
}
