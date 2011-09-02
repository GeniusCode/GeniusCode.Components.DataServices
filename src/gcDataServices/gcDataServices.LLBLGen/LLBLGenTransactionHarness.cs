using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public LLBLGenTransactionHarness(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractFactory, Func<IDataAccessAdapter> createDataAccessAdapterFunc, Func<ILinqMetaData> getMetaDataFunc)
            : base(abstractFactory)
        {
            _createDataAccessAdapterFunc = createDataAccessAdapterFunc;
            _getMetaDataFunc = getMetaDataFunc;
        }

        protected override void InitDataPointsOnScope(TScopeAggregate input)
        {
            var adapter = _createDataAccessAdapterFunc();
            var md = _getMetaDataFunc();

            var prop = md.GetType().GetProperty("AdapterToUse", BindingFlags.Instance | BindingFlags.Public);

            prop.SetValue(md,adapter,null);

            input.DataScope = new LLBLGenDataScope(adapter, md);
        }
    }
}
