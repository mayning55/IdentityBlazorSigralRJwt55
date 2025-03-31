using System.Text.Json.Serialization;

namespace DateClassLibrary.Data;

public class Organization
{
    public long Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public Organization? Porg { get; set; }
    public long? PorgId { get; set; }
    public List<Organization>? OrganizationSub { get; set; } = new List<Organization>();

}
