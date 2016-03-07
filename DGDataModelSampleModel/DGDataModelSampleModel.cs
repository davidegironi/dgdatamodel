using DG.Data.Model;
using DG.DataModelSample.Model.Entity;
using System;

namespace DG.DataModelSample.Model
{
    public class DGDataModelSampleModel : GenericDataModel
    {
        // Logged Repositories
        public PostsRepository Posts { get; private set; }
        public BlogsRepository Blogs { get; protected set; }
        public CommentsRepository Comments { get; protected set; }
        public PostsToTagsRepository PostsToTags { get; protected set; }
        public TagsRepository Tags { get; protected set; }

        // Repositories
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
    }
}
