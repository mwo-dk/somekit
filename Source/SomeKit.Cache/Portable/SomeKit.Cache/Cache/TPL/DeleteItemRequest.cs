namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class DeleteItemRequest<T> : RequestBase
    {
        internal DeleteItemRequest(Record<T>[] source, int key)
        {
            Source = source;
            Key = key;
        }
        internal Record<T>[] Source { get; }
        internal int Key { get; }
    }
}
