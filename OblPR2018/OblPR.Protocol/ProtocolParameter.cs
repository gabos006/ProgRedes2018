using System;

namespace OblPR.Protocol
{
    public class ProtocolParameter
    {


        public ProtocolParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}