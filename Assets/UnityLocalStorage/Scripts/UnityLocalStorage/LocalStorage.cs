using System;
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
        private static bool LogEnabled = false;
        private static string EncyptPassword = "password";

        private static HashSet<string> volatilityData = new HashSet<string>();
        private static Dictionary<string, object> savedData;

        private static Dictionary<string, object> SavedData
        {
            set { savedData = value; }
            get
            {
                if (savedData == null)
                {
                    savedData = Load();
                }
                return savedData;
            }
        }

        /// <summary>
        /// <para>コンストラクタ</para>
        /// <para>ローカルストレージに保存されているデータをメモリにのせる</para>
        /// </summary>
        static LocalStorage()
        {
            // データへのアクセスを高速にするためにあらかじめロードしておく
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
                    return (T)SavedData[key];
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
        /// <para>すべてのデータをメモリから削除する</para>
        /// </summary>
        public static void Clear()
        {
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
            PublishLog(json);
            File.WriteAllBytes(StorageFilePath, RijndaelEncryption.Encrypt(jsonBinary, EncyptPassword));
        }

        /// <summary>
        /// <para>ローカルストレージに保存されているデータを読み込む</para>
        /// </summary>
        public static Dictionary<string, object> Load()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
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
            PublishLog(json);
            return parsedJson;
        }

        private static void PublishLog(string json)
        {
            if (!LogEnabled) return;
            StringBuilder logStr = new StringBuilder();
            logStr.Append("<color=#FFA500>Load LocalStorage Data</color>");
            logStr.Append("\n\n");
            logStr.Append("<color=#FFA500>");
            logStr.Append(json);
            logStr.Append("</color>");
            logStr.Append("\n\n");
            UnityEngine.Debug.Log(logStr.ToString());
        }

        //Threadからでも一応使えるようにしておくための対応
        private static string storageFilePath = null;

        private static string StorageFilePath
        {
            get
            {
                if (storageFilePath != null)
                {
                    return storageFilePath;
                }
                storageFilePath = Path.Combine(UnityEngine.Application.persistentDataPath, "localstorage");
                return storageFilePath;
            }
        }
    }
}