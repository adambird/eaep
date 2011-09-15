using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace eaep
{
	public class EAEPMessage
	{
		public const char HEADER_DELIMITER = ' ';
		public const char AVP_DELIMITER = '=';
		public const char AVP_VALUEDELIMITER = ',';
		public const string END_OF_MESSAGE = "---";

		public const string PROTOCOL_EAEP = "EAEP";
		public const string VERSION_1_0 = "0.2";
		public const string TIMESTAMP_FORMAT = "dd-MM-yyyy-HH:mm:ss.fff";

		// serialised field names 
		public const string FIELD_APPLICATION = "app";
		public const string FIELD_HOST = "host";
		public const string FIELD_EVENT = "event";
		public const string FIELD_TIMESTAMP = "ts";
        public const string FIELD_TIMESTAMP_YEAR = "ts_year";
        public const string FIELD_TIMESTAMP_MONTH = "ts_month";
        public const string FIELD_TIMESTAMP_DAY = "ts_day";
        public const string FIELD_TIMESTAMP_HOUR = "ts_hour";
        public const string FIELD_TIMESTAMP_MINUTE = "ts_min";
        public const string FIELD_TIMESTAMP_SECOND = "ts_sec";
        public const string FIELD_TIMESTAMP_MILLISECOND = "ts_ms";
        public const string FIELD_MESSAGE = "message";

		protected const string PARAM_VALIDATION_EXP = "(^" +
			FIELD_HOST + "$)|(^" +
			FIELD_MESSAGE + "$)|(^" +
			FIELD_TIMESTAMP + "$)|(^" +
			FIELD_EVENT + "$)|(^" +
			FIELD_APPLICATION +
			"$)";

        protected static Regex paramValidationRegEx;

		// well known parameters
		public const string PARAM_USER = "user";

		protected string version = VERSION_1_0;
		protected DateTime timeStamp = DateTime.Now;
		protected string host;
		protected string application;
		protected string eventName;
        protected Dictionary<string, string> paramDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        static EAEPMessage()
        {
            paramValidationRegEx = new Regex(PARAM_VALIDATION_EXP, RegexOptions.Compiled);
        }

		public EAEPMessage()
		{
		}

		public EAEPMessage(string message)
		{
			this.Load(message);
		}

		public EAEPMessage(string host, string application, string eventName)
			: this (DateTime.Now, host, application, eventName)
		{
		}

		public EAEPMessage(DateTime timeStamp, string host, string application, string eventName)
		{
			this.TimeStamp = timeStamp;
			this.Host = host;
			this.Application = application;
			this.Event = eventName;
		}

		
		public string Version
		{
			get { return version; }
			set { version = value; }
		}

		public string Host
		{
			get { return host; }
			set 
            { 
                host = value;
                //SetParameter(FIELD_HOST, value);
            }
		}

		public string Application
		{
			get { return application; }
			set 
            { 
                application = value;
                //SetParameter(FIELD_APPLICATION, value);
            }
		}

		public string Event
		{
			get { return eventName; }
			set 
            { 
                eventName = value;
                //SetParameter(FIELD_EVENT, value);
            }
		}

		public DateTime TimeStamp
		{
			get { return timeStamp; }
			set 
			{ 
				// parse to ensure same level accuracy across serialisation
				timeStamp = DateTime.ParseExact(value.ToString(TIMESTAMP_FORMAT), TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
			}
		}

        public Dictionary<string, string> Parameters
        {
            get { return paramDictionary; }
        }

        public string[] ParameterKeys
        {
            get
            {
                string[] parameterKeys = new string[paramDictionary.Keys.Count];
                paramDictionary.Keys.CopyTo(parameterKeys, 0);
                return parameterKeys;
            }
        }

		public void AddParamAVP(string avp)
		{
			ParseParamLine(avp);
		}

        protected void SetParameter_Validated(string param, string value)
        {
            if (ParamNameIsValid(param))
            {
                SetParameter(param, value);
            }
            else
            {
                throw new ArgumentException("Parameter name reserved", "param");
            }
        }

        protected void SetParameter(string param, string value)
        {
            if (paramDictionary.ContainsKey(param))
            {
                paramDictionary[param] = value;
            }
            else
            {
                paramDictionary.Add(param, value);
            }
        }

		public string this[string param]
		{
			get { return paramDictionary[param]; }
			set { SetParameter_Validated(param, value); }
		}

        /// <summary>
        /// This will ensure any parameter names added via the Item accessor will not 
        /// collide with those reserved names.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
		public static bool ParamNameIsValid(string param)
		{
            return !paramValidationRegEx.IsMatch(param);
		}

		public bool ContainsParameter(string param)
		{
			return paramDictionary.ContainsKey(param);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != this.GetType()) return false;

			EAEPMessage other = (EAEPMessage)obj;

			if (!Object.Equals(other.Version, this.Version)) return false;
			if (!Object.Equals(other.Application, this.Application)) return false;
			if (!Object.Equals(other.Event, this.Event)) return false;
			if (!Object.Equals(other.Host, this.Host)) return false;
			if (!Object.Equals(other.TimeStamp, this.TimeStamp)) return false;

			if (other.paramDictionary.Count != this.paramDictionary.Count) return false;

			foreach (string key in this.paramDictionary.Keys)
			{
				if (!other.paramDictionary.ContainsKey(key)) return false;

				if (!Object.Equals(other[key], this[key])) return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			StringWriter writer = new StringWriter(builder);

			WriteHeaderLine(writer);
			WriteEventLine(writer);
			WriteParams(writer);

			writer.WriteLine(END_OF_MESSAGE);

			return builder.ToString();
		}

		protected void WriteHeaderLine(StringWriter writer)
		{
			writer.WriteLine("{1}{0}{2}{0}{3}", HEADER_DELIMITER, PROTOCOL_EAEP, this.Version, this.TimeStamp.ToString(TIMESTAMP_FORMAT));
		}

		protected void WriteEventLine(StringWriter writer)
		{
			writer.WriteLine("{1}{0}{2}{0}{3}", HEADER_DELIMITER, this.Host, this.Application, this.Event);
		}

		protected void WriteParams(StringWriter writer)
		{
			foreach (string key in paramDictionary.Keys)
			{
                // Only write valid parameters, ie not header values also stored in parameter dictionary
                if (ParamNameIsValid(key))
                {
                    string paramValue = paramDictionary[key];
                    if (!string.IsNullOrEmpty(paramValue))
                    {
                        paramValue = Uri.EscapeDataString(paramValue);
                    }
                    writer.WriteLine("{0}{1}{2}", key, AVP_DELIMITER, paramValue);
                }
			}
		}

		public void Load(string message)
		{
			StringReader reader = new StringReader(message);
			Load(reader);
		}

		public void Load(StringReader reader)
		{
			ParseHeaderLine(reader.ReadLine());
			ParseEventLine(reader.ReadLine());
			ParseParams(reader);
		}

		protected void ParseHeaderLine(string line)
		{
			string[] elements = line.Split(HEADER_DELIMITER);
			this.Version = elements[1];
			this.TimeStamp = DateTime.ParseExact(elements[2], TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
		}

		protected void ParseEventLine(string line)
		{
			string[] elements = line.Split(HEADER_DELIMITER);
			this.Host = elements[0];
			this.Application = elements[1];
			this.Event = elements[2];
		}

		protected void ParseParams(StringReader reader)
		{
			while (reader.Peek() != -1)
			{
				string line = reader.ReadLine();
				if (line != END_OF_MESSAGE)
				{
					ParseParamLine(line);
				}
				else
				{
					break;
				}
			}
		}

		protected void ParseParamLine(string line)
		{
			string[] elements = line.Split(AVP_DELIMITER);
		    string paramName = elements[0];
		    string paramValue = elements[1];
            if (!string.IsNullOrEmpty(paramValue)) 
            {
                paramValue = Uri.UnescapeDataString(paramValue);
            }
		    this[paramName] = paramValue;
		}
	}
}