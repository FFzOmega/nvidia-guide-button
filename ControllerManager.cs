using System;
using System.Linq;
using System.Windows.Forms;
using SharpDX;
using SharpDX.DirectInput;

namespace GuideToF20
{
    internal class ControllerManager
    {
        public event Action<string>? StatusChanged;

        private DirectInput directInput = new DirectInput();
        private Joystick? joystick;

        public string ConnectedControllerName { get; private set; } = "No controller";

        private const int GuideButton = 12;
        private const int HoldMilliseconds = 2000;
        private const int DoubleTapMilliseconds = 300;

        private bool previousGuideState = false;
        private bool waitingForSecondTap = false;
        private bool secondTapInProgress = false;
        private bool longPressSent = false;

        private DateTime guideDownTime;
        private DateTime firstTapTime;
        private DateTime lastReconnectAttempt = DateTime.MinValue;

        public bool Enabled { get; set; } = true;

        public ControllerManager()
        {
            ConnectController();
        }

        public void ReloadController()
        {
            joystick?.Unacquire();
            joystick?.Dispose();
            joystick = null;

            ConnectedControllerName = "No controller";
            ConnectController();
        }

        private void ConnectController()
        {
            var device = directInput
                .GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly)
                .FirstOrDefault();

            if (device == null)
            {
                ConnectedControllerName = "No controller";
                StatusChanged?.Invoke("No controller found");
                return;
            }

            joystick = new Joystick(directInput, device.InstanceGuid);
            joystick.Properties.BufferSize = 128;
            joystick.Acquire();

            ConnectedControllerName = device.InstanceName;
            StatusChanged?.Invoke("Connected: " + ConnectedControllerName);
        }

        public void Update()
        {
            if (!Enabled)
                return;

            if (joystick == null)
            {
                TryReconnect();
                return;
            }

            bool guidePressed;

            try
            {
                joystick.Poll();

                var state = joystick.GetCurrentState();
                if (state == null || state.Buttons.Length <= GuideButton)
                    return;

                guidePressed = state.Buttons[GuideButton];
            }
            catch (SharpDXException)
            {
                ConnectedControllerName = "No controller";
                StatusChanged?.Invoke("Controller disconnected");
                joystick = null;
                return;
            }

            var now = DateTime.Now;

            if (waitingForSecondTap && !secondTapInProgress)
            {
                if ((now - firstTapTime).TotalMilliseconds >= DoubleTapMilliseconds)
                {
                    waitingForSecondTap = false;
                    KeyboardSender.PressCtrlHome();
                    StatusChanged?.Invoke("Single tap: Ctrl+Home");
                }
            }

            if (guidePressed && !previousGuideState)
            {
                guideDownTime = now;
                longPressSent = false;

                if (waitingForSecondTap)
                    secondTapInProgress = true;

                StatusChanged?.Invoke("Guide down");
            }

            if (guidePressed && !longPressSent)
            {
                if ((now - guideDownTime).TotalMilliseconds >= HoldMilliseconds)
                {
                    longPressSent = true;
                    waitingForSecondTap = false;
                    secondTapInProgress = false;

                    KeyboardSender.PressF17();
                    StatusChanged?.Invoke("Hold: F17");
                }
            }

            if (!guidePressed && previousGuideState)
            {
                if (!longPressSent)
                {
                    if (waitingForSecondTap && secondTapInProgress)
                    {
                        waitingForSecondTap = false;
                        secondTapInProgress = false;

                        KeyboardSender.PressF20();
                        StatusChanged?.Invoke("Double tap: F20");
                    }
                    else
                    {
                        waitingForSecondTap = true;
                        secondTapInProgress = false;
                        firstTapTime = now;

                        StatusChanged?.Invoke("First tap");
                    }
                }
            }

            previousGuideState = guidePressed;
        }

        private void TryReconnect()
        {
            if ((DateTime.Now - lastReconnectAttempt).TotalMilliseconds < 1000)
                return;

            lastReconnectAttempt = DateTime.Now;
            ConnectController();
        }
    }
}