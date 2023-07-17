# Newtonsoft.PrettyEnumToJsonConverter
This converter provides you posibility to convert your enum value to the underlying value when serializing to JSON and vice versa.

For example for the next enum:
enum MyEnum : byte
{
  Value1 = 4,
  Value2 = 2,
  Value3 = 1
}
Value1 will be serialized to 4 and not the "Value1" as default StringEnumConverter provided by Newtonsoft does.
