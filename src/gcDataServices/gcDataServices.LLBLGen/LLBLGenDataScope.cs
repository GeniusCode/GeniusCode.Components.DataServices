using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenDataScope : IDataScope
    {
        private readonly IDataAccessAdapter _adapter;
        private readonly ILinqMetaData _metaData;
        private readonly UnitOfWork2 _unitOfWork2;

        public LLBLGenDataScope(IDataAccessAdapter adapter, ILinqMetaData metaData)
        {
            _adapter = adapter;
            _metaData = metaData;


            QueryService = new LLBLGenQueryService(_metaData);

            _unitOfWork2 = new UnitOfWork2();
            CommandService = new LLBLGenCommandService(_unitOfWork2, _adapter);
        }


        public LLBLGenQueryService QueryService { get; private set; }
        public LLBLGenCommandService CommandService { get; private set; }

        ICommandService IDataScope.CommandService
        {
            get { return CommandService; }
        }

        IQueryService IDataScope.QueryService
        {
            get { return QueryService; }
        }
    }
}