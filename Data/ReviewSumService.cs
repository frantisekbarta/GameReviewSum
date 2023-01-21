using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace ReviewSum.Data
{
    // https://rapidapi.com/opencritic-opencritic-default/api/opencritic-api
    // https://app.swaggerhub.com/apis-docs/OpenCritic/OpenCritic-API/1.0.0
    // https://beta.openai.com/docs/api-reference/completions
    // example of OpenAI completion response:
    // '{"id":"cmpl-6KVvRC75ba10XYAQlI2GHN91lTQ1O","object":"text_completion","created":1670347301,"model":"text-curie-001","choices":[{"text":"\\n\\nMy name is Sarah.","index":0,"logprobs":null,"finish_reason":"stop"}],"usage":{"prompt_tokens":5,"completion_tokens":7,"total_tokens":12}}

    public enum GPT3Model
    {
        Davinci,
        Curie,
        Babbage,
        Ada
    }

    public class ReviewSumService : IReviewSumService
    {
        private static HttpClient client = new HttpClient();

        public async Task<List<Game>> SearchGame(string enteredName)
        {
            List<Game> foundGames = new List<Game>();

            if (!string.IsNullOrEmpty(enteredName))
            {
                try
                {
                    string responseString = await SearchGameCallAPI(enteredName);
                    JArray searchJArray = JArray.Parse(responseString);

                    foreach (var item in searchJArray)
                    {
                        foundGames.Add(new Game
                        {
                            Id = (string)item["id"],
                            Name = (string?)item["name"]
                        });
                    }
                }
                catch
                {
                    foundGames = null; // in case service is not available return null
                }
            }
            return foundGames;
        }

        public async Task<Game> GetGame(Game game)
        {
            if (game != null)
            {
                try
                {
                    string responseString = await GetGameCallAPI(game.Id);
                    JObject gameJObject = JObject.Parse(responseString);
                    try
                    {
                        game.SquareImage = (string?)gameJObject["images"]["square"]["lg"];
                    }
                    catch
                    {
                        game.SquareImage = "css/blank_image.png";
                    }
                    game.Url = (string?)gameJObject["url"];
                    game.PercentRecommended = (int?)gameJObject["percentRecommended"];
                    game.NumReviews = (int?)gameJObject["numReviews"];
                    game.NumTopCriticReviews = (int?)gameJObject["numTopCriticReviews"];
                    game.TopCriticScore = (int?)gameJObject["topCriticScore"];
                    game.MedianScore = (int?)gameJObject["medianScore"];
                    game.Description = (string?)gameJObject["description"];
                    game.Companies = new List<string>();

                    foreach (var item in gameJObject["Companies"])
                    {
                        game.Companies.Add((string)item["name"]);
                    }

                    game.Platforms = new List<string>();

                    foreach (var item in gameJObject["Platforms"])
                    {
                        game.Platforms.Add((string)item["shortName"]);
                    }

                    game.Genres = new List<string>();

                    foreach (var item in gameJObject["Genres"])
                    {
                        game.Genres.Add((string)item["name"]);
                    }
                }
                catch
                {
                    game = null; // in case service is not available return null
                }
            }
            return game;
        }

        public async Task<List<Review>> GetReviews(string gameId)
        {
            List<Review> reviews = new List<Review>();

            if (!string.IsNullOrEmpty(gameId))
            {
                try
                {
                    string responseString = await GetReviewsCallAPI(gameId);
                    JArray reviewsJArray = JArray.Parse(responseString);

                    foreach (var item in reviewsJArray)
                    {
                        List<string> authors = new List<string>();

                        foreach (var item2 in item["Authors"])
                        {
                            authors.Add((string)item2["name"]);
                        }

                        List<string> platforms = new List<string>();

                        foreach (var item2 in item["Platforms"])
                        {
                            platforms.Add((string)item2["shortName"]);
                        }
                        reviews.Add(new Review
                        {
                            Id = (string)item["_id"],
                            Title = (string?)item["title"],
                            PublishedDate = (DateTime?)item["publishedDate"],
                            ExternalUrl = (string?)item["externalUrl"],
                            Snippet = (string?)item["snippet"],
                            Score = (int?)item["score"],
                            Recommended = (int?)item["npScore"],
                            Alias = (string?)item["alias"],
                            OutletName = (string?)item["Outlet"]["name"],
                            OutletImage = (string?)item["Outlet"]["imageSrc"]["sm"],
                            Authors = authors,
                            Platforms = platforms
                        });
                    }
                }
                catch
                {
                    reviews = null; // in case service is not available return null
                }
            }
            return reviews;
        }

        private string ExtractSnippets(List<Review> reviews)
        {
            List<string> snippets = new List<string>();
            string allSnippets = "";

            foreach (var review in reviews)
            {
                snippets.Add(review.Snippet);
                allSnippets += review.Snippet + " ";
            }
            allSnippets = allSnippets.Replace("\"", ""); // clean before sending to GPT-3
            allSnippets = allSnippets.Replace("\n", ""); // dtto
            return allSnippets;
        }

        public int GetTopReviewsScore(List<Review> reviews)
        {
            int average = 0;
            int count = 0;

            foreach (var review in reviews)
            {
                if (review.Score != null && review.Score > 0)
                {
                    average += (int)review.Score;
                    count++;
                }
            }
            if (count > 0)
                return average / count;
            else return 0;
        }

        private async Task<string> SearchGameCallAPI(string enteredName)
        {
            enteredName = enteredName.Replace(" ", "%20");
            string responseJson;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://opencritic-api.p.rapidapi.com/game/search?criteria=" + enteredName),
                Headers =
                    {
                        { "X-RapidAPI-Key", "api-key" },
                        { "X-RapidAPI-Host", "opencritic-api.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                responseJson = await response.Content.ReadAsStringAsync();
            }
            return responseJson;
        }

        private async Task<string> GetGameCallAPI(string gameId)
        {
            string responseJson;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://opencritic-api.p.rapidapi.com/game/" + gameId),
                Headers =
                    {
                        { "X-RapidAPI-Key", "api-key" },
                        { "X-RapidAPI-Host", "opencritic-api.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                responseJson = await response.Content.ReadAsStringAsync();
            }
            return responseJson;
        }

        private async Task<string> GetReviewsCallAPI(string gameId)
        {
            string responseJson;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://opencritic-api.p.rapidapi.com/review/game/" + gameId + "?sort=popularity"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "api-key" },
                        { "X-RapidAPI-Host", "opencritic-api.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                responseJson = await response.Content.ReadAsStringAsync();
            }
            return responseJson;
        }

        public async Task<List<string>> GetSummary(List<Review> reviews)
        {
            try
            {
                return await GetCompletion(GPT3Model.Davinci, "Shortly summarise the following video game review highlights: " + ExtractSnippets(reviews), 256, 0, 0, 0);
            }
            catch
            {
                return null; // in case service is not available return null
            }
        }

        public async Task<List<string>> GetProsAndCons(List<Review> reviews)
        {
            try
            {
                return await GetCompletion(GPT3Model.Davinci, "Extract three pros and cons from the following video game review highlights: " + ExtractSnippets(reviews), 256, 0, 0, 0);
            }
            catch
            {
                return null; // in case service is not available return null
            }
        }

        private async Task<List<string>> GetCompletion(GPT3Model gPT3Model, string prompt, int maxTokens, double temperature, int frequencyPenalty, int presencePenalty)
        {
            string model = "";
            if (gPT3Model == GPT3Model.Davinci)
                model = "text-davinci-003";
            if (gPT3Model == GPT3Model.Curie)
                model = "text-curie-001";
            if (gPT3Model == GPT3Model.Babbage)
                model = "text-babbage-001";
            if (gPT3Model == GPT3Model.Ada)
                model = "text-ada-001";
            string stopSequence = "|||";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/completions"),
                Headers =
                    {
                        { "Authorization", "Bearer " + "api-key" },
                    },
                Content = new StringContent("{" +
                "\n \"model\": \"" + model + "\"," +
                "\n \"prompt\": \"" + prompt + "\"," +
                "\n \"max_tokens\": " + maxTokens + "," +
                "\n \"temperature\": " + temperature.ToString(CultureInfo.InvariantCulture) + "," +
                "\n \"stop\": \"" + stopSequence + "\"," +
                "\n \"presence_penalty\": " + presencePenalty + "," +
                "\n \"frequency_penalty\": " + frequencyPenalty + "\n}", Encoding.UTF8, "application/json")
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return FormatCompletion(ParseCompletion(await response.Content.ReadAsStringAsync()));
            }
        }

        private string ParseCompletion(string response)
        {
            JObject completionJObject = JObject.Parse(response);
            return (string)completionJObject["choices"][0]["text"];
        }

        private List<string> FormatCompletion(string completion)
        {
            string[] separatedCompletion = completion.Split("\n");
            return separatedCompletion.ToList();
        }
    }
}
