using System.ComponentModel.DataAnnotations;

namespace StructuralDashboard.Shared.Entities;

public class Project
{
    [Key]
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = null!; 
}
