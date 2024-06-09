using Firebase.Auth;
using Newtonsoft.Json;
using System.ComponentModel;
using Roadside_Rescue_2.Views;
namespace Roadside_Rescue_2.ViewModel
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyD2w51i8GaaXi9ELfEu5__MjIUJCxASZOQ";
        private INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command RegisterBtn { get; }
        public Command LoginBtn { get; }
        public Command ForgotPasswordBtn { get; }

        public string UserName
        {
            get => userName; set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string UserPassword
        {
            get => userPassword; set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
            ForgotPasswordBtn = new Command(ForgotPasswordBtnTappedAsync);


            CheckIfUserAuthenticated();
        }

        private async void CheckIfUserAuthenticated()
        {
            var serializedContent = Preferences.Get("FreshFirebaseToken", null);
            if (!string.IsNullOrEmpty(serializedContent))
            {
                // User is already authenticated, navigate to home page directly
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }


        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }


        }

        private async void RegisterBtnTappedAsync(object obj)
        {
            await this._navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        private async void ForgotPasswordBtnTappedAsync(object obj)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter your email address.", "OK");
                return;
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                await authProvider.SendPasswordResetEmailAsync(UserName);
                await App.Current.MainPage.DisplayAlert("Success", "Password reset email has been sent.", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}