namespace GeniusCode.Components.DataServices
{
    public interface IDataScope
    {
        ICommandService CommandService { get; }
        IQueryService QueryService { get; }
    }
}