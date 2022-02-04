using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebservicesSage.Object.CatTarSearch
{
    

    public partial class CatTarifSearch
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("search_criteria")]
        public SearchCriteria SearchCriteria { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("tax_class_id")]
        public long TaxClassId { get; set; }

        [JsonProperty("tax_class_name")]
        public string TaxClassName { get; set; }
    }

    public partial class SearchCriteria
    {
        [JsonProperty("filter_groups")]
        public List<FilterGroup> FilterGroups { get; set; }
    }

    public partial class FilterGroup
    {
        [JsonProperty("filters")]
        public List<Filter> Filters { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("condition_type")]
        public string ConditionType { get; set; }
    }

    public partial class CatTarifSearch
    {
        public static CatTarifSearch FromJson(string json) => JsonConvert.DeserializeObject<CatTarifSearch>(json, WebservicesSage.Object.CatTarSearch.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CatTarifSearch self) => JsonConvert.SerializeObject(self, WebservicesSage.Object.CatTarSearch.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
