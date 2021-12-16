using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StudyTimer
{
    internal class StudyMetadata
    {
        [JsonProperty("totaltime")]
        public string TotalTime { get; set; }

        [JsonProperty("subjects")]
        public Dictionary<string, string> Subject { get; set; }

        [JsonProperty("sessions")]
        public List<SessionMetadata> Sessions { get; set; }// 10/12/21 algoritmi : 02:25 h
    }
}