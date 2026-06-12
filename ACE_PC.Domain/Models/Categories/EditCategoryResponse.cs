using ACE_PC.Domain.Dtos.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Categories
{
    public class EditCategoryResponse
    {
        public CategoryDto CategoryDto { get; set; } = new();
    }
}
