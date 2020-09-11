using KombiCim.Data.Models;
using System;

namespace KombiCim.Data.Utilities
{
    public class DbHelper : IDisposable
    {
        private bool willDispose;

        public KombiCimEntities Db { get; }

        public DbHelper(KombiCimEntities db)
        {
            Db = db;

            willDispose = Db == null;
            if (Db == null)
                Db = new KombiCimEntities();
        }

        public void Dispose()
        {
            if (willDispose)
                Db.Dispose();
        }
    }
}
