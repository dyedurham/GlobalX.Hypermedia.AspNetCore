using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public interface IHalTypeConfiguration
    {
        IEnumerable<Link> LinksFor(object model, HttpContext context);
        IEnumerable<IEmbeddedResourceInfo> EmbedsFor(object model, HttpContext context);
        IEnumerable<string> Ignored();
    }
}