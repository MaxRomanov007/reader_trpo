using DotNetEnv;

namespace BookService.Utils;

public static class Images
{
    private static readonly string ImagesPath;

    static Images()
    {
        ImagesPath = Env.GetString("IMAGES_PATH");
    }

    public static string FullName(string fileName)
    {
        return Path.Combine(ImagesPath, fileName);
    }

    public static string SaveImage(string path)
    {
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        var extension = Path.GetExtension(path);
        var filename = Guid.NewGuid().ToString().Replace("-", string.Empty);
        var newPath = Path.Combine(ImagesPath, filename + extension);

        File.Copy(path, newPath);

        return filename + extension;
    }

    public static void RemoveImage(string filename)
    {
        var path = Path.Combine(ImagesPath, filename);

        if (!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }
}