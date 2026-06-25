using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;

namespace GuideToF20
{
    internal static class KeyboardSender
    {
        private static readonly InputSimulator sim = new InputSimulator();

        public static void PressA()
        {
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
        }

        public static void PressF17()
        {
            sim.Keyboard.KeyPress(VirtualKeyCode.F17);
        }

        public static void PressF20()
        {
            sim.Keyboard.KeyPress(VirtualKeyCode.F20);
        }

        public static void PressCtrlHome()
        {
            sim.Keyboard.ModifiedKeyStroke(
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.HOME
            );
        }
    }
}