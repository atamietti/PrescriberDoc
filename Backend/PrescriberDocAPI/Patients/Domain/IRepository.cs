namespace PrescriberDocAPI.Patients.Domain
{
    public interface IRepository<T> where T : CrudBase
    {
        Task<T> Get(string propvalue, string propname = "id");
        Task<IEnumerable<T>> Get();
        Task<T> Create(T request);
        Task<T> Update(string id, T request);
        Task<T> Delete(string id);
        Task<bool> Any(string propvalue, string propname = "id");
    }
}
