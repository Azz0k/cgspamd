using cgspamd.core.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Services
{
    public class DatabaseService
    {
        private StoreDbContext _db;
        public DatabaseService(StoreDbContext storeDbContext) 
        { 
            _db = storeDbContext;
        }
        public async Task InitDatabaseAsync()
        {
            await _db.Database.MigrateAsync();
            await _db.Database.OpenConnectionAsync();
            await _db.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");
            await _db.Database.CloseConnectionAsync();
            await TruncateWalAsync();
        }
        public async Task TruncateWalAsync()
        {
            await _db.Database.OpenConnectionAsync();
            await _db.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(TRUNCATE);");
            await _db.Database.CloseConnectionAsync();
        }
    }
}
