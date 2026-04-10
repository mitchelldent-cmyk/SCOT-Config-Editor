using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCOTConfigEditor.Helpers
{
    /// <summary>
    /// Reader/writer for SSCOStrings.en-US.custom.dat (UTF-16 LE with BOM).
    /// </summary>
    public class StringsFile
    {
        private readonly List<string> _lines;
        public string FilePath { get; }
        public bool Loaded { get; private set; }

        public StringsFile(string filePath)
        {
            FilePath = filePath;
            _lines = new List<string>();
            if (File.Exists(filePath))
            {
                _lines.AddRange(File.ReadAllLines(filePath, Encoding.Unicode));
                Loaded = true;
            }
        }

        public string Get(string key)
        {
            foreach (var line in _lines)
            {
                var t = line.TrimStart();
                if (t.StartsWith(";") || !t.Contains("="))
                    continue;
                int eq = t.IndexOf('=');
                if (t.Substring(0, eq).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return t.Substring(eq + 1).Trim();
            }
            return string.Empty;
        }

        public List<(string Key, string Value)> GetAll()
        {
            var result = new List<(string, string)>();
            bool inTextSection = false;
            foreach (var line in _lines)
            {
                var t = line.TrimStart();
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    inTextSection = t.Equals("[Text]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                if (!inTextSection || t.StartsWith(";") || !t.Contains("="))
                    continue;
                int eq = t.IndexOf('=');
                result.Add((t.Substring(0, eq).Trim(), t.Substring(eq + 1).Trim()));
            }
            return result;
        }

        public void Set(string key, string value)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                var t = _lines[i].TrimStart();
                if (t.StartsWith(";") || !t.Contains("="))
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
            File.WriteAllLines(FilePath, _lines, Encoding.Unicode);
        }
    }
}
