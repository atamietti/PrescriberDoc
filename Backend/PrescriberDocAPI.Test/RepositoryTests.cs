using MongoDB.Driver;
using Moq;
using PrescriberDocAPI.Patients.Domain;
using PrescriberDocAPI.Patients.Infrastructure;
using PrescriberDocAPI.Shared.Domain;

namespace PrescriberDocAPI.Test
{
    public class RepositoryTests
    {
        Mock<IMongoCollection<Medicine>> drugkCollection = new();

        Mock<IAsyncCursor<Medicine>> drugCursor = new();

        [Fact]
        public async Task GetMedicines_AddsEntityToDatabase()
        {
            var userConfig = new UserConfig
            {
                DatabaseName = "prescribedocdb"
            };
            // Arrange
            var medicines = new List<Medicine>
            {
                new Medicine {
                    Name = "Test Medicine 1",
                    Company = "Test Company 1",
                    Dosage = "Test Dosage 1",
                    Success = true,
                    Description = "Test Description 1",
                    Message = string.Empty
                    },
                 new Medicine {
                    Name = "Test Medicine 2",
                    Company = "Test Company 2",
                    Dosage = "Test Dosage 2",
                    Success = true,
                    Description = "Test Description 2",
                    Message = string.Empty
                }
            };

            drugCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                   .Returns(true)
                   .Returns(false);

            drugCursor.Setup(_ => _.Current).Returns(medicines);

            drugkCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<Medicine>>(),
                                        It.IsAny<FindOptions<Medicine, Medicine>>(),
                                            It.IsAny<CancellationToken>()))
                 .ReturnsAsync(drugCursor.Object);


            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Medicine>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(drugkCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                .Returns(mockDatabase.Object);

            var userService = new Repository<Medicine>(mockClient.Object, userConfig);

            // Act
            var result = await userService.Get();

            mockClient.Verify();
        }

        [Fact]
        public void InsertMedice_CallsInsertOneOnCollection()
        {
            var userConfig = new UserConfig
            {
                DatabaseName = "prescribedocdb"
            };
            // Arrange
            var mockCollection = new Mock<IMongoCollection<Medicine>>();
            var userToInsert = new Medicine
            {
                Name = "Test Medicine 1",
                Company = "Test Company 1",
                Dosage = "Test Dosage 1",
                Success = true,
                Description = "Test Description 1",
                Message = string.Empty
            };

            mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<Medicine>(), null, default(CancellationToken)))
                          .Verifiable("InsertOne was not called on collection with the correct Medicine.");

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Medicine>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                        .Returns(mockCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                      .Returns(mockDatabase.Object);

            var userService = new Repository<Medicine>(mockClient.Object, userConfig);

            // Act
            userService.Create(userToInsert);

            // Assert
            mockCollection.Verify(); // Verifies that InsertOne was called according to the setup.
        }
    }
}