using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDB.Driver;
using Moq;
using PrescriberDocAPI.Patients.Domain;
using PrescriberDocAPI.Patients.Infrastructure;

namespace PrescriberDocAPI.Test.RepositoryTests;

public class MongoFixture<T> : IDisposable where T : CrudBase
{
    private Mock<IMongoClient> _mongoClient;
    private Mock<IMongoDatabase> _mongodb;
    public Mock<IMongoCollection<T>> EntityCollection;
    private T _domainEntity;
    private Mock<IFindFluent<T, T>> _mockFindFluent;
    private MongoDbSettings _settings;
    public Repository<T> Repository;
    public Mock<IAsyncCursor<T>> EntityCursor;

    public MongoFixture()
    {
        _settings = new MongoDbSettings() { DatabaseName = "prescribedocdb" };
        _mongoClient = new Mock<IMongoClient>();
        EntityCollection = new Mock<IMongoCollection<T>>();
        _mongodb = new Mock<IMongoDatabase>();
        EntityCursor = new Mock<IAsyncCursor<T>>();
        _mockFindFluent = new Mock<IFindFluent<T, T>>();
        _domainEntity = Activator.CreateInstance<T>();
        InitializeMongoPatientCollection();
    }

    private void InitializeMongoPatientCollection()
    {

        EntityCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        EntityCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Returns(Task.FromResult(false));

        EntityCollection.Setup(_ => _.FindAsync(It.IsAny<FilterDefinition<T>>(), It.IsAny<FindOptions<T, T>>(), It.IsAny<CancellationToken>())).ReturnsAsync(EntityCursor.Object);
        EntityCollection.Setup(_ => _.InsertOneAsync(It.IsAny<T>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        EntityCollection.Setup(_ => _.FindOneAndUpdateAsync(It.IsAny<FilterDefinition<T>>(), It.IsAny<UpdateDefinition<T>>(), It.IsAny<FindOneAndUpdateOptions<T>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_domainEntity);
        EntityCollection.Setup(_ => _.FindOneAndDeleteAsync(It.IsAny<FilterDefinition<T>>(), It.IsAny<FindOneAndDeleteOptions<T, T>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(_domainEntity));

        _mongodb.Setup(x => x.GetCollection<T>(typeof(T).Name,
         default)).Returns(EntityCollection.Object);
        _mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(),
             default)).Returns(_mongodb.Object);

        Repository = new Repository<T>(_mongoClient.Object, _settings);

    }
    public void Dispose()
    {

    }
}
