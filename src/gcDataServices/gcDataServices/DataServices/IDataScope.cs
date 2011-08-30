namespace GeniusCode.Components.DataServices
{
    public interface IDataScope
    {
        ICommandService CommandService { get; set; }
        IQueryService QueryService { get; set; }
    }
}