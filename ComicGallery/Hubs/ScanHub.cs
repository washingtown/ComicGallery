using ComicGallery.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Hubs
{
    /// <summary>
    /// 后台扫描漫画的SignalR Hub
    /// </summary>
    public class ScanHub : Hub
    {
        private readonly ScanService _scanService;
        public ScanHub(ScanService scanService)
        {
            _scanService = scanService;
        }

        public async Task GetScanStatus()
        {
            await Clients.Caller.SendAsync("Scanning", _scanService.Scanning);
        }

        public async Task StartScan()
        {
            await _scanService.Scan();
        }
    }
}
