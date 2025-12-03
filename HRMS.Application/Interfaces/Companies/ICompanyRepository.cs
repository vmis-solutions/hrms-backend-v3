using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    // TODO: This repository should eventually inherit from IGeneric<Company>
    public interface ICompanyRepository : IGeneric<Company>
    {
        Task<Company?> FindByNameAsync(string name);
    }
} 