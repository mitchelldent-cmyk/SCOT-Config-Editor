using System;
using System.IO;
using System.Xml;

namespace SCOTConfigEditor.Helpers
{
    /// <summary>
    /// Reader/writer for SSCOUI.exe.config (XML applicationSettings format).
    /// </summary>
    public class XmlConfigFile
    {
        private readonly XmlDocument _doc;
        public string FilePath { get; }
        public bool Loaded { get; private set; }

        public XmlConfigFile(string filePath)
        {
            FilePath = filePath;
            _doc = new XmlDocument();
            if (File.Exists(filePath))
            {
                _doc.Load(filePath);
                Loaded = true;
            }
        }

        public string GetSetting(string sectionName, string settingName)
        {
            var node = _doc.SelectSingleNode(
                "//applicationSettings/" + sectionName +
                "/setting[@name='" + settingName + "']/value");
            return node?.InnerText ?? string.Empty;
        }

        public void SetSetting(string sectionName, string settingName, string value)
        {
            var node = _doc.SelectSingleNode(
                "//applicationSettings/" + sectionName +
                "/setting[@name='" + settingName + "']/value");
            if (node != null)
                node.InnerText = value;
        }

        public void Save()
        {
            if (File.Exists(FilePath))
                File.Copy(FilePath, FilePath + ".bak", overwrite: true);
            _doc.Save(FilePath);
        }
    }
}
