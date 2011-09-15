using System;

namespace eaep
{
    public class EventParameter
    {
        public EventParameter(string name, string value)
        {
            if(name == null)
            {
                throw new ArgumentNullException("name");
            }

            if(value == null)
            {
                throw new ArgumentNullException("value");
            }

            if(name == "")
            {
                throw new ArgumentException("name cannot be null", "name");
            }

            if(value == "")
            {
                throw new ArgumentException("value cannot be null", "value");
            }


            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}