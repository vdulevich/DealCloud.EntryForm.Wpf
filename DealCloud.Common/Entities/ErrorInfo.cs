using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities
{
    /// <summary>
    ///     Error information
    /// </summary>
    public class ErrorInfo
    {
        private string _description;

        public ErrorInfo()
        {
        }

        public ErrorInfo(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public ErrorInfo(int code, string description, ErrorTypes errorInfoTypes) : this(code, description)
        {
            ErrorType = errorInfoTypes;
        }

        /// <summary>
        ///     Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     Field id to which error is related.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        ///     Error description.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value == null ? null : string.Intern(value); }
        }

        /// <summary>
        ///     Error type.
        /// </summary>
        public ErrorTypes ErrorType { get; set; }

        /// <summary>
        ///     Value to which error is related.
        /// </summary>
        public object ErroredValue { get; set; }

        public override bool Equals(object obj)
        {
            var e = obj as ErrorInfo;
            return (object) e != null && e.Code == Code && e.Description == Description;
        }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result*37;
                result += Code.GetHashCode();
                result = result*37;
                if (Description != null) result += Description.GetHashCode();
                result = result*37;
                result += ErrorType.GetHashCode();
            }

            return result;
        }

        public static bool operator ==(ErrorInfo e1, ErrorInfo e2)
        {
            return (object) e1 != null ? e1.Equals(e2) : (object) e2 == null;
        }

        public static bool operator !=(ErrorInfo e1, ErrorInfo e2)
        {
            return !(e1 == e2);
        }

        public override string ToString()
        {
            return $"[{Code}] {Description}";
        }

        public string ToString(bool showErrorCode)
        {
            return showErrorCode ? ToString() : Description;
        }
    }
}