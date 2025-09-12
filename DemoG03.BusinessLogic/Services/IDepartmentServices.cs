using DemoG03.BusinessLogic.DataTransferObjects;

namespace DemoG03.BusinessLogic.Services
{
    public interface IDepartmentServices
    {
        int AddDepartment(CreatedDepartmentDto departmentDto);
        bool DeleteDepartment(int id);
        IEnumerable<DepartmentDto> GetAllDepartments();
        DepartmentDetailsDto? GetDepartmentById(int id);
        int UpdateDepartment(UpdatedDepartmentDto departmentDto);
    }
}