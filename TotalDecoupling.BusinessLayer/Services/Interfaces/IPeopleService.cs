using TotalDecoupling.BusinessLayer.Models;
using TotalDecoupling.Shared.Models;

namespace TotalDecoupling.BusinessLayer.Services.Interfaces;

public interface IPeopleService
{
    Task<OperationResult<IEnumerable<Person>>> GetAsync();

    Task<OperationResult<Person>> GetAsync(Guid id);

    Task<OperationResult> DeleteAsync(Guid id);

    Task<OperationResult> SaveAsync(Person person);
}
