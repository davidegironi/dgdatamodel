using DG.Data.Model;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Models;
#endif
using System.Linq;
using System.Text.RegularExpressions;

namespace DG.DataModelSample.Model
{
    public class PostsRepository : GenericDataRepository<posts, DGDataModelSampleModel>
    {
        public PostsRepository()
            : base()
        { }

        public class PostsRepositoryLanguage : IGenericDataRepositoryLanguage
        {
            public string postRepositoryE001 = "Post title may contain letters (a-z, A-Z), numbers (0-9), dashes (-), underscores (_) and periods (.). It should be 1 to 128 lenght.";
            public string postRepositoryE002 = "Post title could not be empty.";
            public string postRepositoryE003 = "Post already inserted.";
        }

        public PostsRepositoryLanguage language = new PostsRepositoryLanguage();

        public override bool CanAdd(ref string[] errors, params posts[] items)
        {
            errors = new string[] { };

            bool ret = base.CanAdd(ref errors, items);
            if (!ret)
                return ret;

            foreach (posts item in items)
            {
                //to not check external blogs_id key to throw exception

                try
                {
                    if (!Regex.Match(item.posts_title, @"^[a-zA-Z0-9_\.'-]*$", RegexOptions.IgnoreCase).Success
                        ||
                        (item.posts_title.Length < 1 || item.posts_title.Length > 128)
                    )
                    {
                        ret = false;
                        errors = errors.Concat(new string[] { language.postRepositoryE001 }).ToArray();
                    }
                }
                catch
                {
                    ret = false;
                    errors = errors.Concat(new string[] { language.postRepositoryE002 }).ToArray();
                }

                if (ret)
                {
                    if (Any(r => r.posts_title == item.posts_title))
                    {
                        ret = false;
                        errors = errors.Concat(new string[] { language.postRepositoryE003 }).ToArray();
                    }
                }

                if (!ret)
                    break;
            }

            return ret;
        }
    }

}
