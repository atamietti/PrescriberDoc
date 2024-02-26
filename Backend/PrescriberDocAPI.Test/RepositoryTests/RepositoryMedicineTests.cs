using MongoDB.Driver;
using Moq;
using PrescriberDocAPI.Patients.Domain;

namespace PrescriberDocAPI.Test.RepositoryTests;

public class RepositoryTests : IClassFixture<MongoFixture<Drug>>
{
    private readonly MongoFixture<Drug> _mongoFixture;
    public RepositoryTests(MongoFixture<Drug> mongoFixture)
    {
        _mongoFixture = mongoFixture;
    }

    [Fact]
    public async Task GeDrug_ValidData()
    {
        _mongoFixture.EntityCursor.Setup(_ => _.Current).Returns(EntityMocks.Drugs);
        var response = await _mongoFixture.Repository.Get();
        Assert.Equal(2, response.Count());
    }


    [Fact]
    public async Task InsertDrug_ValidData()
    {
        await _mongoFixture.Repository.Create(EntityMocks.Drug);
        _mongoFixture.EntityCollection.Verify(_ => _.InsertOneAsync(EntityMocks.Drug, null, default), Times.Once);
    }

    [Fact]
    public async Task UpdateDrug_ValidData()
    {
        EntityMocks.Drug.Name = "Updated";
        await _mongoFixture.Repository.Update(EntityMocks.Drug.Id, EntityMocks.Drug);
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndUpdateAsync(It.IsAny<FilterDefinition<Drug>>(), It.IsAny<UpdateDefinition<Drug>>(), It.IsAny<FindOneAndUpdateOptions<Drug>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task DeleteDrug_ValidData()
    {
        await _mongoFixture.Repository.Delete(Guid.NewGuid().ToString());
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndDeleteAsync(It.IsAny<FilterDefinition<Drug>>(), It.IsAny<FindOneAndDeleteOptions<Drug, Drug>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


}