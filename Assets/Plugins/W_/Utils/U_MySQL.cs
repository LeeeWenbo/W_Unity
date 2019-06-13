using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

public class U_MySQL
{
    public static MySqlConnection cono;//连接类对象

    private static string host;     //IP地址。如果只是在本地的话，写localhost就可以。
    private static string id;       //用户名。
    private static string pwd;      //密码。
    private static string dataBase; //数据库名称。
    public static int port = 3306;

    public U_MySQL(string _url, int _port, string _user, string _password, string _dbName)
    {
        host = _url;
        port = _port;
        id = _user;
        pwd = _password;
        dataBase = _dbName;
    }

    public void OpenSql()
    {
        try
        {
            string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};", dataBase, host, id, pwd, port);
            cono = new MySqlConnection(mySqlString);
            cono.Open();
        }
        catch (Exception e)
        {
            throw new Exception("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString());
        }
    }

    public DataSet CreateTable(string name, string[] colName, string[] colType)
    {
        if (colName.Length != colType.Length)
        {
            throw new Exception("输入不正确：" + "columns.Length != colType.Length");
        }
        string query = "CREATE TABLE  " + name + "(" + colName[0] + " " + colType[0];
        for (int i = 1; i < colName.Length; i++)
        {
            query += "," + colName[i] + " " + colType[i];
        }
        query += ")";
        return QuerySet(query);
    }


    public DataSet CreateTableAutoID(string name, string[] col, string[] colType)
    {
        if (col.Length != colType.Length)
        {
            throw new Exception("columns.Length != colType.Length");
        }
        string query = "CREATE TABLE  " + name + " (" + col[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ", PRIMARY KEY (" + col[0] + ")" + ")";
        return QuerySet(query);
    }

    public DataSet InsertInfo(string tableName, string[] col, string[] values)
    {
        if (col.Length != values.Length)
        {
            throw new Exception("columns.Length != colType.Length");
        }
        string query = "INSERT INTO " + tableName + " (" + col[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i];
        }
        query += ") VALUES (" + "'" + values[0] + "'";
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + "'" + values[i] + "'";
        }
        query += ")";
        return QuerySet(query);
    }


    public DataTable GetTable(string tableName)
    {
        string query = "SELECT* from " + tableName;
        DataSet ds = QuerySet(query);
        return ds.Tables[0];
    }

    public bool IsExist(string tableName, string colName, string value)
    {
        string query = "SELECT id FROM " + tableName + " WHERE " + colName + "='" + value + "' LIMIT 1";
        //string query = "SELECT id FROM " + tableName + " WHERE " + colName + "='" + value + "' LIMIT 1";
        //string query = "SELECT* from " + tableName;
        //QuerySet(query);

        DataTable dt = QuerySet(query).Tables[0];
        if (dt.Rows.Count == 0)
        {
            return false;

        }
        else
            return true;
    }
    public DataRow GetExist(string tableName, string colName, string value)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + colName + "='" + value + "' LIMIT 1";
        DataTable dt = QuerySet(query).Tables[0];
        if (dt.Rows.Count == 0)
            return null;
        else
            return dt.Rows[0];
    }

    public string GetFromDT(DataRow dr, string colName)
    {
        int colIndex = dr.Table.Columns.IndexOf(colName);
        return dr[colIndex].ToString(); ;
    }
    public string GetFromDT(DataTable dt, int rowIndex, string colName)
    {
        int colIndex = dt.Columns.IndexOf(colName);
        return dt.Rows[rowIndex][colIndex].ToString();
    }

    public string GetFromDT(DataSet ds, int rowIndex, string colName)
    {
        DataTable dt = ds.Tables[0];
        int colIndex = dt.Columns.IndexOf(colName);
        return dt.Rows[rowIndex][colIndex].ToString();
    }

    public void Change(string tableName, string col, string value,string tjCol,string tjValue)
    {
        string query = "update " + tableName + " set " + col + " = " + value+" where "+ tjCol + " = "+ tjValue;
        //string query = "delete from " + tableName + " where " + col + " like '" + value + "'";
        QuerySet(query);
    }
    public void Delect(string tableName, string col, string value)
    {
        string query = "delete from " + tableName + " where " + col + " like '" + value + "'";
        //string query = "delete from " + tableName + " where " + col + " = " + value;
        QuerySet(query);
    }
    public void DELECT_ALL(string tableName)
    {
        string query = "truncate " + tableName;
        DataSet ds = QuerySet(query);
    }

    public string GetTableRowStr(string tableName, int hang = -1, string colMarkStr = "\t")
    {
        string result = "";
        DataTable dt = GetTable(tableName);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hang != -1 && i == hang)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    result += dt.Rows[i][col];
                    result += colMarkStr;
                }
            }
            else if (hang == -1 && i == dt.Rows.Count - 1)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    result += dt.Rows[i][col];
                    result += colMarkStr;
                }
            }
        }
        return result;
    }
    public string GetTableStr(string tableName, string colMarkStr = "\t", string rowMarkStr = "\n")
    {
        string result = "";
        DataTable dt = GetTable(tableName);
        foreach (DataRow row in dt.Rows)
        {
            foreach (DataColumn col in dt.Columns)
            {
                result += row[col];
                result += colMarkStr;
            }
            result += rowMarkStr;
        }
        return result;
    }


    public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
    {
        if (whereColName.Length != operation.Length || operation.Length != value.Length)
        {
            throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
        }
        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            query += "," + items[i];
        }
        query += "  FROM  " + tableName + "  WHERE " + " " + whereColName[0] + operation[0] + " '" + value[0] + "'";
        for (int i = 1; i < whereColName.Length; i++)
        {
            query += " AND " + whereColName[i] + operation[i] + "' " + value[i] + "'";
        }
        return QuerySet(query);
    }


    public static string RowToString(DataRow row, string markStr = "   ", int i = -1)
    {
        string resule = "";

        if (i == -1)
        {
            foreach (DataColumn col in row.Table.Columns)
            {
                resule += row[col];
                resule += markStr;
            }
        }

        return resule;
    }
    public DataTable Select(string tableName, string whereColName, string value, string tarColName = "*", string operation = "=")
    {
        string query = "SELECT " + tarColName + "  FROM  " + tableName + "  WHERE " + " " + whereColName + "=" + value;
        return QuerySet(query).Tables[0];
        //非空和没值，不对，这个返回原值，尽量不该他的，无论查到查不到都这个dt都非空，就直接返回。外边反正也得判断table一行二行,索性外边不再判断非空，外边判断值
    }


    public Dictionary<int, List<string>> QueryInfo(string sql, MySqlConnection con)
    {
        int indexDic = 0;
        int indexList = 0;
        Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
        MySqlCommand com = new MySqlCommand(sql, con);
        MySqlDataReader reader = com.ExecuteReader();
        while (true)
        {
            if (reader.Read())
            {
                List<string> list = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    list.Add(reader[indexList].ToString());
                    indexList++;
                }
                dic.Add(indexDic, list);
                indexDic++;
                indexList = 0;
            }
            else
            {
                break;
            }
        }
        return dic;
    }


    public DataSet UpdateInfo(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += ", " + cols[i] + " =" + colsvalues[i];
        }
        query += " WHERE " + selectkey + " = " + selectvalue + " ";
        return QuerySet(query);
    }



    public void DeleteInfo(string sql, MySqlConnection con)
    {
        MySqlCommand com = new MySqlCommand(sql, con);
        int res = com.ExecuteNonQuery();
    }


    public DataSet Delete(string tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " or " + cols[i] + " = " + colsvalues[i];
        }
        return QuerySet(query);
    }

    public void Close()
    {
        if (cono != null)
        {
            cono.Close();
            cono.Dispose();
            cono = null;
        }
    }
    public static DataSet QuerySet(string sqlString)
    {
        if (cono.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, cono);

                mySqlDataAdapter.Fill(ds);
                //U_Debug.LogStr(ds.Tables.Count.ToString());
            }
            catch (Exception e)
            {
                throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
            }
            finally
            {
            }
            return ds;
        }
        return null;
    }






    public static T ToModel<T>(DataRow dr) where T : new()
    {

        if (dr == null)
        {
            U_Debug.LogErrorStr("转Model时传入了空行");
            return new T();

        }

        T t = new T();

        PropertyInfo[] propertys = t.GetType().GetProperties();
        Type type = typeof(T);
        string tempName = "";
        foreach (PropertyInfo pi in propertys)
        {
            tempName = pi.Name;
            if (dr.Table.Columns.Contains(tempName))
            {
                if (!pi.CanWrite)
                    continue;
                object value = dr[tempName];
                if (value != DBNull.Value)
                {
                    pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                }
            }
        }
        return t;

    }

    public static List<T> ToModelS<T>(DataTable dt) where T : new()
    {
        List<T> ts = new List<T>();
        Type type = typeof(T);
        string tempName = "";
        foreach (DataRow dr in dt.Rows)
        {
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;
                if (dt.Columns.Contains(tempName))
                {
                    if (!pi.CanWrite)
                        continue;
                    object value = dr[tempName];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                }
            }
            ts.Add(t);
        }
        return ts;
    }
    public List<T> ToModelS<T>(string tableName) where T : new()
    {
        DataTable dt = GetTable(tableName);
        return ToModelS<T>(dt);
    }
}


