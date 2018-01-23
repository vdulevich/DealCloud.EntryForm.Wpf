using System;
using System.Runtime.Serialization;
using DealCloud.Common.Interfaces;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities
{
    /// <summary>
    ///     An entity with ID and name.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class NamedEntry : IDeepClonable<NamedEntry>
    {
        public NamedEntry()
        {
        }

        public NamedEntry(NamedEntry src)
        {
            Id = src.Id;
            Name = src.Name;
            EntryListId = src.EntryListId;
        }

        public NamedEntry(int id, string name) : this()
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        ///     Id of and Entry instance. unique across all objects
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Entity name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of the object.
        /// </summary>
        public int EntryListId { get; set; }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }

        /// <summary>
        ///     Returns Name, if it is not null, otherwise DisplayName
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return Name;
        }

        public virtual NamedEntry Clone()
        {
            return new NamedEntry { Id = Id, Name = Name, EntryListId = EntryListId };
        }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + Id;
                result = result * 37 + EntryListId;
                result = result * 37 + (Name?.GetHashCode() ?? 0);
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var entry = obj as NamedEntry;

            return entry != null && entry.Id == Id && string.Equals(entry.Name, Name) && entry.EntryListId == EntryListId;
        }
    }
}