using System;

namespace SensenToolkit
{
    public struct ChatGptRequestCommand
    {
        private const string RequestJsonWithSystemPromptTemplate = @"{{
            ""messages"": [
                {{
                    ""role"": ""system"",
                    ""content"": ""{0}""
                }},
                {{
                    ""role"": ""user"",
                    ""content"": ""{1}""
                }}
            ],
            ""model"": ""{2}"",
            ""max_tokens"": {3},
            ""temperature"": {4},
            ""n"": {5},
            ""stream"": false
        }}";
        private const string RequestJsonSimplePrompt = @"{{
            ""messages"": [
                {{
                    ""role"": ""user"",
                    ""content"": ""{0}""
                }}
            ],
            ""model"": ""{1}"",
            ""max_tokens"": {2},
            ""temperature"": {3},
            ""n"": {4},
            ""stream"": false
        }}";
        public string SystemPrompt { get; private set; }
        public string UserPrompt { get; private set; }
        public ChatGptModel Model { get; private set; }
        public int MaxTokens { get; private set; }
        public float Temperature { get; private set; }
        public int N { get; private set; }
        public bool HasSystemPrompt => !string.IsNullOrEmpty(SystemPrompt);

        public ChatGptRequestCommand(string userPrompt, string systemPrompt = "", ChatGptModel model = null, int maxTokens = 500, float temperature = 0.7f, int n = 1)
        {
            SystemPrompt = systemPrompt;
            UserPrompt = userPrompt;
            Model = model;
            MaxTokens = maxTokens;
            Temperature = temperature;
            N = n;
        }

        public string ToJson()
        {
            string temperatureFormatted = Temperature
                .ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)
                .Replace(",", ".");
            if (this.HasSystemPrompt)
            {
                return string.Format(
                    RequestJsonWithSystemPromptTemplate,
                    EscapeStr(SystemPrompt),
                    EscapeStr(UserPrompt),
                    Model.Id,
                    MaxTokens,
                    temperatureFormatted,
                    N
                );
            }
            else
            {
                return string.Format(
                    RequestJsonSimplePrompt,
                    EscapeStr(UserPrompt),
                    Model.Id,
                    MaxTokens,
                    temperatureFormatted,
                    N
                );
            }
        }

        private static string EscapeStr(string str)
        {
            return str
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t");
        }
    }
}
