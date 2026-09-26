using HidLibrary;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RSoft.MacroPad.BLL.Infrasturture.UsbDevice
{
    public class HidLib
    {
        private bool _deviceStatus;
        private List<HidDevice> _deviceList = new List<HidDevice>();
        private HidDevice _hidDevice;
        private WebHubTransport _webHub;

        public ProtocolType? ProtocolType { get; private set; }

        public ushort VendorId { get; private set; }
        public ushort ProductId { get; private set; }

        public bool DeviceStatus => _deviceStatus;


        public bool ConnectDevice(params (ushort VendorId, ushort ProductId, string PathFragment, ProtocolType ProtocolType)[] supportedProducts)
        {
            foreach (var supportedProduct in supportedProducts)
            {
                _hidDevice = HidDevices.Enumerate(supportedProduct.VendorId, supportedProduct.ProductId).FirstOrDefault();
                if (_hidDevice != null)
                {
                    foreach (HidDevice hidDevice in HidDevices.Enumerate(supportedProduct.VendorId).ToList())
                    {
                        if (hidDevice.DevicePath.IndexOf(supportedProduct.PathFragment) != -1)
                        {
                            if (supportedProduct.ProtocolType == Model.ProtocolType.WebHub)
                            {
                                try
                                {
                                    _webHub = new WebHubTransport(hidDevice.DevicePath, hidDevice.Capabilities.OutputReportByteLength);
                                }
                                catch (IOException)
                                {
                                    continue;
                                }
                            }
                            else
                                hidDevice.OpenDevice();

                            _deviceList.Add(hidDevice);
                            _hidDevice = hidDevice;
                            //// Somehow this is not supported in .net6 but doesn't seem to make any difference
                            //_hidDevice.MonitorDeviceEvents = true;

                            ProtocolType = supportedProduct.ProtocolType;
                            VendorId = supportedProduct.VendorId;
                            ProductId = supportedProduct.ProductId;

                            _deviceStatus = true;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CheckConnection()
        {
            if (_hidDevice.IsConnected)
                return true;
            _hidDevice.CloseDevice();
            _webHub?.Dispose();
            _webHub = null;
            _deviceStatus = false;
            return false;
        }

        public byte[] Request(byte reportId, byte[] buffer)
        {
            return _webHub?.Transfer(reportId, buffer);
        }

        public bool WriteDevice(byte reportId, byte[] buffer)
        {
            if (_webHub != null)
            {
                var length = _hidDevice.Capabilities.OutputReportByteLength - 1;
                HidLog.AppendMsg(reportId, buffer.Take(length));
                return _webHub.Transfer(reportId, buffer) != null;
            }

            var report = _hidDevice.CreateReport();
            report.ReportId = reportId;

            var byteCount = report.Data.Length;

            for (int i = 0; i < byteCount; ++i)
                report.Data[i] = buffer[i];
           
            HidLog.AppendMsg(report.ReportId, ProtocolType == Model.ProtocolType.Legacy ? report.Data.Take(8) : report.Data);

            return _hidDevice.WriteReport(report, 500);
        }
    }
}
