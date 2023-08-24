using System;
using UnityEngine;

namespace SensenToolkit
{
    [Serializable]
    public struct ChatGptResponseRawBodyChoice
    {
        public int index;
        public ChatGptResponseRawBodyChoiceMessage message;
        public string finish_reason;
    }
}
