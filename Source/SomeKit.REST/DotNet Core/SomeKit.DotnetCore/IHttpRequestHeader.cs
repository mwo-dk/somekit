namespace SomeKit.REST
{
    /// <summary>
    /// Represents an HTTP request header.
    /// </summary>
    public interface IHttpRequestHeader
    {
        /// <summary>
        /// The name of the header
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The value of the header
        /// </summary>
        string Value { get; set; }
    }
}
