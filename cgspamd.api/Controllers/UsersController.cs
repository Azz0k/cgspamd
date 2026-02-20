using cgspamd.core.Applications;
using cgspamd.core.Models;
using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cgspamd.api.Controllers 
{ 
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersApplication app;
        public UsersController(UsersApplication application) 
        {
            app = application;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            return await app.GetAllRecordsAsync();
        }
        // POST api/Users
        [HttpPost]
        public async Task<int> Post([FromBody] AddUserRequest request)
        {
            return await app.AddAsync(request);
        }
        // PUT api/Users
        [HttpPut]
        public async Task<StatusCodeResult> Put([FromBody] UpdateUserRequest request)
        {
            int code = await app.UpdateAsync(request);  
            return StatusCode(code);
        }
        // DELETE api/Users/<id>
        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(int id)
        {
            int code = await app.DeleteAsync(id);
            return StatusCode(code);
        }
    }
}
