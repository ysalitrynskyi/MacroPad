using System;
using System.Collections.Generic;
using System.Linq;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.BLL.Infrasturture.UsbDevice;

namespace RSoft.MacroPad.BLL.Infrasturture.Protocol.WebHub
{
    /// <summary>
    /// Reads what a WebHub keypad currently holds, so the app can show it instead of an empty editor
    /// </summary>
    public class WebHubDeviceState
    {
        private const int BlockSize = 56;
        private const int TableEntries = 16 + 3 * 3;

        public Dictionary<InputAction, string> Mappings { get; } = new Dictionary<InputAction, string>();
        public string Backlight { get; private set; }

        public static WebHubDeviceState Read(IUsb usb, byte layerNo)
        {
            var layer = (byte)(layerNo > 0 ? layerNo - 1 : 0);
            var table = new List<byte>();
            for (var offset = 0; offset < TableEntries * 4; offset += BlockSize)
            {
                var reply = usb.Request(WebHubReport.CreateKeyTableRequest(offset, layer));
                if (reply == null || reply.Length < 8)
                    return null;
                table.AddRange(reply.Skip(8));
            }

            var result = new WebHubDeviceState();
            foreach (var action in Enum.GetValues(typeof(InputAction)).Cast<InputAction>())
            {
                var index = WebHubReport.KeyIndex(action);
                if (index == null || (index.Value + 1) * 4 > table.Count)
                    continue;
                result.Mappings[action] = DescribeEntry(table.Skip(index.Value * 4).Take(4).ToArray());
            }

            var light = usb.Request(WebHubReport.Create(WebHubReport.ReadBacklight));
            if (light != null && light.Length >= 16)
                result.Backlight = DescribeBacklight(light[7], light[11], light[13], light[9], light[15]);

            return result;
        }

        public static string DescribeEntry(byte[] entry)
        {
            switch ((WebHubEntryType)entry[0])
            {
                case 0:
                case WebHubEntryType.Disabled:
                    return "nothing";
                case WebHubEntryType.Standard:
                    if (entry[1] == 0 && entry[2] == 0)
                        return "nothing";
                    return DescribeModifiers((Modifier)entry[1]) + DescribeKey(entry[2]);
                case WebHubEntryType.Consumer:
                    return DescribeMedia((ushort)(entry[1] | entry[2] << 8));
                case WebHubEntryType.Mouse:
                    return DescribeMouse(entry[1], (sbyte)entry[3]);
                case WebHubEntryType.KeypadFunction:
                    var field = typeof(KeypadLedFunction).GetField(((KeypadLedFunction)entry[1]).ToString());
                    var description = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                        .Cast<System.ComponentModel.DescriptionAttribute>().FirstOrDefault()?.Description;
                    return description ?? $"keypad function {entry[1]}";
            }
            return $"unknown (type 0x{entry[0]:X2})";
        }

        private static string DescribeModifiers(Modifier m)
        {
            var parts = new List<string>();
            if ((m & (Modifier.LeftCtrl | Modifier.RightCtrl)) != 0) parts.Add("Ctrl");
            if ((m & (Modifier.LeftShift | Modifier.RightShift)) != 0) parts.Add("Shift");
            if ((m & (Modifier.LeftAlt | Modifier.RightAlt)) != 0) parts.Add("Alt");
            if ((m & (Modifier.LeftWin | Modifier.RightWin)) != 0) parts.Add("Win");
            return string.Concat(parts.Select(p => p + "+"));
        }

        private static string DescribeKey(byte usage)
        {
            if (usage == 0)
                return "";
            var key = (KeyCode)usage;
            if (!Enum.IsDefined(typeof(KeyCode), key))
                return $"key 0x{usage:X2}";
            switch (key)
            {
                case KeyCode.SpaceKey: return "Space";
                case KeyCode.Clear: return ",";
                case KeyCode.Period: return ".";
                case KeyCode.Minus: return "-";
                case KeyCode.Plus: return "=";
                case KeyCode.Question: return "/";
                case KeyCode.Colon: return ";";
                case KeyCode.Backslash: return "'";
                case KeyCode.Pipe: return "\\";
                case KeyCode.Tilde: return "`";
                case KeyCode.OpenBracket: return "[";
                case KeyCode.CloseBracket: return "]";
            }
            var name = key.ToString();
            if (name.Length == 2 && name[0] == 'D' && char.IsDigit(name[1]))
                return name.Substring(1);
            return name;
        }

        private static string DescribeMouse(byte buttons, sbyte wheel)
        {
            var parts = new List<string>();
            if ((buttons & 1) != 0) parts.Add("Left click");
            if ((buttons & 2) != 0) parts.Add("Right click");
            if ((buttons & 4) != 0) parts.Add("Middle click");
            if ((buttons & 8) != 0) parts.Add("Back");
            if ((buttons & 16) != 0) parts.Add("Forward");
            if (wheel != 0)
                parts.Add((wheel > 0 ? "Scroll up" : "Scroll down") + (Math.Abs(wheel) > 1 ? $" x{Math.Abs(wheel)}" : ""));
            return parts.Count == 0 ? "nothing" : string.Join(" + ", parts);
        }

        private static string DescribeMedia(ushort usage)
        {
            switch (usage)
            {
                case 0xCD: return "Play / Pause";
                case 0xB5: return "Next track";
                case 0xB6: return "Previous track";
                case 0xE2: return "Mute";
                case 0xE9: return "Volume up";
                case 0xEA: return "Volume down";
                case 0: return "nothing";
            }
            return $"media key 0x{usage:X4}";
        }

        private static string DescribeBacklight(byte mode, byte color, byte hue, byte speed, byte value)
        {
            var names = new[] { "off", "solid", "breathing", "light on keypress", "tide" };
            var name = mode < names.Length ? names[mode] : $"mode {mode}";
            if (mode == 0)
                return name;

            var parts = new List<string> { name };
            if (mode == 4 || color == 0)
                parts.Add("colour cycling");
            else
            {
                var hues = new (int Hue, string Name)[] { (0, "red"), (21, "orange"), (43, "yellow"), (85, "green"), (128, "cyan"), (170, "blue"), (213, "purple"), (256, "red") };
                parts.Add(hues.OrderBy(h => Math.Abs(h.Hue - hue)).First().Name);
                parts.Add($"brightness {Math.Round(value * 100 / 255.0)}%");
            }
            if (mode >= 2)
                parts.Add($"speed {speed}");
            return string.Join(", ", parts);
        }
    }
}
