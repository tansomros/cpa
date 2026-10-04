namespace BigLion.CPA.Application.Features.Users.ViewModel;
public class UserListViewModel
{
    public ICollection<UserViewModel> Users { get; set; }

    public UserListViewModel()
    {
        Users = [];
    }
}
