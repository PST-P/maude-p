
        using System;
        using System.Text.Json;
        using System.Text.Json.Serialization;
        using Plang.CSharpRuntime.Values;

        namespace PImplementation {
            public class PrtIntConverter : JsonConverter<PrtInt>
            {
                public override PrtInt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return reader.GetInt32();
                }

                public override void Write(Utf8JsonWriter writer, PrtInt value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(((int)value).ToString());
                }
            }

            public class PrtStringConverter : JsonConverter<PrtString>
            {
                public override PrtString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return (PrtString)reader.GetString();
                }

                public override void Write(Utf8JsonWriter writer, PrtString value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(value.ToString());
                }
            }
        }
        