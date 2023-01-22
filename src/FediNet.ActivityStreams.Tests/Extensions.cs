public static class Extensions
{
    public static string ReadToEnd(this Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }
}
