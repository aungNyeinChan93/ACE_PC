using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Quotes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Quotes
{
    public interface IQuoteService
    {
        Task<ResultModel<QuotesResponse>> GetAllAsync();
        Task<ResultModel<QuotesResponse>> GetAllAsync(QuotePaginationRequest request);
        Task<ResultModel<QuotesResponse>> GetAllAsync(QuotePaginationRequest paginationRequest,QuoteSearchRequest searchRequest);
        Task<ResultModel<CreateQuoteResponse>> CreateAsync(CreateQuoteRequest request);

        Task<ResultModel<UpdateQuoteResponse>> UpdateAsync(int id,UpdateQuoteRequest request);

        Task<ResultModel<DeleteQuoteResponse>> DeleteAsync(int id);

    }
}
