using DG.Data.Model;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Context;
#endif
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
            Type contextType = typeof(dgdatamodeltestContext);

            object[] contextParameters = null;

            Initialize(contextType, contextParameters);
        }

    }

}
