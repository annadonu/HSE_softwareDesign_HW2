using System.Text.Json;

namespace POS.Infrastructure.Persistence;

internal sealed class JsonStorage(string filePath = "pos-data.json")
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public StorageData Load()
    {
        if (!File.Exists(filePath))
            return new StorageData();

        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
            return new StorageData();

        return JsonSerializer.Deserialize<StorageData>(json, Options)
            ?? new StorageData();
    }

    public void Save(StorageData data)
    {
        var json = JsonSerializer.Serialize(data, Options);
        File.WriteAllText(filePath, json);
    }
}
