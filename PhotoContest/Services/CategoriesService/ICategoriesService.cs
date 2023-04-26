using PhotoContest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<List<Category>> GetAll();
        Task<List<Category>> GetAll(User userLog);
        Task<Category> GetById(int id);
        Task<Category> GetByName(string name);
        Task<Category> Create(Category category, User userLog);
        Task<Category> Update(int id, Category category, User userLog);
    }
}
