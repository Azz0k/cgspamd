using cgspamd.core.Applications;
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
        public async Task<IActionResult> Post([FromBody] AddFilterRuleRequest request)
        {
            (int code, FilterRuleDTO? rule) = await application.AddAsync(GetId(HttpContext), request);
            if (rule == null) 
            {
                return StatusCode(code);
            }
            return Created("",rule);
        }
        // PUT api/FilterRules
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateFilterRuleRequest request)
        {
            (int code, FilterRuleDTO? rule) = await application.UpdateAsync(GetId(HttpContext), request);
            if (rule == null)
            {
                return StatusCode(code); 
            }
            return Ok(rule);
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
