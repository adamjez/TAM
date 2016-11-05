using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SQLite.Net.Platform.WinRT;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using SQLite.Net.Attributes;
using SQLite.Net;

namespace OnRadio.DAL
{
    public class LocalDatabaseStorage
    {
        static string localSqlPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Localdb.sqlite");

        public static void CreateDatabase()
        {
            Debug.WriteLine("Database is located here: " + localSqlPath);
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                conn.CreateTable<CachedData>();
            }
        }

        public static void InsertCachedData(string id, string expireAt, string data)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                Debug.WriteLine(DateTime.Now);
                CachedData record = new CachedData()
                {
                    Id = id,
                    Timestamp = DateTime.Now,
                    ExpireAt = expireAt,
                    Data = data
                };
                conn.Insert(record);
            }
        }
    }

    public class CachedData
    {
        [PrimaryKey] // AutoIncrement not used, ID is from play.cz API
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ExpireAt { get; set; }
        public string Data { get; set; }
    }
}
