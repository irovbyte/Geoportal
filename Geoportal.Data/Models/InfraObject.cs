using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Geoportal.Data.Models;

public class InfraObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public decimal AllocatedBudget { get; set; }
    public decimal SpentBudget { get; set; }
    public string DonorName { get; set; } = string.Empty;
    public int CompletionPercentage { get; set; }

    public int Capacity { get; set; }
    public int CurrentPeopleCount { get; set; }
    public bool HasInternet { get; set; }
    public bool HasDrinkingWater { get; set; }

    [Column(TypeName = "jsonb")]
    public string AdditionalDataJson { get; set; } = "{}";
}
