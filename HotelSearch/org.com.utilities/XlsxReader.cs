using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelSearch.org.com.utilities
{
    class XlsxReader
    {

        private ADODB.Connection conn;

        private static XlsxReader xlsReader = null;

        private XlsxReader()
        {

        }

        public static XlsxReader getInstance()
        {
            if (xlsReader == null)
            {
                xlsReader = new XlsxReader(System.Configuration.ConfigurationManager.AppSettings["DataFolderPath"] + "\\" + System.Configuration.ConfigurationManager.AppSettings["DataFileName"] + ".xls");
            }
            return xlsReader;
        }


        /**
         * Purpose- Constructor to pass Excel file name
         * @param sFileName
         */
        private XlsxReader(String sFilePath)
        {

            try
            {
                if (File.Exists(sFilePath))
                {
                    conn = new ADODB.Connection();
                    string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sFilePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                    conn.Open(str, "", "", -1);

                }
                else
                {
                    //logger.error("Error Reading Excel File from Data Folder. Hence shutting down the application");

                    //Exception e = new Exception("File with name-'" + sFileName + "' doesn't exists in Data Folder");
                    //logger.error("Thread ID = " + Thread.CurrentThread + " Error Occured =" + e.getMessage(), e);
                    //System.exit(0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //printStackTrace();
                // logger.error("Error Reading Excel File from Data Folder. Hence shutting down the application");

                // Exception e1 = new Exception("File with name-'" + sFileName + "' doesn't exists in Data Folder");

                // logger.error("Thread ID = " + Thread.currentThread().getId() + " Error Occured =" + e1.getMessage(), e1);
                // System.exit(0);
            }
        }

        public ADODB.Recordset getRecordSet(String sql)
        {
            ADODB.Recordset rs = new ADODB.Recordset();
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockBatchOptimistic, 0);
            return rs;
        }

        /**
         * @function This method is used to retrieve all test cases from TestCase sheet name where execution is set as YES 
         * @param testCases
         * @return true/false
         * @throws Exception
         */
        public Boolean addTestCasesFromDataSheetName(Queue<String> testCases)
        {
            Boolean status = false;
            ADODB.Recordset rs = getRecordSet("select* from[TestCases$] where Execution = 'YES'");
            while (rs.EOF == false)
            {
                String strTCID = rs.Fields["TCID"].Value;
                Console.WriteLine(strTCID);
                testCases.Enqueue(strTCID);
                status = true;
                rs.MoveNext();
            }
            rs.Close();
            return status;
        }
    }
}