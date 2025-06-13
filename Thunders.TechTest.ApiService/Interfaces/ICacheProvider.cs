namespace Thunders.TechTest.ApiService.Interfaces;
public interface ICacheProvider
{
    bool Existe<T>(T Obj, TimeSpan? expiracaoAdicionar = null);
    void Adicionar<T>(T obj, TimeSpan expiracao);
    void Remover<T>(T obj);
}
