using Newtonsoft.Json;
using System.Collections.Generic;

namespace Indico.Net.Models
{
    public class NamedEntity
    {
        public Dictionary<string, double> Categories { get; set; }

        public double Confidence { get; set; }

        [JsonConstructor]
        public NamedEntity(Dictionary<string, double> categories, double confidence)
        {
            this.Categories = categories;
            this.Confidence = confidence;
        }
    }
}
