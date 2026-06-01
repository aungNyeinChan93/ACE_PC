using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ACE_PC.BlazorServer.Dtos.Common
{
    public class DialogRequest
    {
        [Parameter]
        public string Body { get; set; } = string.Empty;

        [Parameter]
        public string? ButtonText { get; set; }

        [Parameter]
        public Color? Color { get; set; }
    }
}
