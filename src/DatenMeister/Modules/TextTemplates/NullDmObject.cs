using System;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DatenMeister.Modules.TextTemplates
{
    public class NullDmObject : ScriptObject, IConvertible
    {
        public override string ToString(TemplateContext context, SourceSpan span)
        {
            return string.Empty;
        }

        public override bool TryGetValue(TemplateContext context, SourceSpan span, string member, out object value)
        {
            value = new NullDmObject();
            return true;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return 0;
        }

        public char ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return DateTime.MinValue;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return 0;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return 0.0;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return 0;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return 0;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return 0;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return 0;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return 0;
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Empty;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidOperationException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return 0;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return 0;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return 0;
        }
    }
}