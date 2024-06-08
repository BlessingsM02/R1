
using Roadside_Rescue_2.ViewModel;

namespace Roadside_Rescue_2.Views;

public partial class HomePage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    public HomePage()
	{
		InitializeComponent();
        _firebaseService = new FirebaseService();
    }
    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        await GetAndSendLocationAsync();
    }

    private async Task GetAndSendLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";

                // Send coordinates to Firebase
                await _firebaseService.SendCoordinatesAsync(location.Latitude, location.Longitude);
            }
            else
            {
                LocationLabel.Text = "No location data available";
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            LocationLabel.Text = "Feature not supported on device";
        }
        catch (PermissionException pEx)
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
}