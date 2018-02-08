using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace ImageGallery.Client.Test.UI.Fixtures.TestData
{
    public class UserDataCsvDataAttribute : DataAttribute
    {
        public string FileName { get; set; }

        public string FileFullPath { get; set; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var filePath = GetTestFilePath();
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File {filePath} cannot be found.");
            }

            return File.ReadAllLines(filePath).
                Select(u =>
                {
                    var userData = u.Split(new char[] { ';' });
                    return new object[] { userData[0], userData[1], bool.Parse(userData[2]) };
                });
        }

        private string GetTestFilePath()
        {
            if (!string.IsNullOrEmpty(FileFullPath))
            {
                return FileFullPath;
            }

            var assemblyName = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyName);
            return Path.Combine(assemblyDirectory, FileName);
        }
    }
}
