﻿using System;
using System.Collections.Concurrent;

namespace GlobalX.Hypermedia.AspNetCore.Configuration
{
    public class HalConfiguration : IProvideHalTypeConfiguration
    {
        private readonly ConcurrentDictionary<Type, IHalTypeConfiguration> cache = new ConcurrentDictionary<Type, IHalTypeConfiguration>();

        public IHalTypeConfiguration GetTypeConfiguration(Type type)
        {
            IHalTypeConfiguration config;
            cache.TryGetValue(type, out config);
            return config;
        }

        public IHalTypeConfiguration GetTypeConfiguration<T>()
        {
            return GetTypeConfiguration(typeof(T));
        }

        public HalTypeConfiguration<T> For<T>()
        {
            var config = new HalTypeConfiguration<T>();
            cache.TryAdd(typeof(T), config);
            return config;
        }

    }
}