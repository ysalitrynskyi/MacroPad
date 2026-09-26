using System.ComponentModel;

namespace RSoft.MacroPad.BLL.Infrasturture.Model
{
    /// <summary>
    /// Keys that change the keypad's own backlight when pressed (WebHub keypads)
    /// </summary>
    public enum KeypadLedFunction : byte
    {
        [Description("LED brightness up")] BrightnessUp = 1,
        [Description("LED brightness down")] BrightnessDown = 2,
        [Description("Next LED effect")] NextEffect = 3,
        [Description("Next LED colour")] NextColor = 4,
        [Description("LED speed up")] SpeedUp = 5,
        [Description("LED speed down")] SpeedDown = 6,
    }
}
