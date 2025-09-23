using System.ComponentModel.DataAnnotations;

namespace DemoG03.PresentationLayer.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        public string Name { get; set; }
        [Range(10, int.MaxValue)]
        public string Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly DateofCreation { get; set; }
    }
}
