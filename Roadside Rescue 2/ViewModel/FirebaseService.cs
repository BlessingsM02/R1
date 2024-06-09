using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Roadside_Rescue_2.ViewModel
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            // Initialize FirebaseClient with your Firebase database URL
            _firebaseClient = new FirebaseClient("https://trying-74dd0-default-rtdb.firebaseio.com/");
        }

        public async Task SendCoordinatesAsync(string userEmail, double latitude, double longitude)
        {
            var coordinate = new
            {
                UserEmail = userEmail,
                Latitude = latitude,
                Longitude = longitude,
                Timestamp = DateTime.UtcNow
            };

            // Check if the user email already exists in the database
            var existingLocation = (await _firebaseClient
                .Child("locations")
                .OnceAsync<object>())
                .FirstOrDefault(l => ((dynamic)l.Object).UserEmail == userEmail);

            if (existingLocation != null)
            {
                // Update the existing location
                await _firebaseClient
                    .Child("locations")
                    .Child(existingLocation.Key)
                    .PutAsync(coordinate);
            }
            else
            {
                // Insert new location data
                await _firebaseClient
                    .Child("locations")
                    .PostAsync(coordinate);
            }
        }

        public async Task DeleteLocationAsync(string userEmail)
        {
            // Check if the user email exists in the database
            var existingLocation = (await _firebaseClient
                .Child("locations")
                .OnceAsync<object>())
                .FirstOrDefault(l => ((dynamic)l.Object).UserEmail == userEmail);

            if (existingLocation != null)
            {
                // Delete the existing location
                await _firebaseClient
                    .Child("locations")
                    .Child(existingLocation.Key)
                    .DeleteAsync();
            }
        }
    }
}
