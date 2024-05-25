using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository(RepositoryContext repositoryContext) : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) => FindAll(trackChanges).OrderBy(e => e.Name).ToList();

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) => FindByCondition(e => ids.Contains(e.Id), trackChanges).ToList();

    public Company? GetCompany(Guid companyId, bool trackChanges) => FindByCondition(e => e.Id.Equals(companyId), trackChanges).FirstOrDefault();

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);
}
