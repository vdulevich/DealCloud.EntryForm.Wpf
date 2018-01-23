using DealCloud.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Enums
{
    public enum FieldTypes
    {
        [MemberDescription("Text")]
        Text = 1,

        [MemberDescription("Choice")]
        Choice = 2,

        [MemberDescription("Number")]
        Number = 3,

        [MemberDescription("Date and Time")]
        Date = 4,

        [MemberDescription("Reference")]
        Reference = 5,

        [MemberDescription("Yes / No")]
        Boolean = 6,


        /// <summary>
        /// Special Type of reference where it references user of the system
        /// </summary>
        [MemberDescription("User")]
        User = 7,

        [MemberDescription("Binary")]
        Binary = 13,

        [MemberDescription("Name of List")]
        EntryListId = 14,

        [MemberDescription("Autoincremental")]
        Counter = 15

    }

    /// <summary>
    /// Field types used by front end side
    /// </summary>
    public enum VirtualFieldTypes
    {

        [MemberDescription("Attachment")]
        Attachment = -1,
        [MemberDescription("Calculation")]
        Calculation = -2,
        [MemberDescription("Smart")]
        SmartField = -3,

    }

    public static class Extensions
    {

        public static bool IsNumericField(this FieldTypes ft)
        {
            return ft == FieldTypes.Number || ft == FieldTypes.Counter;
        }

        /// <summary>
        /// Default field types for the first initialization of field templates
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldTypes ToDefaultFieldType(this VirtualFieldTypes type)
        {
            switch (type)
            {
                case VirtualFieldTypes.Attachment:
                    return FieldTypes.Reference;
                case  VirtualFieldTypes.Calculation:
                    return FieldTypes.Number;
                case  VirtualFieldTypes.SmartField:
                    return FieldTypes.Date; //added for default: actually selected by smart field on FE
                default:
                    throw new ArgumentException("Field type is undefined");
            }
        }
    }
}
