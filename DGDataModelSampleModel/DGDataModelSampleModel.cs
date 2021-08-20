using DG.Data.Model;
using DG.DataModelSample.Model.Entity;
using System;
#if !NETFRAMEWORK
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DG.DataModelSample.Model
{
    public class DGDataModelSampleModel : GenericDataModel
    {
        // Repositories
        public PostsRepository Posts { get; private set; }
        public BlogsRepository Blogs { get; protected set; }
        public CommentsRepository Comments { get; protected set; }
        public PostsToTagsRepository PostsToTags { get; protected set; }
        public TagsRepository Tags { get; protected set; }
        public FooterTextRepository FooterText { get; protected set; }
        public FooterTextDescRepository FooterTextDesc { get; protected set; }

        /// <summary>
        /// Initialize the model
        /// </summary>
        public DGDataModelSampleModel()
        {
            Type contextType = typeof(dgdatamodeltestEntities);
            object[] contextParameters = null;

            Initialize(contextType, contextParameters);
        }

#if !NETFRAMEWORK
        /// <summary>
        /// Initialize the model
        /// </summary>
        /// <param name="connectionString"></param>
        public static DGDataModelSampleModel Init(string connectionString)
        {
            DGDataModelSampleModel ret = null;

            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<dgdatamodeltestEntities>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });
            var host = builder.Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var employeeContext = services.GetRequiredService<dgdatamodeltestEntities>();
                }
                catch { }
            }

            return ret;
        }
#endif

    }

}
