using System;
using GeniusCode.Components.DataServices.Transactions;
using GeniusCode.Components.Factories.DepedencyInjection;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenTransactionHarness<TScopeAggregate> : TransactionHarnessBase<TScopeAggregate> where TScopeAggregate : class, IScopeAggregate
    {
        private readonly Func<IDataAccessAdapter> _createDataAccessAdapterFunc;
        private readonly Func<ILinqMetaData> _getMetaDataFunc;
        private readonly FunctionMappingStore _mappingStore;


        /// <summary>
        /// Strongly typed method to create LLBLGenTransactionHarness
        /// </summary>
        /// <typeparam name="TMetadata">Type of LinqMetadata to use</typeparam>
        /// <typeparam name="TMappingStore">Type of MappingStore to use</typeparam>
        /// <param name="abstractFactory">instance of abstractfactory to use</param>
        /// <param name="createDataAccessAdapterFunc">function to create an instance of dataaccessadater</param>
        /// <returns></returns>
        public static LLBLGenTransactionHarness<TScopeAggregate> Create<TMetadata, TMappingStore>(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractFactory, Func<IDataAccessAdapter> createDataAccessAdapterFunc)
            where TMappingStore : FunctionMappingStore, new()
            where TMetadata : ILinqMetaData, new()
        {
            var t = new LLBLGenTransactionHarness<TScopeAggregate>(abstractFactory, createDataAccessAdapterFunc, () => new TMetadata(),
                                                                   new TMappingStore());

            return t;
        }

        /// <summary>
        /// Create an instance of an LLBLGenTransactionHarness
        /// </summary>
        /// <param name="abstractFactory"></param>
        /// <param name="createDataAccessAdapterFunc"></param>
        /// <param name="getMetaDataFunc"></param>
        /// <param name="mappingStore"></param>
        public LLBLGenTransactionHarness(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractFactory, Func<IDataAccessAdapter> createDataAccessAdapterFunc, Func<ILinqMetaData> getMetaDataFunc, FunctionMappingStore mappingStore)
            : base(abstractFactory)
        {
            _createDataAccessAdapterFunc = createDataAccessAdapterFunc;
            _getMetaDataFunc = getMetaDataFunc;
            _mappingStore = mappingStore;
        }

        protected override void InitDataPointsOnScope(TScopeAggregate input)
        {
            var adapter = _createDataAccessAdapterFunc();
            var md = _getMetaDataFunc();

            input.DataScope = new LLBLGenDataScope(adapter, md, _mappingStore);
        }
    }
}
