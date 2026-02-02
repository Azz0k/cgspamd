using cgspamd.core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Application
{
    public class FilterRulesApplication
    {
        private StoreDbContext db;
        public FilterRulesApplication(StoreDbContext storeDbContext) 
        {
            db = storeDbContext;
        }
    }
}
