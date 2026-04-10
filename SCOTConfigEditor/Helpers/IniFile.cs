using System;
using System.Collections.Generic;
using System.IO;

namespace SCOTConfigEditor.Helpers
{
    /// <summary>
    /// Line-preserving INI reader/writer. Comments and whitespace are kept intact.
    /// </summary>
    public class IniFile
    {
        private readonly List<string> _lines;
        public string FilePath { get; }
        public bool Loaded { get; private set; }

        public IniFile(string filePath)
        {
            FilePath = filePath;
            _lines = new List<string>();
            if (File.Exists(filePath))
            {
                _lines.AddRange(File.ReadAllLines(filePath));
                Loaded = true;
            }
        }

        public string Get(string section, string key)
        {
            bool inSection = false;
            foreach (var line in _lines)
            {
                var t = line.Trim();
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    inSection = string.Equals(t, "[" + section + "]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                if (!inSection || t.StartsWith(";") || !t.Contains("="))
                    continue;
                int eq = t.IndexOf('=');
                if (t.Substring(0, eq).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return t.Substring(eq + 1).Trim().Trim('"');
            }
            return string.Empty;
        }

        public bool Has(string section, string key)
        {
            bool inSection = false;
            foreach (var line in _lines)
            {
                var t = line.Trim();
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    inSection = string.Equals(t, "[" + section + "]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                if (!inSection || t.StartsWith(";") || !t.Contains("="))
                    continue;
                int eq = t.IndexOf('=');
                if (t.Substring(0, eq).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public void Set(string section, string key, string value)
        {
            bool inSection = false;
            for (int i = 0; i < _lines.Count; i++)
            {
                var t = _lines[i].Trim();
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    inSection = string.Equals(t, "[" + section + "]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                if (!inSection || t.StartsWith(";") || !t.Contains("="))
                    continue;
                int eq = t.IndexOf('=');
                if (t.Substring(0, eq).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    int lineEq = _lines[i].IndexOf('=');
                    _lines[i] = _lines[i].Substring(0, lineEq + 1) + value;
                    return;
                }
            }
        }

        public void Save()
        {
            if (File.Exists(FilePath))
                File.Copy(FilePath, FilePath + ".bak", overwrite: true);
            File.WriteAllLines(FilePath, _lines);
        }
    }
}
