using System;
using UnityEngine;

namespace SensenToolkit
{
    [Serializable]
    public struct ChatGptResponseRawBody
    {
        public ChatGptResponseRawBodyChoice[] choices;
        public ChatGptResponseRawBodyUsage usage;
    }
}
