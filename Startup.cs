using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.ImmutableCollection;
using MessagePack.ReactivePropertyExtension;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SampleAspNetCore_Server.Hubs;

namespace SampleAspNetCore_Server
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMemoryCache();
			services.AddSignalR();
			services.AddCors(options => {
				options.AddPolicy("MyPolicy", builder => {
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			// set extensions to default resolver.
			MessagePack.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
				// enable extension packages first
				ImmutableCollectionResolver.Instance,
				ReactivePropertyResolver.Instance,
				MessagePack.Unity.Extension.UnityBlitResolver.Instance,
				MessagePack.Unity.UnityResolver.Instance,

				// finaly use standard(default) resolver
				StandardResolver.Instance
			);

			//services.AddMvc();
			services.AddMvc().AddMvcOptions(options =>
			{
				// MessagePack.
				options.FormatterMappings.SetMediaTypeMappingForFormat("msgpack", new MediaTypeHeaderValue("application/x-msgpack"));
				options.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Instance));
				options.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Instance));
			});

			services.AddDbContext<SignalRContext>(opt => opt.UseInMemoryDatabase("SampleAspNetCore"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			app.UseStaticFiles(new StaticFileOptions() {
				ServeUnknownFileTypes = true,
				DefaultContentType = "application/octet-stream"
			});

			app.UseCors("MyPolicy");
            app.UseMvc();
			app.UseWebSockets();
			app.UseSignalR(route => {
				route.MapHub<ChatHub>("/chat");
			});
			app.UseDefaultFiles();
        }
    }
}
