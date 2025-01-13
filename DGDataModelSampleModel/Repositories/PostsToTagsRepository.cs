using DG.Data.Model;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Models;
#endif
using System.Linq;

namespace DG.DataModelSample.Model
{
    public class PostsToTagsRepository : GenericDataRepository<poststotags, DGDataModelSampleModel>
    {
        public PostsToTagsRepository() : base() { }

        public override bool CanAdd(ref string[] errors, params poststotags[] items)
        {
            bool ret = base.CanAdd(ref errors, items);
            if (!ret)
                return ret;
            ret = Validate(ref errors, items);
            return ret;
        }

        public override bool CanUpdate(ref string[] errors, params poststotags[] items)
        {
            bool ret = base.CanUpdate(ref errors, items);
            if (!ret)
                return ret;
            ret = Validate(ref errors, items);
            return ret;
        }

        private bool Validate(ref string[] errors, params poststotags[] items)
        {
            bool ret = true;

            foreach (poststotags item in items)
            {
                //to not check external blogs_id key to throw exception

                try
                {
                    if (BaseModel.Posts.Find(item.posts_id) == null)
                    {
                        ret = false;
                        errors = errors.Concat(new string[] { "A Post must be selected." }).ToArray();
                    }
                }
                catch
                {
                    ret = false;
                    errors = errors.Concat(new string[] { "A Post must be selected." }).ToArray();
                }

                try
                {
                    if (BaseModel.Tags.Find(item.tags_id) == null)
                    {
                        ret = false;
                        errors = errors.Concat(new string[] { "A Tag must be selected." }).ToArray();
                    }
                }
                catch
                {
                    ret = false;
                    errors = errors.Concat(new string[] { "A Tag must be selected." }).ToArray();
                }

                if (!ret)
                    break;
            }

            return ret;
        }
    }
}
