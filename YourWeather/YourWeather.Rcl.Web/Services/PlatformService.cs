﻿using Microsoft.JSInterop;

namespace YourWeather.Rcl.Web.Services
{
    public class PlatformService : Rcl.Services.PlatformService, IAsyncDisposable
    {
        protected readonly IJSRuntime JS;
        protected IJSObjectReference module = default!;
        public PlatformService(IJSRuntime js)
        {
            JS = js;

        }

        public async ValueTask DisposeAsync()
        {
            if (module != null)
            {
                await module.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }

        public override async Task OpenBrowserUrl(string url)
        {
            //不要用下面的window.open方法，如果新窗口没有加载完，回到原来窗口，会卡死
            //JS.InvokeVoidAsync("open", url, "_blank");

            await InitModule();
            await module.InvokeVoidAsync("openBrowserUrl", url, url);
        }

        protected async Task InitModule()
        {
            if (module != null)
            {
                return;
            }

            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/YourWeather.Rcl.Web/js/system-service.js");
        }

        public override string GetVersion()
        {
            return base.GetVersion();
        }
    }
}