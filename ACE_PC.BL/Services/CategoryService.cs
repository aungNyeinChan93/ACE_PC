using ACE_PC.Database.Data;
using ACE_PC.Domain.Entity;
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



        //getall
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

            responseModel = ResultModel<CategoriesResponse>.Success(200, "Categories Success", data);

        skip:
            return responseModel;
        }


        //getone
        public async Task<ResultModel<CategoryResponse>> GetOneByIdAsync(int id)
        {
            var responseModel = new ResultModel<CategoryResponse>();

            var category = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category is null)
            {
                responseModel = ResultModel<CategoryResponse>.ValidationError(400, "Category Not found!");
                goto skip;
            }

            responseModel = ResultModel<CategoryResponse>.Success(200, "Category Success", new CategoryResponse { Category = category });
        skip:

            return responseModel;
        }


        // CreateCategory
        public async Task<ResultModel<CreateCategoryResponse>> CreateCategory(CreateCategoryRequest request)
        {
            var responseModel = new ResultModel<CreateCategoryResponse>();

            var newCategory = new Category { Name = request.Name };

            if (newCategory is null)
            {
                responseModel = ResultModel<CreateCategoryResponse>.ValidationError(400, "Fail Create Category");
                goto skip;
            }

            await _context.Categories.AddAsync(newCategory);
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<CreateCategoryResponse>
                .Success(201, "Category Create Success!", new CreateCategoryResponse { Name = newCategory.Name })
                : ResultModel<CreateCategoryResponse>.SystemError(500, "Create Fail");

        skip:
            return responseModel;
        }


        //Delete 
        public async Task<ResultModel<bool>> DeleteCategoryAsync(int id)
        {
            var responseModel = new ResultModel<bool>();

            var deletCategory = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (deletCategory is null)
            {
                responseModel = ResultModel<bool>.ValidationError(400, $"{id} is invalid!");
                goto skip;
            }

            _context.Categories.Remove(deletCategory);
            _context.Entry(deletCategory).State = EntityState.Deleted;
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<bool>.Success(204, "Delete Success", true)
                : ResultModel<bool>.SystemError(500,"Delete fail");

        skip:
            return responseModel;
        }

        public async Task<ResultModel<UpdateCategoryResponse>> UpdateCategory(int id, UpdateCategoryRequest request)
        {

            var responseModel = new ResultModel<UpdateCategoryResponse>();

            var updateCategory = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c=>c.CategoryId == id);

            if (updateCategory is null)
            {
                responseModel = ResultModel<UpdateCategoryResponse>.ValidationError(400, "Category Not Found!");
                goto skip;
            }

            updateCategory.Name = request.Name;
            _context.Entry(updateCategory).State = EntityState.Modified;
            var result = await  _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<UpdateCategoryResponse>
                .Success(200, "Update Success", new UpdateCategoryResponse { Category = updateCategory })
                : ResultModel<UpdateCategoryResponse>.SystemError(500, "Update Fail");

        skip:
            return responseModel;
        }
    }
}
