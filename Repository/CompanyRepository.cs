﻿using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository(RepositoryContext repositoryContext) : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) => FindAll(trackChanges).OrderBy(e => e.Name).ToList();

    public Company? GetCompany(Guid companyId, bool trackChanges) => FindByCondition(e => e.Id.Equals(companyId), trackChanges).FirstOrDefault();
}