namespace NSE.Core.Data
{
    public interface IUnityOfWork
    {
        Task<bool> Commit();
    }
}
