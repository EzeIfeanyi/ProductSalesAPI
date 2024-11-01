using ProductSalesAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces;

public interface IUserRepository
{
    Task<int> AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
}
