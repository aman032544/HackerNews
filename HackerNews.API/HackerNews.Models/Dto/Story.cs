﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HackerNews.Models.Dto
{
    public class Story
    {
        [JsonProperty("by")]
        public string By { get; set; }

        [JsonProperty("descendants")]
        public int Descendants { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
