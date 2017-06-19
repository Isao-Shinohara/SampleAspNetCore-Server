using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.ImmutableCollection;
using MessagePack.ReactivePropertyExtension;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			app.UseCors("MyPolicy");
            app.UseMvc();
			app.UseWebSockets();
			app.UseSignalR();
			app.UseDefaultFiles();
			app.UseStaticFiles();
        }
    }
}
