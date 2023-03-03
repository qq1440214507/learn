namespace File;

public interface IFileSystem
{
    void ShowStructure(Action<int, string> print);
    Task<string> ReadAllTextAsync(string path);
}