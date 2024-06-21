using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository(RepositoryContext repositoryContext) : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public async Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges) => await FindAll(trackChanges).OrderBy(e => e.Name).ToListAsync();

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(e => ids.Contains(e.Id), trackChanges).ToListAsync();

    public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges) => await FindByCondition(e => e.Id.Equals(companyId), trackChanges).FirstOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);
}
