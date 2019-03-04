using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.Hypermedia.AspNetCore.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GlobalX.Hypermedia.AspNetCore.Formatters
{
    public class HalOutputFormatter: TextOutputFormatter
    {
        private IProvideHalTypeConfiguration _configuration;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public HalOutputFormatter()
        {
            SupportedMediaTypes.Add("application/json+hal");

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            _configuration = serviceProvider.GetService(typeof(IProvideHalTypeConfiguration)) as IProvideHalTypeConfiguration;

            var hal = BuildHypermedia(context.Object, context.HttpContext);
            
            var response = context.HttpContext.Response;
            
            string jsonOfTest = JsonConvert.SerializeObject(hal, _jsonSerializerSettings);
            return response.WriteAsync(jsonOfTest);
        }
        
        internal dynamic BuildHypermedia(object model, HttpContext context)
        {
            if (model == null) return null;

            if (model is IEnumerable)
            {
                //how to handle a collection at the root resource level?
                return ((IEnumerable)model).Cast<object>().Select(x => BuildHypermedia(x, context));
            }

            IDictionary<string, object> halModel = model.ToDynamic();
            var globalTypeConfig = _configuration.GetTypeConfiguration(model.GetType());
            var localTypeConfig = context.LocalHalConfig().GetTypeConfiguration(model.GetType());

            var typeConfig = new AggregatingHalTypeConfiguration(new List<IHalTypeConfiguration> { globalTypeConfig, localTypeConfig });

            var links = typeConfig.LinksFor(model, context).ToArray();
            if (links.Any())
                halModel["_links"] = links.GroupBy(l => l.Rel).ToDictionary(grp => grp.Key, grp => BuildDynamicLinksOrLink(grp));

            var embeddedResources = typeConfig.EmbedsFor(model, context).ToArray();
            if (embeddedResources.Any())
            {
                // Remove original objects from the model (if they exist)
                foreach (var embedded in embeddedResources)
                    halModel.Remove(embedded.OriginalPropertyName);
                halModel["_embedded"] = embeddedResources.ToDictionary(info => info.Rel, info => BuildHypermedia(info.GetEmbeddedResource(model), context));
            }

            var ignoredProperties = typeConfig.Ignored().ToArray();
            if (ignoredProperties.Any())
            {
                //remove ignored properties from the output
                foreach (var ignored in ignoredProperties) halModel.Remove(ignored);
            }
            return halModel;
        }

        internal static dynamic BuildDynamicLinksOrLink(IEnumerable<Link> grp)
        {
            return grp.Count()>1 ? grp.Select(l=>BuildDynamicLink(l)) : BuildDynamicLink(grp.First());
        }

        internal static dynamic BuildDynamicLink(Link link)
        {
            dynamic dynamicLink = new ExpandoObject();
            dynamicLink.href = Uri.EscapeUriString(link.Href);
            if (link.IsTemplated) dynamicLink.templated = true;
            if (!string.IsNullOrEmpty(link.Title)) dynamicLink.title = link.Title;
            return dynamicLink;
        }
    }
}