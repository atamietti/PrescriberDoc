using PrescriberDocAPI.Patients.Domain;

namespace PrescriberDocAPI.Patients.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : CrudBase
    {
        public Task<bool> Create(T request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<T>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string id, T request)
        {
            throw new NotImplementedException();
        }
    }
}
