using BrainzParentsPortal.Helpers;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BrainzParentsPortal.Pages.Members;

public partial class ReadMembersPage
{
    [Inject] private IDialogService DialogService { get; set; }
    [CascadingParameter]    private Task<AuthenticationState> AuthenticationState { get; set; }

    private bool dense = false;
    private bool hover = true;
    private bool striped = false;
    private bool bordered = false;
    private string searchString1 = "";
    private Member selectedItem1 = null;
    private HashSet<Member> selectedItems = new HashSet<Member>();
    private IEnumerable<Member> PortalUsers = new List<Member>();

    bool IsProgress { get; set; } = false;
    public void StopProgressBar()
    {
        IsProgress = false;
        StateHasChanged();
    }

    public async void StartProgressBar()
    {
        if (!IsProgress) { return; }

        IsProgress = true;
        StateHasChanged();
        await Task.Delay(100);
    }

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnParametersSetAsync()
    {
        IsProgress = true;
        await Task.Delay(1000);

        var authState = await AuthenticationState;
        if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
        {
            NavManager.NavigateTo($"{NavManager.BaseUri}login", true);

            return;
        }

        IPortalDbMemberService portalDbUserService = new PortalDbMemberService(PortalDbConnectionSettings);
        PortalUsers = portalDbUserService.GetAllMembers();

        IsProgress = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

        }
    }


    private bool FilterFunc1(Member element) => FilterFunc(element, searchString1);
    private bool FilterFunc(Member element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if ($"{element.Email} {element.Phone} ".Contains(searchString))
            return true;
        return false;
    }

    private async void OnRegisterUser()
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}AddMember", true);
    }

    private async void OnUpdateUser(Member portalUser)
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}UpdateMember/{portalUser.MemberID}", true);
    }

    private async Task OnDeleteUser(string email)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", $"A user ({email}) is going to be deleted. It can not be undone!", yesText: "Delete", cancelText: "Cancel");

        if (result != null && result == true)
        {

        }

    }
}