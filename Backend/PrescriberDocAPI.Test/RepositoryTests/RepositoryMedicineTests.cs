using MongoDB.Driver;
using Moq;
using PrescriberDocAPI.Patients.Domain;

namespace PrescriberDocAPI.Test.RepositoryTests;

public class RepositoryTests : IClassFixture<MongoFixture<Medicine>>
{
    private readonly MongoFixture<Medicine> _mongoFixture;
    public RepositoryTests(MongoFixture<Medicine> mongoFixture)
    {
        _mongoFixture = mongoFixture;
    }

    [Fact]
    public async Task GeMedicine_ValidData()
    {
     

        _mongoFixture.EntityCursor.Setup(_ => _.Current).Returns(EntityMocks.Medicines);
        var response = await _mongoFixture.Repository.Get();
        Assert.Equal(2, response.Count());

    }


    [Fact]
    public async Task InsertMedicine_ValidData()
    {
        await _mongoFixture.Repository.Create(EntityMocks.Medicine);
        _mongoFixture.EntityCollection.Verify(_ => _.InsertOneAsync(EntityMocks.Medicine, null, default), Times.Once);
    }

    [Fact]
    public async Task UpdateMedicine_ValidData()
    {
        EntityMocks.Medicine.Name = "Updated";
        await _mongoFixture.Repository.Update(EntityMocks.Medicine.Id, EntityMocks.Medicine);
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndUpdateAsync(It.IsAny<FilterDefinition<Medicine>>(), It.IsAny<UpdateDefinition<Medicine>>(), It.IsAny<FindOneAndUpdateOptions<Medicine>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task DeleteMedicine_ValidData()
    {
        await _mongoFixture.Repository.Delete(Guid.NewGuid().ToString());
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndDeleteAsync(It.IsAny<FilterDefinition<Medicine>>(), It.IsAny<FindOneAndDeleteOptions<Medicine, Medicine>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


}