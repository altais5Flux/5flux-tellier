using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebservicesSage.Object.CustomerSearch
{

    public partial class CustomerSearch
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }
        [JsonProperty("taxvat")]
        public string taxvat { get; set; }

        [JsonProperty("default_billing")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long DefaultBilling { get; set; }

        [JsonProperty("default_shipping")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long DefaultShipping { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("created_in")]
        public string CreatedIn { get; set; }

        [JsonProperty("dob")]
        public DateTimeOffset Dob { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("gender")]
        public long Gender { get; set; }

        [JsonProperty("store_id")]
        public long StoreId { get; set; }

        [JsonProperty("website_id")]
        public long WebsiteId { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("disable_auto_group_change")]
        public long DisableAutoGroupChange { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }

        [JsonProperty("custom_attributes")]
        public List<CustomAttribute> CustomAttributes { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("customer_id")]
        public long CustomerId { get; set; }

        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("region_id")]
        public long RegionId { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("company")]
        public string company { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("default_shipping")]
        public bool DefaultShipping { get; set; }

        [JsonProperty("default_billing")]
        public bool DefaultBilling { get; set; }
        [JsonProperty("custom_attributes")]
        public List<CustomAttribute> CustomAttributes { get; set; }
    }

    public partial class Region
    {
        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region")]
        public string RegionRegion { get; set; }

        [JsonProperty("region_id")]
        public long RegionId { get; set; }
    }

    public partial class CustomAttribute
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ExtensionAttributes
    {
        [JsonProperty("is_subscribed")]
        public bool IsSubscribed { get; set; }
    }

    public partial class CustomerSearch
    {
        public static CustomerSearch FromJson(string json) => JsonConvert.DeserializeObject<CustomerSearch>(json, WebservicesSage.Object.CustomerSearch.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CustomerSearch self) => JsonConvert.SerializeObject(self, WebservicesSage.Object.CustomerSearch.Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
