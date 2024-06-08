using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;

namespace Roadside_Rescue_2.ViewModel
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            // Initialize FirebaseClient with your Firebase database URL
            _firebaseClient = new FirebaseClient("https://final-year-c48d0-default-rtdb.firebaseio.com/");
        }

        public async Task SendCoordinatesAsync(double latitude, double longitude)
        {
            var coordinate = new
            {
                Latitude = latitude,
                Longitude = longitude,
                Timestamp = DateTime.UtcNow
            };

            await _firebaseClient
                .Child("locations")
                .PostAsync(coordinate);
        }
    }
}
