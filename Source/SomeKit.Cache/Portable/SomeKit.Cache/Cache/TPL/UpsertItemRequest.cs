namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class UpsertItemRequest<T> : RequestBase
    {
        internal UpsertItemRequest(Record<T>[] source, int key, T value)
        {
            Source = source;
            Key = key;
            Value = value;
        }

        internal Record<T>[] Source { get; }
        internal int Key { get; }
        internal T Value { get; }
    }
}
