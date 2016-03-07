using DG.Data.Model;
using DG.DataModelSample.Model.Entity;

namespace DG.DataModelSample.Model
{
    public class BlogsRepository : GenericDataRepository<blogs, DGDataModelSampleModel>
    {
        public BlogsRepository() : base() { }
    }
}
