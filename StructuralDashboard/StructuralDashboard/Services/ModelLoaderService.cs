namespace StructuralDashboard.Web.Services;

public static class ModelLoaderService
{
    public static StructuralModel LoadFromFile(string path)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var jsonString = File.ReadAllText(path);
        var model = JsonSerializer.Deserialize<StructuralModel>(jsonString, options);

        return model;
    }
}
