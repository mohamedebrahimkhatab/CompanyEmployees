﻿using Entities.Models;

namespace Contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    Company? GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
}
