using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenDataScope : IDataScope
    {
        /// <summary>
        /// Creates an instance of an llblgen database using strongly typed arguements
        /// </summary>
        /// <typeparam name="TMetadata">type of LinqMetadata to use</typeparam>
        /// <typeparam name="TMappingStore">type of MappingStore to use</typeparam>
        /// <param name="adapter">existing adapter instance to use</param>
        /// <returns></returns>
        public static LLBLGenDataScope Create<TMetadata, TMappingStore>(IDataAccessAdapter adapter)
            where TMetadata : ILinqMetaData, new()
            where TMappingStore : FunctionMappingStore, new()
        {
            return new LLBLGenDataScope(adapter, new TMetadata(), new TMappingStore());
        }

        /// <summary>
        /// Create a new instance of a LLBLGen DataScope
        /// </summary>
        /// <param name="adapter">existing adapter instance</param>
        /// <param name="metaData">existing metadata instance</param>
        /// <param name="mappingStore">existing FunctionMappingStore instance</param>
        public LLBLGenDataScope(IDataAccessAdapter adapter, ILinqMetaData metaData, FunctionMappingStore mappingStore)
        {

            InitLinqMetadata(adapter, metaData, mappingStore);

            QueryService = new LLBLGenQueryService(metaData);

            var unitOfWork2 = new UnitOfWork2();
            CommandService = new LLBLGenCommandService(unitOfWork2, adapter);
        }

        /// <summary>
        /// Initialize the LinqMetadata object with an adapter instance, as well as a mapping store
        /// </summary>
        /// <param name="adapter">adapter instance to use</param>
        /// <param name="metaData">existing metadata object</param>
        /// <param name="mappingStore">mappingstore object</param>
        private static void InitLinqMetadata(IDataAccessAdapter adapter, ILinqMetaData metaData, FunctionMappingStore mappingStore)
        {
            var mdAsDynamic = metaData as dynamic;

            mdAsDynamic.AdapterToUse = adapter;

            if (mappingStore != null)
                mdAsDynamic.CustomFunctionMappings = mappingStore;
        }

        /// <summary>
        /// LLBLGen QueryService Instance
        /// </summary>
        public LLBLGenQueryService QueryService { get; private set; }

        /// <summary>
        /// LLBLGen CommandService instance
        /// </summary>
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