namespace DemoG03.PresentationLayer.ViewModels.Roles
{
    public class RolesViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string>? Users { get; set; }
    }
}
