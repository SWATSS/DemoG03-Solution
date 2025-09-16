namespace DemoG03.PresentationLayer.Models.Departments
{
    public class DepartmentViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly DateofCreation { get; set; }
    }
}
