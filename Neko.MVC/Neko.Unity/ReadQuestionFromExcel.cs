using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Util.DataObject;

namespace Neko.Unity
{
    public static class ReadQuestionFromExcel
    {
        private static string _excelConn = "";
        private static string _excelConnFix = "";

        public static IEnumerable<DataSet> ReadExcelFile(string excelPath)
        {
            _excelConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0; HDR = No; IMEX = 1\";", excelPath);
            _excelConnFix = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR = No; IMEX = 1\";", excelPath);
            OleDbConnection conn = OpenConn(_excelConn);
            if(conn == null)
            {
                throw new Exception("无法读取Excel文件.请检查Excel文件格式是否合法");
            }
            var sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
            List<DataSet> result = new List<DataSet>();
            List<string> sheetCache = new List<string>();
            foreach (DataRow sheet in sheets.Rows)
            {
                string tableName = RowUtil.GetString(sheet, "TABLE_NAME").TrimEnd('_', '$');
                if (sheetCache.Contains(tableName))
                {
                    continue;
                }
                sheetCache.Add(tableName);
                DataSet ds = new DataSet(tableName);
                try
                {
                    string excelSql = $"select * from [{tableName}$]";
                    OleDbDataAdapter da = new OleDbDataAdapter(excelSql, conn);
                    da.Fill(ds);
                    result.Add(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        private static OleDbConnection OpenConn(string connStr)
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(connStr);
                conn.Open();
            }
            catch (OleDbException oe)
            {
                if (oe.Message.Contains("External table is not in the expected format."))
                {
                    conn = OpenConn(_excelConnFix);
                }
                else
                {
                    throw oe;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return conn;
        }
    }
}
