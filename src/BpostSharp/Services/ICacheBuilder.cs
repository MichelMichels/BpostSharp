namespace MichelMichels.BpostSharp.Services;

public interface ICacheBuilder<T> where T : class
{
    IEnumerable<T> Get();
    Task Clear();
    Task Build();

    bool HasCache();
}