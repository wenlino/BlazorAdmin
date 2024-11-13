using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace AdminSenyun.Data.Service;

public class MemoryCacheService : ICache
{
    private readonly IMemoryCache memoryCache;
    private readonly ISqlSugarClient db;
    private readonly ConcurrentDictionary<string, Type> keyValuePairs;


    public MemoryCacheService(IMemoryCache memoryCache, ISqlSugarClient db)
    {
        this.memoryCache = memoryCache;
        this.db = db;
        keyValuePairs = new ConcurrentDictionary<string, Type>();
    }

    /// <summary>
    /// 获取缓存服务
    /// </summary>
    public IMemoryCache MemoryCache => memoryCache;



    /// <summary>
    /// 创建条目
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ICacheEntry CreateEntry(object key)
    {
        return memoryCache.CreateEntry(key);
    }

    /// <summary>
    /// 清理全部缓存
    /// </summary>
    public void ClearAll()
    {
        foreach (var item in keyValuePairs.Keys)
        {
            memoryCache.Remove(item);
        }
        keyValuePairs.Clear();
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    public void Remove(object key)
    {
        memoryCache.Remove(key);
        keyValuePairs.TryRemove(key?.ToString() ?? "", out _);
    }

    public void RemoveContains(string name)
    {
        foreach (var item in keyValuePairs.Keys)
        {
            if (item.Contains(name))
            {
                memoryCache.Remove(item);
                keyValuePairs.TryRemove(item, out _);
            }
        }
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Remove<T>()
    {
        var key = $"Table_{typeof(T).Name}";
        memoryCache.Remove(key);
        keyValuePairs.TryRemove(key, out _);
    }

    /// <summary>
    /// 返回缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(object key, out object? value)
    {
        return memoryCache.TryGetValue(key, out value);
    }

    /// <summary>
    /// 设置缓存 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(object key, object value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        memoryCache.Set(key, value, cacheEntryOptions);
        keyValuePairs[key?.ToString() ?? ""] = value.GetType();
    }

    /// <summary>
    /// 获取数据库中的数据，存入到缓存中 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> GetAll<T>()
    {
        var key = $"Table_{typeof(T).Name}";
        keyValuePairs[key] = typeof(List<T>);
        //查看是否存在数据，存在返回，不存在在数据库中查找
        return memoryCache.GetOrCreate(key, entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return db.Queryable<T>().ToList();
        }) ?? [];
    }


    public T? GetOrCreate<T>(string? key, Func<ICacheEntry, T> factory)
    {
        keyValuePairs[key] = typeof(List<T>);
        var reslult = memoryCache.GetOrCreate(key, entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            var items = factory.Invoke(entry);
            return items;
        });
        return reslult;
    }

    public T? GetOrCreate<T>(Func<ICacheEntry, T> factory)
    {
        var key = typeof(T).Name;
        return GetOrCreate(key, factory);
    }

    public IMemoryCache GetMemoryCache()
    {
        return MemoryCache;
    }

    public List<string> GetAllCacheKeys()
    {
        return new List<string>(keyValuePairs.Keys);
    }

    public ConcurrentDictionary<string, Type> GetKeyValuePairs()
    {
        return keyValuePairs;
    }
}
