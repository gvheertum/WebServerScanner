using Gtk;
using GvH.WebServerScanner.Library;
using GvH.WebServerScanner.Library.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UI = Gtk.Builder.ObjectAttribute;

namespace GvH.WebServerScanner.App
{
    internal class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _buttonScan = null;
        [UI] private Entry _inputText = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _buttonScan.Clicked += ButtonScan_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void ButtonScan_Clicked(object sender, EventArgs a)
        {

            _label1.Text = "Scan is clicked: " + _inputText.Text;

            var scanIp = new IpAddressRepresentation(_inputText.Text);
            var scanRes = new ScanRunner().PollIpAddress(scanIp, new List<HttpScanParameter>() { 
                new HttpScanParameter() { Https = false, Port = 80 },
                new HttpScanParameter() { Https = false, Port = 8080 }
            });
            _label1.Text = GetRepresentation(scanRes);
        }

        private string GetRepresentation(IpAddressScanResult scanRes) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{scanRes.IpAddress.GetRepresentation()} ({scanRes.HostName}) -> {(scanRes.Pingable?"PONG":"N/A")} ({scanRes.PingMs} ms)");
            foreach(var r in scanRes.HttpResults) 
            {
                sb.AppendLine($"{r.Port} ({(r.Https?"HTTPS":"HTTP")}) -> {r.FoundResult} -> {r.Title}");
            }
            return sb.ToString();
        }
    }
}
