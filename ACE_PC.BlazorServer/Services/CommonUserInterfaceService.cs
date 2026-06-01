using ACE_PC.BlazorServer.Components.Shared.Common;
using ACE_PC.BlazorServer.Dtos.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ACE_PC.BlazorServer.Services
{
    public class CommonUserInterfaceService
    {
        public IDialogService DialogService { get; set; }

        public CommonUserInterfaceService(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        public async Task<DialogResult> ShowDialog(DialogRequest request)
        {
            var parameters = new DialogParameters<BaseDialogComponent>
        {
            { x => x.Body, request.Body },
            { x => x.ButtonText,request.ButtonText },
            { x => x.Color, request.Color ?? Color.Default } 
        };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var result = await DialogService.ShowAsync<BaseDialogComponent>(request.ButtonText, parameters, options);
            var dialogResult = result.Result;
            var response = await dialogResult!;
            return response!;
        }
    }
}
