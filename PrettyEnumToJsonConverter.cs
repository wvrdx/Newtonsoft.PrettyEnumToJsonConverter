using Newtonsoft.Json;

namespace Newtonsoft.PrettyEnumToJsonConverter
{
    public class PrettyEnumToJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum || (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                Nullable.GetUnderlyingType(objectType).IsEnum);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;

            //add 'reader.TokenType != JsonToken.String' if it is needed for backward compatibility if used default StringEnumConverter provided by Newtonsoft before
            if (reader.TokenType != JsonToken.Integer)
                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing enum.");

            var rawValue = reader.Value;
            object enumValue;

            //uncomment if it is needed for backward compatibility if used default StringEnumConverter provided by Newtonsoft before
            // if (rawValue is string rawStringValue)
            // {
            //     try
            //     {
            //         if (string.IsNullOrEmpty(rawStringValue)
            //             && objectType.IsGenericType
            //             && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //             return null;
            //
            //         if (Enum.TryParse(enumType, rawStringValue, out var result))
            //             return result;
            //
            //         throw new JsonSerializationException($"Error converting value {rawStringValue} to type '{objectType}'.");
            //     }
            //     catch (System.Exception ex)
            //     {
            //         throw new JsonSerializationException($"Error converting value {rawStringValue} to type '{objectType}'.", ex);
            //     }
            // }

            try
            {
                enumValue = Convert.ChangeType(rawValue, Enum.GetUnderlyingType(enumType));
            }
            catch (OverflowException)
            {
                throw new JsonSerializationException($"Invalid value '{rawValue}' for enum type '{enumType}'.");
            }

            if (enumValue == null)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return null;

                throw new JsonSerializationException($"Invalid value 'null' for non-nullable enum type.");
            }

            if (Enum.IsDefined(enumType, enumValue))
                return Enum.ToObject(enumType, enumValue);

            throw new JsonSerializationException($"Invalid value '{rawValue}' for enum type '{enumType}'.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var underlyingType = Enum.GetUnderlyingType(value.GetType());
                var convertedValue = Convert.ChangeType(value, underlyingType);
                writer.WriteValue(convertedValue);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}