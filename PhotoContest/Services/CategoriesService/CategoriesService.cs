using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Repositories.CategoriesRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Services.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        private const string MODIFY_CATEGORY_ERROR_MESSAGE = "You can not create, update or delete a category!";
        private const string VIEW_CATEGORY_ERROR_MESSAGE = "You are not authorized to view  category.";

        private readonly ICategoriesRepository categoriesRepository;
        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<List<Category>> GetAll()
        {
            return await this.categoriesRepository.GetAll();
        }

            public async Task<List<Category>> GetAll(User userLog)
        {
            if (!(userLog.Role.Name == "junkie" || userLog.Role.Name == "admin"))                                                                                                 
            {
                throw new UnauthorizedException(VIEW_CATEGORY_ERROR_MESSAGE);
            }
            return await this.categoriesRepository.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            try
            {
                return await this.categoriesRepository.GetById(id);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<Category> GetByName(string name)
        {
            try
            {
                return await this.categoriesRepository.GetByName(name);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<Category> Create(Category category, User userLog)
        {
            if (userLog.Role.Name != "admin")            
            {
                throw new UnauthorizedException(MODIFY_CATEGORY_ERROR_MESSAGE);
            }

            bool duplicateExists = true;

            try
            {
                await this.categoriesRepository.GetByName(category.Name);
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Category with {category.Name} already exists  ");
            }

            var createdCategory = await this.categoriesRepository.Create(category);

            return createdCategory;
        }

        public async Task<Category> Update(int id, Category category, User userLog)
        {

            if (!(userLog.Role.Name == "admin"))            
            {
                throw new UnauthorizedException(MODIFY_CATEGORY_ERROR_MESSAGE);
            }

            bool duplicateExists = true;
            try
            {
                var existingCategory = await this.categoriesRepository.GetByName(category.Name);
                if (existingCategory.Id == category.Id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException("Category Name exists in another Id   ");
            }

            var updatedCategory = this.categoriesRepository.Update(id, category);
            return await updatedCategory;
        }

    }
}
