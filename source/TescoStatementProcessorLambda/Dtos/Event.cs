using System.Text.Json.Serialization;

namespace TescoStatementProcessorLambda.Dtos;

public record class Event(string Version, Guid Id, string Source, string Account, DateTime Time, string Region, string[] Resources, Detail Detail)
{
    [JsonPropertyName("detail-type")]
    public string DetailType { get; set; }
}


public record class Folder(string Name);

public record class Bucket(string Name);

public record class ItemDetail(string Key, int Size, string Etag, string sequencer, Folder folder)
{
    [JsonPropertyName("version-id")]
    public string version { get; set; } = string.Empty;
}

public record class Detail(string Version, Bucket Bucket)
{
    [JsonPropertyName("object")]
    public ItemDetail Object { get; set; }
}
