using System;

namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public interface IProvideHalTypeConfiguration
    {
        IHalTypeConfiguration GetTypeConfiguration(Type type);
        IHalTypeConfiguration GetTypeConfiguration<T>();
    }
}