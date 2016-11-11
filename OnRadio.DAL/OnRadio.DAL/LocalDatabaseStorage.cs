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
    public enum CachedDataType
    {
        getRadios,
        getOnAir,
        getOnAirHistory,
        getRadioStream,
        getAllRadioStreams,
        getStyles
    }

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

        public static void InsertOrUpdateCachedData(CachedDataType cacheType, DateTime expireAt, string data)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
                {
                    Debug.Write("EXPIRATION: ");
                    Debug.WriteLine(expireAt);
                    
                    CachedData record = new CachedData()
                    {
                        Type = cacheType,
                        Timestamp = DateTime.Now.ToString(),
                        ExpireAt = expireAt.ToString(),
                        Data = data
                    };

                    // If there is no record in this type, it means that we have to create it - every type of data will have only 1 record in DB.
                    var isEmpty = conn.Table<CachedData>().Where(cachedDataT => cachedDataT.Type == cacheType).FirstOrDefault() == null;
                    if (isEmpty)
                    {
                        Debug.WriteLine("Record \'" + cacheType.ToString() + "\' will be inserted into DB.");
                        conn.Insert(record);
                    }
                    else
                    {
                        Debug.WriteLine("Record \'" + cacheType.ToString() + "\' is being updated in DB.");
                        conn.Update(record);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("[LocalDatabaseStorage.InsertCachedData]: Unknown exception: " + e);
            }
        }

        public static bool IsCached(DateTime currentDateTime, CachedDataType cacheType)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
                {
                        var db_expireAt = conn.Table<CachedData>().Where(cachedDataT => cachedDataT.Type == cacheType).Select(cachedData => cachedData.ExpireAt).FirstOrDefault();
                        var db_timestamp = conn.Table<CachedData>().Where(cachedDataT => cachedDataT.Type == cacheType).Select(cachedData => cachedData.Timestamp).FirstOrDefault();
                        Debug.WriteLine(db_expireAt);
                        Debug.WriteLine(db_timestamp);
                        Debug.WriteLine(currentDateTime);
                        // If the current time is lower than expirated record in DB, it means that data are still valid.
                        // But current time has to be higher than time on which the record was created - just for sure, that we have properly set time on mobile.
                        if (currentDateTime > Convert.ToDateTime(db_timestamp) && currentDateTime < Convert.ToDateTime(db_expireAt))
                        {
                            Debug.WriteLine("Record \'" + cacheType.ToString() + "\' is not expired yet - data in cache are valid.");
                            return true;
                        }
                        else
                        {
                            Debug.WriteLine("Record \'" + cacheType.ToString() + "\' is expired and no longer valid.");
                            return false;
                        }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("[LocalDatabaseStorage.IsCached]: Unknown exception: " + e);
                return false;
            }
        }

        public static string getDataFromCache(CachedDataType cacheType)
        {
            using (SQLiteConnection conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(localSqlPath, storeDateTimeAsTicks: true)))
            {
                return conn.Table<CachedData>().Where(cachedDataT => cachedDataT.Type == cacheType).Select(cachedData => cachedData.Data).FirstOrDefault();
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
                return conn.Table<FavoriteRadio>().Where(fav => fav.RadioId == radioId).FirstOrDefault() != null;
            }
        }
    }

    public class CachedData
    {
        [PrimaryKey] // AutoIncrement not used, ID is from play.cz API
        public CachedDataType Type { get; set; }
        public string Timestamp { get; set; }
        public string ExpireAt { get; set; }
        public string Data { get; set; }
    }

    public class FavoriteRadio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string RadioId { get; set; }

    }
}
