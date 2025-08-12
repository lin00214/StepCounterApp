using System;

namespace StepCounterApp.Services
{
    public class StepCounterService
    {
        private int _stepCount = 0;

        public void StartCounting()
        {
            _stepCount = 0;
            Microsoft.Maui.Controls.MessagingCenter.Send<object, int>(this, "StepUpdated", _stepCount);
        }

        public void AddStep()
        {
            _stepCount++;
            Microsoft.Maui.Controls.MessagingCenter.Send<object, int>(this, "StepUpdated", _stepCount);
        }

        public void StopCounting()
        {
        }
    }
}