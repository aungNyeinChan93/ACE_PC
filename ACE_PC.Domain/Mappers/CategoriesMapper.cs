using ACE_PC.Domain.Dtos.Categories;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Mappers
{
    public static class CategoriesMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }
    }
}
