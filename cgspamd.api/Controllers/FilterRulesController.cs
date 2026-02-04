using cgspamd.core.Application;
using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cgspamd.api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilterRulesController : ControllerBase
    {
        private FilterRulesApplication application;
        public FilterRulesController(FilterRulesApplication application)
        {
            this.application = application;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<FilterRuleDTO>> Get()
        {
            return await application.GetAllRecordsAsync();
        }



        // POST api/<ValuesController>
        [HttpPost]
        public async Task<int> Post([FromBody] AddFilterRuleRequest request, HttpContext context)
        {
            return await application.AddAsync(0,request);
        }

        // PUT api/<ValuesController>
        [HttpPut]
        public async Task<StatusCodeResult> Put([FromBody] UpdateFilterRuleRequest request)
        {
            int code = await application.UpdateAsync(0,request);
            return StatusCode(code);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(int id)
        {
            int code = await application.DeleteAsync(id);
            return StatusCode(code);
        }
    }
}
