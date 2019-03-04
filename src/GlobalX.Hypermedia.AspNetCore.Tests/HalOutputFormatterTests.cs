using GlobalX.Hypermedia.AspNetCore;
using GlobalX.Hypermedia.AspNetCore.Formatters;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class HalOutputFormatterTests
    {

        [Test]
        public void ItShouldUrlEncodeTheUri()
        {
            var response = HalOutputFormatter.BuildDynamicLink(new Link("a", "/foo/bar fax", "t"));

            string actual = response.href.ToString();
            actual.ShouldBe("/foo/bar%20fax");
        }
    }
}