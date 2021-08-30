using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using prismic;
using prismic.fragments;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();
            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            //prismic
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICache, InMemoryCache>();
            builder.Services.AddHttpClient<PrismicHttpClient>();
            builder.Services.AddSingleton<IPrismicApiAccessor, DefaultPrismicApiAccessor>();
            builder.Services.AddSingleton<DocumentLinkResolver, DefaultDocumentLinkResolver>();
            var option = new PrismicOption(builder.Configuration["token"]);
            builder.Services.AddSingleton<IOptions<PrismicSettings>>(option);

            var host = builder.Build();

            await host.RunAsync();
        }
    }

    public class PrismicOption : IOptions<PrismicSettings>
    {
        private string token;

        public PrismicOption(string token)
        {
            this.token = token;
        }
        public PrismicSettings Value => new PrismicSettings(){
            AccessToken = this.token,
            Endpoint = "https://lauener.prismic.io/api/v1"
        };
    }

    public class DefaultDocumentLinkResolver : DocumentLinkResolver
    {
        public override string Resolve(DocumentLink link)
        {
            return link.Uid.ToString();
        }
    }
}