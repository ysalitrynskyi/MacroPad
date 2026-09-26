using System;
using System.Collections.Generic;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.BLL.Infrasturture.Protocol;

namespace RSoft.MacroPad.BLL.Infrasturture.UsbDevice
{
    public interface IUsb
    {
        bool IsConnected { get; }

        ushort VendorId { get; set; }
        ushort ProductId { get; set; }
        ProtocolType ProtocolType { get; }
        byte Version { get; set; }

        IEnumerable<(ushort VendorId, ushort ProductId, string PathFragment, ProtocolType protocolType)> SupportedDevices { get; set; }


        bool CheckIfConnected();
        bool Connect();

        bool Write(Report report);

        /// <summary>
        /// Sends a frame and returns the keypad's answer. Only WebHub keypads answer; the others return null.
        /// </summary>
        byte[] Request(Report report);

        event EventHandler OnConnected;
    }

    public abstract class UsbBase : IUsb
    {
        public ushort VendorId { get; set; } = 0;
        public ushort ProductId { get; set; } = 0;
        public ProtocolType ProtocolType { get; protected set; } = ProtocolType.Legacy;
        public byte Version { get; set; }

        public event EventHandler OnConnected;

        public bool CheckIfConnected()
            => CheckIfConnectedInternal();

        public IEnumerable<(ushort VendorId, ushort ProductId, string PathFragment, ProtocolType protocolType)> SupportedDevices { get; set; }
            = DeviceSample.Devices;

        public bool IsConnected { get; protected set; }

        protected abstract bool CheckIfConnectedInternal();

        public bool Connect()
        {
            return CheckIfConnectedInternal() || ConnectInternal();
        }

        protected abstract bool ConnectInternal();

        protected virtual byte KeyBoardVersionCheck()
        {
            if (Write(VersionCheckReport.Create(0)))
                return Version = 0;
            if (Write(VersionCheckReport.Create(2)))
                return Version = 2;
            return Version = 3;
        }

        protected void Connected()
        {
            // WebHub firmware answers every frame and has a bootloader jump among its commands,
            // so it is not sent probe frames it does not know
            if (ProtocolType == ProtocolType.WebHub)
                Version = 0;
            else
                KeyBoardVersionCheck();
            OnConnected?.Invoke(this, EventArgs.Empty);
        }

        public abstract bool Write(Report report);

        public virtual byte[] Request(Report report) => null;
    }
}
