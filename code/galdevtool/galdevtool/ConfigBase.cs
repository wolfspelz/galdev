using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace galdevtool
{
    public class ConfigBase : ICallbackConfig
    {
        private static Random _random;
        public Random Random => GetRandom();
        public static Random GetRandom()
        {
            return _random ?? (_random = new Random());
        }

        object ICallbackConfig.Get(string name, object defaultValue)
        {
            var value = Get(name);
            if (value == null)
            {
                Log.Warning($"No config for: {name}");
            }

            return value ?? defaultValue;
        }

        void ICallbackConfig.Set(string name, object value)
        {
            string s;
            if (value is string alreadyString)
            {
                s = alreadyString;
            }
            else if (value is float f)
            {
                s = f.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is double d)
            {
                s = d.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is int i)
            {
                s = i.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is long l)
            {
                s = l.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is DateTime dt)
            {
                s = dt.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                s = value.ToString();
            }

            Set(name, s);
        }

        public bool Set(string key, string value)
        {
            return Set(this, key, value);
        }

        protected bool Set(object obj, string key, string value)
        {
            var parts = key.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1)
            {
                var name = parts[0];
                var remainingKey = string.Join(".", parts.ToList().Skip(1));
                var prop = obj.GetType().GetProperty(name);
                if (prop != null)
                {
                    var member = prop.GetValue(obj, null);
                    return Set(member, remainingKey, value);
                }
                var field = obj.GetType().GetField(name);
                if (field != null)
                {
                    var member = field.GetValue(obj);
                    return Set(member, remainingKey, value);
                }
                return false;
            }

            // ReSharper disable once RedundantAssignment
            PropertyInfo propInfo = null;
            FieldInfo fieldOnfo = null;
            Type varType = null;
            object varValue = null;

            propInfo = obj.GetType().GetProperty(key);
            if (propInfo != null)
            {
                varType = propInfo.PropertyType;
            }
            else
            {
                fieldOnfo = obj.GetType().GetField(key);
                if (fieldOnfo != null)
                {
                    varType = fieldOnfo.FieldType;
                }
            }

            if (varType != null)
            {
                if (varType == typeof(string))
                {
                    varValue = value;
                }
                else if (varType == typeof(int))
                {
                    varValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                else if (varType == typeof(long))
                {
                    varValue = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                }
                else if (varType == typeof(float))
                {
                    varValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }
                else if (varType == typeof(double))
                {
                    varValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }
                else if (varType == typeof(bool))
                {
                    varValue = value.IsTrue();
                }
                else if (varType == typeof(DateTime))
                {
                    varValue = DateTime.Parse(value, CultureInfo.InvariantCulture);
                }
            }

            if (propInfo != null)
            {
                propInfo.SetValue(obj, varValue, null);
                return true;
            }
            else if (fieldOnfo != null)
            {
                fieldOnfo.SetValue(obj, varValue);
                return true;
            }

            return false;
        }

        public object Get(string key)
        {
            object value = null;

            var propInfo = GetType().GetProperty(key);
            if (propInfo != null)
            {
                value = propInfo.GetValue(this, null);
            }
            else
            {
                var fieldInfo = GetType().GetField(key);
                if (fieldInfo != null)
                {
                    value = fieldInfo.GetValue(this);
                }
            }

            return value;
        }

        public string GetAsString(string key)
        {
            object value = Get(key);

            if (value != null)
            {
                if (value is float f) { return f.ToString(CultureInfo.InvariantCulture); }
                if (value is double d) { return d.ToString(CultureInfo.InvariantCulture); }
                if (value is DateTime dt) { return dt.ToString(CultureInfo.InvariantCulture); }
            }

            return value?.ToString() ?? "";
        }

        public Dictionary<string, object> GetAll()
        {
            var type = GetType();
            var props = type.GetProperties().Select(pi => new { Name = pi.Name, Value = pi.GetValue(this, null) });
            var fields = type.GetFields().Select(fi => new { Name = fi.Name, Value = fi.GetValue(this) });
            return props.Union(fields).ToDictionary(ks => ks.Name, vs => vs.Value);
        }

        public delegate object FilterAction(string name, object value);
        public List<FilterAction> FilterList { get; set; } = new List<FilterAction>();

        protected bool Filter(string name, bool value)
        {
            foreach (var filter in FilterList)
            {
                value = (bool)filter(name, value);
            }
            return value;
        }

        public static string GetAssemblyNameVersion()
        {
            return $"{GetAssemblyName()}/{GetAssemblyVersion()}";
        }

        public static string GetAssemblyName()
        {
            var assembly = Assembly.GetEntryAssembly();
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileInfo.FileDescription;
        }

        public static string GetAssemblyVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileInfo.FileVersion;
        }

        public static string GetAssemblyCompany()
        {
            var assembly = Assembly.GetEntryAssembly();
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileInfo.CompanyName;
        }

        public static string GetAssemblyProduct()
        {
            var assembly = Assembly.GetEntryAssembly();
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileInfo.ProductName;
        }

        public void ParseCommandline(IList<string> args)
        {
            BeforeCommandline();

            foreach (var arg in args)
            {
                HandleCommandlineParameter(arg);
            }

            AfterCommandline();
        }

        protected virtual void HandleCommandlineParameter(string arg)
        {
        }

        protected virtual void BeforeCommandline()
        {
        }

        protected virtual void AfterCommandline()
        {
        }

    }
}
