using System;
using UnityEngine;

namespace SensenToolkit
{
    [Serializable]
    public struct ChatGptResponseRawBodyUsage
    {
        public int prompt_tokens;
        public string completion_tokens;
        public string total_tokens;
    }
}
