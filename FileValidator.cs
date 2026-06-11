using System.IO;

namespace md5er;

public class UnhashableFileException(string message) : Exception(message);

public static class FileValidator
{
    public static void EnsureHashable(string path)
    {
        var attr = File.GetAttributes(path);

        if (attr.HasFlag(FileAttributes.Directory))
            throw new UnhashableFileException("Cannot hash a directory.");

        if (attr.HasFlag(FileAttributes.Device))
            throw new UnhashableFileException("Cannot hash a device file.");
    }
}
