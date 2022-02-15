using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain
{
    //readmodel DTO
    public class MetaTag : IComparable<MetaTag>, IComparable, IEquatable<MetaTag>
    {
        public readonly Guid Id;
        public readonly string Tag;
        public MetaTag(string tag)
        {
            Tag = tag;
        }
        #region IEquatable<T> Implementation
        public bool Equals(MetaTag other)
        {
            if (other is null) return false;
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj) => Equals(obj as MetaTag);
        public override int GetHashCode()
        {
            // Use `unchecked` so if results overflows it is truncated          
            unchecked
            {
                // Computing hashCode from https://aaronstannard.com/overriding-equality-in-dotnet/
                var hashCode = 13;
                hashCode = ComputeHash(hashCode, Id.GetHashCode());
                return hashCode;
            }
        }
        // == and != 
        public static bool operator ==(MetaTag x, MetaTag y) => x?.Equals(y) ?? y is null;
        public static bool operator !=(MetaTag x, MetaTag y) => !(x?.Equals(y) ?? y is null);
        public int ComputeHash(int currentHash, int value) => (currentHash * 397) ^ value;
        #endregion IEquatable<T> Implementation

        #region IComparable<T> Implementation
        public int CompareTo(object other) => CompareTo(other as MetaTag);
        public int CompareTo(MetaTag other)
        {
            if (other == null) return 1;
            return string.CompareOrdinal(Tag, other.Tag);
        }
        // >, <, >=, <= from source 2
        public static bool operator >(MetaTag op1, MetaTag op2) => op1?.CompareTo(op2) == 1;
        public static bool operator <(MetaTag op1, MetaTag op2) => op1?.CompareTo(op2) == -1;
        public static bool operator >=(MetaTag op1, MetaTag op2) => op1?.CompareTo(op2) >= 0;
        public static bool operator <=(MetaTag op1, MetaTag op2) => op1?.CompareTo(op2) <= 0;
        #endregion IComparable<T> Implementation
    }
}
