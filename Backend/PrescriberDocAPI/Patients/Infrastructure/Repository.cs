using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using PrescriberDocAPI.Patients.Domain;
using PrescriberDocAPI.Shared.Domain;

namespace PrescriberDocAPI.Patients.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : CrudBase
    {
        private readonly IMongoCollection<T> _colection;

        public Repository(UserConfig userConfig)
        {
            var mongoClient = new MongoClient(userConfig.ConnectionStrinfg);
            var mongoDatabase = mongoClient.GetDatabase(userConfig.DatabaseName);
            _colection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public Repository(IMongoClient mongoClient, UserConfig userConfig)
        {

            var mongoDatabase = mongoClient.GetDatabase(userConfig.DatabaseName);
            _colection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public Repository(IMongoClient mongoClient, MongoDbSettings dbSegtings)
        {

            var mongoDatabase = mongoClient.GetDatabase(dbSegtings.DatabaseName);
            _colection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> Create(T request)
        {
            try
            {
                await _colection.InsertOneAsync(request);

                if (!string.IsNullOrWhiteSpace(request.Id))
                    return request;

                return CrudBase.CreateErrorMessage<T>($"Cannot create {typeof(T).Name}");

            }
            catch (Exception ex)
            {
                return CrudBase.CreateErrorMessage<T>($"Cannot create {typeof(T).Name}", ex);
            }
        }

        public async Task<T> Delete(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var response = await _colection.FindOneAndDeleteAsync(filter);

                if (!string.IsNullOrWhiteSpace(response?.Id))
                    return response;

                return CrudBase.CreateErrorMessage<T>($"Cannot delete {typeof(T).Name} {id}");

            }
            catch (Exception ex)
            {
                return CrudBase.CreateErrorMessage<T>($"Cannot get {typeof(T).Name} {id}", ex);
            }
        }

        public async Task<IEnumerable<T>> Get()
        {
            try
            {
                var r = await _colection.FindAsync(new BsonDocument(), null, default(CancellationToken));
                var response = await r.ToListAsync();

                if (response?.Any() ?? false)
                    return response;

                return new List<T> { };

            }
            catch (Exception)
            {
                return new List<T> { };
            }
        }

        public async Task<T> Get(string propvalue, string propname = "Id")
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(propname, propvalue);
                var response = await _colection.Find(filter).FirstOrDefaultAsync();

                if (!string.IsNullOrWhiteSpace(response?.Id))
                    return response;

                return CrudBase.CreateErrorMessage<T>($"Cannot get {typeof(T).Name}");

            }
            catch (Exception ex)
            {
                return CrudBase.CreateErrorMessage<T>($"Cannot get {typeof(T).Name}", ex);
            }
        }

        public async Task<T> Update(string id, T request)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(f => f.Id, id);
                request.Id = id;
                var update = Builders<T>.Update
                    .Set(f => f, request);
                var options = new FindOneAndUpdateOptions<T>
                {
                    ReturnDocument = ReturnDocument.After
                };
                var response = await _colection.FindOneAndUpdateAsync(filter, update, options);
                if (!string.IsNullOrWhiteSpace(response.Id))
                    return response;

                return CrudBase.CreateErrorMessage<T>($"Cannot update {typeof(T).Name}");

            }
            catch (Exception ex)
            {
                return CrudBase.CreateErrorMessage<T>($"Cannot update {typeof(T).Name}", ex);
            }
        }

        public async Task<bool> Any(string propvalue, string propname = "Id")
        {
            var response = await Get(propvalue, propname);
            return !string.IsNullOrWhiteSpace(response.Id);
        }
    }
}
