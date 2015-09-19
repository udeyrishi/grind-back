using Newtonsoft.Json;
using System.Collections.Generic;

namespace Indico.Net.Models
{
    public class NamedEntity
    {
        [JsonProperty("categories")]
        public Dictionary<string, double> Categories { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }
}
