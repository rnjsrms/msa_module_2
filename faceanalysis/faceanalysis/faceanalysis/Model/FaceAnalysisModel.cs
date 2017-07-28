using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace faceanalysis.Model
{
    public class FaceAnalysisModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "Age")]
        public float Age { get; set; }

        [JsonProperty(PropertyName = "Smile")]
        public float Smile { get; set; }

        [JsonProperty(PropertyName = "Anger")]
        public float Anger { get; set; }

        [JsonProperty(PropertyName = "Contempt")]
        public float Contempt { get; set; }

        [JsonProperty(PropertyName = "Disgust")]
        public float Disgust { get; set; }

        [JsonProperty(PropertyName = "Fear")]
        public float Fear { get; set; }

        [JsonProperty(PropertyName = "Happiness")]
        public float Happiness { get; set; }

        [JsonProperty(PropertyName = "Neutral")]
        public float Neutral { get; set; }

        [JsonProperty(PropertyName = "Sadness")]
        public float Sadness { get; set; }

        [JsonProperty(PropertyName = "Surprise")]
        public float Surprise { get; set; }

        [JsonProperty(PropertyName = "Glasses")]
        public string Glasses { get; set; }

        [JsonProperty(PropertyName = "Bald")]
        public float Bald { get; set; }

        [JsonProperty(PropertyName = "Black")]
        public float Black { get; set; }

        [JsonProperty(PropertyName = "Other")]
        public float Other { get; set; }

        [JsonProperty(PropertyName = "Red")]
        public float Red { get; set; }

        [JsonProperty(PropertyName = "Brown")]
        public float Brown { get; set; }

        [JsonProperty(PropertyName = "Gray")]
        public float Gray { get; set; }

        [JsonProperty(PropertyName = "Blonde")]
        public float Blonde { get; set; }
    }
}
