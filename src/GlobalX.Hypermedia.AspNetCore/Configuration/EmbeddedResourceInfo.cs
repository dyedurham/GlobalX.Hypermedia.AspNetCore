using System;

namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public class EmbeddedResourceInfo<TModel> : IEmbeddedResourceInfo
    {
        private readonly Func<TModel, dynamic> getter;

        internal EmbeddedResourceInfo(string rel, string propertyName, Func<TModel, dynamic> getter)
        {
            Rel = rel;
            OriginalPropertyName = propertyName;
            this.getter = getter;
        }

        public string Rel { get; private set; }

        public string OriginalPropertyName { get; private set; }

        public object GetEmbeddedResource(object model)
        {
            return getter((TModel)model);
        }
    }
}