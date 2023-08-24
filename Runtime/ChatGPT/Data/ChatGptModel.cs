namespace SensenToolkit
{
    public sealed class ChatGptModel
    {
        public static readonly ChatGptModel Gpt3_5_Turbo16k = new("gpt-3.5-turbo-16k");
        public static readonly ChatGptModel Gpt3_5_Turbo = new("gpt-3.5-turbo");
        public static readonly ChatGptModel Gpt4_32k = new("gpt-4-32k");
        public static readonly ChatGptModel Gpt4 = new("gpt-4");
        public string Id { get; private set; }

        private ChatGptModel(string id)
        {
            Id = id;
        }
    }
}
