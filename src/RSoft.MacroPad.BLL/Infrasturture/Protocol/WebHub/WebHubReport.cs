using System;
using RSoft.MacroPad.BLL.Infrasturture.Model;

namespace RSoft.MacroPad.BLL.Infrasturture.Protocol.WebHub
{
    /// <summary>
    /// Frames of the protocol used by the SDCX / Huali family of keypads (e.g. the SDINNOVATION SIDE-KEYBOARD),
    /// which the vendor configures with a browser based WebHID tool.
    /// Every frame is 64 bytes on report id 0. Byte 0 is always 6, byte 1 is the sub-command.
    /// Protocol notes: https://github.com/dozzenn/knurl#protocol-notes
    /// </summary>
    public class WebHubReport : Report
    {
        public const byte Command = 6;

        public const byte ReadDeviceInfo = 5;
        public const byte ReadKeys = 8;
        public const byte ReadBacklight = 10;
        public const byte WriteBacklight = 11;
        public const byte WriteKey = 16;

        private WebHubReport() { }

        public static WebHubReport Create(params byte[] payload)
        {
            var r = new WebHubReport();
            r.ReportId = 0;
            r.Data[0] = Command;
            for (var i = 0; i < payload.Length; i++)
                r.Data[1 + i] = payload[i];
            return r;
        }

        /// <summary>
        /// Writes one 4 byte entry of the key table: [type, code1, code2, code3]
        /// </summary>
        public static WebHubReport CreateKey(int keyIndex, byte layer, WebHubEntryType type, byte c1, byte c2, byte c3)
        {
            var offset = 4 * keyIndex;
            return Create(WriteKey, 7, (byte)(offset & 0xFF), (byte)((offset >> 8) & 0xFF), 0, layer, 0, (byte)type, c1, c2, c3);
        }

        /// <param name="brightness">0-100 percent. The firmware dims through the colour's HSV value; its own brightness byte (0-4) is written alongside, as the vendor tool does</param>
        /// <param name="speed">0-4, used by the animated effects</param>
        public static WebHubReport CreateBacklight(byte mode, bool usePalette, byte hue, byte brightness = 100, byte speed = 2)
        {
            const byte type = 1, direction = 0;
            var color = (byte)(usePalette || mode == 0 ? 0 : 1);
            var percent = Math.Min((int)brightness, 100);
            var level = (byte)Math.Round(percent * 4 / 100.0);
            var value = (byte)Math.Round(percent * 255 / 100.0);
            return Create(WriteBacklight, 11, 0, 0, type, 0, mode, level, Math.Min(speed, (byte)4), direction, color, 0, hue, 255, value);
        }

        public static WebHubReport CreateDeviceInfoRequest() => Create(ReadDeviceInfo);

        public static WebHubReport CreateKeyTableRequest(int blockOffset, byte layer)
            => Create(ReadKeys, 58, (byte)(blockOffset & 0xFF), (byte)((blockOffset >> 8) & 0xFF), 0, layer);

        /// <summary>
        /// Buttons occupy table indices 0-15. Each knob owns three consecutive slots from 16: push, turn right, turn left.
        /// </summary>
        public static int? KeyIndex(InputAction action)
        {
            if (action >= InputAction.Key1 && action <= InputAction.Key12)
                return (byte)action - (byte)InputAction.Key1;

            if (action < InputAction.Knob1Left || action > InputAction.Knob3Right)
                return null;

            var knob = ((byte)action - (byte)InputAction.Knob1Left) / 3;
            var part = ((byte)action - (byte)InputAction.Knob1Left) % 3;
            var slot = part == 1 ? 0 : part == 2 ? 1 : 2;
            return 16 + knob * 3 + slot;
        }
    }

    public enum WebHubEntryType : byte
    {
        Mouse = 0x10,
        Disabled = 0x13,
        KeypadFunction = 0x1F,
        Standard = 0x20,
        Consumer = 0x30,
    }
}
