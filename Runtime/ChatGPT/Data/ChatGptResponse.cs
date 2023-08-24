namespace SensenToolkit
{
    public class ChatGptResponse
    {
        public string RawResponseJson { get; internal set; }
        public string PromptRawResponse { get; internal set; }
        public string Error { get; internal set; }
        public bool IsComplete => !string.IsNullOrEmpty(PromptRawResponse) || !string.IsNullOrEmpty(Error);
        public bool IsError => !string.IsNullOrEmpty(Error);
        public bool IsSuccess => !IsError && !string.IsNullOrEmpty(PromptRawResponse);
    }
}
