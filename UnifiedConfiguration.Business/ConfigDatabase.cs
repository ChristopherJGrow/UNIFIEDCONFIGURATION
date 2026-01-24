using Config.Core;

namespace UnifiedConfiguration.Business;

public class ConfigDatabase : DatabaseBase
{
    static ConfigDatabase()
    {
    }

    protected ConfigDatabase() : base( "ConfigDataConnection" )
    {
    }

    static Lazy<ConfigDatabase> __Gist5Database = new Lazy<ConfigDatabase>( ()=> new ConfigDatabase() );
    public static ConfigDatabase Instance => __Gist5Database.Value;


}