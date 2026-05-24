using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Categories
{
    public interface ICategoryService
    {
        Task<ResultModel<CategoriesResponse>> GetAllAsync();

        Task<ResultModel<CategoryResponse>> GetOneByIdAsync(int id);
    }
}
