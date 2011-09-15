using System;
using System.IO;
using System.Reflection;

namespace eaep.servicehost.http
{
    public class ResourceRepository : IResourceRepository
    {
        #region IResourceRepository Members

        public void WriteResource(string resourceName, Stream stream)
        {
            using (Stream resourceStream = GetResourceStream(resourceName))
            {
                if (resourceStream != null)
                {
                    byte[] buffer = new byte[1024];

                    int bytesRead = resourceStream.Read(buffer, 0, buffer.Length);
                    while (bytesRead > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                        bytesRead = resourceStream.Read(buffer, 0, buffer.Length);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("{0} not found", resourceName));
                }
            }

        }

        public string GetResourceAsString(string resourceName)
        {
            using (Stream resourceStream = GetResourceStream(resourceName))
            {
                if (resourceStream != null)
                {
                    using (StreamReader resourceReader = new StreamReader(resourceStream))
                    {
                        return resourceReader.ReadToEnd();
                    }
                }
                else
                {
                    return string.Format("-- Resource: {0} not found --", resourceName);
                }
            }
        }

        public void WriteResource(string resourceName, StreamWriter writer)
        {
            writer.WriteLine(GetResourceAsString(resourceName));
        }

        protected Stream GetResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("eaep.http.resources." + resourceName);
        }

        #endregion
    }
}
