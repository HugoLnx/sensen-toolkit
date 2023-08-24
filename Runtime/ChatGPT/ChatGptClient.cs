using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SensenToolkit
{
    public class ChatGptClient
    {
        private const string BaseUrl = "https://api.openai.com/v1/chat/completions";
        private readonly MonoBehaviour _mono;
        private readonly string _apiKey;

        public ChatGptClient(MonoBehaviour mono, string apiKey)
        {
            _mono = mono;
            _apiKey = apiKey;
        }

        public async Task<string> Ask(ChatGptRequestCommand command)
        {
            ChatGptResponse response = new();
            _mono.StartCoroutine(RequestCoroutine(command, response));
            await TaskUtils
                .DelayUntil(() => response.IsComplete)
                .AwaitInAnyThread();

            if (response.IsError)
            {
                Debug.LogWarning($"[{nameof(ChatGptClient)}] {response.Error}");
                throw new Exception(response.Error);
            }
            return response.PromptRawResponse;
        }

        private IEnumerator RequestCoroutine(ChatGptRequestCommand command, ChatGptResponse response)
        {
            // Send post request with json body data
            string requestJsonBody = command.ToJson();

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJsonBody);
            using UnityWebRequest request = new(BaseUrl, "POST");
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");
            yield return request.SendWebRequest();

            string responseJson = request.downloadHandler.text;
            response.RawResponseJson = responseJson;
            if (request.result != UnityWebRequest.Result.Success)
            {
                string resultName = request.result.ToString();
                response.Error = $"Request result was not success. Result:{resultName}"
                    + $"\nError:{request.error}"
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }

            if (request.responseCode < 200 || request.responseCode >= 400)
            {
                response.Error = $"Request response status code was not success. Code:{request.responseCode}"
                    + $"\nError:{request.error}"
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }

            ChatGptResponseRawBody rawResponseBody = default;
            try
            {
                rawResponseBody = JsonUtility.FromJson<ChatGptResponseRawBody>(responseJson);
            }
            catch (Exception e)
            {
                response.Error = $"Error on parsing response json. Error:{e.Message}"
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }

            if (rawResponseBody.choices.Length == 0)
            {
                response.Error = "Request response body did not contain any choices"
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }

            ChatGptResponseRawBodyChoice choice = rawResponseBody.choices[0];
            if (!choice.finish_reason.Equals("stop"))
            {
                response.Error = "The API didn't answered the prompt up to the end."
                    + $" Finish Reason:{rawResponseBody.choices[0].finish_reason}."
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }

            response.PromptRawResponse = choice.message.content;

            if (!response.IsComplete)
            {
                response.Error = "The API answered the prompt with an empty response."
                    + $"\nRequest Body:{requestJsonBody}"
                    + $"\nResponse Body:{responseJson}";
                yield break;
            }
        }
    }
}
