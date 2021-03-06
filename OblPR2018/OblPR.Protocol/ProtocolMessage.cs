﻿using System.Collections.Generic;

namespace OblPR.Protocol
{
    public class ProtocolMessage
    {
        public ProtocolMessage()
        {
            Parameters = new List<ProtocolParameter>();
        }
        public int Command { get; set; }
        public List<ProtocolParameter> Parameters { get; private set; }
    }
}