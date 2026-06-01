using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Helpers.BaseModel
{
    public abstract class BasePaginationRequest
    {
        public int PageCount { get; set; } = 10;

        public int PageNumber { get; set; } = 1;
    }
}
