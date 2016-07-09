namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetItemRequest<T> : RequestBase
    {
        internal GetItemRequest(Record<T>[] source, int key)
        {
            Source = source;
            Key = key;
        }
        internal Record<T>[] Source { get; }
        internal int Key { get; }
    }
}
