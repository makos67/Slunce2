using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Slunce2 {
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageView ivImage;
        Button btStart;
        TextView uvodniText;
        TextView lblLat, lblLong, lblAlt, pokusApi, atb, ate, ntb, nte, ctb, cte, sunr, suns, noon, dlen, poloha, astro;
        string Lat, Long, Alt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            ivImage = FindViewById<ImageView>(Resource.Id.ivImage);
            btStart = FindViewById<Button>(Resource.Id.btStart);
            uvodniText = FindViewById<TextView>(Resource.Id.uvodniText);
            lblLat = FindViewById<TextView>(Resource.Id.lblLat);
            lblLong = FindViewById<TextView>(Resource.Id.lblLong);
            lblAlt = FindViewById<TextView>(Resource.Id.lblAlt);
            pokusApi = FindViewById<TextView>(Resource.Id.pokusApi);
            atb = FindViewById<TextView>(Resource.Id.atb);
            ate = FindViewById<TextView>(Resource.Id.ate);
            nte = FindViewById<TextView>(Resource.Id.nte);
            ntb = FindViewById<TextView>(Resource.Id.ntb);
            cte = FindViewById<TextView>(Resource.Id.cte);
            ctb = FindViewById<TextView>(Resource.Id.ctb);
            sunr = FindViewById<TextView>(Resource.Id.sunr);
            suns = FindViewById<TextView>(Resource.Id.suns);
            noon = FindViewById<TextView>(Resource.Id.noon);
            dlen = FindViewById<TextView>(Resource.Id.dlen);
            poloha = FindViewById<TextView>(Resource.Id.poloha);
            astro = FindViewById<TextView>(Resource.Id.astro);

            btStart.Click += BtStart_Click;

            ivImage.SetImageBitmap(Bitmap.CreateScaledBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.obrazek), 400, 400, false));
        }

        private async void BtStart_Click(object sender, System.EventArgs e) {
            await Lokalizace();
            ivImage.SetImageBitmap(null);
            uvodniText.Text = "";

            //Long = "30";
            //Lat = "60";
            //Alt = "90";

            poloha.Text = "Poloha:";
            lblLong.Text = "Zeměpisná délka: " + Long+"°";
            lblLat.Text = "Zeměpisná šířka: " + Lat+"°";
            lblAlt.Text = "Nadmořská výška: " + Alt + "m.n.m.";
            SunInfo sunInfo = await SunInfoService.GetCurrentSunInfo(Lat, Long);
            pokusApi.Text = sunInfo.status;
            astro.Text = "Astro:";
            atb.Text = "Astronomické svítání: " + sunInfo.astronomical_twilight_begin;
            ate.Text = "Astronomický soumrak: " + sunInfo.astronomical_twilight_end;
            nte.Text = "Nautický soumrak: " + sunInfo.nautical_twilight_end;
            ntb.Text = "Nautické svítání: " + sunInfo.nautical_twilight_begin;
            ctb.Text = "Občanské svítání: " + sunInfo.civil_twilight_begin;
            cte.Text = "Občanský soumrak: " + sunInfo.civil_twilight_end;
            suns.Text = "Západ slunce: " + sunInfo.sunset;
            sunr.Text = "Východ slunce: " + sunInfo.sunrise;
            noon.Text = "Sluneční poledne: " + sunInfo.solar_noon;
            dlen.Text = "Délka dne: " + sunInfo.day_length;


        }


        async Task Lokalizace() {
            try {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                CancellationTokenSource cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null) {
                    Lat = location.Latitude.ToString();
                    Long = location.Longitude.ToString();
                    Alt = location.Altitude.ToString();
                    int index = Alt.IndexOf(",");
                    Alt = Alt.Substring(0, index);
                    //pokusApi.Text = Lat;
                }
            }
            catch (FeatureNotSupportedException fnsEx) {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx) {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx) {
                // Handle permission exception
            }
            catch (Exception ex) {
                // Unable to get location
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}