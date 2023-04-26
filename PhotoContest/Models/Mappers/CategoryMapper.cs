using PhotoContest.Dtos.Categories;
using PhotoContest.Models.Mappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models.Mappers
{
    public class CategoryMapper : ICategoryMapper
    {
        public CategoryResponseDto ConvertCategoryToCategoryResponseDto(Category category)
        {
            var categoryDto = new CategoryResponseDto(category) ;
            return categoryDto;
        }

        public List<CategoryResponseDto> ConvertCategoriesToCategoriesResponseDto(List<Category> categories)
        {
            List<CategoryResponseDto> model = categories.Select(category => new CategoryResponseDto(category)).ToList();

            return model;
        }

        public Category ConvertCategoryRequestDtoToCategoryForCreate(CategoryRequestDto categoryDto)
        {
            var category = new Category() { Name = categoryDto.Name };
            return category;
        }
        public Category ConvertViewToModel(CategoryViewModel viewModel)
        {
            var category = new Category() { Name = viewModel.Name };
            return category;
        }
        public CategoryViewModel ConvertModelToViewForUpdate(Category model)
        {
            var viewModel = new CategoryViewModel() { Name = model.Name };
            return viewModel;
        }
        public Category ConvertCategoryRequestDtoToCategoryForUpdate(CategoryRequestDto categoryDto, Category oldCategoryToUpdate)
        {
            var category = oldCategoryToUpdate;
            category.Name =  categoryDto.Name ;
            return category;
        }

    }
}
