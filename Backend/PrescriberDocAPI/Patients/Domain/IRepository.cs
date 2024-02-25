namespace PrescriberDocAPI.Patients.Domain
{
    public interface IRepository<T> where T : CrudBase
    {
        Task<T> Get(string id);
        Task<IEnumerable<T>> Get();
        Task<bool> Create(T request);
        Task<bool> Update(string id, T request);
        Task<bool> Delete(string id);
    }
}
