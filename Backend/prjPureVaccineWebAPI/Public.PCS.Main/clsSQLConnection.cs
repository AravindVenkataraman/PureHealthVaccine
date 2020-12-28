using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Public.Library.ErrorHandeling;
using System.Configuration;

namespace Public.PCS.Main
{
    public class clsSQLConnection
    {
        #region Get SQL Connection
        public static string strsrvName = null; 
        public static string strUserid = null;
        public static string strUserPwd = null;
        public static string strdbName = null;
        #endregion

        #region Get SQL Connection
        public static SqlConnection getSQLConnection()
        {  
            SqlConnection sqlConnection = new SqlConnection(); 
            try
            {
                string[] strarry = CreateReadConfig();
                if (strarry != null && strarry.Length > 0)
                {
                    strsrvName = strarry[0];
                    strdbName = strarry[1];
                    strUserid = strarry[2];
                    strUserPwd = strarry[3]; 
                    if (!string.IsNullOrEmpty(strsrvName) && !string.IsNullOrEmpty(strdbName) && !string.IsNullOrEmpty(strUserid) && !string.IsNullOrEmpty(strUserPwd))
                    {
                        string sqlConnstr = string.Format("Server={0};Database={1};User Id={2};Password={3};",
                        strsrvName, strdbName, strUserid, strUserPwd);
                        
                            sqlConnection.ConnectionString = sqlConnstr; 

                        if (sqlConnection.State ==ConnectionState.Closed)
                        {
                                sqlConnection.Open();
                        }
                    }
                    else clsEvntvwrLogging.fnMsgWritter("Invalid configuration defined plese check web.config in application path.");
                }
                else
                {
                    clsEvntvwrLogging.fnMsgWritter("No configuration defined plese check web.config in application path.");
                }
            }
            catch (SqlException ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }

            return sqlConnection; 
        }

        #endregion

        private static string[] CreateReadConfig()
        {
            try
            {
                string[] strarry = new string[4];
                strarry[0] = System.Configuration.ConfigurationManager.AppSettings["dbserver"].Trim(); 
                strarry[1] = System.Configuration.ConfigurationManager.AppSettings["dbname"].Trim();
                strarry[2] = System.Configuration.ConfigurationManager.AppSettings["dbuid"].Trim();
                strarry[3] = System.Configuration.ConfigurationManager.AppSettings["dbpwd"].Trim();
                return strarry;
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return null;
        }
    }
}
