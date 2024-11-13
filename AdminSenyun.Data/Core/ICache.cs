using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace AdminSenyun.Data.Core;

public interface ICache
{
    /// <summary>
    /// 获取缓存实例
    /// </summary>
    /// <returns></returns>
    IMemoryCache GetMemoryCache();

    /// <summary>
    /// 创建条目
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ICacheEntry CreateEntry(object key);

    /// <summary>
    /// 清理所有缓存
    /// </summary>
    void ClearAll();

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    void Remove(object key);

    /// <summary>
    /// 删除包含 指定字符的全部缓存
    /// </summary>
    /// <param name="name"></param>
    void RemoveContains(string name);

    /// <summary>
    /// 删除实例缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Remove<T>();

    /// <summary>
    /// 返回缓存数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGetValue(object key, out object? value);

    /// <summary>
    /// 设置缓存 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set(object key, object value);


    /// <summary>
    /// 获取数据库中的数据，存入到缓存中 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<T> GetAll<T>();

    /// <summary>
    /// 获取或者创建数据缓存 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ikey"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    T? GetOrCreate<T>(string? ikey, Func<ICacheEntry, T> factory);

    /// <summary>
    /// 获取或者创建数据缓存 滑动过期 5分钟 绝对过期1小时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <returns></returns>
    T? GetOrCreate<T>(Func<ICacheEntry, T> factory);
    /// <summary>
    /// 获取缓存列表
    /// </summary>
    /// <returns></returns>
    List<string> GetAllCacheKeys();

    /// <summary>
    /// 获取缓存列表
    /// </summary>
    /// <returns></returns>
    ConcurrentDictionary<string, Type> GetKeyValuePairs();
}
