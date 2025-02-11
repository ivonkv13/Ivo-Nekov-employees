using Ivo_Nekov_employees.Server.Application.Dtos;
using Ivo_Nekov_employees.Server.Domain.Enitites;

namespace Ivo_Nekov_employees.Server.Application.Interfaces
{
    public interface IEmployeeService
    {
        public List<EmployeePairDto> FindAllWorkingPairs(List<Employee> employees);
    }
}
