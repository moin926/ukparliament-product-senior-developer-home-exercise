﻿namespace UKParliament.CodeTest.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAsync();
    Task<Department?> GetByIdAsync(int id);
}
