namespace BookService.Database;

public static class DbInitializer
{
    private static bool _isInitialized;
    private static readonly object Lock = new();

    public static async Task InitializeAsync()
    {
        lock (Lock)
        {
            if (_isInitialized) return;
        }

        if (_isInitialized) return;

        await Task.Run(async () =>
        {
            try
            {
                await using var context = new BooksContext();

                _ = await context.Database.CanConnectAsync();

                lock (Lock)
                {
                    _isInitialized = true;
                }

                Console.WriteLine("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB initialization failed: {ex.Message}");
            }
        }).ConfigureAwait(false);
    }
}