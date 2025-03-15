
namespace Domain.Contracts
{
    public interface IDbInitializer
    {
        public Task InitializeIdentityAsync();
    }
}
