using Microsoft.Extensions.Caching.Memory;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMemoryCache _cache;
    private readonly string _cacheKey = "DepartmentsCache";
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

    public DepartmentService(IDepartmentRepository departmentRepository, IMemoryCache cache)
    {
        _departmentRepository = departmentRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        if (!_cache.TryGetValue(_cacheKey, out IEnumerable<Department> departments))
        {
            departments = await _departmentRepository.GetAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheExpiration);

            _cache.Set(_cacheKey, departments, cacheEntryOptions);
        }

        return departments;
    }

    public async Task<Department?> GetDepartmentAsync(int id) =>
        await _departmentRepository.GetByIdAsync(id);
}