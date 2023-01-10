﻿using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace ReviewSum.Data
{
    // https://rapidapi.com/opencritic-opencritic-default/api/opencritic-api
    // https://app.swaggerhub.com/apis-docs/OpenCritic/OpenCritic-API/1.0.0
    // https://beta.openai.com/docs/api-reference/completions
    // example of Open AI completion response:
    // '{"id":"cmpl-6KVvRC75ba10XYAQlI2GHN91lTQ1O","object":"text_completion","created":1670347301,"model":"text-curie-001","choices":[{"text":"\\n\\nMy name is Sarah.","index":0,"logprobs":null,"finish_reason":"stop"}],"usage":{"prompt_tokens":5,"completion_tokens":7,"total_tokens":12}}

    public enum GPT3Model
    {
        Davinci,
        Curie,
        Babbage,
        Ada
    }

    public class ReviewSumService
    {
        private static HttpClient client = new HttpClient();

        public async Task<List<Game>> SearchGame(string enteredName)
        {
            List<Game> foundGames = new List<Game>();

            if (!string.IsNullOrEmpty(enteredName))
            {
                try
                {
                    //await TestError(false);
                    //for testing:
                    //string responseString = "[{\"id\":7816,\"name\":\"Death Stranding\",\"dist\":0},{\"id\":12011,\"name\":\"Death Stranding Director's Cut\",\"dist\":0.448276},{\"id\":5155,\"name\":\"Death Coming\",\"dist\":0.619048},{\"id\":13344,\"name\":\"Witch Strandings\",\"dist\":0.625},{\"id\":9369,\"name\":\"Stranded Deep\",\"dist\":0.636364},{\"id\":5990,\"name\":\"Flipping Death\",\"dist\":0.652174},{\"id\":11687,\"name\":\"Death's Door\",\"dist\":0.666667},{\"id\":7097,\"name\":\"In Death\",\"dist\":0.684211},{\"id\":3935,\"name\":\"Death Squared\",\"dist\":0.695652},{\"id\":1263,\"name\":\"Deathtrap\",\"dist\":0.7}]";
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
                    //await TestError(false);
                    //for testing:
                    //string responseString = "{\"images\":{\"box\":{\"og\":\"game/7816/o/9Vz54SUN.jpg\",\"sm\":\"game/7816/hmh3jmVw.jpg\"},\"square\":{\"og\":\"game/7816/o/HOQx3TiO.jpg\",\"xs\":\"game/7816/i5QCldxw.jpg\",\"sm\":\"game/7816/Qp5nMXgj.jpg\",\"lg\":\"game/7816/agKNuB8j.jpg\"},\"masthead\":{\"og\":\"game/7816/o/NEcDsaYh.jpg\",\"xs\":\"game/7816/3MSzhH2g.jpg\",\"sm\":\"game/7816/5yxUJDws.jpg\",\"md\":\"game/7816/P7QzvK4z.jpg\",\"lg\":\"game/7816/hYPmiIPz.jpg\",\"xl\":\"game/7816/sqlRJsxM.jpg\"},\"banner\":{\"og\":\"game/7816/o/gdydeCt8.jpg\",\"sm\":\"game/7816/Bg5YUYMr.jpg\"},\"screenshots\":[{\"_id\":\"62f47065c25b595e6c50d67d\",\"og\":\"game/7816/o/K9Mlk1RQ.jpg\",\"sm\":\"game/7816/TIu0w4nE.jpg\"},{\"_id\":\"62f47065c25b595e6c50d67e\",\"og\":\"game/7816/o/NpfGFPb3.jpg\",\"sm\":\"game/7816/38pKhSGU.jpg\"},{\"_id\":\"62f47065c25b595e6c50d67f\",\"og\":\"game/7816/o/kL7pRsF0.jpg\",\"sm\":\"game/7816/3LacM3Zx.jpg\"}]},\"Rating\":{\"imageSrc\":\"\"},\"hasLootBoxes\":null,\"percentRecommended\":79.05759162303664,\"numReviews\":199,\"numTopCriticReviews\":117,\"medianScore\":88,\"topCriticScore\":83.17948717948718,\"percentile\":91,\"tier\":\"Strong\",\"name\":\"Death Stranding\",\"description\":\"From legendary game creator Hideo Kojima comes an all-new, genre-defying experience for the PlayStation®4 system.\\n\\nSam Bridges must brave a world utterly transformed by the Death Stranding. Carrying the disconnected remnants of our future in his hands, he embarks on a journey to reconnect the shattered world one step at a time. \\n\\nWith spectral creatures plaguing the landscape, and humanity on the verge of a mass extinction, it’s up to Sam to journey across the ravaged continent and save mankind from impending annihilation.  \\n\\nWhat is the mystery of the Death Stranding? What will Sam discover on the road ahead? An unprecedented gameplay experience holds these answers and more.\\n\\nStarring Norman Reedus, Mads Mikkelsen, Léa Seydoux, and Lindsay Wagner.\\n\\n\",\"screenshots\":[{\"_id\":\"5da8ccd83b0a6d62a15e69e0\",\"fullRes\":\"//c.opencritic.com/images/games/7816/FtT6byWbvQvxFnxaxR.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/FtT6byWbvQvxFnxaxR_th.jpg\"},{\"_id\":\"5da8ccd83b0a6d62a15e69df\",\"fullRes\":\"//c.opencritic.com/images/games/7816/CJx1HiYsswPdonkWk3.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/CJx1HiYsswPdonkWk3_th.jpg\"},{\"_id\":\"5da8ccd83b0a6d62a15e69de\",\"fullRes\":\"//c.opencritic.com/images/games/7816/1I90aHUv3k518e33q6.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/1I90aHUv3k518e33q6_th.jpg\"},{\"_id\":\"5da8ccd83b0a6d62a15e69dd\",\"fullRes\":\"//c.opencritic.com/images/games/7816/b88rEyalytt7NJjHAv.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/b88rEyalytt7NJjHAv_th.jpg\"}],\"trailers\":[{\"publishedDate\":\"2019-10-03T00:00:00.000Z\",\"title\":\"Death Stranding - The Drop Promotional Trailer | PS4\",\"videoId\":\"YyH4KQGehtg\",\"externalUrl\":\"https://www.youtube.com/watch?v=YyH4KQGehtg\",\"channelTitle\":\"PlayStation\",\"channelId\":\"UC-2Y8dQb0S6DtpxNgAKoJKA\",\"description\":\"https://www.playstation.com/games/death-stranding/?emcid=or-1s-412983\\n\\nReconnect a fractured society as Sam Bridges. Save us all. \\n\\nPre-Order Death Stranding for PS4. Out Nov 8th, 2019.\\n\\nhttps://www.playstation.com/en-us/games/death-stranding-ps4/#buynow\"},{\"publishedDate\":\"2019-09-12T00:00:00.000Z\",\"title\":\"Death Stranding – Briefing Trailer | PS4\",\"videoId\":\"T7wi7LX9F40\",\"externalUrl\":\"https://www.youtube.com/watch?v=T7wi7LX9F40\",\"channelTitle\":\"PlayStation\",\"channelId\":\"UC-2Y8dQb0S6DtpxNgAKoJKA\",\"description\":\"https://www.playstation.com/games/death-stranding/?emcid=or-1s-412983\\n\\nPre-Order Now. Available for PlayStation®4 on November 8, 2019.\\n\\nSam must help rebuild the country by reconnecting the people. \\n\\nFrom legendary game creator Hideo Kojima comes an all-new, genre-defying experience for the PlayStation®4. Sam Bridges must brave a world utterly transformed by the DEATH STRANDING. Carrying the disconnected remnants of our future in his hands, he embarks on a journey to reconnect the shattered world one step at a time. \\n\\nStarring Norman Reedus, Mads Mikkelsen, Léa Seydoux, and Lindsay Wagner.\\n\\nLearn more: http://playstation.com/deathstranding\\n\\nRated Mature:  Blood, Intense Violence, Partial Nudity, Strong Language\"},{\"publishedDate\":\"2019-05-29T00:00:00.000Z\",\"title\":\"Death Stranding – Release Date Reveal Trailer | PS4\",\"videoId\":\"piIgkJWDuQg\",\"externalUrl\":\"https://www.youtube.com/watch?v=piIgkJWDuQg\",\"channelTitle\":\"PlayStation\",\"channelId\":\"UC-2Y8dQb0S6DtpxNgAKoJKA\",\"description\":\"https://www.playstation.com/games/death-stranding/?emcid=or-1s-412983\\n\\nDEATH STRANDING will be available November 8, 2019. Learn more: https://www.playstation.com/en-us/games/death-stranding-ps4/\\n\\nMay contain content inappropriate for children. Visit http://www.esrb.org for rating information.\"}],\"mastheadScreenshot\":{\"fullRes\":\"//c.opencritic.com/images/games/7816/aaP4V726On3k2mu1xV.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/aaP4V726On3k2mu1xV_th.jpg\"},\"embargoDate\":\"2019-11-01T07:03:09.000Z\",\"Companies\":[{\"name\":\"Kojima Productions\",\"type\":\"DEVELOPER\"}],\"Platforms\":[{\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\",\"imageSrc\":\"https://c.opencritic.com/images/platforms/ps4.png\",\"releaseDate\":\"2019-11-08T00:00:00.000Z\"},{\"id\":27,\"name\":\"PC\",\"shortName\":\"PC\",\"imageSrc\":\"https://c.opencritic.com/images/platforms/pc.png\",\"releaseDate\":\"2020-06-27T00:00:00.000Z\",\"displayRelease\":\"Early Summer 2020\"},{\"id\":3,\"name\":\"PlayStation 5\",\"shortName\":\"PS5\",\"imageSrc\":\"https://c.opencritic.com/images/platforms/ps5.png\",\"releaseDate\":\"2020-11-12T00:00:00.000Z\"}],\"Genres\":[{\"id\":27,\"name\":\"Action\"},{\"id\":76,\"name\":\"Adventure\"}],\"id\":7816,\"firstReleaseDate\":\"2019-11-08T00:00:00.000Z\",\"createdAt\":\"2019-09-21T19:54:37.988Z\",\"updatedAt\":\"2022-08-24T23:15:25.598Z\",\"firstReviewDate\":\"2019-10-31T00:00:00.000Z\",\"latestReviewDate\":\"2020-09-17T00:00:00.000Z\",\"bannerScreenshot\":{\"fullRes\":\"//c.opencritic.com/images/games/7816/banner.jpg\"},\"squareScreenshot\":{\"fullRes\":\"//c.opencritic.com/images/games/7816/J8vFvLdWJ0J3kHLgbBvudW9C8x7vFqKUMbzW1xgevjSvZuLt.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/J8vFvLdWJ0J3kHLgbBvudW9C8x7vFqKUMbzW1xgevjSvZuLt_th.jpg\"},\"verticalLogoScreenshot\":{\"fullRes\":\"//c.opencritic.com/images/games/7816/mHo5edxfuNVbVjbrfOXEZxxjdwcNhfOB0x1qU23dBS5S37RQ.jpg\",\"thumbnail\":\"//c.opencritic.com/images/games/7816/mHo5edxfuNVbVjbrfOXEZxxjdwcNhfOB0x1qU23dBS5S37RQ_th.jpg\"},\"imageMigrationComplete\":true,\"tenthReviewDate\":\"2019-11-01T00:00:00.000Z\",\"criticalReviewDate\":\"2019-11-01T00:00:00.000Z\",\"url\":\"https://opencritic.com/game/7816/death-stranding\"}";
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
                    //await TestError(false);
                    //for testing:
                    //string responseString = "[{\"_id\":\"5dbbd87bc55ae12d47d2b589\",\"overrideRecommendation\":false,\"magic\":5600,\"isChosen\":true,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.eurogamer.net/articles/2019-11-01-death-stranding-review-a-baffling-haunting-grand-folly\",\"title\":\"Death Stranding review: a baffling, haunting, grand folly\",\"score\":80,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866ded93765d6778737265\",\"image\":false,\"name\":\"Oli Welsh\",\"id\":751}],\"snippet\":\"Hideo Kojma's first post-Metal Gear game is a messy, indulgent vanity project - but also a true original.\",\"ScoreFormat\":{\"id\":31,\"name\":\"Essential, Recommended, No Rec, Avoid\",\"shortName\":\"Essential <--> Avoid\",\"scoreDisplay\":null,\"isNumeric\":false,\"isSelect\":true,\"isStars\":false,\"numDecimals\":null,\"base\":null,\"options\":[{\"_id\":\"5dbbd87bc55ae12d47d2b58e\",\"val\":100,\"label\":\"Essential\"},{\"_id\":\"5dbbd87bc55ae12d47d2b58d\",\"val\":80,\"label\":\"Recommended\"},{\"_id\":\"5dbbd87bc55ae12d47d2b58c\",\"val\":75,\"label\":\"No Recommendation / Blank\"},{\"_id\":\"5dbbd87bc55ae12d47d2b58b\",\"val\":0,\"label\":\"Avoid\"}]},\"Platforms\":[{\"_id\":\"5dbbd87bc55ae12d47d2b58f\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Oli Welsh\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Eurogamer\",\"isContributor\":true,\"id\":114,\"imageSrc\":{\"og\":\"outlet/114/o/ZotSh1ir.jpg\",\"sm\":\"outlet/114/U5o0V4A8.jpg\",\"lg\":\"outlet/114/mICJYF7b.jpg\"}},\"createdAt\":\"2019-11-01T07:02:19.524Z\",\"updatedAt\":\"2022-08-11T16:47:25.628Z\",\"__v\":0},{\"_id\":\"5dbbd924c55ae12d47d2b5df\",\"overrideRecommendation\":false,\"magic\":5600,\"isChosen\":true,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"http://www.ign.com/articles/2019/11/01/death-stranding-review\",\"title\":\"Games\",\"score\":68,\"npScore\":0,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866e6393765d6778739783\",\"image\":false,\"name\":\"Tristan Ogilvie\",\"id\":236}],\"snippet\":\"Death Stranding delivers a fascinating world of supernatural sci-fi, but its gameplay struggles to support its weight.\",\"ScoreFormat\":{\"id\":18,\"name\":\"0 to 10 incl decimals\",\"shortName\":\"x.x / 10.0\",\"scoreDisplay\":\" / 10.0\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":1,\"base\":10,\"options\":null},\"Platforms\":[{\"_id\":\"5dbbd924c55ae12d47d2b5e1\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Tristan Ogilvie\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"IGN\",\"isContributor\":false,\"id\":56,\"imageSrc\":{\"og\":\"outlet/56/o/ljTZSOGp.jpg\",\"sm\":\"outlet/56/1v75XFcW.jpg\",\"lg\":\"outlet/56/AmLzvUhj.jpg\"}},\"createdAt\":\"2019-11-01T07:05:08.541Z\",\"updatedAt\":\"2022-08-11T16:47:21.953Z\",\"__v\":0},{\"_id\":\"5f0c73d9d88e035744c1a4f1\",\"magic\":5600,\"isChosen\":false,\"overrideRecommendation\":false,\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"PC Gamer\",\"isContributor\":false,\"id\":162,\"imageSrc\":{\"og\":\"outlet/162/o/FpKaL1Hj.jpg\",\"sm\":\"outlet/162/Tzt3P7so.jpg\",\"lg\":\"outlet/162/LQgtWlmu.jpg\"}},\"ScoreFormat\":{\"id\":17,\"name\":\"0 to 100, whole numbers\",\"shortName\":\"x / 100\",\"scoreDisplay\":\" / 100\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":0,\"base\":1,\"options\":null},\"medianAtTimeOfReview\":78,\"externalUrl\":\"https://www.pcgamer.com/death-stranding-review/\",\"language\":\"en-us\",\"Platforms\":[{\"_id\":\"5f0c77c62282ae3f4424ae9f\",\"id\":27,\"name\":\"PC\",\"shortName\":\"PC\"}],\"Authors\":[{\"_id\":\"5f0c77c72282ae3f4424aea0\",\"id\":137,\"name\":\"Andy Kelly\",\"image\":true,\"imageSrc\":{\"og\":\"critic/137/o/X5OjsfVb.jpg\",\"sm\":\"critic/137/TsEfv2W5.jpg\",\"lg\":\"critic/137/kdV3EtcV.jpg\"}}],\"title\":\"Death Stranding review\",\"publishedDate\":\"2020-07-13T00:00:00.000Z\",\"snippet\":\"Slow, weird, and indulgent, but a true original, and a journey that will linger in your mind long after it's over.\",\"score\":85,\"alias\":\"Andy Kelly\",\"npScore\":100,\"createdAt\":\"2020-07-13T14:46:49.506Z\",\"updatedAt\":\"2022-08-11T16:47:27.325Z\",\"__v\":0},{\"_id\":\"5dbbd85bc55ae12d47d2b57e\",\"overrideRecommendation\":false,\"magic\":5550,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.youtube.com/watch?v=3zUxXf1k5iM\",\"title\":\"Death Stranding - Easy Allies Review\",\"score\":80,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866ddf93765d6778736f83\",\"image\":false,\"name\":\"Ben Moore\",\"id\":1436}],\"snippet\":\"Death Stranding is a fearless game that often stumbles, but is still fascinating overall.\",\"ScoreFormat\":{\"id\":18,\"name\":\"0 to 10 incl decimals\",\"shortName\":\"x.x / 10.0\",\"scoreDisplay\":\" / 10.0\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":1,\"base\":10,\"options\":null},\"Platforms\":[],\"alias\":\"Ben Moore\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Easy Allies\",\"isContributor\":false,\"id\":394,\"imageSrc\":{\"og\":\"outlet/394/o/ihvAPPrT.jpg\",\"sm\":\"outlet/394/NlEUCO3t.jpg\",\"lg\":\"outlet/394/jJE834KC.jpg\"}},\"createdAt\":\"2019-11-01T07:01:47.121Z\",\"updatedAt\":\"2022-08-11T16:47:37.038Z\",\"__v\":0,\"isYoutube\":true,\"lastRefreshDate\":\"2021-11-09T21:03:49.544Z\",\"youtubeVideoId\":\"3zUxXf1k5iM\"},{\"_id\":\"5dbbda35c55ae12d47d2b5f5\",\"overrideRecommendation\":false,\"magic\":5550,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://metro.co.uk/2019/11/01/death-stranding-review-delivering-the-goods-11022052/\",\"score\":70,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866d8d93765d67787364c3\",\"image\":false,\"name\":\"David Jenkins\",\"id\":368}],\"snippet\":\"A work of unbridled ambition and imagination but also a pretentious, contrived, and frequently quite dull gameplay experience – Death Stranding is peak Hideo Kojima.\",\"ScoreFormat\":{\"id\":25,\"name\":\"0 to 10, whole numbers\",\"shortName\":\"x / 10\",\"scoreDisplay\":\" / 10\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"base\":10,\"options\":null,\"numDecimals\":0},\"Platforms\":[{\"_id\":\"5dbbda35c55ae12d47d2b5f7\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Metro GameCentral\",\"isContributor\":false,\"id\":75,\"imageSrc\":{\"og\":\"outlet/75/o/eSOrjn86.jpg\",\"sm\":\"outlet/75/lP7sobQ6.jpg\",\"lg\":\"outlet/75/7ZpgylR7.jpg\"}},\"createdAt\":\"2019-11-01T07:09:41.509Z\",\"updatedAt\":\"2022-08-11T16:47:23.382Z\",\"__v\":0},{\"_id\":\"5dbbe851c55ae12d47d2b64f\",\"overrideRecommendation\":false,\"magic\":5550,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.gamesradar.com/death-stranding-review/\",\"title\":\"Death Stranding review: “An okay game ironically lost in its lengthy delivery” | GamesRadar+\",\"score\":70,\"npScore\":70,\"language\":\"en-us\",\"Authors\":[],\"snippet\":\"Kojima's mysterious would be epic has its moments but can't carry the weight of expectation.\",\"ScoreFormat\":{\"id\":19,\"name\":\"0 to 5 stars, incl half stars\",\"scoreDisplay\":\" / 5 stars\",\"shortName\":\"x.5 / 5 stars\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":true,\"numDecimals\":1,\"base\":20,\"options\":null},\"Platforms\":[],\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"GamesRadar+\",\"isContributor\":false,\"id\":91,\"imageSrc\":{\"og\":\"outlet/91/o/Ox8p9mPd.jpg\",\"sm\":\"outlet/91/qqIbhWtu.jpg\",\"lg\":\"outlet/91/ALrwgt76.jpg\"}},\"createdAt\":\"2019-11-01T08:09:53.898Z\",\"updatedAt\":\"2022-08-11T16:47:24.048Z\",\"__v\":0},{\"_id\":\"5f0c92aed88e035744c1a594\",\"magic\":5550,\"isChosen\":false,\"overrideRecommendation\":false,\"Outlet\":{\"name\":\"Metro GameCentral\",\"isContributor\":false,\"id\":75,\"imageSrc\":{\"og\":\"outlet/75/o/eSOrjn86.jpg\",\"sm\":\"outlet/75/lP7sobQ6.jpg\",\"lg\":\"outlet/75/7ZpgylR7.jpg\"}},\"ScoreFormat\":{\"id\":25,\"name\":\"0 to 10, whole numbers\",\"shortName\":\"x / 10\",\"scoreDisplay\":\" / 10\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"base\":10,\"options\":null,\"numDecimals\":0},\"externalUrl\":\"https://metro.co.uk/2020/07/13/death-stranding-pc-review-strange-delivery-12983507/\",\"language\":\"en-us\",\"Platforms\":[{\"_id\":\"5f0c71932282ae3f4424ae8b\",\"id\":27,\"name\":\"PC\",\"shortName\":\"PC\"}],\"Authors\":[],\"publishedDate\":\"2020-07-13T00:00:00.000Z\",\"score\":70,\"snippet\":\"The coronavirus has made its themes more relevant than ever and while there are many odd decisions, in terms of plot and game design, the overall experience remains strangely engrossing.\",\"game\":{\"id\":7816,\"name\":\"Death Stranding\"},\"isQuoteManual\":true,\"medianAtTimeOfReview\":70,\"npScore\":100,\"createdAt\":\"2020-07-13T16:58:22.400Z\",\"updatedAt\":\"2022-08-11T16:47:23.382Z\",\"__v\":0},{\"_id\":\"5dbbd8c2c55ae12d47d2b5a1\",\"overrideRecommendation\":false,\"magic\":5520,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.gameinformer.com/review/death-stranding/death-stranding-review-the-wanderer\",\"title\":\"Death Stranding Review – The Wanderer\",\"score\":70,\"npScore\":70,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866df393765d677873738f\",\"image\":false,\"name\":\"Matthew Kato\",\"id\":777}],\"snippet\":\"The pillars of gameplay, combat, and story all bear the mark of creator Hideo Kojima, but none of them stand out or carry the experience\",\"ScoreFormat\":{\"id\":18,\"name\":\"0 to 10 incl decimals\",\"shortName\":\"x.x / 10.0\",\"scoreDisplay\":\" / 10.0\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":1,\"base\":10,\"options\":null},\"Platforms\":[{\"_id\":\"5dbbd8c2c55ae12d47d2b5a3\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Matthew Kato\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Game Informer\",\"isContributor\":false,\"id\":35,\"imageSrc\":{\"og\":\"outlet/35/o/9FCDG7wC.jpg\",\"sm\":\"outlet/35/eaPNPgDC.jpg\",\"lg\":\"outlet/35/c3KiPiCS.jpg\"}},\"createdAt\":\"2019-11-01T07:03:30.678Z\",\"updatedAt\":\"2022-08-11T16:47:21.269Z\",\"__v\":0},{\"_id\":\"5dbbd87ac55ae12d47d2b587\",\"overrideRecommendation\":false,\"magic\":5510,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.polygon.com/reviews/2019/11/1/20942070/death-stranding-review-hideo-kojima-ps4\",\"title\":\"Death Stranding review: Hideo Kojima tries to make fetch happen\",\"npScore\":null,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866ece93765d677873c990\",\"image\":false,\"name\":\"Russ Frushtick\",\"id\":1503}],\"snippet\":\"Having been smitten by the core world-building gameplay of Death Stranding, I am stunned to realize that many of the game’s strongest, most appealing gameplay ideas (specifically the world-building and cooperation) are tossed aside in the final acts, in favor of a much more linear, scripted, cutscene-ridden experience. The freedom and sense of ownership I enjoyed while creating this world are dashed in favor of explaining and wrapping up a story that never had much going for it to begin with.\",\"ScoreFormat\":{\"id\":30,\"name\":\"No Verdict\",\"shortName\":\"No Verdict\",\"scoreDisplay\":\" / 100\",\"isNumeric\":false,\"isSelect\":false,\"isStars\":false,\"numDecimals\":null,\"base\":null,\"options\":null},\"Platforms\":[],\"alias\":\"Russ Frushtick\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Polygon\",\"isContributor\":false,\"id\":87,\"imageSrc\":{\"og\":\"outlet/87/o/XP3i8Uoz.jpg\",\"sm\":\"outlet/87/tG3biCpx.jpg\",\"lg\":\"outlet/87/m7AWXHMy.jpg\"}},\"createdAt\":\"2019-11-01T07:02:18.927Z\",\"updatedAt\":\"2022-08-11T16:47:23.820Z\",\"__v\":0},{\"_id\":\"5dbbd9d7c55ae12d47d2b5e9\",\"overrideRecommendation\":false,\"magic\":5510,\"isChosen\":true,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.gamespot.com/reviews/death-stranding-review-strand-and-deliver/1900-6417358/\",\"title\":\"Death Stranding Review - Strand And Deliver\",\"score\":90,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866fc293765d67787426fc\",\"image\":false,\"name\":\"Kallie Plagge\",\"id\":200}],\"snippet\":\"Death Stranding is dense, complex, and powerful, steadfast in its belief in the power of love and hope when faced with overwhelming adversity.\",\"ScoreFormat\":{\"id\":25,\"name\":\"0 to 10, whole numbers\",\"shortName\":\"x / 10\",\"scoreDisplay\":\" / 10\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"base\":10,\"options\":null,\"numDecimals\":0},\"Platforms\":[{\"_id\":\"5dbbd9d7c55ae12d47d2b5eb\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Kallie Plagge\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"GameSpot\",\"isContributor\":false,\"id\":32,\"imageSrc\":{\"og\":\"outlet/32/o/HVjoXjEt.jpg\",\"sm\":\"outlet/32/yXf7DyuJ.jpg\",\"lg\":\"outlet/32/puzMbnOY.jpg\"}},\"createdAt\":\"2019-11-01T07:08:07.155Z\",\"updatedAt\":\"2022-08-11T16:47:20.966Z\",\"__v\":0},{\"_id\":\"5dbbd912c55ae12d47d2b5d3\",\"overrideRecommendation\":false,\"magic\":5460,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://kotaku.com/death-stranding-the-kotaku-review-1839474313\",\"title\":\"Death Stranding: The Kotaku Review\",\"npScore\":null,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d8672bf93765d677875450d\",\"image\":false,\"name\":\"Heather Alexandra\",\"id\":1942}],\"snippet\":\"It’s hard not to like Sam Bridges, who faces all of Death Stranding’s bizarreness with a welcome everyman’s weariness, encapsulated in in Norman Reedus’ characteristic growl.\",\"ScoreFormat\":{\"id\":30,\"name\":\"No Verdict\",\"shortName\":\"No Verdict\",\"scoreDisplay\":\" / 100\",\"isNumeric\":false,\"isSelect\":false,\"isStars\":false,\"numDecimals\":null,\"base\":null,\"options\":null},\"Platforms\":[],\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Kotaku\",\"isContributor\":false,\"id\":276,\"imageSrc\":{\"og\":\"outlet/276/o/noSyqjap.jpg\",\"sm\":\"outlet/276/XlUpExhx.jpg\",\"lg\":\"outlet/276/dygouIe5.jpg\"}},\"createdAt\":\"2019-11-01T07:04:50.960Z\",\"updatedAt\":\"2022-08-11T16:47:27.972Z\",\"__v\":0},{\"_id\":\"5dc3be983496c41f3bb5bcea\",\"overrideRecommendation\":false,\"magic\":5460,\"isChosen\":false,\"publishedDate\":\"2019-11-06T00:00:00.000Z\",\"externalUrl\":\"https://www.giantbomb.com/reviews/death-stranding-review/1900-795/\",\"title\":\"Death Stranding Review\",\"score\":40,\"npScore\":0,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866e4093765d6778738b53\",\"image\":false,\"name\":\"Alex Navarro\",\"id\":664}],\"snippet\":\"Kojima's first post-Konami project is a bizarre, self-indulgent mess that never quite manages to tie its myriad pieces together.\",\"ScoreFormat\":{\"id\":22,\"name\":\"0 to 5 stars, whole stars\",\"shortName\":\"x / 5 stars\",\"scoreDisplay\":\" / 5 stars\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":true,\"numDecimals\":0,\"base\":20,\"options\":null},\"Platforms\":[{\"_id\":\"5dc3be983496c41f3bb5bced\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"},{\"_id\":\"5dc3be983496c41f3bb5bcec\",\"id\":27,\"name\":\"PC\",\"shortName\":\"PC\"}],\"alias\":\"Alex Navarro\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Giant Bomb\",\"isContributor\":false,\"id\":132,\"imageSrc\":{\"og\":\"outlet/132/o/SLUpPE8f.jpg\",\"sm\":\"outlet/132/puEqkhOk.jpg\",\"lg\":\"outlet/132/OhwaBtOJ.jpg\"}},\"createdAt\":\"2019-11-07T06:50:00.038Z\",\"updatedAt\":\"2022-08-11T16:47:26.344Z\",\"__v\":0},{\"_id\":\"5dbbd87ec55ae12d47d2b593\",\"overrideRecommendation\":false,\"magic\":5450,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.usgamer.net/articles/death-stranding-review\",\"title\":\"Death Stranding Review: The Boldest, And Most Perplexing, Game of 2019\",\"score\":70,\"npScore\":70,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866e1093765d6778737bce\",\"image\":false,\"name\":\"Kat Bailey\",\"id\":303}],\"snippet\":\"Death Stranding might be Kojima's boldest game to date. It may also be his most tedious. Either way, its originality outweighs its sometimes exhausting structure and poor pacing... but only just. Maybe not a game I would recommend to everyone, but certainly one of the most interesting games of 2019.\",\"ScoreFormat\":{\"id\":19,\"name\":\"0 to 5 stars, incl half stars\",\"scoreDisplay\":\" / 5 stars\",\"shortName\":\"x.5 / 5 stars\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":true,\"numDecimals\":1,\"base\":20,\"options\":null},\"Platforms\":[{\"_id\":\"5dbbd87ec55ae12d47d2b595\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Kat Bailey\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"USgamer\",\"isContributor\":false,\"id\":66,\"imageSrc\":{\"og\":\"outlet/66/o/SK3SRPmD.jpg\",\"sm\":\"outlet/66/YaA3s59d.jpg\",\"lg\":\"outlet/66/q1lI0NCc.jpg\"}},\"createdAt\":\"2019-11-01T07:02:22.269Z\",\"updatedAt\":\"2022-08-11T16:47:22.632Z\",\"__v\":0},{\"_id\":\"5dbbd863c55ae12d47d2b580\",\"overrideRecommendation\":false,\"magic\":5410,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.destructoid.com/review-death-stranding-568760.phtml\",\"title\":\"Review: Death Stranding\",\"score\":80,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866d9393765d677873657e\",\"image\":true,\"name\":\"Chris Carter\",\"id\":431,\"imageSrc\":{\"og\":\"critic/431/o/ywRVe71y.jpg\",\"sm\":\"critic/431/FtoAZ0GG.jpg\",\"lg\":\"critic/431/iZz8RTzp.jpg\"}}],\"snippet\":\"Death Stranding is not the overly-strange inaccessible walled garden the marketing has made it out to be. It's weird, don't get me wrong! But anyone with a surface-level understanding of surrealism in art should be able to acclimate to what is essentially a playable Hollywood production.\",\"ScoreFormat\":{\"id\":18,\"name\":\"0 to 10 incl decimals\",\"shortName\":\"x.x / 10.0\",\"scoreDisplay\":\" / 10.0\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":1,\"base\":10,\"options\":null},\"Platforms\":[{\"_id\":\"5dbbd863c55ae12d47d2b582\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"Chris Carter\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Destructoid\",\"isContributor\":false,\"id\":90,\"imageSrc\":{\"og\":\"outlet/90/o/ntiElgkw.jpg\",\"sm\":\"outlet/90/zUGVXx8a.jpg\",\"lg\":\"outlet/90/OqI9z1j3.jpg\"}},\"createdAt\":\"2019-11-01T07:01:55.463Z\",\"updatedAt\":\"2022-08-11T16:47:24.016Z\",\"__v\":0},{\"_id\":\"5dbbdb08c55ae12d47d2b613\",\"overrideRecommendation\":false,\"magic\":5410,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.gamerevolution.com/review/611833-death-stranding-review\",\"title\":\"Death Stranding Review | A weapon to surpass Metal Gear\",\"score\":100,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866fda93765d6778742d94\",\"image\":false,\"name\":\"Jason Faulkner\",\"id\":448}],\"snippet\":\"Death Stranding is one of the best games I’ve ever played. It’s smart, it’s well-produced, and it just feels good to play.\",\"ScoreFormat\":{\"id\":19,\"name\":\"0 to 5 stars, incl half stars\",\"scoreDisplay\":\" / 5 stars\",\"shortName\":\"x.5 / 5 stars\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":true,\"numDecimals\":1,\"base\":20,\"options\":null},\"Platforms\":[],\"alias\":\"Jason Faulkner\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Game Revolution\",\"isContributor\":false,\"id\":93,\"imageSrc\":{\"og\":\"outlet/93/o/HPqpNuyz.jpg\",\"sm\":\"outlet/93/bcVKvom2.jpg\",\"lg\":\"outlet/93/cj2IXWVT.jpg\"}},\"createdAt\":\"2019-11-01T07:13:12.596Z\",\"updatedAt\":\"2022-08-11T16:47:24.123Z\",\"__v\":0},{\"_id\":\"5dbbe730c55ae12d47d2b64c\",\"overrideRecommendation\":false,\"magic\":5400,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.hobbyconsolas.com/reviews/analisis-death-stranding-juego-hideo-kojima-ps4-pc-517957\",\"title\":\"Análisis de Death Stranding, el juego de Hideo Kojima para PS4 y PC   - HobbyConsolas Juegos\",\"score\":93,\"npScore\":100,\"language\":\"es-es\",\"Authors\":[{\"_id\":\"5d866ee593765d677873d5c5\",\"image\":false,\"name\":\"David Martinez\",\"id\":2884}],\"snippet\":\"Death Stranding is Kojima´s most personal game. A complex and emotional work of art, which is not adequate for everyone because of the slow pace and the unusual gameplay. But once you get it, it´s something you will never forget.\",\"ScoreFormat\":{\"id\":17,\"name\":\"0 to 100, whole numbers\",\"shortName\":\"x / 100\",\"scoreDisplay\":\" / 100\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":0,\"base\":1,\"options\":null},\"Platforms\":[{\"_id\":\"5dbbe730c55ae12d47d2b64e\",\"id\":6,\"name\":\"PlayStation 4\",\"shortName\":\"PS4\"}],\"alias\":\"David Martínez\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Hobby Consolas\",\"isContributor\":false,\"id\":499,\"imageSrc\":{\"og\":\"outlet/499/o/LhEJJe3Z.jpg\",\"sm\":\"outlet/499/OBc8ktCZ.jpg\",\"lg\":\"outlet/499/enArDAQC.jpg\"}},\"createdAt\":\"2019-11-01T08:05:04.510Z\",\"updatedAt\":\"2022-08-11T16:47:47.602Z\",\"__v\":0},{\"_id\":\"5dbbec45c55ae12d47d2b66d\",\"overrideRecommendation\":false,\"magic\":5400,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://areajugones.sport.es/videojuegos/analisis-death-stranding/\",\"title\":\"Análisis Death Stranding para PlayStation 4\",\"score\":97,\"npScore\":100,\"language\":\"es-es\",\"Authors\":[{\"_id\":\"5d8671e993765d677874e057\",\"image\":true,\"name\":\"Juan Linares\",\"id\":3449,\"imageSrc\":{\"og\":\"critic/3449/o/CJSNFX6g.jpg\",\"sm\":\"critic/3449/BwWaZHlD.jpg\",\"lg\":\"critic/3449/QCOklbZs.jpg\"}}],\"snippet\":\"Death Stranding is one the best games to be found in PS4. Kojima-san delivers an open world in which we have to represent a bridge between the narrative of the game and other players in order to keep moving forward as part of a world that needs us. The game introduces characters that keep on evolving until we reach an ending for the ages through a story told like very few would be able to. Death Stranding knows how to toy with our emotions, and it manages to arise anguish, tension, solitude, sorrow, joy and provides a conclusion that completely stuns us. The end is only the beginning.\",\"ScoreFormat\":{\"id\":18,\"name\":\"0 to 10 incl decimals\",\"shortName\":\"x.x / 10.0\",\"scoreDisplay\":\" / 10.0\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"numDecimals\":1,\"base\":10,\"options\":null},\"Platforms\":[],\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"Areajugones\",\"isContributor\":false,\"id\":470,\"imageSrc\":{\"og\":\"outlet/470/o/YBcejN43.jpg\",\"sm\":\"outlet/470/ZL8qf9LX.jpg\",\"lg\":\"outlet/470/Rah7OHPS.jpg\"}},\"createdAt\":\"2019-11-01T08:26:45.572Z\",\"updatedAt\":\"2022-08-11T16:47:44.573Z\",\"__v\":0},{\"_id\":\"5dbbfe44c55ae12d47d2b67b\",\"overrideRecommendation\":false,\"magic\":5400,\"isChosen\":false,\"publishedDate\":\"2019-11-01T00:00:00.000Z\",\"externalUrl\":\"https://www.thesixthaxis.com/2019/11/01/death-stranding-review-ps4-pro/\",\"title\":\"Death Stranding Review – TheSixthAxis\",\"score\":100,\"npScore\":100,\"language\":\"en-us\",\"Authors\":[{\"_id\":\"5d866ddc93765d6778736eb3\",\"image\":false,\"name\":\"Tuffcub\",\"id\":314}],\"snippet\":\"Death Stranding is like nothing I have ever played; beautiful, heart racing, heart breaking, frustrating, epic, stunning, and utterly nuts. I laughed, I cried, I cursed, and I went to the toilet an awful lot. Death Stranding isn't just my Game of the Year, it's a contender for Game of the Generation too.\",\"ScoreFormat\":{\"id\":25,\"name\":\"0 to 10, whole numbers\",\"shortName\":\"x / 10\",\"scoreDisplay\":\" / 10\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"base\":10,\"options\":null,\"numDecimals\":0},\"Platforms\":[],\"alias\":\"Tuffcub\",\"game\":{\"name\":\"Death Stranding\",\"id\":7816},\"Outlet\":{\"name\":\"TheSixthAxis\",\"isContributor\":false,\"id\":68,\"imageSrc\":{\"og\":\"outlet/68/o/a60wYXaS.jpg\",\"sm\":\"outlet/68/NhIFRpLf.jpg\",\"lg\":\"outlet/68/EvgsxMq3.jpg\"}},\"createdAt\":\"2019-11-01T09:43:32.013Z\",\"updatedAt\":\"2022-08-11T16:47:23.001Z\",\"__v\":0},{\"_id\":\"5f0c8e4fd88e035744c1a531\",\"magic\":5400,\"isChosen\":false,\"overrideRecommendation\":false,\"Outlet\":{\"name\":\"TheSixthAxis\",\"isContributor\":false,\"id\":68,\"imageSrc\":{\"og\":\"outlet/68/o/a60wYXaS.jpg\",\"sm\":\"outlet/68/NhIFRpLf.jpg\",\"lg\":\"outlet/68/EvgsxMq3.jpg\"}},\"ScoreFormat\":{\"id\":25,\"name\":\"0 to 10, whole numbers\",\"shortName\":\"x / 10\",\"scoreDisplay\":\" / 10\",\"isNumeric\":true,\"isSelect\":false,\"isStars\":false,\"base\":10,\"options\":null,\"numDecimals\":0},\"game\":{\"id\":7816,\"name\":\"Death Stranding\"},\"externalUrl\":\"https://www.thesixthaxis.com/2020/07/13/death-stranding-pc-review/\",\"language\":\"en-us\",\"Platforms\":[],\"Authors\":[{\"_id\":\"5f0c77da2282ae3f4424aea6\",\"id\":321,\"name\":\"Miguel Moran\",\"image\":true,\"imageSrc\":{\"og\":\"critic/321/o/UkMLwUYY.jpg\",\"sm\":\"critic/321/R35BgFhF.jpg\",\"lg\":\"critic/321/hRhjWgHU.jpg\"}}],\"alias\":\"Miguel Moran\",\"publishedDate\":\"2020-07-13T00:00:00.000Z\",\"score\":80,\"snippet\":\"Death Stranding takes on a whole new meaning in 2020, and it's a story well worth revisiting or experiencing for the first time on PC. The pacing of the game can certainly drag to a screeching halt through the middle, but with boosted frame rate and crisper visuals, you'll likely be too mesmerised by the hauntingly gorgeous world of Hideo Kojima's latest masterpiece to even care about the uneven story beats.\",\"title\":\"Death Stranding PC Review – TheSixthAxis\",\"isQuoteManual\":true,\"medianAtTimeOfReview\":70,\"npScore\":100,\"createdAt\":\"2020-07-13T16:39:43.694Z\",\"updatedAt\":\"2022-08-11T16:47:23.001Z\",\"__v\":0},{\"_id\":\"5f0c923ad88e035744c1a587\",\"magic\":5400,\"isChosen\":false,\"overrideRecommendation\":false,\"Outlet\":{\"name\":\"Hobby Consolas\",\"isContributor\":false,\"id\":499,\"imageSrc\":{\"og\":\"outlet/499/o/LhEJJe3Z.jpg\",\"sm\":\"outlet/499/OBc8ktCZ.jpg\",\"lg\":\"outlet/499/enArDAQC.jpg\"}},\"ScoreFormat\":{\"id\":17,\"name\":\"0 to 100, whole numbers\",\"shortName\":\"x / 100\",\"scoreDisplay\":\" / 100\",\"isNumeric\":true,\"isSelect\":false,\"base\":1,\"options\":null,\"isStars\":false,\"numDecimals\":0},\"game\":{\"id\":7816,\"name\":\"Death Stranding\"},\"externalUrl\":\"https://www.hobbyconsolas.com/reviews/analisis-death-stranding-pc-personal-aventura-kojima-luce-mejor-nunca-676157\",\"language\":\"es-es\",\"Platforms\":[{\"_id\":\"5f0c707a2282ae3f4424ae77\",\"id\":27,\"name\":\"PC\",\"shortName\":\"PC\"}],\"Authors\":[{\"_id\":\"5f0c707b2282ae3f4424ae78\",\"id\":6451,\"name\":\"Alejandro Alcolea Huertos\",\"image\":false}],\"alias\":\"Alejandro Alcolea Huertos\",\"publishedDate\":\"2020-07-13T00:00:00.000Z\",\"score\":93,\"snippet\":\"Death Stranding is one of those special games that comes out from time to time. Hideo Kojima shows that he's a great storyteller, but he also knows how to create a solid and attractive gameplay. It may not appeal to everyone, but it's certainly one of those games that have to play once in your life.\",\"title\":\"Análisis Death Stranding en PC - la personal aventura de Kojima luce mejor que nunca   - HobbyConsolas Juegos\",\"isQuoteManual\":true,\"medianAtTimeOfReview\":80,\"npScore\":100,\"createdAt\":\"2020-07-13T16:56:26.693Z\",\"updatedAt\":\"2022-08-11T16:47:47.602Z\",\"__v\":0}]";
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

        public string ExtractSnippets(List<Review> reviews)
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
                        { "X-RapidAPI-Key", "api_key" },
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
                        { "X-RapidAPI-Key", "api_key" },
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
                        { "X-RapidAPI-Key", "api_key" },
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
                //await TestError(true);
                //for testing:
                return new List<string> { "Death Stranding is Hideo Kojima's first post-Metal Gear game and is a messy, indulgent vanity project, but also a true original. It delivers a fascinating world of supernatural sci-fi, but its gameplay struggles to support its weight. It is slow, weird, and indulgent, but a journey that will linger in your mind long after it's over. It is a work of unbridled ambition and imagination, but also pretentious, contrived, and frequently dull. It has its moments, but can't carry the weight of expectation. It is a game of love and hope, with a welcome everyman's weariness, but its strongest ideas are tossed aside in the final acts. It is dense, complex, and powerful, but with uneven story beats. It is beautiful, heart racing, heart breaking, frustrating, epic, stunning, and utterly nuts, and a contender for Game of the Generation. It may not appeal to everyone, but it is certainly one of those games that have to be played once in your life." };
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
                //await TestError(true);
                //for testing:
                return new List<string> { "Pros:", "- Fascinating world of supernatural sci-fi", "- Fearless game that often stumbles, but is still fascinating overall", "- Smart, well-produced, and feels good to play", "Cons:", "- Messy, indulgent vanity project", "- Struggles to support its weight", "- Poor pacing and uneven story beats"};
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
                        { "Authorization", "Bearer " + "api_key" },
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

        // delay + exception for UI testing:
        private async Task TestError(bool throwException)
        {
            await Task.Delay(500);
            if (throwException)
                throw new Exception();
        }
    }
}