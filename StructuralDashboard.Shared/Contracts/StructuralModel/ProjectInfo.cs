using System;

namespace StructuralDashboard.Shared.Contracts
{
    public class ProjectInfo
    {
        public string ModelName { get; set; }   
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string SavedBy { get; set; }
        public string ProjectId { get; set; }
        public string SchemaVersion { get; set; }
    }
}