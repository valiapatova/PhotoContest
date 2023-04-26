using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.CategoriesRepository
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> GetByName(string categoryName);
        Task<Category> Create(Category category);
        Task<Category> Update(int id, Category category);
        Task<bool> CategoryExists(string categoryName);
    }
}
