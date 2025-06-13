using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thunders.TechTest.ApiService.Interfaces;

namespace Thunders.TechTest.ApiService.Services;
public class CacheProvider : ICacheProvider
{
    private readonly IMemoryCache memoryCache;
    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true
    };

    public CacheProvider(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public bool Existe<T>(T obj, TimeSpan? expiracaoAdicionar = null)
    {
        var chave = JsonSerializer.Serialize(obj, jsonSerializerOptions);

        var existe = memoryCache.TryGetValue(chave, out _);

        if (!existe && expiracaoAdicionar != null)
            Adicionar<T>(obj, (TimeSpan)expiracaoAdicionar);

        return existe;
    }

    public void Adicionar<T>(T obj, TimeSpan expiracao)
    {
        var chave = JsonSerializer.Serialize(obj, jsonSerializerOptions);

        memoryCache.Set(chave, true, expiracao);
    }

    public void Remover<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, jsonSerializerOptions);
        memoryCache.Remove(json);
    }
}
