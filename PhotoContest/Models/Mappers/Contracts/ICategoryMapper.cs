using PhotoContest.Dtos.Categories;
using System.Collections.Generic;

namespace PhotoContest.Models.Mappers.Contracts
{
    public interface ICategoryMapper
    {
        CategoryResponseDto ConvertCategoryToCategoryResponseDto(Category category);
        List<CategoryResponseDto> ConvertCategoriesToCategoriesResponseDto(List<Category> categories);
        Category ConvertCategoryRequestDtoToCategoryForCreate(CategoryRequestDto categoryDto);
        Category ConvertCategoryRequestDtoToCategoryForUpdate(CategoryRequestDto categoryDto, Category oldCategoryToUpdate);
        Category ConvertViewToModel(CategoryViewModel viewModel);
        CategoryViewModel ConvertModelToViewForUpdate(Category model);        
    }
}
