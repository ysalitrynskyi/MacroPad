using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace RSoft.MacroPad.BLL.Infrasturture.UsbDevice
{
    /// <summary>
    /// Request / reply channel for WebHub keypads.
    /// The firmware answers every frame, and it stops accepting new frames until the previous answer is read,
    /// so every write has to be paired with a read. HidLibrary's timed read is not usable on .NET 6, hence the raw handle.
    /// </summary>
    public sealed class WebHubTransport : IDisposable
    {
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ_WRITE = 3;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        private const byte ReplyMarker = 0xAA;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern SafeFileHandle CreateFile(string fileName, uint access, uint share, IntPtr security, uint creation, uint flags, IntPtr template);

        private readonly FileStream _stream;
        private readonly int _reportLength;
        private Task<int> _pendingRead;
        private byte[] _readBuffer;

        public WebHubTransport(string devicePath, int reportLength)
        {
            _reportLength = reportLength;
            var handle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            if (handle.IsInvalid)
                throw new IOException($"Could not open {devicePath} (error {Marshal.GetLastWin32Error()})");
            _stream = new FileStream(handle, FileAccess.ReadWrite, 0, true);
        }

        /// <summary>
        /// Sends one frame and waits for the device's answer.
        /// </summary>
        /// <returns>The reply without the report id, or null if the device did not answer in time</returns>
        public byte[] Transfer(byte reportId, byte[] data, int timeoutMs = 1000)
        {
            var frame = new byte[_reportLength];
            frame[0] = reportId;
            Array.Copy(data, 0, frame, 1, Math.Min(data.Length, _reportLength - 1));

            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            try
            {
                // Reports queued before this frame are answers to other programs' requests, or status the keypad
                // pushes by itself (e.g. after a brightness key), never the answer to this one
                HidD_FlushQueue(_stream.SafeFileHandle);
                StartRead();
                if (!_stream.WriteAsync(frame, 0, frame.Length).Wait(timeoutMs))
                    return null;

                while (true)
                {
                    var remaining = (int)(deadline - DateTime.UtcNow).TotalMilliseconds;
                    if (remaining <= 0 || !_pendingRead.Wait(remaining))
                        return null;

                    var reply = _readBuffer.Skip(1).Take(_pendingRead.Result - 1).ToArray();
                    _pendingRead = null;
                    if (IsAnswerTo(data, reply))
                        return reply;
                    StartRead();
                }
            }
            catch (AggregateException)
            {
                _pendingRead = null;
                return null;
            }
        }

        /// <summary>
        /// An answer starts with 0xAA and repeats the sub-command, except the key table read (8), which is answered as 7.
        /// Key table reads and key writes also echo the offset.
        /// </summary>
        private static bool IsAnswerTo(byte[] request, byte[] reply)
        {
            if (reply.Length < 5 || reply[0] != ReplyMarker)
                return false;
            var sub = request[1];
            if (reply[1] != (sub == 8 ? 7 : sub))
                return false;
            if (sub == 8 || sub == 16)
                return reply[3] == request[3] && reply[4] == request[4];
            return true;
        }

        [DllImport("hid.dll", SetLastError = true)]
        private static extern bool HidD_FlushQueue(SafeFileHandle device);

        // A read left over from a timed out transfer is reused while it is still waiting.
        // If it has completed meanwhile, it holds the late answer to an earlier frame and is dropped.
        private void StartRead()
        {
            if (_pendingRead != null && !_pendingRead.IsCompleted)
                return;
            _readBuffer = new byte[_reportLength];
            _pendingRead = _stream.ReadAsync(_readBuffer, 0, _readBuffer.Length);
        }

        public void Dispose()
        {
            try { _stream.Dispose(); } catch { }
        }
    }
}
