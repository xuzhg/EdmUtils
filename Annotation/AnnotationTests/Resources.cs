using System;
using System.IO;

namespace AnnotationGenerator.Tests
{
    internal static class Resources
    {
        public static string GetString(string fileName)
        {
            using (Stream stream = GetStream(fileName))
            using (TextReader reader = new StreamReader(stream))
            {
                string fileString = reader.ReadToEnd();

                return fileString;
            }
        }

        private static Stream GetStream(string fileName)
        {
            string path = GetPath(fileName);
            Stream stream = typeof(Resources).Assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                string message = String.Format("The embedded resource '{0}' was not found.", path);
                throw new FileNotFoundException(message, path);
            }

            return stream;
        }

        private static string GetPath(string fileName)
        {
            const string projectDefaultNamespace = "AnnotationTests";
            const string resourcesFolderName = "Resources";
            const string pathSeparator = ".";
            return projectDefaultNamespace + pathSeparator + resourcesFolderName + pathSeparator + fileName;
        }

    }
}
