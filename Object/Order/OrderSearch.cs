﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using WebservicesSage.Object.Order;
//
//    var orderSearch = OrderSearch.FromJson(jsonString);

namespace WebservicesSage.Object.Order
{
    public partial class OrderSearch
    {
        [JsonProperty("items")]
        public List<OrderSearchItem> Items { get; set; }

        [JsonProperty("search_criteria")]
        public SearchCriteria SearchCriteria { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }
    }

    public partial class OrderSearchItem
    {
        [JsonProperty("applied_rule_ids")]
       
        public string AppliedRuleIds { get; set; }

        [JsonProperty("base_currency_code")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("base_discount_amount")]
        public long BaseDiscountAmount { get; set; }

        [JsonProperty("base_grand_total")]
        public long BaseGrandTotal { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public long BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("base_shipping_amount")]
        public long BaseShippingAmount { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public long BaseShippingDiscountAmount { get; set; }

        [JsonProperty("base_shipping_discount_tax_compensation_amnt")]
        public long BaseShippingDiscountTaxCompensationAmnt { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public long BaseShippingInclTax { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public long BaseShippingTaxAmount { get; set; }

        [JsonProperty("base_subtotal")]
        public long BaseSubtotal { get; set; }

        [JsonProperty("base_subtotal_incl_tax")]
        public long BaseSubtotalInclTax { get; set; }

        [JsonProperty("base_tax_amount")]
        public long BaseTaxAmount { get; set; }

        [JsonProperty("base_total_due")]
        public long BaseTotalDue { get; set; }

        [JsonProperty("base_to_global_rate")]
        public long BaseToGlobalRate { get; set; }

        [JsonProperty("base_to_order_rate")]
        public long BaseToOrderRate { get; set; }

        [JsonProperty("billing_address_id")]
        public long BillingAddressId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("customer_email")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customer_firstname")]
        public string CustomerFirstname { get; set; }

        [JsonProperty("customer_gender")]
        public long CustomerGender { get; set; }

        [JsonProperty("customer_group_id")]
        public long CustomerGroupId { get; set; }

        [JsonProperty("customer_is_guest")]
        public long CustomerIsGuest { get; set; }

        [JsonProperty("customer_id")]
        public long CustomerId { get; set; }

        [JsonProperty("customer_lastname")]
        public string CustomerLastname { get; set; }

        [JsonProperty("customer_note_notify")]
        public long CustomerNoteNotify { get; set; }

        [JsonProperty("discount_amount")]
        public long DiscountAmount { get; set; }

        [JsonProperty("entity_id")]
        public long EntityId { get; set; }

        [JsonProperty("global_currency_code")]
        public string GlobalCurrencyCode { get; set; }

        [JsonProperty("grand_total")]
        public long GrandTotal { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public long DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("increment_id")]
        public string IncrementId { get; set; }

        [JsonProperty("is_virtual")]
        public long IsVirtual { get; set; }

        [JsonProperty("order_currency_code")]
        public string OrderCurrencyCode { get; set; }

        [JsonProperty("protect_code")]
        public string ProtectCode { get; set; }

        [JsonProperty("quote_id")]
        public long QuoteId { get; set; }

        [JsonProperty("remote_ip")]
        public string RemoteIp { get; set; }

        [JsonProperty("shipping_amount")]
        public long ShippingAmount { get; set; }

        [JsonProperty("shipping_description")]
        public string ShippingDescription { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public long ShippingDiscountAmount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public long ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public long ShippingInclTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public long ShippingTaxAmount { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("store_currency_code")]
        public string StoreCurrencyCode { get; set; }

        [JsonProperty("store_id")]
        public long StoreId { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("store_to_base_rate")]
        public long StoreToBaseRate { get; set; }

        [JsonProperty("store_to_order_rate")]
        public long StoreToOrderRate { get; set; }

        [JsonProperty("subtotal")]
        public long Subtotal { get; set; }

        [JsonProperty("subtotal_incl_tax")]
        public long SubtotalInclTax { get; set; }

        [JsonProperty("tax_amount")]
        public long TaxAmount { get; set; }

        [JsonProperty("total_due")]
        public long TotalDue { get; set; }

        [JsonProperty("total_item_count")]
        public long TotalItemCount { get; set; }

        [JsonProperty("total_qty_ordered")]
        public long TotalQtyOrdered { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("items")]
        public List<ParentItemElement> Items { get; set; }

        [JsonProperty("billing_address")]
        public Address BillingAddress { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("status_histories")]
        public List<object> StatusHistories { get; set; }

        [JsonProperty("extension_attributes")]
        public ItemExtensionAttributes ExtensionAttributes { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("address_type")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("company")]
        public string company { get; set; }

        [JsonProperty("customer_address_id")]
        public long CustomerAddressId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_id")]
        public long EntityId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("postcode")]
        
        public string Postcode { get; set; }

        [JsonProperty("region")]

        public string Region { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
        [JsonProperty("Li_No")]
        public int Li_No { get; set; }
    }

    public partial class ItemExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public List<ShippingAssignment> ShippingAssignments { get; set; }

        [JsonProperty("payment_additional_info")]
        public List<PaymentAdditionalInfo> PaymentAdditionalInfo { get; set; }

        [JsonProperty("applied_taxes")]
        public List<object> AppliedTaxes { get; set; }

        [JsonProperty("item_applied_taxes")]
        public List<object> ItemAppliedTaxes { get; set; }

        [JsonProperty("altais_order_flag")]
        
        public string AltaisOrderFlag { get; set; }
    }

    public partial class PaymentAdditionalInfo
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("items")]
        public List<ParentItemElement> Items { get; set; }
    }

    public partial class ParentItemElement
    {
        [JsonProperty("amount_refunded")]
        public Double AmountRefunded { get; set; }

        [JsonProperty("applied_rule_ids", NullValueHandling = NullValueHandling.Ignore)]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("base_amount_refunded")]
        public Double BaseAmountRefunded { get; set; }

        [JsonProperty("base_discount_amount")]
        public Double BaseDiscountAmount { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public Double BaseDiscountInvoiced { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount", NullValueHandling = NullValueHandling.Ignore)]
        public Double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("base_original_price", NullValueHandling = NullValueHandling.Ignore)]
        public Double BaseOriginalPrice { get; set; }

        [JsonProperty("base_price")]
        public Double BasePrice { get; set; }

        [JsonProperty("base_price_incl_tax", NullValueHandling = NullValueHandling.Ignore)]
        public Double BasePriceInclTax { get; set; }

        [JsonProperty("base_row_invoiced")]
        public Double BaseRowInvoiced { get; set; }

        [JsonProperty("base_row_total")]
        public Double BaseRowTotal { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public Double BaseRowTotalInclTax { get; set; }

        [JsonProperty("base_tax_amount")]
        public Double BaseTaxAmount { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public Double BaseTaxInvoiced { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("discount_amount")]
        public Double DiscountAmount { get; set; }

        [JsonProperty("discount_invoiced")]
        public Double DiscountInvoiced { get; set; }

        [JsonProperty("discount_percent")]
        public Double DiscountPercent { get; set; }

        [JsonProperty("free_shipping")]
        public Double FreeShipping { get; set; }

        [JsonProperty("discount_tax_compensation_amount", NullValueHandling = NullValueHandling.Ignore)]
        public Double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("is_qty_decimal")]
        public Double IsQtyDecimal { get; set; }

        [JsonProperty("is_virtual")]
        public long IsVirtual { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("no_discount")]
        public long NoDiscount { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("original_price")]
        public Double OriginalPrice { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("price_incl_tax", NullValueHandling = NullValueHandling.Ignore)]
        public Double PriceInclTax { get; set; }

        [JsonProperty("product_id")]
        public long ProductId { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("qty_canceled")]
        public long QtyCanceled { get; set; }

        [JsonProperty("qty_invoiced")]
        public long QtyInvoiced { get; set; }

        [JsonProperty("qty_ordered")]
        public long QtyOrdered { get; set; }

        [JsonProperty("qty_refunded")]
        public long QtyRefunded { get; set; }

        [JsonProperty("qty_shipped")]
        public long QtyShipped { get; set; }

        [JsonProperty("quote_item_id")]
        public long QuoteItemId { get; set; }

        [JsonProperty("row_invoiced")]
        public long RowInvoiced { get; set; }

        [JsonProperty("row_total")]
        public long RowTotal { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public long RowTotalInclTax { get; set; }

        [JsonProperty("row_weight")]
        public long RowWeight { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("store_id")]
        public long StoreId { get; set; }

        [JsonProperty("tax_amount")]
        public Double TaxAmount { get; set; }

        [JsonProperty("tax_invoiced")]
        public Double TaxInvoiced { get; set; }

        [JsonProperty("tax_percent")]
        public Double TaxPercent { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public Double Weight { get; set; }

        [JsonProperty("product_option", NullValueHandling = NullValueHandling.Ignore)]
        public ProductOption ProductOption { get; set; }

        [JsonProperty("parent_item_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentItemId { get; set; }

        [JsonProperty("parent_item", NullValueHandling = NullValueHandling.Ignore)]
        public ParentItemElement ParentItem { get; set; }
    }

    public partial class ProductOption
    {
        [JsonProperty("extension_attributes")]
        public ProductOptionExtensionAttributes ExtensionAttributes { get; set; }
    }

    public partial class ProductOptionExtensionAttributes
    {
        [JsonProperty("configurable_item_options")]
        public List<ConfigurableItemOption> ConfigurableItemOptions { get; set; }
    }

    public partial class ConfigurableItemOption
    {
        [JsonProperty("option_id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OptionId { get; set; }

        [JsonProperty("option_value")]
        public long OptionValue { get; set; }
    }

    public partial class Shipping
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("total")]
        public Total Total { get; set; }
    }

    public partial class Total
    {
        [JsonProperty("base_shipping_amount")]
        public long BaseShippingAmount { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public long BaseShippingDiscountAmount { get; set; }

        [JsonProperty("base_shipping_discount_tax_compensation_amnt")]
        public long BaseShippingDiscountTaxCompensationAmnt { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public long BaseShippingInclTax { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public long BaseShippingTaxAmount { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public long ShippingDiscountAmount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public long ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public long ShippingInclTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public long ShippingTaxAmount { get; set; }
    }

    public partial class Payment
    {
        [JsonProperty("account_status")]
        public object AccountStatus { get; set; }

        [JsonProperty("additional_information")]
        public List<string> AdditionalInformation { get; set; }

        [JsonProperty("amount_ordered")]
        public long AmountOrdered { get; set; }

        [JsonProperty("base_amount_ordered")]
        public long BaseAmountOrdered { get; set; }

        [JsonProperty("base_shipping_amount")]
        public long BaseShippingAmount { get; set; }

        [JsonProperty("cc_last4")]
        public object CcLast4 { get; set; }

        [JsonProperty("entity_id")]
        public long EntityId { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }
        [JsonProperty("po_number")]
        public string Po_number { get; set; }

        [JsonProperty("shipping_amount")]
        public long ShippingAmount { get; set; }
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

    public partial class OrderSearch
    {
        public static OrderSearch FromJson(string json) => JsonConvert.DeserializeObject<OrderSearch>(json, WebservicesSage.Object.Order.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this OrderSearch self) => JsonConvert.SerializeObject(self, WebservicesSage.Object.Order.Converter.Settings);
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
