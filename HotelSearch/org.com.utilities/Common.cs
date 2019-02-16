using System;
using System.Configuration;
using HotelSearch.org.com.elements;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;

namespace HotelSearch.org.com.utilities
{
    public class Common
     {

        private XlsxReader sXL;
        public static Elements o = new Elements();


        public Common()
        {

        }

        #region for RunTestCase ...
        public Boolean RunTestCase(String strTCID)
         {
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            Boolean blnRunFlag = true;
            sXL = XlsxReader.getInstance();
            ADODB.Recordset rs = sXL.getRecordSet("select* from[TestCases$] where Execution = 'YES' and TCID = '" + strTCID + "'");
             while (rs.EOF == false)
             {
                string strTestCaseNo = Convert.ToString(rs.Fields["TCID"].Value);
                string strTestCaseName = Convert.ToString(rs.Fields["TestCaseName"].Value);
                HTML.fnInitilization(strTestCaseNo + "-" + strTestCaseName);

                //ConfigurationManager.AppSettings["TCID"] = strTestCaseNo;
                //ConfigurationManager.AppSettings["TestCaseName"] = strTestCaseName;
                cm.setProperty("TCID", strTestCaseNo);
                cm.setProperty("TestCaseName", strTestCaseName);

                for (int i = 0; i < rs.Fields.Count; i++)
                {
                    string strColumnName = rs.Fields[i].Name; 
                    if (strColumnName.Contains("Component"))
                    {
                            string strComponentName = Convert.ToString(rs.Fields[i].Value);
                        if ((!strComponentName.Equals("")))
                        {
                            //ConfigurationManager.AppSettings["ComponentName"] = strComponentName;
                            cm.setProperty("ComponentName", strComponentName);

                            HTML.fnInsertResult("Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), cm.getProperty("ComponentName") , strComponentName + " Component execution should be started", strComponentName + " Component execution has been started", "PASS");

                            string AssemblyName = "HotelSearch";
                            string typeName = "HotelSearch.org.com.screen.{0}, " + AssemblyName;

                            string strClassName = strComponentName;
                            string strCompoName = "SCR" + strComponentName;
                            try
                            {
                                string innerTypeName = string.Format(typeName, strClassName);
                                Type type1 = Type.GetType(innerTypeName);
                                object obj1 = Activator.CreateInstance(type1);
                                MethodInfo methodInfo1 = type1.GetMethod(strCompoName);
                                blnRunFlag = (Boolean)methodInfo1.Invoke(obj1, null);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                blnRunFlag = false;
                            }
                            if (blnRunFlag != true)
                            {
                                blnRunFlag = false;
                                break;
                            }
                            else
                            {
                                blnRunFlag = true;
                                HTML.fnInsertResult("Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), cm.getProperty("ComponentName"), strCompoName + " Component execution should be executed successfully", strCompoName + " Component  executed successfully", "PASS");
                            }
                        }
                    }
                }
                if (blnRunFlag)
                {
                    blnRunFlag = true;
                    HTML.fnSummaryInsertTestCase();
                    CommonManager.getInstance().GetCommon().Terminate();
                }
                else
                {
                    blnRunFlag = false;
                    HTML.fnInsertResult("Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), cm.getProperty("ComponentName") , cm.getProperty("ComponentName") + " Component should be executed successfully", cm.getProperty("ComponentName") + " Component not executed successfully", "FAIL");
                    HTML.fnSummaryInsertTestCase();
                    CommonManager.getInstance().GetCommon().Terminate();
                }
                rs.MoveNext();
             }
            rs.Close();
            return blnRunFlag;
         }
        #endregion
        #region for RunComponent...
        public Boolean RunComponent(String sheetname, Elements o)
        {
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            Boolean status = false;
            sXL = XlsxReader.getInstance();
            String sql = "select* from[" + sheetname + "$] where TCID = '" + cm.getProperty("TCID") + "'";
            ADODB.Recordset rs = sXL.getRecordSet(sql);
            while (rs.EOF == false)
            {
                for (int i = 0; i < rs.Fields.Count; i++)
                {
                    string strColumnName = rs.Fields[i].Name;
                    string strClassName = strColumnName.Substring(0, 3);
                    if (strClassName.Equals("ele") || strClassName.Equals("edt") || strClassName.Equals("btn") || strClassName.Equals("lst") || strClassName.Equals("fun") || strClassName.Equals("cfu"))
                    {
                        string strValue = rs.Fields[i].Value;
                        if ((!strValue.Equals("")))
                        {

                            status = SafeAction(o.getObject(strColumnName), strValue, strColumnName);
                        }
                        if (!status)
                        {
                            return false;
                        }
                    }
                }
                rs.MoveNext();
            }
            rs.Close();
            return status;
        }
        #endregion

        #region for SafeAction...
        public Boolean SafeAction(String element, String strValue, String ColumnName)
        {
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            Boolean blnRunFlag = true;
            UITestControl obj = null;
            String strActionType = null;
            Boolean blnDisabled = true;
            Common common = CommonManager.getInstance().GetCommon();
            string strClass = ColumnName.Substring(0, 3);
            //IJavaScriptExecutor js = (IJavaScriptExecutor)ManagerDriver.getInstance().GetDriver();

           
            //obj = ManagerDriver.getInstance().GetDriver().FindElement(element);
            obj = GetHtmlControlBySearchProperties(ManagerDriver.getInstance().GetDriver(), element);
            if (obj.WaitForControlReady(Convert.ToInt32(ConfigurationManager.AppSettings["LongSyncTime"])))
            //if (obj.Exists)
            {
            }
            else
            {
                HTML.fnInsertResult("Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), cm.getProperty("ComponentName"), ColumnName + " object should be displayed", ColumnName + " object is not displayed", "FAIL");
                blnRunFlag = false;
                return blnRunFlag;
            }
            if (strClass == "PGE" || strClass == "LNK" || strClass == "WEL" || strClass == "IMG")
            {
                //Skip
            }
            else
            {
                blnDisabled = obj.Enabled;
            }
            if (blnDisabled)
            {
                if (strClass == "edt")
                {
                    strActionType = "Text";
                }
                if (strClass == "lst")
                {
                    strActionType = "SelectedItem";
                }
                if (strClass == "rdo")
                {
                    strActionType = "Value";
                }
                if (strClass == "chk")
                {
                    strActionType = "Checked";
                }
                if (strClass == "btn" || strClass == "ele")
                {
                    Mouse.Click(obj);
                }
                else
                {
                    obj.SetProperty(strActionType, strValue);
                }
            }
            else
            {
                HTML.fnInsertResult("Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), cm.getProperty("ComponentName"), "Verification: " + ColumnName + " object should be enabled", "Verification: " + ColumnName + " object is disabled", "FAIL");
                blnRunFlag = false;
            }
            return blnRunFlag;
        }
        #endregion

        public void Terminate()
        {
            ManagerDriver.getInstance().GetDriver().Close();
        }


        public HtmlControl GetHtmlControlBySearchProperties(UITestControl parent, string propertyValuePairs)
        {
            if (parent == null || string.IsNullOrEmpty(propertyValuePairs) || string.IsNullOrWhiteSpace(propertyValuePairs))
            {
                //throw new Exception("Object Properties and Values can not be null, empty or white-space");
                ConfigurationManager.AppSettings["errMessage"] = "Object Properties and Values can not be null, empty or white-space";
            }

            string[] strProp = propertyValuePairs.Split((','));
            HtmlControl control = new HtmlControl(parent);

            foreach (string val in strProp)
            {
                string[] strPropVal = val.Split(':');
                control.SearchProperties.Add(strPropVal[0], strPropVal[1]);
            }

            return control;
        }

    }

}
