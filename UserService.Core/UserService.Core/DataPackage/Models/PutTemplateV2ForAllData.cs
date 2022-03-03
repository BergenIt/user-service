using System.Collections.Generic;

using Nest;

namespace UserService.Core.Models
{
    public class PutTemplateV2ForAllData
    {
        [Newtonsoft.Json.JsonProperty("index_patterns")]
        public string[] IndexPatterns { get; set; }

        [Newtonsoft.Json.JsonProperty("data_stream")]
        public object DataStream { get; set; }

        [Newtonsoft.Json.JsonProperty("template")]
        public PutTemplateData Template { get; set; }
    };

    public class PutTemplateData
    {
        [Newtonsoft.Json.JsonProperty("settings")]
        public PutTemplateSettingsData Settings { get; set; }

        [Newtonsoft.Json.JsonProperty("mappings")]
        public PutMapping TypeMapping { get; set; }
    }

    public class PutMapping
    {
        [Newtonsoft.Json.JsonProperty("_source")]
        public PutSourceMapping Source => new();

        [Newtonsoft.Json.JsonProperty("properties")]
        public IDictionary<string, PutTemplateIProperty> Properties { get; set; }
    }

    public class PutTemplateIProperty : IProperty
    {
        public IDictionary<string, object> LocalMetadata { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public PropertyName Name { get; set; }
        public string Type { get; set; }
        public bool? Fielddata { get; set; }
    }

    public class PutSourceMapping
    {
        [Newtonsoft.Json.JsonProperty("enabled")]
        public bool Enabled => true;
    }

    public class PutTemplateSettingsData
    {
        [Newtonsoft.Json.JsonProperty("number_of_shards")]
        public int NumberOfShards { get; set; }

        [Newtonsoft.Json.JsonProperty("number_of_replicas")]
        public int NumberOfReplicas { get; set; }

        [Newtonsoft.Json.JsonProperty("index.lifecycle.name")]
        public string IndexLifecycleName { get; set; }

        [Newtonsoft.Json.JsonProperty("index.lifecycle.rollover_alias")]
        public string IndexLifecycleAlias { get; set; }
    }
}
