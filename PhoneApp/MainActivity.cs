using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "Lab07", Theme = "@android:style/Theme.Holo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);

            ValidateButton.Click += (object sender, System.EventArgs e) =>
            {
                Validate();
            };

        }

        private async void Validate()
        {
            var ServiceClient = new SALLab07.ServiceClient();

            var EmailAddressText = FindViewById<EditText>(Resource.Id.EmailAddressText);
            var PasswordText = FindViewById<EditText>(Resource.Id.PasswordText);

            string StudentEmail = EmailAddressText.Text;
            string Password = PasswordText.Text;

            string myDevice = Android.Provider.Settings.Secure.GetString(
                ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            var Result = await ServiceClient.ValidateAsync(
                StudentEmail, Password, myDevice);

            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var Builder = new Notification.Builder(this)
                    .SetContentTitle("Validación de Actividad")
                    .SetContentText($"{Result.Status}\n{Result.Fullname}\n{Result.Token}")
                    .SetSmallIcon(Resource.Drawable.Icon);

                Builder.SetCategory(Notification.CategoryMessage);

                var ObjectNotification = Builder.Build();
                var Manager = GetSystemService(
                    Android.Content.Context.NotificationService) as NotificationManager;
                Manager.Notify(0, ObjectNotification);
            }
            else
            {
                var ValidateText = FindViewById<TextView>(Resource.Id.ValidateText);

                ValidateText.Text = $"{Result.Status}\n{Result.Fullname}\n{Result.Token}";
            }

            
        }
    }
}

