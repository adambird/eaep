namespace eaep.servicehost.http
{
    public class Constants
    {
        public const int HTTP_200_OK = 200;
        public const int HTTP_500_SERVER_ERROR = 500;

        public const string QUERY_STRING_QUERY = "q";
        public const string QUERY_STRING_FROM = "from";
        public const string QUERY_STRING_TO = "to";
        public const string QUERY_STRING_TIMESLICES = "timeslices";
        public const string QUERY_STRING_GROUPBY = "groupby";
        public const string QUERY_STRING_FIELD = "field";

        public const string FORMAT_DATETIME = EAEPMessage.TIMESTAMP_FORMAT;

        public const string CONTENT_TYPE_JSON = "application/json";
    }
}
