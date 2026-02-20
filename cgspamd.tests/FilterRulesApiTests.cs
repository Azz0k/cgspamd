using cgspamd.api;
using cgspamd.api.Application;
using cgspamd.core.Applications;
using cgspamd.core.Contexts;
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
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace cgspamd.tests
{
    public class FilterRulesApiTests :
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private string apiUri = "/api/FilterRules";
        public FilterRulesApiTests(
    CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            DoMigrate().Wait();
        }
        private async Task DoMigrate()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<StoreDbContext>();
                db.Database.Migrate();
                var userApp = scopedServices.GetRequiredService<UsersApplication>();
                await userApp.AddAsync(new AddUserRequest() { FullName= "test",UserName="test",IsAdmin=true,Enabled=true, Password = "test1234"});
                var authApp = scopedServices.GetRequiredService<UserAuthenticationApplication>();
                string token = authApp.GenerateJwt(new cgspamd.core.Models.User() { FullName = "test", UserName = "test", Id = 1, IsAdmin = true, Hash = "" });
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
        private string GenerateRandomStr()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task<HttpResponseMessage?> PostToFilterRulesAPI(string value, string comment, int type)
        {
            AddFilterRuleRequest request = new() { Value = value, Comment = comment, Type = type };
            return await _client.PostAsJsonAsync(apiUri, request);
        }
        private async Task<HttpResponseMessage?> PostToFilterRulesAPI()
        {
            return await PostToFilterRulesAPI(GenerateRandomStr(), GenerateRandomStr(), 0);
        }
        private async Task<List<FilterRuleDTO>?> GetAsync()
        {
            var response = await _client.GetAsync(apiUri);
            var content = await response.Content.ReadFromJsonAsync<List<FilterRuleDTO>>();
            return content;
        }
        [Fact]
        public async Task FilterRulesApi_POST_ShouldWorkCorrectly()
        {
            string value = GenerateRandomStr();
            string comment = GenerateRandomStr();
            var response = await PostToFilterRulesAPI(value, comment, 0); 
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            FilterRuleDTO? data = await response.Content.ReadFromJsonAsync<FilterRuleDTO>();
            Assert.NotNull(data);
            response = await PostToFilterRulesAPI(value, comment, 0);
            Assert.NotNull(response);
            var code = response.StatusCode;
            Assert.Equal(HttpStatusCode.Conflict, code);
            response = await PostToFilterRulesAPI(GenerateRandomStr(), GenerateRandomStr(), -1);
            Assert.NotNull(response);
            code = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, code);

        }

        [Fact]
        public async Task FilterRulesApi_GET_ShouldWorkCorrectly()
        {
            var response = await PostToFilterRulesAPI();
            Assert.NotNull(response);
            var data = response.Content.ReadFromJsonAsync<FilterRuleDTO>();
            List<FilterRuleDTO>? getResponse = await GetAsync();
            Assert.NotNull(getResponse);
            Assert.NotEmpty(getResponse);
            Assert.Contains(getResponse, e => e.Id == data.Id);
        }
        [Fact]
        public async Task FilterRulsApi_DELETE_ShouldWorkCorrectly()
        {
            var response = await PostToFilterRulesAPI();
            Assert.NotNull(response);
            var data = response.Content.ReadFromJsonAsync<FilterRuleDTO>();
            response = await _client.DeleteAsync($"{apiUri}/{data.Id}");
            response.EnsureSuccessStatusCode();
            response = await _client.DeleteAsync($"{apiUri}/{data.Id}");
            var code = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, code);
            var rulesAfterDelete = await GetAsync();
            Assert.NotNull(rulesAfterDelete);
            Assert.DoesNotContain(rulesAfterDelete, e => e.Id == data.Id);
        }
        [Fact]
        public async Task UsersApi_PUT_ShouldWorkCorrectly()
        {
            await PostToFilterRulesAPI();
            await PostToFilterRulesAPI();
            List<FilterRuleDTO>? rules = await GetAsync();
            Assert.NotNull(rules);
            var updateRequest = new UpdateFilterRuleRequest() { Value = "test", Comment = "test", Id = 1};
            var response = await _client.PutAsJsonAsync(apiUri, updateRequest);
            response.EnsureSuccessStatusCode();
            rules = await GetAsync();
            Assert.NotNull(rules);
            Assert.Contains<FilterRuleDTO>(rules, (FilterRuleDTO r) => r.Value == updateRequest.Value);
            updateRequest = new UpdateFilterRuleRequest() { Value = "test", Comment = "test", Id = -1 };
            response = await _client.PutAsJsonAsync(apiUri, updateRequest);
            var code = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, code);
            updateRequest = new UpdateFilterRuleRequest() { Value = "test", Comment = "test", Id = 100 };
            response = await _client.PutAsJsonAsync(apiUri, updateRequest);
            code = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, code);
        }
    }
}
