using System;
using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Entities.Query.Filters;
using Newtonsoft.Json;
using System.Collections;
using DealCloud.Common.Attributes;
using Newtonsoft.Json.Linq;

namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    /// Info about filtering by 1 field
    /// </summary>
    public class FilterInfo
    {
        // NOTE: if updating this set need to update ValueEquals method accordingly
        // NOTE: These types are serialized to DB. Assembly changes have an impact on the logic after deserialization and may break an application
        private static readonly HashSet<Type> _allowedValues = new HashSet<Type>
        {
            // NOTE: upon saving to database object will be serialized to json and after retrieval somehow deserialization takes value of int as Int64
            typeof (long),
            typeof (decimal),
            typeof (string),
            typeof (DateTime),
            typeof (bool),
            typeof (Money),
            typeof (List<int>),
            typeof (List<string>),
            typeof (List<object>),
            typeof (SmartFilter),
            typeof (IList)
        };

        private object _value;

        private object _valueTo;

        /// <summary>
        /// Column To Filter
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Value To Filter
        /// </summary>
        [Sanitize]
        public object Value
        {
            get { return _value; }
            set
            {
                if (value != null)
                {
                    if (value is int)
                    {
                        value = Convert.ToInt64(value);
                    }
                    else if (value is double)
                    {
                        value = Convert.ToDecimal(value);
                    }
                    else if (!_allowedValues.Contains(value.GetType()) && !(value is IList))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Value)} type can not be {value.GetType().Name}. Allowed types are: {string.Join(", ", _allowedValues.Select(t => t.Name))}.");
                    }

                    var type = value.GetType();
                    var valueToType = ValueTo?.GetType();

                    if (valueToType != null && type != valueToType)
                    {
                        throw new ArgumentException($"{nameof(Value)} type should be same type as {nameof(ValueTo)} i.e. {valueToType.Name}.");
                    }
                }

                _value = value;
            }
        }

        /// <summary>
        /// second value for Between filters
        /// </summary>
        public object ValueTo
        {
            get { return _valueTo; }
            set
            {
                if (value != null)
                {
                    if (value is int)
                    {
                        value = Convert.ToInt64(value);
                    }
                    else if (value is double)
                    {
                        value = Convert.ToDecimal(value);
                    }
                    else if (!_allowedValues.Contains(value.GetType()) && !(value is IList))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(ValueTo)} type can not be {value.GetType().Name}. Allowed types are: {string.Join(", ", _allowedValues.Select(t => t.Name))}.");
                    }

                    var type = value.GetType();
                    var valueType = Value?.GetType();

                    if (valueType != null && type != valueType)
                    {
                        throw new ArgumentException($"{nameof(ValueTo)} type should be same type as {nameof(Value)} i.e. {valueType.Name}.");
                    }
                }

                _valueTo = value;
            }
        }

        public string CurrencyCode { get; set; }

        public FilterOperation FilterOperation { get; set;}

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + ColumnId;
                result = result * 37 + (Value?.GetHashCode() ?? 0);
                result = result * 37 + (ValueTo?.GetHashCode() ?? 0);
                result = result * 37 + (int) FilterOperation;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var filter = obj as FilterInfo;
            var result = false;

            if (filter != null)
            {
                result = ColumnId == filter.ColumnId && string.Equals(CurrencyCode, filter.CurrencyCode) && FilterOperation == filter.FilterOperation;

                if (result)
                {
                    result = ValueEquals(Value, filter.Value) && ValueEquals(ValueTo, filter.ValueTo);
                }
            }

            return result;
        }

        private bool ValueEquals(object obj1, object obj2)
        {
            var result = true;

            if (obj1 != null && obj2 != null)
            {
                var list1 = obj1 as IList;
                var list2 = obj2 as IList;

                //equal comparison of FilterInfo is used while comparing modefied notification and old one.
                //If FilterInfo value is List<object> then it is In filter, so possible modification for such filters is changing set of values in the list
                //Filter is binded to a column, so if ColumnIds are equal, then types of elements in two lists are the same
                //it is enough for this reason just compare, if unordered sets of values in lists are equal.
                //Also we assume, that possible types in list are the following: int, string, ChoiceFieldValue, NamedEntry, UserInfo
                if (list1 != null && list2 != null)
                {
                    result = GetDecimalSequence(list1).OrderBy(id => id).SequenceEqual(GetDecimalSequence(list2).OrderBy(id => id));
                }
                else
                {
                    result = obj1.Equals(obj2);
                }
            }

            return result;
        }

        private IEnumerable<decimal> GetDecimalSequence(IList list)
        {
            foreach (var o in list)
            {
                decimal ret = 0;
                if (o is NamedEntry) //ChoiceFieldValue, NamedEntry, UserInfo are derived from NamedEntry
                {
                    ret = (o as NamedEntry).Id;
                }
                else
                {
                    try //other values are int or string id
                    {
                        ret = Convert.ToDecimal(o);
                    }
                    catch { continue; }
                }
                yield return ret;
            }
        }
    }

    /// <summary>
    /// for First level queries, you can specify AND and OR between groups
    /// </summary>
    public class FilterTerm : FilterInfo
    {
        public FilterTermType FilterTermType { get; set; }

        /// <summary>
        /// set to true if user can provide value(s) for this filter
        /// </summary>
        public bool CanBeSuppliedByUser { get; set; }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + base.GetHashCode();
                result = result * 37 + (int) FilterTermType;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var result = base.Equals(obj);

            if (result)
            {
                var filter = obj as FilterTerm;

                result = filter != null && FilterTermType == filter.FilterTermType;
            }

            return result;
        }
    }

    public enum FilterOperation
    {
        Equals = 0,
        Contains = 1,
        Greater = 2,
        GreaterOrEqual = 3,
        Less = 4,
        LessOrEqual = 5,
        StartsWith = 6,
        In = 7,
        Between = 8,
        NotIn = 9,
        NotEqualTo = 10,
        DoesntContain = 11,
        NoData = 12,
        NotNoData = 13
    }

    public enum FilterTermType
    {
        None = 0,
        Filter = 1,
        OpenBracket = 2,
        CloseBracket = 3,
        And = 4,
        Or = 5
    }

    public static class FilterOperatorExtentions
    {
        public static bool IsNot(this FilterOperation op)
        {
            return op == FilterOperation.NotEqualTo || op == FilterOperation.NotIn || op == FilterOperation.DoesntContain || op == FilterOperation.NotNoData;
        }
    }
}
