using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebservicesSage.Object
{

    public partial class ProductSearchCriteria
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

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("attribute_set_id")]
        public long AttributeSetId { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("visibility")]
        public long Visibility { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }

        [JsonProperty("product_links")]
        public List<object> ProductLinks { get; set; }

        [JsonProperty("options")]
        public List<object> Options { get; set; }

        [JsonProperty("media_gallery_entries")]
        public List<object> MediaGalleryEntries { get; set; }

        [JsonProperty("tier_prices")]
        public List<TiersPrice> TierPrices { get; set; }

        [JsonProperty("custom_attributes")]
        public List<CustomAttribute> CustomAttributes { get; set; }
    }
    public partial class TiersPrice
    {
        [JsonProperty("customer_group_id")]
        public string CustomerGroupId { get; set; }

        [JsonProperty("qty")]
        public string Qty { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
    public partial class CustomAttribute
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class ExtensionAttributes
    {
        [JsonProperty("website_ids")]
        public List<long> WebsiteIds { get; set; }
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

    public partial struct Value
    {
        public List<object> AnythingArray;
        public string String;

        public static implicit operator Value(List<object> AnythingArray) => new Value { AnythingArray = AnythingArray };
        public static implicit operator Value(string String) => new Value { String = String };
    }

    public partial class ProductSearchCriteria
    {
        public static ProductSearchCriteria FromJson(string json) => JsonConvert.DeserializeObject<ProductSearchCriteria>(json, WebservicesSage.Object.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ProductSearchCriteria self) => JsonConvert.SerializeObject(self, WebservicesSage.Object.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ValueConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ValueConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Value) || t == typeof(Value?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Value { String = stringValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<List<object>>(reader);
                    return new Value { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type Value");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Value)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            throw new Exception("Cannot marshal type Value");
        }

        public static readonly ValueConverter Singleton = new ValueConverter();
    }
}
