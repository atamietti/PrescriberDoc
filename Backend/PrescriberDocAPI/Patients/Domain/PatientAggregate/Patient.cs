namespace PrescriberDocAPI.Patients.Domain;

public class Patient : CrudBase
{
   
    public string DoctorId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<Drug> Drugs { get; set; } = new List<Drug>();
}
