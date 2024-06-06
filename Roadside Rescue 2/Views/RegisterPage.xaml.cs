using Roadside_Rescue_2.ViewModel;
namespace Roadside_Rescue_2.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterViewModel(Navigation);
    }
}