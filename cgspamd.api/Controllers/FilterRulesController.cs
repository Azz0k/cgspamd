using cgspamd.core.Application;
using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static cgspamd.core.Utils.Utils;

namespace cgspamd.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilterRulesController : ControllerBase
    {
        private FilterRulesApplication application;
        public FilterRulesController(FilterRulesApplication application)
        {
            this.application = application;
        }
        // GET: api/FilterRules
        [HttpGet]
        public async Task<IEnumerable<FilterRuleDTO>> Get()
        {
            return await application.GetAllRecordsAsync();
        }
        // POST api/FilterRules
        [HttpPost]
        public async Task<int> Post([FromBody] AddFilterRuleRequest request)
        {
            return await application.AddAsync(GetId(HttpContext), request);
        }
        // PUT api/FilterRules
        [HttpPut]
        public async Task<StatusCodeResult> Put([FromBody] UpdateFilterRuleRequest request)
        {
            int code = await application.UpdateAsync(GetId(HttpContext), request);
            return StatusCode(code);
        }
        // DELETE api/FilterRules/5
        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(int id)
        {
            int code = await application.DeleteAsync(id);
            return StatusCode(code);
        }
    }
}
