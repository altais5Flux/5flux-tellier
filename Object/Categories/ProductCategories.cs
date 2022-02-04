using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebservicesSage.Object.Categories
{
    

    public partial class ProductCategories
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("parent_id")]
    public long ParentId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("position")]
    public long Position { get; set; }

    [JsonProperty("level")]
    public long Level { get; set; }

    [JsonProperty("product_count")]
    public long ProductCount { get; set; }

    [JsonProperty("children_data")]
    public List<ProductCategories> ChildrenData { get; set; }
}

public partial class ProductCategories
{
    public static ProductCategories FromJson(string json) => JsonConvert.DeserializeObject<ProductCategories>(json, WebservicesSage.Object.Categories.Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this ProductCategories self) => JsonConvert.SerializeObject(self, WebservicesSage.Object.Categories.Converter.Settings);
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