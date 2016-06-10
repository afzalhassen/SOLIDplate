namespace SOLIDplate.Application.Services.SignalR.Hubs.Interfaces
{
    public interface IEntityHubContextBroadcaster<in TEntity>
        where TEntity : class, new()
    {
        void NotifyAllOfNew(TEntity entity);
        void NotifyAllOfUpdate(TEntity entity);
        void NotifyAllOfDelete(int id);

        void NotifyAllExceptCallerOfNew(string connectionId, TEntity entity);
        void NotifyAllExceptCallerOfUpdate(string connectionId, TEntity entity);
        void NotifyAllExceptCallerOfDelete(string connectionId, int id);
    }
}