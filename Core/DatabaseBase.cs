using Config.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration; // Added for reading appsettings.json

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core;

    /// <summary>   
    /// Base class for database access
    /// </summary>
    public class DatabaseBase
    {
        // Build configuration from appsettings.json once per AppDomain.
        private static readonly Lazy<IConfigurationRoot> __configuration = new(() =>
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(); // Allow overriding via env vars
            return builder.Build();
        });

        static DatabaseBase()
        {
        }

        public int Timeout { get; set; } = 3600;
        public string ConnectionString { get; set; } = "";

        protected DatabaseBase()
        {
        }

        protected DatabaseBase(string connectionStringKey)
        {
            // Read the connection string from appsettings.json (ConnectionStrings section)
            // Example appsettings.json:
            // "ConnectionStrings": { "MyDb": "Server=...;Database=...;Trusted_Connection=True;" }
            this.ConnectionString = __configuration.Value.GetConnectionString(connectionStringKey);

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new InvalidOperationException($"Connection string '{connectionStringKey}' not found in configuration.");
            }
        }


        /// <summary>
        /// Returns the database connection 
        /// </summary>
        /// <returns></returns>
        public SqlConnection ConnectionGet()
        {
            var connectionString = this.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Sproc parameter cache which maps sproc names to parameter name lists
        /// </summary>
        protected ConcurrentDictionary<string,List<string> >  _sprocParams = new ConcurrentDictionary<string,List<string>>();

        /// <summary>
        /// Reads a stored procedure and returns the parameter names
        /// </summary>
        /// <param name="storeProcedure"></param>
        /// <returns></returns>
        protected List<string> ParamtersNamesGet(string sprocName)
        {
            // Look in cache first
            List<string> retval  = this._sprocParams.GetOrAdd( sprocName, (KEY) =>
            {
                // Not in cache so read from database
                var retvalAdd = new List<string>();
                using (var con = this.ConnectionGet())
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = sprocName;
                        cmd.CommandTimeout = this.Timeout;
                        SqlCommandBuilder.DeriveParameters( cmd );

                        foreach (SqlParameter param in cmd.Parameters.OfType<SqlParameter>().Skip( 1 )) // Skip return value
                        {
                            retvalAdd.Add( param.ParameterName );
                        }
                    }
                }
                return retvalAdd;
            });

            return retval;
        }

        /// <summary>
        /// Reads the stored procedure parameter names and builds an array of SqlParameters from the provided values
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected virtual SqlParameter[] ParamatersGet(string sprocName, params object[] sqlParams)
        {
            // Get the parameter names for the stored procedure
            var names = this.ParamtersNamesGet(sprocName);
            if (names.Count != sqlParams.Length)
            {
                throw new ArgumentException( $"Database.ExecuteDataSet: Stored Procedure {sprocName} expects {names.Count} parameters but {sqlParams.Length} were provided." );
            }

            SqlParameter[] parameters = new SqlParameter[sqlParams.Length];
            for (int cLoop = 0; cLoop < sqlParams.Length; cLoop++)
            {
                var value = sqlParams[cLoop] ?? DBNull.Value;

                if (value is DataTable dt)
                {
                    var tvp = new SqlParameter( names[cLoop], SqlDbType.Structured ) { Value = dt };
                    parameters[cLoop] = tvp;
                }
                else
                    parameters[cLoop] = new SqlParameter( names[cLoop], value );
            }
            return parameters;
        }


        protected virtual void PrepareCommand(SqlCommand cmd)
        {
            // Override hook for custom behaviors (e.g. auditing).
        }



        //--------------------------------------------------------------------------




        /// <summary>
        /// Execute a stored procedure and return a DataSet
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(string query, params object[] sqlParams)
        {
            var cmdType = query.Trim().SectionCount( ' ' ) > 1 ? CommandType.Text : CommandType.StoredProcedure;

            DataSet retval = new DataSet();
            using (var con = this.ConnectionGet())
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = this.Timeout;

                    if (cmdType == CommandType.StoredProcedure)
                        cmd.Parameters.AddRange( this.ParamatersGet( query, sqlParams ) );

                    this.PrepareCommand( cmd );

                    using (SqlDataAdapter da = new SqlDataAdapter( cmd ))
                    {
                        da.Fill( retval );
                    }
                }
            }
            return retval;
        }

        /// <summary>
        /// Execute a stored procedure and return the number of rows affected
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string query, params object[] sqlParams)
        {
            int retval=0;
            var cmdType = query.Trim().SectionCount( ' ' ) > 1 ? CommandType.Text : CommandType.StoredProcedure;

            using (var con = this.ConnectionGet())
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = this.Timeout;

                    if (cmdType == CommandType.StoredProcedure)
                        cmd.Parameters.AddRange( this.ParamatersGet( query, sqlParams ) );

                    this.PrepareCommand( cmd );

                    retval = cmd.ExecuteNonQuery();

                }
            }
            return retval;
        }

        /// <summary>
        /// Execute a stored procedure and return a scalar value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string query, params object[] sqlParams)
        {
            var cmdType = query.Trim().SectionCount( ' ' ) > 1 ? CommandType.Text : CommandType.StoredProcedure;

            T retval=default;
            using (var con = this.ConnectionGet())
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = this.Timeout;

                    if (cmdType == CommandType.StoredProcedure)
                        cmd.Parameters.AddRange( this.ParamatersGet( query, sqlParams ) );

                    this.PrepareCommand( cmd );

                    retval = (T) cmd.ExecuteScalar();
                }
            }
            return retval;
        }

    }

