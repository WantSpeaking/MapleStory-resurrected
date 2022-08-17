﻿/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

using Loxodon.Framework.Observables;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Loxodon.Framework.Localizations
{
    public class Localization : ILocalization
    {
        private static readonly object _instanceLock = new object();
        private static Localization instance;

        private readonly object _lock = new object();
        private readonly ConcurrentDictionary<string, IObservableProperty> data = new ConcurrentDictionary<string, IObservableProperty>();
        private readonly List<ProviderEntry> providers = new List<ProviderEntry>();
        private CultureInfo cultureInfo;
        private EventHandler cultureInfoChanged;

        public static Localization Current
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (_instanceLock)
                {
                    if (instance == null)
                        instance = new Localization();
                    return instance;
                }
            }
            set { lock (_instanceLock) { instance = value; } }
        }

        protected Localization() : this(null)
        {
        }

        protected Localization(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
            if (this.cultureInfo == null)
                this.cultureInfo = Locale.GetCultureInfo();
        }

        public event EventHandler CultureInfoChanged
        {
            add { lock (_lock) { this.cultureInfoChanged += value; } }
            remove { lock (_lock) { this.cultureInfoChanged -= value; } }
        }

        public virtual CultureInfo CultureInfo
        {
            get { return this.cultureInfo; }
            set
            {
                if (value == null || (this.cultureInfo != null && this.cultureInfo.Equals(value)))
                    return;

                this.cultureInfo = value;
                this.OnCultureInfoChanged();
            }
        }

        protected void RaiseCultureInfoChanged()
        {
            try
            {
                this.cultureInfoChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception) { }
        }

        protected virtual void OnCultureInfoChanged()
        {
            RaiseCultureInfoChanged();
            this.Refresh();
        }

        public Task AddDataProvider(IDataProvider provider)
        {
            return DoAddDataProvider(provider);
        }

        protected virtual async Task DoAddDataProvider(IDataProvider provider)
        {
            if (provider == null)
                return;

            var entry = new ProviderEntry(provider);
            lock (_lock)
            {
                if (this.providers.Exists(e => e.Provider == provider))
                    return;
                this.providers.Add(entry);
            }

            await this.Load(entry);
        }

        public virtual void RemoveDataProvider(IDataProvider provider)
        {
            if (provider == null)
                return;

            lock (_lock)
            {
                for (int i = this.providers.Count - 1; i >= 0; i--)
                {
                    var entry = providers[i];
                    if (entry.Provider == provider)
                    {
                        this.providers.RemoveAt(i);
                        OnUnloadCompleted(entry.Keys);
                        (provider as IDisposable)?.Dispose();
                        return;
                    }
                }
            }
        }

        public Task Refresh()
        {
            return this.Load(this.providers.ToArray());
        }

        protected virtual async Task Load(params ProviderEntry[] providers)
        {
            if (providers == null || providers.Length <= 0)
                return;

            int count = providers.Length;
            var cultureInfo = this.CultureInfo;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var entry = providers[i];
                    var provider = entry.Provider;
                    var dict = await provider.Load(cultureInfo);
                    OnLoadCompleted(entry, dict);
                }
                catch (Exception) { }
            }
        }

        protected virtual void OnLoadCompleted(ProviderEntry entry, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count <= 0)
                return;

            lock (_lock)
            {
                var keys = entry.Keys;
                keys.Clear();

                foreach (KeyValuePair<string, object> kv in dict)
                {
                    var key = kv.Key;
                    var value = kv.Value;
                    keys.Add(key);
                    AddValue(key, value);
                }
            }
        }

        protected virtual void AddValue(string key, object value)
        {
            IObservableProperty property;
            if (!data.TryGetValue(key, out property))
            {
                Type valueType = value != null ? value.GetType() : typeof(object);
#if NETFX_CORE
                TypeCode typeCode = WinRTLegacy.TypeExtensions.GetTypeCode(valueType);
#else
                TypeCode typeCode = Type.GetTypeCode(valueType);
#endif
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            property = new ObservableProperty<bool>();
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            property = new ObservableProperty<byte>();
                            break;
                        }
                    case TypeCode.Char:
                        {
                            property = new ObservableProperty<char>();
                            break;
                        }
                    case TypeCode.DateTime:
                        {
                            property = new ObservableProperty<DateTime>();
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            property = new ObservableProperty<Decimal>();
                            break;
                        }
                    case TypeCode.Double:
                        {
                            property = new ObservableProperty<Double>();
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            property = new ObservableProperty<short>();
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            property = new ObservableProperty<int>();
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            property = new ObservableProperty<long>();
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            property = new ObservableProperty<sbyte>();
                            break;
                        }
                    case TypeCode.Single:
                        {
                            property = new ObservableProperty<float>();
                            break;
                        }
                    case TypeCode.String:
                        {
                            property = new ObservableProperty<string>();
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            property = new ObservableProperty<UInt16>();
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            property = new ObservableProperty<UInt32>();
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            property = new ObservableProperty<UInt64>();
                            break;
                        }
                    case TypeCode.Object:
                        {
                            if (valueType.Equals(typeof(Vector2)))
                            {
                                property = new ObservableProperty<Vector2>();
                            }
                            else if (valueType.Equals(typeof(Vector3)))
                            {
                                property = new ObservableProperty<Vector3>();
                            }
                            else if (valueType.Equals(typeof(Vector4)))
                            {
                                property = new ObservableProperty<Vector4>();
                            }
                            else if (valueType.Equals(typeof(Color)))
                            {
                                property = new ObservableProperty<Color>();
                            }
                            else
                            {
                                property = new ObservableProperty();
                            }
                            break;
                        }
                    default:
                        {
                            property = new ObservableProperty();
                            break;
                        }
                }
                data[key] = property;
            }
            property.Value = value;
        }

        protected virtual void OnUnloadCompleted(List<string> keys)
        {
            foreach (string key in keys)
            {
                IObservableProperty value;
                if (data.TryRemove(key, out value) && value != null)
                    value.Value = null;
            }
        }

        /// <summary>
        /// Return a decorator localization containing every key from the current
        /// localization that starts with the specified prefix.The prefix is
        /// removed from the keys in the subset.
        /// </summary>
        /// <param name="prefix">The prefix used to select the localization.</param>
        /// <returns>a subset localization</returns>
        public virtual ILocalization Subset(string prefix)
        {
            return new SubsetLocalization(this, prefix);
        }

        /// <summary>
        /// Whether the localization file contains this key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        /// <summary>
        /// Gets a message based on a message key or if no message is found the provided key is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetText(string key)
        {
            return this.GetText(key, key);
        }

        /// <summary>
        /// Gets a message based on a key, or, if the message is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual string GetText(string key, string defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a message based on a key using the supplied args, as defined in "string.Format", or the provided key if no message is found.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual string GetFormattedText(string key, params object[] args)
        {
            string format = this.Get<string>(key, null);
            if (format == null)
                return key;

            return string.Format(format, args);
        }

        /// <summary>
        /// Gets a boolean value based on a key, or, if the value is not found, the value 'false' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool GetBoolean(string key)
        {
            return this.Get(key, false);
        }

        /// <summary>
        /// Gets a boolean value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual bool GetBoolean(string key, bool defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a int value based on a key, or, if the value is not found, the value '0' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int GetInt(string key)
        {
            return this.Get<int>(key);
        }

        /// <summary>
        /// Gets a int value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual int GetInt(string key, int defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a long value based on a key, or, if the value is not found, the value '0' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual long GetLong(string key)
        {
            return this.Get<long>(key);
        }

        /// <summary>
        /// Gets a long value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual long GetLong(string key, long defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a double value based on a key, or, if the value is not found, the value '0' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual double GetDouble(string key)
        {
            return this.Get<double>(key);
        }

        /// <summary>
        /// Gets a double value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual double GetDouble(string key, double defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a float value based on a key, or, if the value is not found, the value '0' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual float GetFloat(string key)
        {
            return this.Get<float>(key);
        }

        /// <summary>
        /// Gets a float value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual float GetFloat(string key, float defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a color value based on a key, or, if the value is not found, the value '#000000' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Color GetColor(string key)
        {
            return this.Get<Color>(key);
        }

        /// <summary>
        /// Gets a color value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual Color GetColor(string key, Color defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a vector3 value based on a key, or, if the value is not found, the value 'Vector3.zero' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Vector3 GetVector3(string key)
        {
            return this.Get<Vector3>(key);
        }

        /// <summary>
        /// Gets a vector3 value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a datetime value based on a key, or, if the value is not found, the value 'DateTime(0)' is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual DateTime GetDateTime(string key)
        {
            return this.Get(key, new DateTime(0));
        }

        /// <summary>
        /// Gets a datetime value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual DateTime GetDateTime(string key, DateTime defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        /// <summary>
        /// Gets a value based on a key, or, if the value is not found, the value 'default(T)' is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return this.Get(key, default(T));
        }

        /// <summary>
        /// Gets a value based on a key, or, if the value is not found, a supplied default value is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key, T defaultValue)
        {
            if (typeof(IObservableProperty).IsAssignableFrom(typeof(T)))
                return (T)GetValue(key);

            IObservableProperty value;
            if (data.TryGetValue(key, out value))
            {
                var p = value as IObservableProperty<T>;
                if (p != null)
                    return p.Value;

                if (value.Value is T)
                    return (T)(value.Value);

                return (T)Convert.ChangeType(value.Value, typeof(T));
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a IObservableProperty value based on a key, if the value is not found, a default value will be created.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IObservableProperty GetValue(string key)
        {
            return GetValue(key, true);
        }

        /// <summary>
        /// Gets a IObservableProperty value based on a key, if the value is not found, a default value will be created.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IObservableProperty GetValue(string key, bool isAutoCreated)
        {
            IObservableProperty value;
            if (data.TryGetValue(key, out value))
                return value;

            if (!isAutoCreated)
                return null;

            lock (_lock)
            {
                if (data.TryGetValue(key, out value))
                    return value;

                value = new ObservableProperty();
                data[key] = value;
                return value;
            }
        }

        protected class ProviderEntry
        {
            public ProviderEntry(IDataProvider provider)
            {
                this.Provider = provider;
                this.Keys = new List<string>();
            }

            public IDataProvider Provider { get; private set; }
            public List<string> Keys { get; private set; }
        }
    }
}
