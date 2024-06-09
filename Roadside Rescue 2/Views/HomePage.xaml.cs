using Newtonsoft.Json;
using Roadside_Rescue_2.ViewModel;


namespace Roadside_Rescue_2.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly FirebaseService _firebaseService;
        private CancellationTokenSource _cancellationTokenSource;

        public HomePage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseService();
        }

        private async void OnWorkingToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                // User is working, start sending location
                _cancellationTokenSource = new CancellationTokenSource();
                await StartSendingLocationAsync(_cancellationTokenSource.Token);
            }
            else
            {
                // User is not working, stop sending location and delete existing location
                _cancellationTokenSource?.Cancel();
                await DeleteLocationAsync();
            }
        }

        private async Task StartSendingLocationAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await GetAndSendLocationAsync();
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
            }
            catch (TaskCanceledException)
            {
                // Task was canceled
            }
        }

        private async Task GetAndSendLocationAsync()
        {
            try
            {
                // Get the current location
                var geolocationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(20));
                var location = await Geolocation.GetLocationAsync(geolocationRequest);

                // Retrieve the user email from the stored Firebase token
                var userInfo = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FreshFirebaseToken", ""));
                string userEmail = userInfo?.User?.Email ?? "Unknown";

                if (location != null)
                {
                    LocationLabel.Text = $"Email: {userEmail}, Latitude: {location.Latitude}, Longitude: {location.Longitude}";

                    // Send coordinates to Firebase
                    await _firebaseService.SendCoordinatesAsync(userEmail, location.Latitude, location.Longitude);
                }
                else
                {
                    LocationLabel.Text = "No location data available";
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                LocationLabel.Text = "Feature not supported on device";
            }
            catch (PermissionException)
            {
                // Handle permission exception
                LocationLabel.Text = "Permission to access location was denied";
            }
            catch (Exception ex)
            {
                // Unable to get location
                LocationLabel.Text = $"Unable to get location: {ex.Message}";
            }
        }

        private async Task DeleteLocationAsync()
        {
            try
            {
                // Retrieve the user email from the stored Firebase token
                var userInfo = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FreshFirebaseToken", ""));
                string userEmail = userInfo?.User?.Email ?? "Unknown";

                await _firebaseService.DeleteLocationAsync(userEmail);
                LocationLabel.Text = "Location deleted";
            }
            catch (Exception ex)
            {
                // Handle exception
                LocationLabel.Text = $"Unable to delete location: {ex.Message}";
            }
        }
    }
}
