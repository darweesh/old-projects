using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web.Services;


/// <summary>
/// Summary description for Records
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Records : WebService {

	private readonly string[] FieldNames = {"id","patiantName","wieght","FileName","ImageName","Info","date"};

    [WebMethod]
    public DataTable FindAll(string patiantName,string orderField, bool orderDesc)
    {
    	try
    	{
			StringBuilder sql = new StringBuilder(128);

			sql.Append("SELECT ");
			sql.Append(String.Join(",", FieldNames));
			sql.Append(" FROM records ");

			sql.Append("WHERE patiantName LIKE @patiantName ");

			if (!String.IsNullOrEmpty(orderField) &&
				Array.IndexOf(FieldNames, orderField) > -1)
			{
				sql.Append("ORDER BY " + orderField);

				if (orderDesc)
				{
					sql.Append(" DESC");
				}
			}

			using(DbConnection cnn = Connect.CreateConnection())
			using(DbCommand command = cnn.CreateCommand())
			using(DbDataAdapter adapter = Connect.Factory.CreateDataAdapter())
			{
				command.CommandText = sql.ToString();

				Connect.CreateParameter(command, "@patiantName", "%" + patiantName + '%');

				DataTable result = new DataTable("Table0");
				adapter.SelectCommand = command;
				adapter.Fill(result);
				return result;
			}
		}
    	catch (Exception ex)
    	{
    		throw CreateSafeException(ex);
    	}
    }

	[WebMethod]
	public void Update(Decimal id,string patiantName,Decimal wieght,string FileName,string ImageName,string Info,string date)
	{
		try
		{
			string sql = @"
						UPDATE
							records
						SET
							patiantName = @patiantName,							wieght = @wieght,							FileName = @FileName,							ImageName = @ImageName,							Info = @Info,							date = @date						WHERE
							id = @id ";
		
			using(DbConnection cnn = Connect.CreateConnection())
			using(DbCommand command = cnn.CreateCommand())
			{
				command.CommandText = sql;
				Connect.CreateParameter(command, "@id", id);
				Connect.CreateParameter(command, "@patiantName", patiantName);
				Connect.CreateParameter(command, "@wieght", wieght);
				Connect.CreateParameter(command, "@FileName", FileName);
				Connect.CreateParameter(command, "@ImageName", ImageName);
				Connect.CreateParameter(command, "@Info", Info);
				Connect.CreateParameter(command, "@date", date);
				command.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			throw CreateSafeException(ex);
		}
	}

	[WebMethod]
	public void Insert(string patiantName,Decimal wieght,string FileName,string ImageName,string Info,string date)
	{
		try
		{
			string sql = @"
						INSERT INTO 
							records (
			patiantName,			wieght,			FileName,			ImageName,			Info,			date								)
						VALUES (
			@patiantName,			@wieght,			@FileName,			@ImageName,			@Info,			@date							) ";

			using (DbConnection cnn = Connect.CreateConnection())
			using (DbCommand command = cnn.CreateCommand())
			{
				command.CommandText = sql;

				Connect.CreateParameter(command, "@patiantName", patiantName);
				Connect.CreateParameter(command, "@wieght", wieght);
				Connect.CreateParameter(command, "@FileName", FileName);
				Connect.CreateParameter(command, "@ImageName", ImageName);
				Connect.CreateParameter(command, "@Info", Info);
				Connect.CreateParameter(command, "@date", date);
				command.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			throw CreateSafeException(ex);
		}
	}

	[WebMethod]
	public void Remove(Decimal id) 
	{
		try
		{
			string sql = @"
						DELETE
						FROM	records
						WHERE	id = @id ";

			using (DbConnection cnn = Connect.CreateConnection())
			using (DbCommand command = cnn.CreateCommand())
			{
				command.CommandText = sql;

				Connect.CreateParameter(command, "@id", id);

				command.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			throw CreateSafeException(ex);
		}
	}

	private Exception CreateSafeException(Exception ex)
	{
		Context.Trace.Write("Exception", "An unexpected error occurred processing request", ex);
		return new Exception("An unexpected error occurred and has been logged.");
	}
	
}

