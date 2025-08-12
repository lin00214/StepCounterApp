using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Hardware;
using Android.Content;
using Android.Runtime;
using AndroidX.Core.App;

namespace StepCounterApp;

[Activity(
    Theme = "@style/Maui.SplashTheme", 
    Label = "StepCounterApp", 
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
public class MainActivity : MauiAppCompatActivity, ISensorEventListener
{
    SensorManager? sensorManager;
    Sensor? stepSensor;
    private int initialStepCount = -1;
    private int currentStepCount = 0;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        sensorManager = (SensorManager?)GetSystemService(SensorService);
        stepSensor = sensorManager?.GetDefaultSensor(SensorType.StepCounter);
        
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            if (CheckSelfPermission(Android.Manifest.Permission.ActivityRecognition) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.ActivityRecognition }, 1001);
            }
        }
    }

    protected override void OnResume()
    {
        base.OnResume();
        if (stepSensor != null)
        {
            sensorManager?.RegisterListener(this, stepSensor, SensorDelay.Ui);
        }
    }

    protected override void OnPause()
    {
        base.OnPause();
        sensorManager?.UnregisterListener(this);
    }

    public void OnAccuracyChanged(Sensor? sensor, [GeneratedEnum] SensorStatus accuracy) { }

    public void OnSensorChanged(SensorEvent? e)
    {
        if (e?.Sensor?.Type == SensorType.StepCounter && e.Values != null && e.Values.Count > 0)
        {
            int totalSteps = (int)e.Values[0];
            
            if (initialStepCount == -1)
            {
                initialStepCount = totalSteps;
                currentStepCount = 0;
            }
            else
            {
                currentStepCount = totalSteps - initialStepCount;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Microsoft.Maui.Controls.MessagingCenter.Send<object, int>(this, "StepUpdated", currentStepCount);
            });
        }
    }
}