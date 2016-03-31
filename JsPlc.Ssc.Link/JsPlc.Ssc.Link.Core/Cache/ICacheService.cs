using System;

namespace JsPlc.Ssc.Link.Core.Cache
{
	public interface ICacheService
	{
		T GetOrSet<T>(string cacheKey, DateTime expireAt, Func<T> getItemCallback);
	}
}
