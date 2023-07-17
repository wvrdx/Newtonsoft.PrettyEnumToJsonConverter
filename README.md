# Newtonsoft.PrettyEnumToJsonConverter
This converter provides you with the possibility to convert your enum value to the underlying value when serializing to JSON and vice versa.

For example for the next enum:
```C#
enum MyEnum : byte
{
  Value1 = 4,
  Value2 = 2,
  Value3 = 1
}
```
Value1 will be serialized to 4 and not to the "Value1" as the default StringEnumConverter provided by Newtonsoft does.
