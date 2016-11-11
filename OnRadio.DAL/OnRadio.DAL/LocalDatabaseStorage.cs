using System;
using System.Collections;
using System.Collections.Generic;
using SQLite.Net.Platform.WinRT;
using System.IO;
using System.Diagnostics;
using System.Linq;
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
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                conn.CreateTable<CachedData>();
                conn.CreateTable<FavoriteRadio>();         
            }
        }

        public static void InsertOrUpdateCachedData(string url, DateTime expireAt, string data)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
                {
                    Debug.Write("EXPIRATION: ");
                    Debug.WriteLine(expireAt);
                    
                    CachedData record = new CachedData()
                    {
                        Url = url,
                        Timestamp = DateTime.UtcNow,
                        ExpireAt = expireAt,
                        Data = data
                    };
                    conn.InsertOrReplace(record);
                    //// If there is no record in this type, it means that we have to create it - every type of data will have only 1 record in DB.
                    //var isEmpty = conn.Table<CachedData>().Where(cachedDataT => cachedDataT.Type == cacheType).FirstOrDefault() == null;
                    //if (isEmpty)
                    //{
                    //    Debug.WriteLine("Record \'" + cacheType.ToString() + "\' will be inserted into DB.");
                    //    conn.Insert(record);
                    //}
                    //else
                    //{
                    //    Debug.WriteLine("Record \'" + cacheType.ToString() + "\' is being updated in DB.");
                    //    conn.Update(record);
                    //}
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("[LocalDatabaseStorage.InsertCachedData]: Unknown exception: " + e);
            }
        }

        public static CachedData GetDataFromCache(string url)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                return conn.Table<CachedData>().FirstOrDefault(cachedDataT => cachedDataT.Url == url);
            }
        }

        public static void InsertFavorite(string radioId)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                FavoriteRadio record = new FavoriteRadio()
                {
                    RadioId = radioId
                };
                Debug.WriteLine(record.RadioId);
                conn.Insert(record);
                
            }
        }

        public static void DeleteFavorite(string radioId)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, true)))
            {
                conn.Table<FavoriteRadio>().Delete(fav => fav.RadioId == radioId);
            }
        }

        public static List<FavoriteRadio> GetFavorites()
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, true)))
            {
                return conn.Table<FavoriteRadio>().ToList();
            }

        }

        public static void DeleteAllFavorites()
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, true)))
            {
                conn.DeleteAll<FavoriteRadio>();
            }
        }

        public static bool IsFavorite(string radioId)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, true)))
            {
                return conn.Table<FavoriteRadio>().FirstOrDefault(fav => fav.RadioId == radioId) != null;
            }
        }
    }

    public class CachedData
    {
        [PrimaryKey] // AutoIncrement not used, ID is from play.cz API
        public string Url { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime ExpireAt { get; set; }
        public string Data { get; set; }
    }

    public class FavoriteRadio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string RadioId { get; set; }

    }
}
