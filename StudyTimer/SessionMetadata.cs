using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTimer
{
    internal class SessionMetadata
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("subjects")]
        public Dictionary<string, string> Subjects { get; set; }

        public SessionMetadata(string date, Dictionary<string, string> subjects)
        {
            Date = date;
            Subjects = subjects;
        }
    }
}