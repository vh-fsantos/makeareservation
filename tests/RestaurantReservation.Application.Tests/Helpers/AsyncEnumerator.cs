namespace RestaurantReservation.Application.Tests.Helpers;

public class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _enumerator;

    public AsyncEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
    }

    public T Current 
        => _enumerator.Current;

    public ValueTask<bool> MoveNextAsync() 
        => new(Task.FromResult(_enumerator.MoveNext()));

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return new ValueTask();
    }
}