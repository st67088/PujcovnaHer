using LiteDB;
using PujcovnaHerPesout.Model;
using System.Windows;
using System;
namespace PujcovnaHerPesout.ViewModel
{
    public class DatabaseManager
    {
        private static LiteDatabase db;

        private DatabaseManager() { }

        public static LiteDatabase GetDatabaseInstance()
        {
            if (db == null)
            {
                try
                {                  
                    string dbPath = "Databaze.db";
                    var mapper = BsonMapper.Global;
                    mapper.Entity<Hra>().Id(h => h.Id);
                    mapper.Entity<Uzivatel>().Id(h => h.Id);
                    mapper.Entity<Uzivatel>().Id(h => h.Id);
                    mapper.Entity<PujcenaHra>().Id(h => h.Id);

                    db = new LiteDatabase(dbPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při inicializaci databáze: " + ex.Message);
                }

                return db;
            }

            return db;
        }
    }
}