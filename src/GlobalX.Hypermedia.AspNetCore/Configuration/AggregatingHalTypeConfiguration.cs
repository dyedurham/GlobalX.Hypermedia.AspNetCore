using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public class AggregatingHalTypeConfiguration : IHalTypeConfiguration
    {
        private readonly IEnumerable<IHalTypeConfiguration> _delegates;

        public AggregatingHalTypeConfiguration(IEnumerable<IHalTypeConfiguration> delegates)
        {
            _delegates = delegates.Where(el => el != null);
        }

        public IEnumerable<Link> LinksFor(object model, HttpContext context)
        {
            return _delegates.SelectMany(c => c.LinksFor(model, context));
        }

        public IEnumerable<IEmbeddedResourceInfo> EmbedsFor(object model, HttpContext context)
        {
            return _delegates.SelectMany(c => c.EmbedsFor(model, context));
        }

        public IEnumerable<string> Ignored()
        {
            return _delegates.SelectMany(c => c.Ignored());
        }
    }
}