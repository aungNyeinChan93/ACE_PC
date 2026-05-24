using ACE_PC.Database.Data;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Categories;
using ACE_PC.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace ACE_PC.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<CategoriesResponse>> GetAllAsync()
        {
            var responseModel = new ResultModel<CategoriesResponse>();

            var categories = await _context.Categories.AsNoTracking()
                .ToListAsync();

            if (categories is null || categories.Count <= 0)
            {
                responseModel = ResultModel<CategoriesResponse>.ValidationError(400, "Category Not found!");
                goto skip;
            }

            var data = new CategoriesResponse
            {
                Categoreis = categories,
            };

            responseModel = ResultModel<CategoriesResponse>.Success(200,"Categories Success",data);

        skip:
            return responseModel;
        }

        public async Task<ResultModel<CategoryResponse>> GetOneByIdAsync(int id)
        {
            var responseModel = new ResultModel<CategoryResponse>();

            var category = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c=>c.CategoryId == id);

            if (category is null )
            {
                responseModel = ResultModel<CategoryResponse>.ValidationError(400, "Category Not found!");
                goto skip;
            }

            responseModel = ResultModel<CategoryResponse>.Success(200,"Category Success", new CategoryResponse { Category = category});
        skip:

            return responseModel;
        }
    }
}
