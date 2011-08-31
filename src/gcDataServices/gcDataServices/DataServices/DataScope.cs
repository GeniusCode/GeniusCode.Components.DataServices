namespace GeniusCode.Components.DataServices
{
    public class DataScope : IDataScope
    {
        public ICommandService CommandService { get; set; }
        public IQueryService QueryService { get; set; }
    }
}