using Ivo_Nekov_employees.Server.Application.Dtos;
using Ivo_Nekov_employees.Server.Application.Interfaces;
using Ivo_Nekov_employees.Server.Domain.Enitites;

namespace Ivo_Nekov_employees.Server.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        public List<EmployeePairDto> FindAllWorkingPairs(List<Employee> employees)
        {
            var employeePairs = new List<EmployeePairDto>();

            // Group employees by project
            var projectGroups = employees.GroupBy(e => e.ProjectId);

            foreach (var projectGroup in projectGroups)
            {
                var projectEmployees = projectGroup.OrderBy(e => e.DateFrom).ToList();

                // Compare every pair in the same project
                for (int a = 0; a < projectEmployees.Count; a++)
                {
                    for (int b = a + 1; b < projectEmployees.Count; b++)
                    {
                        var emp1 = projectEmployees[a];
                        var emp2 = projectEmployees[b];

                        int overlapDays = CalculateOverlap(emp1, emp2);

                        if (overlapDays > 0) // Only store valid pairs
                        {
                            employeePairs.Add(new EmployeePairDto(emp1.EmpId, emp2.EmpId, emp1.ProjectId, overlapDays));
                        }
                    }
                }
            }

            // Sort theem by descending order
            return employeePairs.OrderByDescending(pair => pair.OverlapDays).ToList();
        }

        private int CalculateOverlap(Employee emp1, Employee emp2)
        {
            DateTime emp1EndDate = emp1.DateTo;
            DateTime emp2EndDate = emp2.DateTo;

            DateTime overlapStart = emp1.DateFrom > emp2.DateFrom ? emp1.DateFrom : emp2.DateFrom;
            DateTime overlapEnd = emp1EndDate < emp2EndDate ? emp1EndDate : emp2EndDate;

            int overlapDays = (overlapEnd - overlapStart).Days;
            return overlapDays > 0 ? overlapDays : 0; // Ensure no negative overlap
        }
    }
}
