using System.Diagnostics;
using Entitas.MatchLine;
using Performance.ViewModels;

public sealed class TestTimeService : Service, ITimeService
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();

    public TestTimeService(Contexts contexts, MainViewModel viewModel) : base(contexts, viewModel)
    {
    }

    public float DeltaTime()
    {
        return 0.1f;
    }

    public float RealtimeSinceStartup()
    {
        return (float)_sw.Elapsed.TotalSeconds;
    }
}