using cgspamd.api.Models;
using cgspamd.core.Context;
using Microsoft.Extensions.Options;


namespace cgspamd.api.Application
{
    public class UserAuthenticationApplication
    {
        private StoreDbContext db;
        private IOptions<APISettings> settings;
        public UserAuthenticationApplication(IOptions<APISettings> settings, StoreDbContext storeDbContext) 
        {
            this.settings = settings;
            db = storeDbContext;
        }
    }
}
