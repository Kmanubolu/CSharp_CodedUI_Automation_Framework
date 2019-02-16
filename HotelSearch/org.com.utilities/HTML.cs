using System;
using System.IO;
using System.Configuration;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Security.Principal;
using HotelSearch.org.com.utilities;

namespace HotelSearch.org.com.utilities
{
    class HTML
    {

        public static int g_iPass_Count = 0; //'Pass Count
        public static int g_iFail_Count;//=0; //'Fail Count

        public static int g_Total_TC;//=0;
        public static int g_Total_Pass;//=0;
        public static int g_Total_Fail;//=0;
        public static int g_Flag;//=0;
        public static int g_Flag1;//=0;
        public static  DateTimeOffset g_tSummaryStart_Time;// 'Start Time
        public static DateTimeOffset g_tSummaryEnd_Time; //'End Time
        public static int g_SummaryTotal_TC;//=0;
        public static int g_SummaryTotal_Pass;//=0;
        public static int g_SummaryTotal_Fail;//=0;
        public static int g_SummaryFlag = 0;

        public static void fnSummaryInitialization(string strSummaryReportName)
        {

            String SummaryFolderName = ConfigurationManager.AppSettings["ResultsFolderPath"];
            
            if (!Directory.Exists(SummaryFolderName))
            {
                Directory.CreateDirectory(SummaryFolderName);
            }
            String SummaryFileName = SummaryFolderName + "\\" + strSummaryReportName +DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".htm";

            ConfigurationManager.AppSettings["SummaryFileName"] = SummaryFileName;
            //ThreadCache.getInstance().setProperty("SummaryFileName", SummaryFileName)
            fnSummaryOpenHtmlFile(strSummaryReportName, SummaryFileName);
            fnSummaryInsertSection(SummaryFileName);
        }
        public static void fnSummaryOpenHtmlFile(string SummaryReportName, String SummaryFileName)
        {
            //Required
            g_SummaryTotal_TC = 0;
            g_SummaryTotal_Pass = 0;
            g_SummaryTotal_Fail = 0;
            g_SummaryFlag = 0;
            g_iPass_Count = 0;
            g_iFail_Count = 0;

            using (StreamWriter sw = File.AppendText(SummaryFileName))
            {
                sw.Write("<HTML><BODY><TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR COLS=2><TD BGCOLOR=WHITE WIDTH=6%><IMG SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD><TD WIDTH=94% BGCOLOR=WHITE><FONT FACE=VERDANA COLOR=NAVY SIZE=3><B>&nbsp;Automation Test Results<BR/><FONT FACE=VERDANA COLOR=SILVER SIZE=2>&nbsp; Date: " + DateTime.Now + "</BR>&nbsp;On Machine :" + ConfigurationManager.AppSettings["LocalHostName"] + "</B></FONT></TD><TD BGCOLOR=WHITE WIDTH=6%><IMG SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD></TR></TABLE>");
                sw.Write("<TABLE BORDER=0 BGCOLOR=BLACK CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR><TD BGCOLOR=#66699 WIDTH=15%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Module Name:</B></FONT></TD><TD COLSPAN=6 BGCOLOR=#66699 ><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>" + SummaryReportName + "</B></FONT></TD></TR>");
                sw.Write("</TABLE></BODY></HTML>");
                sw.Close();
            }
            g_tSummaryStart_Time = DateTime.Now;

        }

        public static void fnSummaryInsertSection(String SummaryFileName)
        {
            using (StreamWriter sw = File.AppendText(SummaryFileName))
            {
                sw.Write("<HTML><BODY><TABLE BORDER=1 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR COLS=6><TD BGCOLOR=#FFCC99 WIDTH=15><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Test Case ID</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=45%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Test Case Name</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=20%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Scenario Name</B></FONT></TD><TD BGCOLOR=#FFCC99 SIZE=2 WIDTH=10%><B>Time</B></FONT></TD><TD BGCOLOR=#FFCC99 SIZE=2 WIDTH=10%><B>Result</B></FONT></TD></TR>");
                sw.Close();
            }
        }

        
        public static void fnInitilization(string TestCaseName)
        {
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            String TestCaseFolderName = ConfigurationManager.AppSettings["ResultsFolderPath"];

            if (!Directory.Exists(TestCaseFolderName))
            {
                Directory.CreateDirectory(TestCaseFolderName);
            }
            String TestCaseFileName = TestCaseFolderName + "\\" + TestCaseName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_ThreadID-" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + ".htm";

            //ConfigurationManager.AppSettings["TestCaseFileName"] = TestCaseFileName;
            cm.setProperty("TestCaseFileName", TestCaseFileName);
            fnOpenHtmlFile(TestCaseName, TestCaseFileName);
            fnInsertSection(TestCaseFileName);
            fnInsertTestCaseName(TestCaseName,TestCaseFileName);
        }

        public static void fnOpenHtmlFile(string TestCaseName, String TestCaseFileName)
        {
            g_iPass_Count = 0;
            g_iFail_Count = 0;
            g_Total_TC = 0;
            g_Total_Pass = 0;
            g_Total_Fail = 0;
            g_Flag = 0;
            g_Flag1 = 0;

            ConfigManager cm = ThreadCache.getInstance().getConfigManager();

            using (StreamWriter sw = File.AppendText(TestCaseFileName))
            {
                sw.Write("<HTML><BODY><TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR COLS=2><TD BGCOLOR=WHITE WIDTH=6%><IMG SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD><TD WIDTH=94% BGCOLOR=WHITE><FONT FACE=VERDANA COLOR=NAVY SIZE=3><B>Automation Test Results<BR/><FONT FACE=VERDANA COLOR=SILVER SIZE=2>Date: " + DateTime.Now + "</BR>on Machine :" + ConfigurationManager.AppSettings["LocalHostName"] + "</B></FONT></TD><TD BGCOLOR=WHITE WIDTH=6%><IMG  SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD></TR></TABLE>");
                sw.Write("<TABLE BORDER=0 BGCOLOR=BLACK CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR><TD BGCOLOR=#66699 WIDTH=15%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>TCID and Name:</B></FONT></TD><TD COLSPAN=6 BGCOLOR=#66699 ><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>" + TestCaseName + "</B></FONT></TD></TR>");
                sw.Write("</TABLE></BODY></HTML>");
                sw.Close();
            }
            DateTime d  = DateTime.Now;
            cm.setProperty("TCStartTime", d.ToString());
        }

        public static void fnInsertSection(String TestCaseFileName)
        {
            using (StreamWriter sw = File.AppendText(TestCaseFileName))
            {
                sw.Write("<HTML><BODY><TABLE BORDER=1 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
                sw.Write("<TR COLS=6><TD BGCOLOR=#FFCC99 WIDTH=15%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>ThreadID</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=15%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Component</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=15%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Expected Value</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=25%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Actual Value</B></FONT></TD><TD BGCOLOR=#FFCC99 WIDTH=25%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Result</B></FONT></TD></TR>");
                sw.Close();
            }
        }

        public static void fnInsertTestCaseName(string sDesc, String TestCaseFileName)
        {
            g_Total_TC = g_Total_TC + 1;
            if (g_Flag1 != 0)
            {
                if (g_Flag == 0)
                {
                    g_Total_Pass = g_Total_Pass + 1;
                }
                else
                {
                    g_Total_Fail = g_Total_Fail + 1;
                }
            }
            g_Flag = 0;
        }

        public static void fnInsertResult(string sTestCaseName, string sDesc, string sExpected, string sActual, string sResult)
        {
	            g_Flag1=1;
            String g_iCapture_Count;
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            using (StreamWriter sw = File.AppendText(cm.getProperty("TestCaseFileName"))) 
            {
                if (sResult.ToUpper() == "PASS")
                {
                    g_iPass_Count = g_iPass_Count + 1;
                    if (ConfigurationManager.AppSettings["CaptureScreenShotforPass"].ToUpper() == "YES")
                    {
                        string I_sFile = "";
                        g_iCapture_Count = "Screen" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                        I_sFile = ConfigurationManager.AppSettings["ResultsFolderPath"] + "\\Screen" + g_iCapture_Count + ".jpeg";

                        Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                        Graphics graphics = Graphics.FromImage(bitmap as Image);
                        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                        bitmap.Save(I_sFile, ImageFormat.Jpeg);

                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=WINGDINGS SIZE=4>2></FONT><FONT FACE=VERDANA SIZE=2><A HREF='" + I_sFile + "'>" + sActual + "</A></FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=GREEN><B>" + sResult + "</B></FONT></TD></TR>");
                    }
                    else
                    {
                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sActual + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=GREEN><B>" + sResult + "</B></FONT></TD></TR>");
                    }
                }
                else if (sResult.ToUpper() == "FAIL")
                {
                    g_Flag = 1;
                    g_SummaryFlag = 1;
                    g_iFail_Count = g_iFail_Count + 1;
                    if (ConfigurationManager.AppSettings["CaptureScreenShotforFail"].ToUpper() == "YES")
                    {
                        string I_sFile = "";
                        g_iCapture_Count = "Screen" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                        I_sFile = ConfigurationManager.AppSettings["ResultsFolderPath"] + "\\Screen" + g_iCapture_Count + ".jpeg";
                        Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                        Graphics graphics = Graphics.FromImage(bitmap as Image);
                        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                        bitmap.Save(I_sFile, ImageFormat.Jpeg);

                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=WINGDINGS SIZE=4>2></FONT><FONT FACE=VERDANA SIZE=2><A HREF='" + I_sFile + "'>" + sActual + "</A></FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=RED><B>" + sResult + "</B></FONT></TD></TR>");
                    }
                    else
                    {
                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sActual + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=RED><B>" + sResult + "</B></FONT></TD></TR>");
                    }

                }
                else if (sResult.ToUpper() == "WARNING")
                {
                    if (ConfigurationManager.AppSettings["CaptureScreenShotforWarning"].ToUpper() == "YES")
                    {
                        string I_sFile = "";
                        g_iCapture_Count = "Screen" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                        I_sFile = ConfigurationManager.AppSettings["ResultsFolderPath"] + "\\Screen" + g_iCapture_Count + ".jpeg";

                        Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                        Graphics graphics = Graphics.FromImage(bitmap as Image);
                        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                        bitmap.Save(I_sFile, ImageFormat.Jpeg);

                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=WINGDINGS SIZE=4>2></FONT><FONT FACE=VERDANA SIZE=2><A HREF='" + I_sFile + "'>" + sActual + "</A></FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=GREEN><B>" + sResult + "</B></FONT></TD></TR>");
                    }
                    else
                    {
                        sw.Write("<TR COLS=5><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sTestCaseName + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA SIZE=2>" + sDesc + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sExpected + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=30%><FONT FACE=VERDANA SIZE=2>" + sActual + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE='WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=GREEN><B>" + sResult + "</B></FONT></TD></TR>");
                    }

                }
                sw.Close();
            }
        }

        public static void fnSummaryInsertTestCase()
        {
            ConfigManager cm = ThreadCache.getInstance().getConfigManager();
            g_SummaryTotal_TC = g_SummaryTotal_TC + 1;
            if (g_SummaryFlag == 0)
            {
                g_SummaryTotal_Pass = g_SummaryTotal_Pass + 1;
            }
            else
            {
                g_SummaryTotal_Fail = g_SummaryTotal_Fail + 1;
            }

            string strStatus = "";
            switch (g_SummaryFlag)
            {
                case 0:
                    strStatus = "Passed";
                    break;
                case 1:
                    strStatus = "Failed";
                    break;
                default:
                    strStatus = "Failed";
                    break;
            }
            DateTime TCEndTime = DateTime.Now;
            DateTime TCStartTime = Convert.ToDateTime(cm.getProperty("TCStartTime"));
            string intDateDiff = "";
            var diff = TCEndTime.Subtract(TCStartTime);
            intDateDiff = String.Format("{0}:{1}:{2}", diff.Hours + "-Hours", diff.Minutes + "-Minutes", diff.Seconds+ "-Seconds");

            using (StreamWriter sw = File.AppendText(ConfigurationManager.AppSettings["SummaryFileName"]))
            {
                if (strStatus.ToUpper() == "PASSED")
                {
                    sw.Write("<TR COLS=6><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + cm.getProperty("TCID") + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=45%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><A HREF='" + cm.getProperty("TestCaseFileName") + "'>" + cm.getProperty("TestCaseName") + "</A></FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=20%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + "Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + intDateDiff + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE=WINGDINGS 2' SIZE=5 COLOR=GREEN>P</FONT><FONT FACE=VERDANA SIZE=2 COLOR=GREEN><B>" + strStatus + "</B></FONT></TD></TR>");
                }
                else if (strStatus.ToUpper() == "FAILED")
                {
                    sw.Write("<TR COLS=6><TD BGCOLOR=#EEEEEE WIDTH=15%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + cm.getProperty("TCID") + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=45%><FONT FACE=VERDANA COLOR=BLACK SIZE=2><A HREF='" + cm.getProperty("TestCaseFileName") + "'>" + cm.getProperty("TestCaseName") + "</A></FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=20%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + "Thread ID - " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE=VERDANA COLOR=BLACK SIZE=2>" + intDateDiff + "</FONT></TD><TD BGCOLOR=#EEEEEE WIDTH=10%><FONT FACE=WINGDINGS 2' SIZE=5 COLOR=RED>O</FONT><FONT FACE=VERDANA SIZE=2 COLOR=RED><B>" + strStatus + "</B></FONT></TD></TR>");
                    g_SummaryFlag = 0;
                }
                sw.Close();
                
            }
        }

        public static void fnSummaryCloseHtml(string strRelease)
        {        
			        g_tSummaryEnd_Time = DateTime.Now;

                    string sTime = "";
                    var diff = g_tSummaryEnd_Time.Subtract(g_tSummaryStart_Time);
                    sTime = String.Format("{0}:{1}:{2}", diff.Hours + "-Hours", diff.Minutes + "-Minutes", diff.Seconds + "-Seconds");

	                 using (StreamWriter sw = File.AppendText(ConfigurationManager.AppSettings["SummaryFileName"]))
                    {
             		        sw.Write("<HTML><BODY><TABLE BORDER=1 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>");
			                sw.Write("<TR><TD BGCOLOR=#66699 WIDTH=15%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Release :</B></FONT></TD><TD COLSPAN=6 BGCOLOR=#66699><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>" + strRelease + "</B></FONT></TD></TR>");
			                sw.Write("<TR COLS=5><TD BGCOLOR=#66699 WIDTH=25%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Test Case Executed : " + g_SummaryTotal_TC + "</B></FONT></TD><TD BGCOLOR=#66699 WIDTH=15%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Test Cases Passed : " + g_SummaryTotal_Pass + "</B></FONT></TD><TD BGCOLOR=#66699 WIDTH=25%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B> Total Test Cases Failed : " + g_SummaryTotal_Fail + "</B></FONT></TD><TD BGCOLOR=#66699 WIDTH=25%><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Execution Time : " + sTime + " </B></FONT></TD></TR>");
			                sw.Write("</TABLE></BODY></HTML>");
			                sw.Close();
                     }
            if (ConfigurationManager.AppSettings["MailTo"] != "")
            {
                if (ConfigurationManager.AppSettings["SendMail"] == "YES")
                {
                    HTML.fnSendSummarySnapshotEmail();
                }
            }
        }

        public static void fnSendSummarySnapshotEmail()
        {
                    string strModuleName="CC";

                    string strTime = "";
                    g_tSummaryEnd_Time = DateTime.Now;
                    var diff = g_tSummaryEnd_Time.Subtract(g_tSummaryStart_Time);
                    strTime = String.Format("{0}:{1}:{2}", diff.Hours + "-Hours", diff.Minutes + "-Minutes", diff.Seconds + "-Seconds");

                        Outlook.Application objOutlook = new Outlook.Application();
                        // Create a new mail item.
                        Outlook.MailItem objMail = (Outlook.MailItem)objOutlook.CreateItem(Outlook.OlItemType.olMailItem);

				        objMail.To = ConfigurationManager.AppSettings["MailTo"].ToString();
                    
				        if( ConfigurationManager.AppSettings["MailCC"] !="")
                        {
						        objMail.CC = ConfigurationManager.AppSettings["MailCC"].ToString();
				        }
				        objMail.Subject = "Automation Execution Summary Snapshot - " + DateTime.Now;
				        //'html and body tage,virtical bar
				        string SHTML = "<HTML><BODY><BR><BR><TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=70%><TR><TD BGCOLOR=RED WIDTH=100%></TD></TR></TABLE><br>";
				        //'wellsfargo logo
				        SHTML = SHTML + "<TABLE WIDTH=70%><TR>";
				        SHTML = SHTML + "<TD BGCOLOR=WHITE WIDTH=6%><IMG SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD>";
				        SHTML = SHTML + "<TD WIDTH=88% BGCOLOR=WHITE align=center><table width=100%><TR><td align=center><FONT FACE=Calibri COLOR=Black SINZE=5><B>Automation Execution Summary Snapshot</B></FONT></td></tr><tr><table width=85% align=center><tr><td width=50% style='margin;0in;margin-bottom:.0001pt;text-align:left;font-size:10.5pt;color:black;mso-font-kerning:12.0pt'>" + strModuleName +"</B></FONT></td><td width=50% style='margin:0in;margin-bottom:.0001pt;text-align:right;font-size:10.5pt;color:black;mso-font-kerning:12.0pt'>" + ConfigurationManager.AppSettings["Release"] + "</B></FONT></td></tr></tr></table></table></TD>";
				        SHTML = SHTML + "<TD BGCOLOR=WHITE WIDTH=6%><IMG SRC='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRG63U9tJWVk8xUplMi6tr6WAlX4-FowUnkM81oMc3ry0oCDIcQ-g'></TD>";
				        SHTML = SHTML + "</TR></TABLE>";
				        //'Details Bar
				        SHTML =  SHTML + "<P style='margin:0in;margin-bottom:.0001pt;font-size:11.0pt;color:black;mso-font-kerning:12.0pt'>Here are the details of your batch executio:</p><BR>";
				        //'execution info
				        SHTML = SHTML + "<TABLE WIDTH=55%><tr width=100%><table width=95% align =center>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Total Number of Test Cases</td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + g_SummaryTotal_TC +"</td></tr></table><table width=100%><tr><table align=left height=10 width=90%><tr><td bgcolor=#C0C0C0 width=100%></td></tr></table></tr></table></TR>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Total Test Cases Passed:</font></td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + g_SummaryTotal_Pass +"</font></td></tr></table><table width=100%><tr><table align=left height=10 width=90%><tr><td bgcolor=#C0C0C0 width=100%></td></tr></table></tr></table></TR>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Test Cases Failed:</b></font></td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + g_SummaryTotal_Fail +"</b></font></td></tr></table><table width=100%><tr><table align=left height=10 width=90%><tr><td bgcolor=#C0C0C0 width=100%></td></tr></table></tr></table></TR>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Test Execution Time:</b></font></td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + strTime +"</b></font></td></tr></table><table width=100%><tr><table align=left height=10 width=90%><tr><td bgcolor=#C0C0C0 width=100%></td></tr></table></tr></table></TR>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Test Execution Time:</b></font></td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + DateTime.Now+"</b></font></td></tr></table><table width=100%><tr><table align=left height=10 width=90%><tr><td bgcolor=#C0C0C0 width=100%></td></tr></table></tr></table></TR>";
				        SHTML = SHTML + "<TR WIDTH=100><table width=100%><TR><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp; Executed By:</b></font></td><td width=50% height=15 style='font-size:9.0pt;color:black;mso-font-kerning:12.0pt'>&nbsp;" + WindowsIdentity.GetCurrent().Name + "</b></font></td></tr></table></TR>";
				        SHTML = SHTML + "</table></tr></TABLE><br>";

				        //'vertical bar, html,body close
				        SHTML = SHTML + "<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=70%><TR><TD BGCOLOR=RED WIDTH=100%></TD></TR></TABLE></BODY></HTML>";
				        objMail.HTMLBody = SHTML;
				        objMail.Attachments.Add(ConfigurationManager.AppSettings["SummaryFileName"]);
				        //objMail.Send();
                    }

        public static string fnSecondsToTime(int intSeconds)
        {
			        int hours, minutes, seconds;
			        hours = intSeconds / 3600;
			        intSeconds = intSeconds % 3600;
			        minutes = intSeconds / 60;
			        seconds = intSeconds % 60;
			        return hours + ":" + minutes + ":" + seconds;
        }

    }
}
