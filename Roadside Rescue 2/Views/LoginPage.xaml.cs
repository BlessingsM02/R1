using Roadside_Rescue_2.ViewModel;

namespace Roadside_Rescue_2.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel(Navigation);
    }
    private void OnShowPasswordCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        passwordEntry.IsPassword = !e.Value;
    }
}