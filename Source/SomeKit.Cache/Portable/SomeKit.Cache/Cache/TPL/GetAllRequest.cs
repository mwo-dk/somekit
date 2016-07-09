namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetAllRequest<T> : RequestBase
    {
        internal GetAllRequest(Record<T>[] source)
        {
            Source = source;
        }
        internal Record<T>[] Source { get; }
    }
}
