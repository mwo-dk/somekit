namespace SomeKit
{
    /// <inheritdoc/>
    public sealed class HttpRequestHeader : IHttpRequestHeader
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Value { get; set; }
    }
}
