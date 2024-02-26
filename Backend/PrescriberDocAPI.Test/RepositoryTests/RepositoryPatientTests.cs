using MongoDB.Driver;
using Moq;
using PrescriberDocAPI.Patients.Domain;

namespace PrescriberDocAPI.Test.RepositoryTests;

public class RepositoryPatientTests : IClassFixture<MongoFixture<Patient>>
{
    private readonly MongoFixture<Patient> _mongoFixture;
    public RepositoryPatientTests(MongoFixture<Patient> mongoFixture)
    {
        _mongoFixture = mongoFixture;

       
    }
    [Fact]
    public async Task GePatient_ValidData()
    {

        _mongoFixture.EntityCursor.Setup(_ => _.Current).Returns(EntityMocks.Patients);
        var response = await _mongoFixture.Repository.Get();
        Assert.Equal(2, response.Count());

    }


    [Fact]
    public async Task InsertPatient_ValidData()
    {
        await _mongoFixture.Repository.Create(EntityMocks.Patient);
        _mongoFixture.EntityCollection.Verify(_ => _.InsertOneAsync(EntityMocks.Patient, null, default), Times.Once);
    }

    [Fact]
    public async Task UpdatePatient_ValidData()
    {
        EntityMocks.Patient.Name = "Updated";
        await _mongoFixture.Repository.Update(EntityMocks.Patient.Id, EntityMocks.Patient);
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndUpdateAsync(It.IsAny<FilterDefinition<Patient>>(), It.IsAny<UpdateDefinition<Patient>>(), It.IsAny<FindOneAndUpdateOptions<Patient>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task DeletePatient_ValidData()
    {
        await _mongoFixture.Repository.Delete(Guid.NewGuid().ToString());
        _mongoFixture.EntityCollection.Verify(_ => _.FindOneAndDeleteAsync(It.IsAny<FilterDefinition<Patient>>(), It.IsAny<FindOneAndDeleteOptions<Patient, Patient>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
