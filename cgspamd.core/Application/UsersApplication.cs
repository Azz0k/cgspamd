using cgspamd.core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Application
{
    public class UsersApplication
    {
        private StoreDbContext db;
        public UsersApplication(StoreDbContext storeDbContext) 
        {
            db = storeDbContext;
        }
    }
}
