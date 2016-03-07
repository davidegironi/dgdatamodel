using DG.Data.Model;
using DG.DataModelSample.Model.Entity;

namespace DG.DataModelSample.Model
{
    public class TagsRepository : GenericDataRepository<tags, DGDataModelSampleModel>
    {
        public TagsRepository() : base() { }
    }
}
