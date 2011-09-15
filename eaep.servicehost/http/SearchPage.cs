using System;
using System.Collections;
using System.IO;
using System.Text;
using eaep.servicehost.store;

namespace eaep.servicehost.http
{
    public class SearchPage : HtmlPage
    {
        IEAEPMonitorStore monitor;

        public SearchPage(IEAEPMonitorStore monitor)
        {
            this.monitor = monitor;
        }

        protected override void InternalWriteContent(StreamWriter writer, IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            resourceRepository.WriteResource("header.htm", writer);

            bool activeSearch = (request.Query != null);

            try
            {

                EAEPMessages messages = new EAEPMessages();
                string searchResultText = string.Empty;

                if (activeSearch)
                {
                    messages = monitor.GetMessages(request.Query);

                    searchResultText = string.Format("{0} message(s) found", messages.Count);
                }

                WriteSearchResultHeader(writer, request, resourceRepository, searchResultText);

                int maxItems = 100;
                if (maxItems > messages.Count) { maxItems = messages.Count; }

                string messageTemplate = resourceRepository.GetResourceAsString("eaepmsg.htm");

                for (int i = 0; i < maxItems; i++)
                {
                    WriteResultItem(writer, messageTemplate, messages[i]);
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine(ReplaceCRLFwithHTMLLineBreaks(ex.ToString()));
            }

            resourceRepository.WriteResource("searchresultfooter.htm", writer);

            resourceRepository.WriteResource("footer.htm", writer);
        }

        private static void WriteSearchResultHeader(StreamWriter writer, IServiceRequest request, IResourceRepository resourceRepository, string searchResultText)
        {
            string header = resourceRepository.GetResourceAsString("searchresultheader.htm");
            Hashtable values = new Hashtable();
            values.Add("queryparam", Constants.QUERY_STRING_QUERY);
            values.Add("query", request.Query);
            values.Add("searchresulttext", searchResultText);
            writer.WriteLine(TemplateParser.Parse(header, values));
        }

        private void WriteResultItem(StreamWriter targetWriter, string template, EAEPMessage message)
        {
            Hashtable values = new Hashtable();
            values.Add("message", ReplaceCRLFwithHTMLLineBreaks(message.ToString()));
            values.Add("host", CreateSearchLink(EAEPMessage.FIELD_HOST, message.Host, message.Host));
            values.Add("app", CreateSearchLink(EAEPMessage.FIELD_APPLICATION, message.Application, message.Application));
            values.Add("event", CreateSearchLink(EAEPMessage.FIELD_EVENT, message.Event, message.Event));
            values.Add("timestamp", message.TimeStamp.ToString(EAEPMessage.TIMESTAMP_FORMAT));

            StringBuilder parameters = new StringBuilder();
            StringWriter parametersWriter = new StringWriter(parameters);
            foreach (string parameter in message.ParameterKeys)
            {
                // only write out if not a reserved parameter name, displayed above
                if (EAEPMessage.ParamNameIsValid(parameter))
                {
                    parametersWriter.WriteLine(CreateSearchLink(parameter, message[parameter], string.Format("{0}={1}", parameter, message[parameter])));
                }
            }
            values.Add("params", parameters.ToString());

            targetWriter.WriteLine(TemplateParser.Parse(template, values));
        }

        private string CreateSearchLink(string fieldname, string value, string label)
        {
            return string.Format("<a href=\"?q={0}:{1}\">{2}</a>", fieldname, value, label);
        }
    }
}
