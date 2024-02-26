using PrescriberDocAPI.Patients.Domain;

namespace PrescriberDocAPI.Test
{
    public static class EntityMocks
    {
        public static Drug Drug { get; set; } = new Drug
        {
            Name = "Test Medicine 1",
            Company = "Test Company 1",
            Dosage = "Test Dosage 1",
            Success = true,
            Description = "Test Description 1",
            Message = string.Empty
        };

        public static List<Drug> Drugs { get; set; } = new List<Drug>
        {
            Drug,

            new Drug
            {
                Name = "Test Medicine 2",
                Company = "Test Company 2",
                Dosage = "Test Dosage 2",
                Success = true,
                Description = "Test Description 2",
                Message = string.Empty
            }
        };

        public static Patient Patient { get; set; } = new Patient
        {
            Name = "Test Medicine 1",
            Drugs = new List<string> { "Medicine1", "Medicine2" },
            Success = true,
            Message = string.Empty,
            DoctorId = Guid.NewGuid().ToString(),
        };

        public static List<Patient> Patients { get; set; } = new List<Patient>
        {

            Patient,

            new Patient
                    {
                    Name = "Test Medicine 1",
                    Drugs = new List<string> { "Medicine1", "Medicine2" },
                    Success = true,
                    Message = string.Empty,
                    DoctorId = Guid.NewGuid().ToString(),
            }

        };
    }
}
