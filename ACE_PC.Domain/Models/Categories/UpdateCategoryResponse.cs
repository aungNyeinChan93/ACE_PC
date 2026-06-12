using ACE_PC.Domain.Dtos.Categories;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Categories
{
    public class UpdateCategoryResponse
    {
        public CategoryDto? Category { get; set; }
    }
}
