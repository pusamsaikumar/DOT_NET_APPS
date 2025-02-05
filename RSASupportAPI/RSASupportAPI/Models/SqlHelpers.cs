using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Dynamic;
using Amazon.S3.Model;

namespace RSASupportAPI.Models
{
    //public class SqlHelpers : IDisposable
    public class SqlHelpers : IAsyncDisposable

    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        public SqlHelpers(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        //public  void Dispose()
        //{
        //    if (_connection != null)
        //    {
        //        if(_connection.State != System.Data.ConnectionState.Closed)
        //        {
        //             _connection.Close();
        //        }
        //        _connection.Dispose();
        //        _connection = null;
        //    }
        //}

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    await _connection.CloseAsync();
                }
                _connection.Dispose();
                _connection = null;
            }
        }

        public async Task EnsureConnection()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }
            }
        }

        // sql command:
        public SqlCommand CreateCommand(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            var command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }



        // Create StoredProcedure command:

        //public   SqlCommand CreateStoredProcedureCommand(string storedProcedureName)
        //{

        //    return CreateCommand(storedProcedureName,CommandType.StoredProcedure);
        //}
        public SqlCommand CreateStoredProcedureCommand(string storedProcedureName)
        {
            //var command = _connection.CreateCommand();
            //command.CommandText = storedProcedureName;
            //command.CommandType = CommandType.StoredProcedure;
            //return command;
            var command = new SqlCommand(storedProcedureName, _connection);
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        // create command for select query execution:
        public SqlCommand CreateSelectQueryCommand(string query)
        {
            var command = new SqlCommand(query, _connection);
            return command;
        }


        // SqlDataAdapter and SqlDataReader
        // To Read Table Data using SqlDataAdapter and SqlDataReader for 


        public async Task<DataTable> ReadDataTableAsync(string procName, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

           
                await EnsureConnection();

            using (var command = CreateStoredProcedureCommand(procName))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            

            return dataTable;
        }

        public async Task<DataTable> ExecuteDataTableAsync(SqlCommand command)
        {

            DataTable dataTable = new DataTable();
            await EnsureConnection();
            using (var adapter = new SqlDataAdapter(command))
            {

                await Task.Run(() => adapter.Fill(dataTable));
                return dataTable;
            }
        }
        public async Task<DataTable> ReadDataTable(SqlCommand command)
        {

            DataTable dataTable = new DataTable();
            await EnsureConnection();
            using (var reader = await ExecuteReader(command))
            {
                dataTable.Load(reader);

                return dataTable;
            }
        }
        public async Task<DataSet> GetMultipleTablesData(string[] storedProcNames, SqlParameter[] parameters )
        { 
            await EnsureConnection();
            var dataSet = new DataSet();
            var tasks = storedProcNames.Select(async procName =>
            {
                using (var cmd = CreateStoredProcedureCommand(procName))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));

                        dataSet.Tables.Add(dataTable);


                    }
                }
            });

            await Task.WhenAll(tasks);

            return dataSet;
        }




        // To Read database records using SqlDataReader: for read single row and multiple rows and datarowcollections.
        // use following methods:
        public async Task<SqlDataReader> ExecuteReader(SqlCommand command)
        {
            // await EnsureConnection();
            //return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return await command.ExecuteReaderAsync();

        }

       public async Task<SqlDataReader> ExecuteReaderAsync(string storedProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();


            using (var command = CreateStoredProcedureCommand(storedProcName))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }


                return await ExecuteReader(command);
            }

            }

        

        public async Task<DataRow> GetSingleRow(string storedProcedureName, params SqlParameter[] parameters)
        {

            await EnsureConnection();


            using (var command = CreateStoredProcedureCommand(storedProcedureName))
            {
                command.Parameters.AddRange(parameters);

                using (var reader = await ExecuteReader(command))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    if (dataTable.Rows.Count > 0)
                    {
                        return dataTable.Rows[0];
                    }

                }
            }


            return null;
        }

        // table readers async multiple rows:
        public async Task<List<DataRow>> GetMultipleRowsdata(string storedProcedureName, SqlParameter[] parameters)
        {
            var result = new List<DataRow>();

            // Use the connection string to create a connection within the method
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Open the connection

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    //command.CommandTimeout = 180;
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);

                        if (dataTable.Rows.Count > 0)
                        {
                            // Convert the rows to a list of DataRow
                            result = dataTable.AsEnumerable().ToList();
                        }
                    }
                }
            }

            return result;
        }


        //  To read record data using select query  with single record:
        public async Task<DataRow> GetSelectQuerySingleDataRow(string query,SqlParameter[]? parameters = null )
        {
          
            await EnsureConnection();
            using (var cmd = CreateSelectQueryCommand(query)) { 
                if(cmd.Parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var reader = await ExecuteReader(cmd)) {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0) {
                        return dataTable.Rows[0];
                    }
                }
            }
            return null;
        }
        public async Task<DataTable> GetSelectQuerySingleDataTable(string query, SqlParameter[]? parameters = null)
        {
            var dataTable = new DataTable();

            await EnsureConnection();
            using (var cmd = CreateSelectQueryCommand(query))
            {
                if (cmd.Parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var reader = await ExecuteReader(cmd))
                {
                  
                    dataTable.Load(reader);
                
                }
            }
            return dataTable;
        }
        // single row dynamically:
        public async Task<dynamic> GetDynamicQueryTableData(string  query, SqlParameter[]? parameters = null)
        {
            var dataTable =await GetSelectQuerySingleDataTable(query, parameters);
            if (dataTable.Rows.Count > 0) { 
              DataRow row = dataTable.Rows[0];
                dynamic result = new ExpandoObject();
                var resultDictionary = (IDictionary<string, object>)result;
                dataTable.Columns
                    .Cast<DataColumn>()
                    .ToList()
                    .ForEach(column => resultDictionary[column.ColumnName] = row[column]);
                return result;


            }
            return null;
        }   
        // multiple rows dynamically:
        public async Task<List<dynamic>> GetDynamicQueryTableDataWithMultipleRows(string query, SqlParameter[]? parameters = null)
        {
            var dataTable = await GetSelectQuerySingleDataTable(query, parameters);
            if(dataTable.Rows.Count > 0)
            {
                var result = dataTable.AsEnumerable().
                    Select(
                    row =>
                    {
                        dynamic rowExpand = new ExpandoObject();
                        var rowDictionary = (IDictionary<string, object>)rowExpand;
                        dataTable.Columns .Cast<DataColumn>()
                        .ToDictionary(column => column.ColumnName, column => row[column] )
                        .ToList()
                        .ForEach(x => rowDictionary[x.Key] = x.Value);

                        return rowExpand;
                    }
                    
                    )
                    
                    .ToList();
                return result;

            }
            return null;
        }
        public async Task<Dictionary<string, object>> GetFirstRowAsDictionary(string query, SqlParameter[]? parameters = null)
        {
            var dataTable = await GetSelectQuerySingleDataTable(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                var result = dataTable.Columns.Cast<DataColumn>()   
                    .ToList()
                    .Select(column => new {column.ColumnName,value= row[column] })
                    .ToDictionary(x => x.ColumnName,y =>y.value);
                return result;
            }

            return null; // Or return an empty dictionary if preferred
        }
        public async Task<string> GetDynamicDataAsJson(string query, SqlParameter[]? parameters = null)
        {
            var results = new List<Dictionary<string, object>>();

            await EnsureConnection();

            using (var cmd = CreateSelectQueryCommand(query))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using (var reader = await ExecuteReader(cmd))
                {
                    if (reader.HasRows)
                    {
                        while ( reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object columnValue = reader.GetValue(i);
                                row[columnName] = columnValue;
                            }
                            results.Add(row); // Add the row to the results
                        }
                    }
                }
            }

            // Serialize the list of dictionaries to a JSON string
            return await Task.FromResult(JsonConvert.SerializeObject(results));
        }



        public async Task<List<DataRow>> GetSelectQueryMultipleRows(string query, SqlParameter[] parameters)
        {

            await EnsureConnection();

            var result = new List<DataRow>();

            using (var command = CreateSelectQueryCommand(query))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }


                using (var reader = await ExecuteReader(command))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        result = dataTable.AsEnumerable().Select((row) => row).ToList();
                    }

                }
            }

            return result;

        }

        public async Task<List<DataRow>> GetMultipleRowsData(string storedProcedureName, SqlParameter[] parameters)
        {
            await EnsureConnection();
            var result = new List<DataRow>();

            using (var command = CreateStoredProcedureCommand(storedProcedureName))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                try
                {
                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        do
                        {
                            if (!reader.IsClosed && reader.HasRows)
                            {
                                var dataTable = new DataTable();
                                dataTable.Load(reader);
                                result.AddRange(dataTable.AsEnumerable());
                            }
                        } while (!reader.IsClosed && reader.NextResult());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                }
            }

            return result;
        }

        public async Task<List<DataRow>> GetMultipleRows(string storedProcedureName, SqlParameter[] parameters)
        {

            await EnsureConnection();

            var result = new List<DataRow>();

            using (var command = CreateStoredProcedureCommand(storedProcedureName))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }


                using (var reader = await ExecuteReader(command))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        result = dataTable.AsEnumerable().Select((row) => row).ToList();
                    }

                }

            }

            return result;

        }
        public async Task<DataRowCollection> GetMultipleDataRows(string storedProcedureName, SqlParameter[] parameters)
        {

            await EnsureConnection();



            using (var command = CreateStoredProcedureCommand(storedProcedureName))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }


                using (var reader = await ExecuteReader(command))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    if (dataTable.Rows.Count > 0)
                    {
                        return dataTable.Rows;
                    }

                }
            }

            return null;

        }





        // ExecuteNonQuery: for Update and Insert Data Records
        // using following methods:
        public async Task<int> ExecuteNonQueryAsync(SqlCommand command)
        {
            await EnsureConnection();
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> InsertTable(string storedProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();
            using (var command = CreateStoredProcedureCommand(storedProcName))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                int rowAffected = await ExecuteNonQueryAsync(command);
                return rowAffected;

            }
        }

        public async Task<int> UpdateTable(string storedProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();
            using (var command = CreateStoredProcedureCommand(storedProcName))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                int rowAffected = await ExecuteNonQueryAsync(command);
                return rowAffected;

            }
        }



        // ExecuteScalar methods:

        //public async Task<object> ExecuteScalarAsync(SqlCommand command)
        //{
        //    await EnsureConnection();
        //    return await command.ExecuteScalarAsync();
        //}
        public async Task<String> ExecuteScalarString(SqlCommand command)
        {
            await EnsureConnection();

            return (string)await command.ExecuteScalarAsync();
        }
        public async Task<int> ExecuteScalarInt(SqlCommand command)
        {
            await EnsureConnection();
            return (int)await command.ExecuteScalarAsync();
        }
        public async Task<int> ExecuteIntScalar(string storeProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();
            using (var cmd = CreateStoredProcedureCommand(storeProcName))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                int variable = (int)await ExecuteScalarInt(cmd);
                return variable;
            }

        }

        public async Task<string> ExecuteStringScalar(string storeProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();
            using (var cmd = CreateStoredProcedureCommand(storeProcName))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                string variable = (string)await ExecuteScalarString(cmd);
                return variable;
            }

        }


        // IDataReader Methods:
        // use following methods:
        public async Task<IDataReader> ExecuteIDataReader(SqlCommand command)
        {
            await EnsureConnection();
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
        public async Task<IDataReader> IDataReaderAsync(string storeProcName, params SqlParameter[] parameters)
        {
            await EnsureConnection();
            using (var cmd = CreateStoredProcedureCommand(storeProcName))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return await ExecuteIDataReader(cmd);
            }
        }

        public async Task<DataRowCollection> IDataReaderMultiRow(string storeProcName, params SqlParameter[] parameters)
        {
            using (IDataReader reader = await IDataReaderAsync(storeProcName, parameters))
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable.Rows;

            }
        }
        public async Task<List<DataRow>> IDataReaderMultiRows(string storeProcName, params SqlParameter[] parameters)
        {
            using (IDataReader reader = await IDataReaderAsync(storeProcName, parameters))
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable.Rows.Cast<DataRow>().ToList();

            }
        }

        public async Task<List<IDataRecord>> IDataRecordsData(string storedProcName, params SqlParameter[] parameters)
        {
            var records = new List<IDataRecord>();
            await EnsureConnection();
            using (var cmd = CreateStoredProcedureCommand(storedProcName))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (IDataReader reader = await ExecuteIDataReader(cmd))
                {
                    while (reader.Read())
                    {
                        records.Add(reader);
                    }

                }
            }
            return records;
        }

    }
}
