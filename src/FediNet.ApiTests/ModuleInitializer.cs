using System.Runtime.CompilerServices;
using VerifyTests.Http;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        UseProjectRelativeDirectory("Snapshots");

        VerifyHttp.Initialize();
#if NET5_0_OR_GREATER
        HttpRecording.StartRecording();
#endif
    }
}
