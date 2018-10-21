

namespace Xebia.CommonUtility.Interface
{
    public interface IVaultConfigProvider
    {
        VaultDataContext GetDatabaseConnectionAsync();
    }
}
