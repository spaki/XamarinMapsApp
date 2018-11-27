using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinMapsApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!await ShouldHaveLocationPermission())
                return;

            var location = await Geolocation.GetLocationAsync();
            var position = new Position(location.Latitude, location.Longitude);

            var pin = new Pin { Position = position, Type = PinType.Place, Label = "You are here!" };

            Mapa.Pins.Add(pin);
            Mapa.IsShowingUser = true;
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));
        }

        private async Task<bool> ShouldHaveLocationPermission()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (permissionStatus == PermissionStatus.Granted)
                return true;

            var permissions = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });

            return permissions.ContainsKey(Permission.Location);
        }
    }
}
