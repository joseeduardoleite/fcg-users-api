using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;
using FiapCloudGames.Users.Domain.Enums;

namespace FiapCloudGames.Users.Api.Converters;

[ExcludeFromCodeCoverage]
public class RoleConverter : JsonConverter<ERole>
{
    public override ERole Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Enum.TryParse<ERole>(reader.GetString(), true, out var role))
            return role;

        throw new JsonException($"Valor de role inválido: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, ERole value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}