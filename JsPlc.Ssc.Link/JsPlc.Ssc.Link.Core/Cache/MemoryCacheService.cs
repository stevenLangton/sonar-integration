using System;
using System.Runtime.Caching;

namespace JsPlc.Ssc.Link.Core.Cache
{
	public class InMemoryCache : ICacheService
	{
		public T GetOrSet<T>(string cacheKey, DateTime expireAt, Func<T> getItemCallback)
		{
			var item = MemoryCache.Default.Get(cacheKey);
			if (item == null)
			{
				item = getItemCallback();
				MemoryCache.Default.Add(cacheKey, item, expireAt);
			}
			return (T)item;
		}
	}
}
