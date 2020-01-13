using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityLocalStorage;

namespace UnityLocalStorageTest
{
    //この中でいろんな書き方を実践しています。
    [TestFixture]
    public class LocalStorageTest
    {
        private const string UnitTestDebugKey = "unittestdebugkey";

        // 全てのテストが始まる時に呼ばれる。
        [SetUp]
        public void SetUp()
        {
        }

        // 全てのテストが終わった時に呼ばれる。
        [TearDown]
        public void TearDown()
        {
            LocalStorage.Clear();
            LocalStorage.Save();
        }

        /// <summary>
        /// <para>LocalStorageにデータを設定する</para>
        /// </summary>
        [TestCase(-1, 101)]
        [TestCase(100, 1)]
        [TestCase(0, 55431)]
        public void SetValueTest(int defaultValue, int setValue)
        {
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, defaultValue), Is.EqualTo(defaultValue));
            LocalStorage.SetValue(UnitTestDebugKey, setValue);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, defaultValue), Is.EqualTo(setValue));
        }

        [Test]
        public void SetValueUnitTest()
        {
            Assert.AreEqual(LocalStorage.GetInt(UnitTestDebugKey, 1), 1);
            LocalStorage.SetValue(UnitTestDebugKey, 23);
            Assert.AreEqual(LocalStorage.GetInt(UnitTestDebugKey, 1), 23);
        }

        /// <summary>
        /// <para>LocalStorageにデータを設定しStorageに保存する</para>
        /// </summary>
        [TestCase(-1, 101)]
        [TestCase(100, 1)]
        [TestCase(0, 55431)]
        public void SetValueAndSaveTest(int defaultValue, int setValue)
        {
            LocalStorage.SetValue(UnitTestDebugKey, setValue);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, defaultValue), Is.EqualTo(setValue));
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, defaultValue), Is.Not.EqualTo(setValue));
            LocalStorage.SetValue(UnitTestDebugKey, setValue);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, defaultValue), Is.EqualTo(setValue));
        }

        /// <summary>
        /// <para>メモリ上には乗っけたものから値を取得できること</para>
        /// </summary>
        [Test]
        public void SetVolatilityValueTest()
        {
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.EqualTo(-1));
            LocalStorage.SetVolatilityValue(UnitTestDebugKey, 101);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.EqualTo(101));
        }

        /// <summary>
        /// <para>メモリ上にのみ乗っけたものをDiskに保存しようとするが保存されないこと</para>
        /// </summary>
        [Test]
        public void SetVolatilityValueAndSaveTest()
        {
            LocalStorage.SetVolatilityValue(UnitTestDebugKey, 101);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.EqualTo(101));
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.Not.EqualTo(101));
            LocalStorage.SetVolatilityValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.Not.EqualTo(101));
        }

        /// <summary>
        /// <para>設定されているデータを Floatで取得する</para>
        /// </summary>
        [Test]
        public void GetFloatTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234f);
            Assert.That(LocalStorage.GetFloat(UnitTestDebugKey, 1.203f), Is.EqualTo(101.0234f));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをFloat形式で読み込む</para>
        /// </summary>
        [Test]
        public void GetFloatAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234f);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetFloat(UnitTestDebugKey, 1), Is.EqualTo(101.0234f));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetIntTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234f);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(101));
            LocalStorage.SetValue(UnitTestDebugKey, 1010);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1010));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをInt形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetIntAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234f);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(101));
            LocalStorage.SetValue(UnitTestDebugKey, 1010);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1010));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetIntIfNotExistTest()
        {
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1010), Is.EqualTo(1010));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Uint形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetUintTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, long.MaxValue);
            Assert.That(LocalStorage.GetLong(UnitTestDebugKey, 1), Is.EqualTo(long.MaxValue));
            LocalStorage.SetValue(UnitTestDebugKey, 1010);
            Assert.That(LocalStorage.GetLong(UnitTestDebugKey, 1), Is.EqualTo(1010));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをUint形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetUintAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, long.MaxValue);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetLong(UnitTestDebugKey, 1), Is.EqualTo(long.MaxValue));
            LocalStorage.SetValue(UnitTestDebugKey, 1010);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetLong(UnitTestDebugKey, 1), Is.EqualTo(1010));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetUintIfNotExistTest()
        {
            Assert.That(LocalStorage.GetLong(UnitTestDebugKey, long.MaxValue), Is.EqualTo(long.MaxValue));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetStringTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            Assert.That(LocalStorage.GetString(UnitTestDebugKey), Is.EqualTo("101"));
            LocalStorage.SetValue(UnitTestDebugKey, "hoge");
            Assert.That(LocalStorage.GetString(UnitTestDebugKey), Is.EqualTo("hoge"));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetStringIfNotExistTest()
        {
            Assert.That(LocalStorage.GetString(UnitTestDebugKey), Is.EqualTo(""));
            Assert.That(LocalStorage.GetString(UnitTestDebugKey, "fuga"), Is.EqualTo("fuga"));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをInt形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetStringAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetString(UnitTestDebugKey), Is.EqualTo("101"));
            LocalStorage.SetValue(UnitTestDebugKey, "hogehoge");
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetString(UnitTestDebugKey), Is.EqualTo("hogehoge"));
        }

        /// <summary>
        /// <para>設定されているデータから指定されたKeyのデータを削除する</para>
        /// </summary>
        [Test]
        public void DeleteKeyTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 1010);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1010));
            LocalStorage.DeleteKey(UnitTestDebugKey);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1));
        }

        /// <summary>
        /// <para>設定されているデータがない場合特に何も起こらない</para>
        /// </summary>
        [Test]
        public void DeleteKeyIfNotExistTest()
        {
            LocalStorage.DeleteKey(UnitTestDebugKey);
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1));
        }

        /// <summary>
        /// <para>Keyを消してもSaveしていなかったら、Diskからは消えていない</para>
        /// </summary>
        [Test]
        public void DeleteKeyAndNotSaveFromDiskTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.DeleteKey(UnitTestDebugKey);
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(101));
        }

        /// <summary>
        /// <para>Keyを消したものをSaveするとディスクに保存されているものも消えた状態になること</para>
        /// </summary>
        [Test]
        public void DeleteKeyAndSaveFromDiskTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.DeleteKey(UnitTestDebugKey);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, 1), Is.EqualTo(1));
        }

        /// <summary>
        /// <para>ディスクにデータが保存されること</para>
        /// </summary>
        [Test]
        public void SaveTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey, -1), Is.EqualTo(-1));
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetInt(UnitTestDebugKey), Is.EqualTo(101));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Int形式で取得しようとする</para>
        /// </summary>
        [Test]
        public void LoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            Dictionary<string, object> loaded = LocalStorage.Load();
            Assert.That(loaded.ContainsKey(UnitTestDebugKey), Is.EqualTo(true));
            Assert.That(loaded[UnitTestDebugKey].ToString(), Is.EqualTo("101"));
        }

        /// <summary>
        /// <para>SetしたKeyが存在したらtrueそうでないならfalseを返すこと</para>
        /// </summary>
        [Test]
        public void HasKeyTest()
        {
            Assert.That(LocalStorage.HasKey(UnitTestDebugKey), Is.EqualTo(false));
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            Assert.That(LocalStorage.HasKey(UnitTestDebugKey), Is.EqualTo(true));
        }

        /// <summary>
        /// <para>Setした後にSave処理をする/しないでReloadした時にKeyが存在したらtrueそうでないならfalseを返すこと</para>
        /// </summary>
        [Test]
        public void HasKeyAndReloadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Reload();
            Assert.That(LocalStorage.HasKey(UnitTestDebugKey), Is.EqualTo(false));

            LocalStorage.SetValue(UnitTestDebugKey, 101);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.HasKey(UnitTestDebugKey), Is.EqualTo(true));
        }

        /// <summary>
        /// <para>設定されているデータを Floatで取得する</para>
        /// </summary>
        [Test]
        public void GetDoubleTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234);
            Assert.That(LocalStorage.GetDouble(UnitTestDebugKey, 1.203), Is.EqualTo(101.0234));
        }

        /// <summary>
        /// <para>設定されているデータがない場合Double形式で取得しようとする</para>
        /// </summary>
        [TestCase(-0.46778623462, ExpectedResult = -0.46778623462)]
        [TestCase(100, ExpectedResult = 100)]
        [TestCase(1.203, ExpectedResult = 1.203)]
        public double GetDoubleIfNotExistTest(double defaultValue)
        {
            return LocalStorage.GetDouble(UnitTestDebugKey, defaultValue);
        }

        /// <summary>
        /// <para>ディスクに保存されているものをDouble形式で読み込む</para>
        /// </summary>
        [Test]
        public void GetDoubleAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetDouble(UnitTestDebugKey, 1), Is.EqualTo(101.0234));
        }

        /// <summary>
        /// <para>FloatとDoubleはちがう</para>
        /// </summary>
        [Test]
        public void GetFloatDiffDoubleTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, 101.0234);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetFloat(UnitTestDebugKey, 1), Is.Not.EqualTo(101.0234));
        }

        /// <summary>
        /// <para>設定されているデータを DateTime型で取得する</para>
        /// </summary>
        [Test]
        public void GetDateTimeTest()
        {
            DateTime now = DateTime.Now;
            LocalStorage.SetValue(UnitTestDebugKey, now);
            Assert.That(LocalStorage.GetDateTime(UnitTestDebugKey), Is.EqualTo(now));
        }

        /// <summary>
        /// <para>設定されているデータがない場合、指定した値を取得できる</para>
        /// </summary>
        [Test]
        public void GetDateTimeIfNotExistTest()
        {
            DateTime defaultValue = new DateTime(2017, 3, 29, 16, 52, 15, 123);
            Assert.That(LocalStorage.GetDateTime(UnitTestDebugKey, defaultValue), Is.EqualTo(new DateTime(2017, 3, 29, 16, 52, 15, 123)));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをDateTime型のデフォルト値で取得しようとする</para>
        /// </summary>
        [Test]
        public void GetDateTimeDefaultTest()
        {
            Assert.That(LocalStorage.GetDateTime(UnitTestDebugKey), Is.EqualTo(new DateTime(1, 1, 1, 0, 0, 0, 0)));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをDateTime型で読み込む</para>
        /// </summary>
        [Test]
        public void GetDateTimeAfterLoadTest()
        {
            DateTime now = DateTime.Now;
            LocalStorage.SetValue(UnitTestDebugKey, now);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetDateTime(UnitTestDebugKey), Is.EqualTo(now));
        }

        /// <summary>
        /// <para>設定されているデータを Booleanで取得する</para>
        /// </summary>
        [Test]
        public void GetBooleanTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, true);
            Assert.That(LocalStorage.GetBoolean(UnitTestDebugKey), Is.EqualTo(true));
        }

        /// <summary>
        /// <para>設定されているデータがない場合、指定した値を取得できる</para>
        /// </summary>
        [Test]
        public void GetBooleanIfNotExistTest()
        {
            Assert.That(LocalStorage.GetBoolean(UnitTestDebugKey, true), Is.EqualTo(true));
        }

        /// <summary>
        /// <para>設定されているデータがない場合falseが取得できる</para>
        /// </summary>
        [Test]
        public void GetBooleanDefaultTest()
        {
            Assert.That(LocalStorage.GetBoolean(UnitTestDebugKey), Is.EqualTo(false));
        }

        /// <summary>
        /// <para>ディスクに保存されているものをBoolean型で取得する</para>
        /// </summary>
        [Test]
        public void GetBooleanAfterLoadTest()
        {
            LocalStorage.SetValue(UnitTestDebugKey, true);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.That(LocalStorage.GetBoolean(UnitTestDebugKey), Is.EqualTo(true));
        }

        /// <summary>
        /// <para>色々とデータを突っ込んでもClearしたら全部消える</para>
        /// </summary>
        [Test]
        public void ClearTest()
        {
            string key1 = UnitTestDebugKey + "aaaaaaaaaaaaaaaaaaa";
            string key2 = UnitTestDebugKey + "bbbbbbbbbbbbbbbbb";
            string key3 = UnitTestDebugKey + "cccccccc";
            LocalStorage.SetValue(key1, 3454136);
            LocalStorage.SetValue(key2, "hogehoge");
            LocalStorage.SetValue(key3, true);
            Assert.AreEqual(LocalStorage.HasKey(key1), true);
            Assert.AreEqual(LocalStorage.HasKey(key2), true);
            Assert.AreEqual(LocalStorage.HasKey(key3), true);
            LocalStorage.Clear();
            Assert.AreEqual(LocalStorage.HasKey(key1), false);
            Assert.AreEqual(LocalStorage.HasKey(key2), false);
            Assert.AreEqual(LocalStorage.HasKey(key3), false);
        }

        /// <summary>
        /// <para>保存したものをClearしてSaveすると全部消える</para>
        /// </summary>
        [Test]
        public void ClearAndSaveTest()
        {
            string key1 = UnitTestDebugKey + "aaaaaaaaaaaaaaaaaaa";
            string key2 = UnitTestDebugKey + "bbbbbbbbbbbbbbbbb";
            string key3 = UnitTestDebugKey + "cccccccc";
            LocalStorage.SetValue(key1, 3454136);
            LocalStorage.SetValue(key2, "hogehoge");
            LocalStorage.SetValue(key3, true);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.AreEqual(LocalStorage.HasKey(key1), true);
            Assert.AreEqual(LocalStorage.HasKey(key2), true);
            Assert.AreEqual(LocalStorage.HasKey(key3), true);
            LocalStorage.Clear();
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.AreEqual(LocalStorage.HasKey(key1), false);
            Assert.AreEqual(LocalStorage.HasKey(key2), false);
            Assert.AreEqual(LocalStorage.HasKey(key3), false);
        }

        /// <summary>
        /// <para>ClearしてもSaveしなかったら復旧できる</para>
        /// </summary>
        [Test]
        public void ClearAndReloadTest()
        {
            string key1 = UnitTestDebugKey + "aaaaaaaaaaaaaaaaaaa";
            string key2 = UnitTestDebugKey + "bbbbbbbbbbbbbbbbb";
            string key3 = UnitTestDebugKey + "cccccccc";
            LocalStorage.SetValue(key1, 3454136);
            LocalStorage.SetValue(key2, "hogehoge");
            LocalStorage.SetValue(key3, true);
            LocalStorage.Save();
            LocalStorage.Reload();
            Assert.AreEqual(LocalStorage.HasKey(key1), true);
            Assert.AreEqual(LocalStorage.HasKey(key2), true);
            Assert.AreEqual(LocalStorage.HasKey(key3), true);
            LocalStorage.Clear();
            Assert.AreEqual(LocalStorage.HasKey(key1), false);
            Assert.AreEqual(LocalStorage.HasKey(key2), false);
            Assert.AreEqual(LocalStorage.HasKey(key3), false);
            LocalStorage.Reload();
            Assert.AreEqual(LocalStorage.HasKey(key1), true);
            Assert.AreEqual(LocalStorage.HasKey(key2), true);
            Assert.AreEqual(LocalStorage.HasKey(key3), true);
        }

        /// <summary>
        /// <para>設定されているデータを Object(Dictionary型)で取得できること</para>
        /// </summary>
        [Test]
        public void GetGenericObjectDicTest()
        {
            Dictionary<string, string> sampleDic = new Dictionary<string, string>();
            sampleDic.Add("aaa", "bbb");
            LocalStorage.SetValue(UnitTestDebugKey, sampleDic);

            Dictionary<string, string> savedDic = LocalStorage.GetGenericObject<Dictionary<string, string>>(UnitTestDebugKey);
            Assert.That(savedDic["aaa"], Is.EqualTo("bbb"));
        }

        /// <summary>
        /// <para>設定されているデータを Object(List型)で取得できること</para>
        /// </summary>
        [Test]
        public void GetGenericObjectListTest()
        {
            List<int> sampleList = new List<int>();
            sampleList.Add(198751);
            sampleList.Add(518634146);
            LocalStorage.SetValue(UnitTestDebugKey, sampleList);

            List<int> savedList = LocalStorage.GetGenericObject<List<int>>(UnitTestDebugKey);
            Assert.That(savedList[0], Is.EqualTo(198751));
            Assert.That(savedList[1], Is.EqualTo(518634146));
        }

        /// <summary>
        /// <para>一度ディスクに保存したObject(Dictionary型)がちゃんと取得できること</para>
        /// </summary>
        [Test]
        public void GetReloadGenericObjectDicTest()
        {
            Dictionary<string, string> sampleDic = new Dictionary<string, string>();
            sampleDic.Add("aaa", "bbb");
            LocalStorage.SetValue(UnitTestDebugKey, sampleDic);
            LocalStorage.Save();
            LocalStorage.Reload();

            Dictionary<string, string> savedDic = LocalStorage.GetGenericObject<Dictionary<string, string>>(UnitTestDebugKey);
            Assert.That(savedDic["aaa"], Is.EqualTo("bbb"));
        }

        /// <summary>
        /// <para>一度ディスクに保存したObject(List型)がちゃんと取得できること</para>
        /// </summary>
        [Test]
        public void GetReloadGenericObjectListTest()
        {
            List<int> sampleList = new List<int>();
            sampleList.Add(198751);
            sampleList.Add(518634146);
            LocalStorage.SetValue(UnitTestDebugKey, sampleList);
            LocalStorage.Save();
            LocalStorage.Reload();

            List<int> savedList = LocalStorage.GetGenericObject<List<int>>(UnitTestDebugKey);
            Assert.That(savedList[0], Is.EqualTo(198751));
            Assert.That(savedList[1], Is.EqualTo(518634146));
        }

        /// <summary>
        /// <para>Setしていない、Objectを取得しようとするとき、default値を指定したらその値を取得できる</para>
        /// </summary>
        [Test]
        public void GetGenericObjectIfNotExistTest()
        {
            List<float> sampleList = new List<float>();
            sampleList.Add(1751.5234f);

            List<float> defaultList = LocalStorage.GetGenericObject<List<float>>(UnitTestDebugKey, sampleList);
            Assert.That(defaultList[0], Is.EqualTo(1751.5234f));
        }

        /// <summary>
        /// <para>Setしていない、Objectを取得しようとしたらそれぞれのdefault値が取得できる</para>
        /// </summary>
        [Test]
        public void GetGenericObjectDefaultTest()
        {
            Assert.AreEqual(LocalStorage.GetGenericObject<List<double>>(UnitTestDebugKey), null);
            Assert.AreEqual(LocalStorage.GetGenericObject<int>(UnitTestDebugKey), 0);
        }
    }
}