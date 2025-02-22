using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.App.Pages.Identity
{
    public partial class RegisterPage : ComponentBase
    {
        #region Dependecies
        [Inject]
        //Service that provides methods to show snackbar messages
        public ISnackbar Snackbar { get; set; } = null!;


        [Inject] //Decorator that injects a service into a Blazor component
        public IAccountHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        #endregion

        #region Properties
        public bool IsBusy { get; set; } = false;
        public RegisterRequest InputModel { get; set; } = new();
        #endregion

        #region Overrrides
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
                Navigation.NavigateTo("/");
        }
        #endregion

        #region Methods
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await Handler.RegisterAsync(InputModel);

                if (result.Sucess)
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    Navigation.NavigateTo("/login");
                }
                else
                    Snackbar.Add(result.Message, Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Success);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
