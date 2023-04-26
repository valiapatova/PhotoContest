using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Dtos.Categories
{
    public class CategoryResponseDto
    {
        //TODO Do we need a constructor here ?
        public CategoryResponseDto()
        {

        }

        public CategoryResponseDto(Category category)
        { 
            Name = category.Name;
            Id = category.Id;
        }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
