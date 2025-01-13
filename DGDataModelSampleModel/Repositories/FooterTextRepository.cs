using DG.Data.Model;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Models;
#endif

namespace DG.DataModelSample.Model
{
    public class FooterTextRepository : GenericDataRepository<footertext, DGDataModelSampleModel>
    {
        public FooterTextRepository() : base() { }
    }
}
