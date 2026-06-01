using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Helpers.BaseModel
{

    public class BaseTitle
    {
        public string Title { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public string Icon { get; set; } = "bi bi-file-text";

    }

}

