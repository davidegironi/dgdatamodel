using DG.Data.Model;
using DG.DataModelSample.Model.Entity;

namespace DG.DataModelSample.Model
{
    public class CommentsRepository : GenericDataRepository<comments, DGDataModelSampleModel>
    {
        public CommentsRepository() : base() { }
    }
}
