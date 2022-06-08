using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Networking;

using AppsFlyerSDK;

namespace MistplaySDK
{
    public class MistplayFeedbackManager : Singleton<MistplayFeedbackManager>
    {
        MistplayFeedbackPrompt prompt;
        string country;

        protected override void OnAwake()
        {
            prompt = GetComponentInChildren<MistplayFeedbackPrompt>(true);
            StartCoroutine(GetCountry());
        }

        public void Show()
        {
            prompt.Show(Create);
        }

        public void Create(string feedback)
        {
            StartCoroutine(CreateAsync(feedback));
        }

        IEnumerator CreateAsync(string feedback)
        {
            feedback = Regex.Replace(feedback, @"\r\n?|\n", "\\n");

            var request = new UnityWebRequest("https://api.notion.com/v1/pages", UnityWebRequest.kHttpVerbPOST);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", "2022-02-22");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer secret_Sq2AyuHY6ZhELLe2l3y65ot111ynlqcP2tQ5tMK40xx");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(CreateJSON(feedback)));

            yield return request.SendWebRequest();

            Log("New feedback : " + request.result);
        }

        string CreateJSON(string feedback)
        {
            return @$"
            {{
                ""parent"": {{ ""database_id"": ""c820b91eac434f34b53fb235ee3b95dd"" }},
                ""properties"": {{
                    {CreateTitleJSON(Application.identifier)},
                    {CreateDateJSON()},
                    {CreateVersionJSON()},
                    {CreateInfosJSON()}
                }},
                {CreateCommentJSON(feedback)}
            }}";
        }

        string CreateDateJSON()
        {
            return @$"
            ""Date"": {{
                ""date"": {{
                    ""start"" : ""{DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}""
                }}
            }}";
        }

        string CreateVersionJSON()
        {
            return @$"
            ""Version"": {{
                ""rich_text"": [
                    {{
                        ""text"": {{
                            ""content"": ""{Application.version}""
                        }}
                    }}
                ]
            }}";
        }

        string CreateTitleJSON(string title)
        {
            return @$"
            ""Name"": {{
                ""title"": [
                    {{
                        ""text"": {{
                            ""content"": ""{title}""
                        }}
                    }}
                ]
            }}";
        }

        string CreateCommentJSON(string comment)
        {
            return @$"
            ""children"": [
                {{
                    ""object"": ""block"",
                    ""type"": ""paragraph"",
                    ""paragraph"": {{
                        ""rich_text"": [
                            {{
                                ""type"": ""text"",
                                ""text"": {{
                                    ""content"": ""{comment}""
                                }}
                            }}
                        ]
                    }}
                }}
	        ]";
        }

        string CreateInfosJSON()
        {
            return @$"
            ""Device"": {{
                ""rich_text"": [
                    {{
                        ""text"": {{
                            ""content"": ""{SystemInfo.deviceModel}""
                        }}
                    }}
                ]
            }},
            ""ID"": {{
                ""rich_text"": [
                    {{
                        ""text"": {{
                            ""content"": ""{AppsFlyer.getAppsFlyerId()}""
                        }}
                    }}
                ]
            }},
            ""OS"": {{
                ""rich_text"": [
                    {{
                        ""text"": {{
                            ""content"": ""{SystemInfo.operatingSystem}""
                        }}
                    }}
                ]
            }},
            ""Country"": {{
                ""rich_text"": [
                    {{
                        ""text"": {{
                            ""content"": ""{country}""
                        }}
                    }}
                ]
            }}";
        }

        IEnumerator GetCountry()
        {
            string ip = "";
            using(var request = UnityWebRequest.Get("https://api.ipify.org"))
            {
                ip = request.downloadHandler.text;
            }

            string uri = $"https://ipapi.co/{ip}/json/";

            using(var request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                
                var key = "country_name";
                country = request.downloadHandler.text.Substring(request.downloadHandler.text.IndexOf(key) + key.Length + 4);
                country = country.Substring(0, country.IndexOf('"'));
            }
        }
    }
}
