using System.Collections.Generic;
using System.Linq;
using RSoft.MacroPad.BLL.Infrasturture.Model;

namespace RSoft.MacroPad.BLL.Infrasturture.Protocol.WebHub
{
    public class WebHubReportComposer : IReportComposer
    {
        // The firmware counts layers from 0, the UI from 1
        private static byte DeviceLayer(byte layerNo) => (byte)(layerNo > 0 ? layerNo - 1 : 0);

        public IEnumerable<Report> Key(InputAction action, byte layerNo, ushort delay, IEnumerable<(KeyCode Key, Modifier Modifiers)> sequence)
        {
            var index = WebHubReport.KeyIndex(action);
            if (index == null)
                return Enumerable.Empty<Report>();

            // A key table entry holds exactly one keystroke
            var stroke = sequence.FirstOrDefault(s => s.Key != KeyCode.None || s.Modifiers != Modifier.None);
            if (stroke.Key == KeyCode.None && stroke.Modifiers == Modifier.None)
                return new[] { WebHubReport.CreateKey(index.Value, DeviceLayer(layerNo), WebHubEntryType.Disabled, 0, 0, 0) };

            return new[] { WebHubReport.CreateKey(index.Value, DeviceLayer(layerNo), WebHubEntryType.Standard, (byte)stroke.Modifiers, (byte)stroke.Key, 0) };
        }

        public IEnumerable<Report> Media(InputAction action, byte layerNo, MediaKey key)
        {
            var index = WebHubReport.KeyIndex(action);
            if (index == null)
                return Enumerable.Empty<Report>();

            var usage = ConsumerUsage(key);
            return new[] { WebHubReport.CreateKey(index.Value, DeviceLayer(layerNo), WebHubEntryType.Consumer, (byte)(usage & 0xFF), (byte)(usage >> 8), 0) };
        }

        // The mouse entry encoding of this family is not known yet
        public IEnumerable<Report> Mouse(InputAction action, byte layerNo, MouseButton func, Modifier modifiers)
            => Enumerable.Empty<Report>();

        public IEnumerable<Report> Led(byte layerNo, LedMode mode, LedColor color)
        {
            // Modes: 0 off, 1 solid, 2 breathing, 3 blink, 4 tide
            if ((byte)mode > 4)
                return Enumerable.Empty<Report>();
            return new[] { WebHubReport.CreateBacklight((byte)mode, color == LedColor.Random, Hue(color)) };
        }

        private static ushort ConsumerUsage(MediaKey key)
        {
            switch (key)
            {
                case MediaKey.PlayPause: return 0xCD;
                case MediaKey.NextTrack: return 0xB5;
                case MediaKey.PrevTrack: return 0xB6;
                case MediaKey.VolMute: return 0xE2;
                case MediaKey.VolUp: return 0xE9;
                case MediaKey.VolDn: return 0xEA;
            }
            return 0;
        }

        // Hue on a 0-255 scale
        private static byte Hue(LedColor color)
        {
            switch (color)
            {
                case LedColor.Red: return 0;
                case LedColor.Orange: return 21;
                case LedColor.Yellow: return 43;
                case LedColor.Green: return 85;
                case LedColor.Cyan: return 128;
                case LedColor.Blue: return 170;
                case LedColor.Purple: return 213;
            }
            return 255;
        }
    }
}
