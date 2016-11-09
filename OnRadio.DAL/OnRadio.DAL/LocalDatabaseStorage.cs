using System;
using System.Collections;
using System.Collections.Generic;
using SQLite.Net.Platform.WinRT;
using System.IO;
using System.Diagnostics;
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

        public static void InsertCachedData(string id, DateTime expireAt, string data)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
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
            int Id = GetFavoriteId(radioId);
            if (Id == 0) return;

            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                conn.Delete<FavoriteRadio>(primaryKey: Id);
            }
        }

        public static List<string> GetFavorites()
        {
            var favoriteList = new List<string>();

            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                
                IEnumerator e = conn.Table<FavoriteRadio>().GetEnumerator();
                while (e.MoveNext())
                {
                    var curr = (FavoriteRadio)e.Current;
                    Debug.WriteLine(curr.RadioId);
                    favoriteList.Add(curr.RadioId);
                }
            }

            return favoriteList;
        }

        public static void DeleteAllFavorites()
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                conn.DeleteAll<FavoriteRadio>();
            }
        }

        public static bool IsFavorite(string radioId)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                IEnumerator e = conn.Table<FavoriteRadio>().GetEnumerator();
                while (e.MoveNext())
                {
                    var curr = (FavoriteRadio) e.Current;
                    if (curr.RadioId == radioId) return true;
                }

            }

            return false;
        }

        public static int GetFavoriteId(string radioId)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                IEnumerator e = conn.Table<FavoriteRadio>().GetEnumerator();
                while (e.MoveNext())
                {
                    var curr = (FavoriteRadio)e.Current;
                    if (curr.RadioId == radioId) return curr.Id;
                }

            }

            return 0;
        }

        
    }

    public class CachedData
    {
        [PrimaryKey] // AutoIncrement not used, ID is from play.cz API
        public string Id { get; set; }
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
