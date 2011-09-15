using System.Collections.Generic;
using System.IO;
using System.Text;
using eaep.servicehost.http;

namespace eaep.servicehost.test.http
{
    class MockResourceRepository : IResourceRepository
    {
        Dictionary<string, MemoryStream> repository = new Dictionary<string, MemoryStream>();

        public void LoadResource(string name, byte[] content)
        {
            LoadResource(name, new MemoryStream(content));
        }

        public void LoadResource(string name, MemoryStream stream)
        {
            repository.Add(name, stream);
        }

        #region IResourceRepository Members

        public void WriteResource(string resourceName, StreamWriter writer)
        {
            writer.Write(GetResourceAsString(resourceName));
        }

        public void WriteResource(string resourceName, Stream stream)
        {
            using (MemoryStream source = repository[resourceName])
            {
                source.WriteTo(stream);
            }
        }

        public string GetResourceAsString(string resourceName)
        {
            using (MemoryStream source = repository[resourceName])
            {
                return Encoding.UTF8.GetString(source.ToArray());
            }
        }

        #endregion
    }
}
