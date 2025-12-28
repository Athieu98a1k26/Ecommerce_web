using Ecommerce.Debugging;

namespace Ecommerce
{
    public class EcommerceConsts
    {
        public const string LocalizationSourceName = "Ecommerce";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "979905d758ad441fbe1f2c719acb117c";
    }
}
