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
}