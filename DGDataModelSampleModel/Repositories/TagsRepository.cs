using DG.Data.Model;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Models;
#endif

namespace DG.DataModelSample.Model
{
    public class TagsRepository : GenericDataRepository<tags, DGDataModelSampleModel>
    {
        public TagsRepository() : base() { }
    }
}
