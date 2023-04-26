using Microsoft.EntityFrameworkCore;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.CategoriesRepository
{
    public class CategoriesRepository : ICategoriesRepository       
    {
        private readonly DataContext context;
        public CategoriesRepository(DataContext context)
        {
            this.context = context;
        }

        private IQueryable<Category> CategoryQuery
        {
            get
            {
                return this.context.Categories
                    .Include(c => c.Constests);
            }
        }

        public async Task<List<Category>> GetAll()
        {
            return await this.CategoryQuery.ToListAsync();  
        }


        public async Task<Category> GetById(int id)
        {
            var category = await this.CategoryQuery.Where(category => category.Id == id ).FirstOrDefaultAsync();
            return category ?? throw new EntityNotFoundException("Category not found!");
        }

        public async Task<Category> GetByName(string categoryName)
        {
            var category = await context.Categories.Where(c => c.Name == categoryName).FirstOrDefaultAsync();     
            return category ?? throw new EntityNotFoundException($"Category with name {categoryName} not found!");
        }

        public async Task<Category> Create(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> Update(int id, Category category)
        {
            var categoryToUpdate = await this.GetById(id);
            categoryToUpdate.Name = category.Name;                       
            await context.SaveChangesAsync();
            return await this.GetById(id);

        }

        public async Task<bool> CategoryExists(string categoryName)
        {
            var IsExistsCategory = await this.context.Categories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());                                                                                                                               
            return IsExistsCategory;
        }

    }
}
