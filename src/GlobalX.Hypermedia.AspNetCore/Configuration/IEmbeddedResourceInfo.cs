namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public interface IEmbeddedResourceInfo
    {
        string Rel { get; }
        string OriginalPropertyName { get; }
        object GetEmbeddedResource(object model);
    }
}