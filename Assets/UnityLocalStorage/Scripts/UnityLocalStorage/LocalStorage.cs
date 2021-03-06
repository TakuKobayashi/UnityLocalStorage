﻿using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityCipher;

namespace UnityLocalStorage
{
    public class LocalStorage
    {
        public static bool LogEnabled = false;
        public static string EncyptPassword = "<4Q0fXVnj07OFF?N_Wske1YbYLdYm.YD";
        public static string SaveFileName = "unitylocalstorage";

        private static HashSet<string> volatilityData = new HashSet<string>();
        private static Dictionary<string, object> savedDataCache = null;
        private static Dictionary<string, object> SavedData
        {
            set { savedDataCache = value; }
            get
            {
                if (savedDataCache == null)
                {
                    savedDataCache = Load();
                }
                return savedDataCache;
            }
        }

        private static string storageFilePathCache = null;
        private static string StorageFilePath
        {
            get
            {
                if (storageFilePathCache != null)
                {
                    return storageFilePathCache;
                }
                storageFilePathCache = Path.Combine(UnityEngine.Application.persistentDataPath, SaveFileName);
                return storageFilePathCache;
            }
        }

        /// <summary>
        /// <para>各種機能がどの状態でも問題なく使えるように事前に準備しておく</para>
        /// </summary>
        public static void Setup()
        {
            storageFilePathCache = StorageFilePath;
            // Preload From Data
            SavedData = Load();
        }

        /// <summary>
        /// <para>key-value形式でデータをメモリにのせる</para>
        /// <para>※データを保存して、データを永続化はまだ行われていない</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】value</para>
        /// </summary>
        public static void SetValue(string key, object value)
        {
            volatilityData.Remove(key);
            if (SavedData.ContainsKey(key))
            {
                SavedData[key] = value;
            }
            else
            {
                SavedData.Add(key, value);
            }
        }

        /// <summary>
        /// <para>key-value形式でデータをメモリにのせる</para>
        /// <para>※このメソッドでメモリに乗せたデータはSave保存してもストレージには保存されない。(アプリを落としたら消える)</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】value</para>
        /// </summary>
        public static void SetVolatilityValue(string key, object value)
        {
            SetValue(key, value);
            volatilityData.Add(key);
        }

        /// <summary>
        /// <para>現在メモリには乗っているが保存されない設定になっているKeyの配列</para>
        /// </summary>
        public static string[] GetCurrentVolatilityKeys()
        {
            return volatilityData.ToArray();
        }

        /// <summary>
        /// <para>メモリには乗っているが保存されない設定になっているKeyを全て消す</para>
        /// </summary>
        public static void ClearVolatilityKeys()
        {
            volatilityData.Clear();
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータに指定したkeyが存在するかどうか判別する</para>
        /// <para>【第1引数】key</para>
        /// </summary>
        public static bool HasKey(string key)
        {
            return SavedData.ContainsKey(key);
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータを指定したObjectの形に変換して取得する</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】keyがなかった時に取得するデフォルトの値</para>
        /// </summary>
        public static T GetGenericObject<T>(string key, T defaultValue = default(T))
        {
            if (SavedData.ContainsKey(key))
            {
                if (SavedData[key] is T)
                {
                    return (T) SavedData[key];
                }
                return JsonConvert.DeserializeObject<T>(GetString(key));
            }
            return defaultValue;
        }


        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをfloat形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】keyがなかった時に取得するデフォルトの値</para>
        /// </summary>
        public static float GetFloat(string key, float defaultValue = 0.0F)
        {
            if (SavedData.ContainsKey(key))
            {
                return Convert.ToSingle(SavedData[key]);
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをdouble形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】keyがなかった時に取得するデフォルトの値</para>
        /// </summary>
        public static double GetDouble(string key, double defaultValue = 0.0d)
        {
            if (SavedData.ContainsKey(key))
            {
                return Convert.ToDouble(SavedData[key]);
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをint形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】keyがなかった時に取得するデフォルトの値</para>
        /// </summary>
        public static int GetInt(string key, int defaultValue = 0)
        {
            if (SavedData.ContainsKey(key))
            {
                return Convert.ToInt32(SavedData[key]);
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをuint形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// <para>【第2引数】keyがなかった時に取得するデフォルトの値</para>
        /// </summary>
        public static long GetLong(string key, long defaultValue = 0)
        {
            if (SavedData.ContainsKey(key))
            {
                return Convert.ToInt64(SavedData[key]);
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをstring形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// </summary>
        public static string GetString(string key, string defaultValue = "")
        {
            if (SavedData.ContainsKey(key))
            {
                return SavedData[key].ToString();
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをDateTime形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// </summary>
        public static DateTime GetDateTime(string key, DateTime defaultValue = new DateTime())
        {
            if (SavedData.ContainsKey(key))
            {
                return (DateTime)SavedData[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>key-value形式でメモリにのっているデータをBoolean形式で取得する</para>
        /// <para>【第1引数】key</para>
        /// </summary>
        public static bool GetBoolean(string key, bool defaultValue = false)
        {
            if (SavedData.ContainsKey(key))
            {
                return Convert.ToBoolean(SavedData[key]);
            }
            return defaultValue;
        }

        /// <summary>
        /// <para>指定したKeyをメモリから削除する</para>
        /// <para>【第1引数】key</para>
        /// </summary>
        public static void DeleteKey(string key)
        {
            if (SavedData.ContainsKey(key))
            {
                SavedData.Remove(key);
            }
        }

        /// <summary>
        /// <para>今メモリに乗っているものを捨てて、ディスクからデータを読み込む</para>
        /// </summary>
        public static void Reload()
        {
            SavedData = Load();
        }

        /// <summary>
        /// <para>すべてのデータを削除する</para>
        /// </summary>
        public static void DeleteAll()
        {
            ClearValues();
            Save();
        }

        /// <summary>
        /// <para>すべてのデータをメモリから削除する</para>
        /// </summary>
        public static void ClearValues()
        {
            volatilityData.Clear();
            SavedData.Clear();
        }

        /// <summary>
        /// <para>メモリにのっているデータをローカルストレージに保存する</para>
        /// </summary>
        public static void Save()
        {
            Dictionary<string, object> willSaveData = new Dictionary<string, object>();
            List<string> keys = SavedData.Keys.ToList();
            for (int i = 0; i < keys.Count; ++i)
            {
                if (volatilityData.Contains(keys[i]))
                {
                    continue;
                }
                willSaveData.Add(keys[i], SavedData[keys[i]]);
            }

            string json = JsonConvert.SerializeObject(willSaveData);
            byte[] jsonBinary = Encoding.UTF8.GetBytes(json);
            if (LogEnabled)
            {
                PublishLog(json);
            }
            File.WriteAllBytes(StorageFilePath, RijndaelEncryption.Encrypt(jsonBinary, EncyptPassword));
        }

        /// <summary>
        /// <para>ローカルストレージに保存されているデータを読み込む</para>
        /// </summary>
        public static Dictionary<string, object> Load()
        {
            string json = "";
            string filePath = StorageFilePath;
            if (File.Exists(filePath))
            {
                byte[] savedDataJsonBinary = RijndaelEncryption.Decrypt(File.ReadAllBytes(filePath), EncyptPassword);
                json = Encoding.UTF8.GetString(savedDataJsonBinary);
            }
            Dictionary<string, object> parsedJson = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(json))
            {
                parsedJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            if (LogEnabled)
            {
                PublishLog(json);
            }
            return parsedJson;
        }

        /// <summary>
        /// <para>メモリにのっているデータの内容をLogに出力する</para>
        /// </summary>
        public static void PublishCurrentAllDataLog()
        {
            PublishLog(JsonConvert.SerializeObject(SavedData));
        }

        /// <summary>
        /// <para>メモリにのっているファイルに保存されるデータの内容をLogに出力する</para>
        /// </summary>
        public static void PublishCurrentWillSaveDataLog()
        {
            Dictionary<string, object> willSaveData = new Dictionary<string, object>();
            List<string> keys = SavedData.Keys.ToList();
            for (int i = 0; i < keys.Count; ++i)
            {
                if (volatilityData.Contains(keys[i]))
                {
                    continue;
                }
                willSaveData.Add(keys[i], SavedData[keys[i]]);
            }
            PublishLog(JsonConvert.SerializeObject(willSaveData));
        }

        private static void PublishLog(string json)
        {
            StringBuilder logStr = new StringBuilder();
            logStr.Append("<color=#FFA500>Load LocalStorage Data</color>");
            logStr.Append("\n\n");
            logStr.Append("<color=#FFA500>");
            logStr.Append(json);
            logStr.Append("</color>");
            logStr.Append("\n\n");
            UnityEngine.Debug.Log(logStr.ToString());
        }
    }
}