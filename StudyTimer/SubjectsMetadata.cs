using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTimer
{
    internal class SubjectsMetadata
    {
        [JsonProperty("subjects")]
        public List<string> Subjects { get; set; }

        [JsonProperty("last")]
        public string LastSubject { get; set; }
    }
}