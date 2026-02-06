using cgspamd.api;
using cgspamd.api.Application;
using cgspamd.core.Context;
using cgspamd.core.Models.APIModels;
using cgspamd.core.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;

namespace cgspamd.tests
{
    public class ApiAccessRequiresAuthorization :
    IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program>
            _factory;
        public ApiAccessRequiresAuthorization(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            DoMigrate();
        }
        private void DoMigrate()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<StoreDbContext>();
                db.Database.Migrate();
            }
        }
        [Fact]
        public async Task UsersApi_ShouldAnswer401()
        {
            string apiUri = "/api/Users";
            var addRequest = new AddUserRequest() { UserName = "userName", FullName = "fullName", IsAdmin = true, Enabled = true, Password = "password" };
            var response = await _client.PostAsJsonAsync(apiUri, addRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            response = await _client.GetAsync(apiUri);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            response = await _client.DeleteAsync($"{apiUri}/1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            var updateRequest = new UpdateUserRequest() { Id = 1, UserName = "Test", FullName = "Test", Enabled = false, IsAdmin = false };
            response = await _client.PutAsJsonAsync(apiUri, updateRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task FilterRulesApi_ShouldAnswer401()
        {
            string apiUri = "/api/FilterRules";
            var addRequest = new AddFilterRuleRequest() { Value = "", Comment = "", Type = 0 };
            var response = await _client.PostAsJsonAsync(apiUri, addRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            response = await _client.GetAsync(apiUri);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            response = await _client.DeleteAsync($"{apiUri}/1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            var updateRequest = new UpdateFilterRuleRequest() { Value = "", Comment = "", Id = 1 };
            response = await _client.PutAsJsonAsync(apiUri, updateRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
