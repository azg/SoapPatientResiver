using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
//using MSXML2;
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.Win32;


namespace SoapPatientResiver
{
    public partial class Form1 : Form
    {
        string mBodypart1;
        string mBodypart2;
        bool mrbMondayUncheck;
        StreamWriter log2;
        StreamWriter log3;
        public Form1()
        {
            InitializeComponent();

            mBodypart1 = "<?xml version=\"1.0\"?>" +
                        "<SOAP-ENV:Envelope" +
                        " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"" +
                        " xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\"" +
                        " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                        " xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                        "<SOAP-ENV:Body>";
            mBodypart2 = "</SOAP-ENV:Body>" +
                                "</SOAP-ENV:Envelope>";
            mrbMondayUncheck = false;
            log2 = new StreamWriter("z_log2");
            log3 = new StreamWriter("z_log3");
            log2.AutoFlush = true;
            log3.AutoFlush = true;

            if(DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                txtFromDate.Text = DateTime.Today.AddDays(-3).ToString("dd.MM.yyyy 00:00");
            else if(DateTime.Today.DayOfWeek == DayOfWeek.Tuesday)
                txtFromDate.Text = DateTime.Today.AddDays(-1).ToString("dd.MM.yyyy 00:00");
            else 
                txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("dd.MM.yyyy 00:00"); 
        
        }

       

        private string GetWDSLResponse(string oRequest)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" +cmbMis.Text+ "/services/PatientPrinterService?wsdl");
            req.Headers.Add("SOAPAction", "");
            req.ContentType = "text/xml; charset=\"utf-8\"";
            req.ContentLength = oRequest.Length;
            req.Accept = "text/xml";
            req.Credentials = CredentialCache.DefaultNetworkCredentials;
            req.Method = "POST";
            req.KeepAlive = true;
            
            
            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(oRequest);
                }
            }
             
            //Gets the response
            
            WebResponse response = req.GetResponse();
        
           // WebResponse response2 = req.GetResponse();
            //Writes the Response
            Stream responseStream = response.GetResponseStream();

            byte[] data = new byte[5000];
            string txt = "";

            int readed;
            while ((readed = responseStream.Read(data, 0, data.Length)) > 0)
            {
                string appendtxt = ASCIIEncoding.UTF8.GetString(data, 0, readed);
                txt += appendtxt;
            }
            
            txt = txt.Replace("soap:Envelope", "soapEnvelope");
            txt = txt.Replace("ns1:", "ns1");
            txt = txt.Replace("ns2:", "ns2");
            txt = txt.Replace("xsi:", "xsi");
            txt = txt.Replace("soap:Body", "soapBody");
            txt = txt.Replace("xmlns:", "xmlns");
            return txt;
         }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\his-dv\PatientResiver");
            if (registryKey != null)
            {

                dbfpath.Text = registryKey.GetValue("filename").ToString();
                cmbMis.Text = registryKey.GetValue("mis").ToString();
            }

            string version = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            this.Text = "his-dv mis data extractor "+version;
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result==DialogResult.OK)
                        dbfpath.Text = folderBrowserDialog.SelectedPath;                
        }

        private string SoapGet(string itempath, XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode(itempath);
            if (node != null && node.InnerText != "") return node.InnerText;
            return System.DBNull.Value.ToString();
        }

        private void SetAllEntitysId(ref long[] allid,string squery, string itempath)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://"+cmbMis.Text+squery);
            WebResponse response = req.GetResponse();
            // WebResponse response2 = req.GetResponse();
            //Writes the Response
            Stream responseStream = response.GetResponseStream();
            byte[] data = new byte[5000];
            string txt = "";
            int readed;
            while ((readed = responseStream.Read(data, 0, data.Length)) > 0)
            {
                string appendtxt = ASCIIEncoding.UTF8.GetString(data, 0, readed);
                txt += appendtxt;
            }
            XmlDocument docPatients = new XmlDocument();
            docPatients.LoadXml(txt);
            XmlNodeList plist = docPatients.SelectNodes(itempath);
            if(plist.Count==0)
            {
                plist = docPatients.SelectNodes(itempath.Replace("//list/","//"));
                if (plist.Count == 0) return;
            }
            allid = new long[plist.Count];
            for(int j = 0; j<plist.Count; j++)
            {
                string t = plist.Item(j).InnerText;
                allid[j] = long.Parse(t);
            }
        }
        private XmlNodeList GetItemsListByQuery(string squery, string itempath)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Int64 st0 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + cmbMis.Text + squery);
            WebResponse response = req.GetResponse();
            // WebResponse response2 = req.GetResponse();
            //Writes the Response
            Stream responseStream = response.GetResponseStream();
            Int64 st1 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            byte[] data = new byte[5000];
            string txt = "";
            int readed;
            while ((readed = responseStream.Read(data, 0, data.Length)) > 0)
            {
                string appendtxt = ASCIIEncoding.UTF8.GetString(data, 0, readed);
                txt += appendtxt;
            }
            Int64 st2 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            XmlDocument docPatients = new XmlDocument();
            docPatients.LoadXml(txt);
            XmlNodeList plist = docPatients.SelectNodes(itempath);
            Int64 st3 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            if(plist.Count==0)
            {
                XmlNodeList plist2 = docPatients.SelectNodes(itempath.Replace("//list/", "//"));
                if (plist2.Count > 0) return plist2;
            }
            Int64 st4 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            log3.WriteLine((st1 - st0) + " " + (st2 - st1) + " " + (st3 - st2) + " " + (st4 - st3));
            return plist;
        }
        private void GetItemValueByQuery(ref string val, string squery, string itempath)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Int64 st0 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            XmlNodeList plist = GetItemsListByQuery(squery, itempath);
            Int64 st1 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            if(plist.Count > 0)
                val = plist.Item(0).InnerText;
            else
                val = null;
            Int64 st2 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
            log2.WriteLine((st1 - st0) + " " + (st2 - st1));
            stopWatch.Stop();
            
        }
        private void SetAllPatientsId(ref long[] allid)
        {
            SetAllEntitysId(ref allid, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.id%20FROM%20Patient%20p%20WHERE%20p.id>0","//list/long");
        }
        private void SetAllTicketsId(ref long[] allid)
        {
            SetAllEntitysId(ref allid, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.id%20FROM%20MedicalCase%20m%20WHERE%20m.id>0","//list/long");
        }
        private void btnDoDownload_Click(object sender, EventArgs e)
        {
            if (dbfpath.Text == "")
            {
                MessageBox.Show("Specify the file name plz!");
                return;
            }
            long[] patients = { };
            SetAllPatientsId(ref patients);

            progressBar.Visible = true;
            progressBar.Maximum = 60 + patients.Length;
            progressBar.Value = 0;
            
            DataTable texport = new DataTable("export");
            DataRow row;
            AddColumn(texport, "numberCard", "System.String");
            AddColumn(texport, "lastName", "System.String");
            AddColumn(texport, "firstName", "System.String");
            AddColumn(texport, "middleName", "System.String");
            AddColumn(texport, "sex", "System.Int16");
            AddColumn(texport, "birthDate", "System.DateTime");
            AddColumn(texport, "passportSeries", "System.String");
            AddColumn(texport, "passportNumber", "System.String");
            AddColumn(texport, "snils", "System.String");
            AddColumn(texport, "IsLocalTerritoryCode", "System.String");
            AddColumn(texport, "PaymanyCompanyCode", "System.String");
            AddColumn(texport, "policySeries", "System.String");
            AddColumn(texport, "policyNumber", "System.String");
            AddColumn(texport, "socialstatus", "System.String");
            AddColumn(texport, "workPlace", "System.String");
            AddColumn(texport, "position", "System.String");
            AddColumn(texport, "areacode", "System.String");
            AddColumn(texport, "cityarea", "System.String");
            AddColumn(texport, "cityname", "System.String");
            AddColumn(texport, "areaname", "System.String");
            AddColumn(texport, "homePhone", "System.String");
            AddColumn(texport, "datareg", "System.DateTime");
            AddColumn(texport, "mobilePhone", "System.String");
            AddColumn(texport, "profession", "System.String");
            

            progressBar.Increment(20);
            for (long i = 0; i < patients.Length; i++, progressBar.Increment(1))
            {
                long patientid = patients[i];
                string wdslMedCard =
                        "<m:getPatientMedicalCard xmlns:m=\"mis.patient\">" +
                        "<patientID>" + patientid + "</patientID>" +
                        "</m:getPatientMedicalCard>";
                string wdslgetPatient =
                       "<m:getPatient xmlns:m=\"mis.patient\">" +
                       "<patientId>" + patientid + "</patientId>" +
                       "</m:getPatient>";


                string oRequest = mBodypart1 + wdslMedCard + mBodypart2;
                string oRequest2 = mBodypart1 + wdslgetPatient + mBodypart2;


                string txt2, txt;

                try
                {
                    txt2 = GetWDSLResponse(oRequest2);
                    txt = GetWDSLResponse(oRequest);
                }
                catch (Exception) { continue; }

                XmlDocument docMedcard = new XmlDocument();
                XmlDocument docPatient = new XmlDocument();
                docMedcard.LoadXml(txt);
                docPatient.LoadXml(txt2);

                row = texport.NewRow();
                string itemsroot = "//soapEnvelope/soapBody/ns1getPatientMedicalCardResponse/MedicalCardResult";
                string itemsroot2 ="//soapEnvelope/soapBody/ns1getPatientResponse/patientResult";
                string tmp, tmp2;
                row["numberCard"] = SoapGet(itemsroot + "/numberCard", docMedcard);

                row["lastName"] = SoapGet(itemsroot + "/lastName", docMedcard);
                row["firstName"] = SoapGet(itemsroot + "/firstName", docMedcard);
                row["middleName"] = SoapGet(itemsroot + "/middleName", docMedcard);

                tmp  = SoapGet(itemsroot + "/birthDate", docMedcard);
                if(tmp!="") row["birthDate"]   = tmp;

                row["passportSeries"] = SoapGet(itemsroot + "/passportSeries", docMedcard);
                row["passportNumber"] = SoapGet(itemsroot + "/passportNumber", docMedcard);
                row["snils"] = SoapGet(itemsroot + "/snils", docMedcard);
                row["policyNumber"] = SoapGet(itemsroot + "/policyNumber", docMedcard);
                row["policySeries"] = SoapGet(itemsroot + "/policySeries", docMedcard);
                row["workPlace"] = SoapGet(itemsroot + "/workPlace", docMedcard);
                row["cityname"] = SoapGet(itemsroot2 + "/personalData/registration/cityCode/name", docPatient);
                tmp = SoapGet(itemsroot2 + "/registrationDate", docPatient);
                if (tmp != "") row["datareg"] = tmp;

                tmp = SoapGet(itemsroot2 + "/personalData/registration/areaCode/name", docPatient);
                if(tmp!="") row["areaname"] = tmp + " район";

                tmp = SoapGet(itemsroot2 + "/personalData/registration/fiasCode", docPatient);
                if(tmp!="") row["areacode"] = tmp.Substring(0, 8) + "00000";

                tmp = SoapGet(itemsroot2+"/personalData/homePhone", docPatient);
                tmp2 = SoapGet(itemsroot2+"/personalData/mobilePhone", docPatient);
                if (tmp != "") row["homePhone"] = tmp;
                else if (tmp2 != "") row["homePhone"] = tmp2;
                if (tmp != "" && tmp2 != "") row["mobilePhone"] = tmp2;
                
                tmp = SoapGet(itemsroot2 + "/personalData/sex/code", docPatient);
                if (tmp != "") row["sex"] = Int16.Parse(tmp);

                tmp = SoapGet(itemsroot2 + "/personalData/payment/company/code", docPatient);
                tmp2 = SoapGet(itemsroot2 + "/personalData/payment/territory/code", docPatient);
                if (tmp != "")
                {
                    row["PaymanyCompanyCode"] = tmp;
                    //string areacode = "1";
                    //else areacode = "0";
                    row["IsLocalTerritoryCode"] = tmp2;
                    /*else if (tmp == "025005" || tmp == "025003" || tmp == "025001")
                    {
                        row["policyNumber"] = null;
                        row["policySeries"] = null;
                        row["PaymanyCompanyCode"] = null;
                    }*/
                }
                tmp = SoapGet(itemsroot + "/socialStatusItem/code",docMedcard);
                if (tmp != null)
                {
                    if (tmp == "5") row["socialstatus"] = 1;
                    if (tmp == "6" || tmp == "2") row["socialstatus"] = 2;
                    if (tmp == "22" || tmp == "7") row["socialstatus"] = 22;
                    //row["socialstatus"]  = Int16.Parse(tmp);
                }
                
                tmp = SoapGet(itemsroot + "/position", docMedcard);
                tmp2 =  SoapGet(itemsroot + "/profession", docMedcard);
                if (tmp != null) row["position"] = tmp;
                else if (tmp2 != null) row["position"] = tmp2;
                if (tmp != null && tmp2 != null) row["profession"] = tmp2;
                
                texport.Rows.Add(row);
                patientid++;
            }
            progressBar.Increment(20);
            sbsit.utils.DBF.DataTableSaveToDBF(texport, dbfpath.Text, "export");
            progressBar.Increment(20);
            progressBar.Visible = false;
        }

        private void AddColumn(DataTable table, string colname, string coltype)
        {
            DataColumn column = new DataColumn(colname, System.Type.GetType(coltype));
            table.Columns.Add(column);
        }

        private void btnDownloadTalons_Click(object sender, EventArgs e)
        {

            if (dbfpath.Text == "")
            {
                MessageBox.Show("Specify the file name plz!");
                return;
            }
            
            
            progressBar.Visible = true;
            long[] Ticketsid = {};
            SetAllTicketsId(ref Ticketsid);
            progressBar.Maximum = 60 + Ticketsid.Length;
            progressBar.Value = 0;
            

            DataTable texport = new DataTable("export");
            DataRow row;

            AddColumn(texport, "NUSL", "System.Int64");
            AddColumn(texport, "CARD", "System.Int32");
            AddColumn(texport, "FAM", "System.String");
            AddColumn(texport, "IMYA", "System.String");
            AddColumn(texport, "OTCH", "System.String");
            AddColumn(texport, "POL", "System.Int16");
            AddColumn(texport, "DATE_BIRTH", "System.DateTime");
            AddColumn(texport, "KATEG", "System.Int16"); //1 - oms else 0 -??
            AddColumn(texport, "IST_FIN", "System.Int16"); //1 - oms else 8 -??  
            AddColumn(texport, "C_INSUR", "System.String");
            AddColumn(texport, "P_SER", "System.String");
            AddColumn(texport, "P_NUM", "System.String");
            AddColumn(texport, "FROM_FIRM", "System.Int16"); //ever null?
            AddColumn(texport, "PURP", "System.Int16");
            AddColumn(texport, "URGENT", "System.Int16"); //ever 1?
            AddColumn(texport, "date_in", "System.DateTime");
            AddColumn(texport, "date_out", "System.DateTime");
            AddColumn(texport, "Q_U", "System.Int16"); //ever 2?
            AddColumn(texport, "RESULT_ILL", "System.Int16"); //1 ,3 - other lpu (5), 2 - death (6 result), 5(10) 6(11)
            AddColumn(texport, "CODE_MES", "System.String"); //ever null
            AddColumn(texport, "RESULT_TRE", "System.Int16");  //Результат (1-4) 4 defoult
            AddColumn(texport, "C53.9", "System.String"); //DS
            AddColumn(texport, "CHAR_MAIN", "System.Int16"); //character
            AddColumn(texport, "VISIT_POL", "System.Int16"); //policlinic service place
            AddColumn(texport, "VISIT_HOM", "System.Int16"); //home service place
            AddColumn(texport, "VISIT_DS", "System.Int16"); 
            AddColumn(texport, "VISIT_HS", "System.Int16");
            AddColumn(texport, "NSNDHOSP", "System.String"); //number of referance
            AddColumn(texport, "TYPE_HOSP", "System.Int16"); //1 - hospitalization  0 - else
            AddColumn(texport, "SPECFIC", "System.Int16"); //48
            AddColumn(texport, "DOCFIC", "System.String"); //code
            AddColumn(texport, "TYPE_PAY", "System.Int16"); //1
            AddColumn(texport, "D_TYPE", "System.String"); //1  - only paspport 5 - no midlename 2 - doc is no pasport
            AddColumn(texport, "K_PR", "System.Int16");  // 1 - primary  2 - repeat
            AddColumn(texport, "VISIT_PR", "System.Int16");  // number of visits
            AddColumn(texport, "SMO", "System.String"); //company by smo
            AddColumn(texport, "NPR_MO", "System.String"); //ever null?
            AddColumn(texport, "PRVS", "System.Int16");  // ever 1128?
            AddColumn(texport, "IDDOKT", "System.String"); //ever lpu - doccode 
            AddColumn(texport, "VPOLIS", "System.Int16");  // 3 - new 1 - old null else
            AddColumn(texport, "NOVOR", "System.Int16"); //ever 0?
            AddColumn(texport, "OS_SLUCH", "System.Int16"); //ever 0?
            AddColumn(texport, "PROFIL", "System.Int16"); //ever 60?
            AddColumn(texport, "DET", "System.Int16"); //ever 0?
            AddColumn(texport, "VIDPOM", "System.Int16"); //ever 1?
            AddColumn(texport, "USL_OK", "System.Int16"); //ever 3?
            AddColumn(texport, "ISHOD", "System.String"); //"30" + 4, 3 (2 RESULT_TRE), 5 (3)
            AddColumn(texport, "RSLT", "System.String"); // "30" + 4(RESULT_ILL 1)  5 (3)
            AddColumn(texport, "DOCTYPE", "System.String"); //14 is passport
            AddColumn(texport, "DOCSER", "System.String"); //
            AddColumn(texport, "DOCNUM", "System.String"); //
            AddColumn(texport, "SNILS", "System.String"); //
            AddColumn(texport, "UR_MO", "System.String"); //code of register of medical organization
            AddColumn(texport, "OKATO_OMS", "System.String"); //code of ocato of register of medical organization
            



            /*
            AddColumn(texport, "Visits", "System.String");
            AddColumn(texport, "resultReference", "System.String");
            AddColumn(texport, "servicePlace", "System.String");
            AddColumn(texport, "medicalCaseResult", "System.String");
            AddColumn(texport, "receptionType", "System.String");
            AddColumn(texport, "dispensary", "System.String");
            AddColumn(texport, "hospitalizationItem", "System.String");
            AddColumn(texport, "diagnosisMKB", "System.String");
            AddColumn(texport, "CharacterCode", "System.String");
            AddColumn(texport, "referralNumber", "System.String");
            AddColumn(texport, "signDate", "System.DateTime");*/
            
            progressBar.Increment(26);
            for (long i = 0; i < Ticketsid.Length; i++,progressBar.Increment(1) )
            {
                long medicalCaseId = Ticketsid[i];
                string tmp="";
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.id%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//long");
                long patientId;
                if(tmp!=null)
                    patientId = long.Parse(tmp);

                string wdslMedicalCase = "<m:getMedicalCase xmlns:m=\"mis.patient\">" +
                                         "<medicalCaseId>" + medicalCaseId + "</medicalCaseId>" +
                                            "</m:getMedicalCase>";
                string wdslMDCaseVisits = "<m:getVisitsByMedicalCase xmlns:m=\"mis.patient\">" +
                                            "<medicalCaseId>" + medicalCaseId + "</medicalCaseId>" +
                                            "</m:getVisitsByMedicalCase>";
                
                string oRequest = mBodypart1 + wdslMedicalCase + mBodypart2;
                string oRequest2 = mBodypart1 + wdslMDCaseVisits + mBodypart2;
                string txt = "", txt2 = "";
                try
                {
                    txt = GetWDSLResponse(oRequest);
                    txt2 = GetWDSLResponse(oRequest2);
                }
                catch (Exception) { continue; }

                XmlDocument docMedTicket = new XmlDocument();
                docMedTicket.LoadXml(txt);
                XmlDocument docMedVisits = new XmlDocument();
                docMedVisits.LoadXml(txt2);
        
                row = texport.NewRow();

                string itemsroot = "//soapEnvelope/soapBody/ns1getMedicalCaseResponse/MedicalCaseDTOResult";
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.code%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                int nuslbegin = 102000;
                if (tmp != null)
                    row["NUSL"] = nuslbegin+System.Int64.Parse(tmp);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.code%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if(tmp!=null)
                {
                    int islash = tmp.IndexOf("/");
                    tmp = tmp.Substring(islash);
                    row["CARD"] = System.Int32.Parse(tmp);
                }
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.lastName%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                row["FAM"] = tmp;
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.firstName%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                row["IMYA"] = tmp;
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.middleName%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                row["D_TYPE"] = "";
                if(tmp!=null)
                    row["OTCH"] = tmp;
                else
                    row["D_TYPE"] = "5";
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.sex.code%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                row["POL"] = System.Int16.Parse(tmp);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.birthDate%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//gregorian-calendar/time");
                if (tmp != null)
                {
                    System.DateTime s = new DateTime(1970, 1, 1, 12, 0, 0);
                    s = s.AddMilliseconds(double.Parse(tmp));
                    row["DATE_BIRTH"] = s;
                }
                bool no_oms = true;      
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.payment.company.code%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if (tmp != null)
                {
                    //row["PaymanyCompanyCode"] = tmp;
                    no_oms = false;
                    if (tmp == "025006") row["C_INSUR"] = "011"; //cut "25"
                    else if (tmp == "025007") row["C_INSUR"] = "012";
                    else if (tmp == "025004") row["C_INSUR"] = "016";
                    else if (tmp == "025005") row["C_INSUR"] = "005";
                    else if (tmp == "025001") row["C_INSUR"] = "001";
                    else if (tmp == "025003") row["C_INSUR"] = "003";
                    else
                        no_oms = true;
                    if (!no_oms)
                        row["SMO"] = "25" + row["C_INSUR"];
                    /*else if (tmp == "025005" || tmp == "025003" || tmp == "025001")
                    {
                        row["policyNumber"] = null;
                        row["policySeries"] = null;
                        row["PaymanyCompanyCode"] = null;
                    }*/
                }
        /*
                string sign = SoapGet(itemsroot + "/signDate", docMedTicket);
                if (sign == null) continue;
                row["signDate"] = sign;
         */
                row["P_SER"] = SoapGet(itemsroot + "/payment/series", docMedTicket);
                row["P_NUM"] = SoapGet(itemsroot + "/payment/number", docMedTicket);

                tmp = SoapGet(itemsroot + "/purposeVisiting/code", docMedTicket);
                if (tmp != null)
                    row["PURP"] = System.Int16.Parse(tmp);
                row["URGENT"] = 1;
                //dates here
                XmlNodeList nlist = docMedVisits.SelectNodes("/soapEnvelope/soapBody/ns1getVisitsByMedicalCaseResponse/VisitsByMedicalCaseResult/item/date");
                if (nlist.Count > 0)
                {
                    System.DateTime min = new DateTime();
                    System.DateTime max = new DateTime();
                    min = System.DateTimeOffset.Parse(nlist.Item(0).InnerText).Date;
                    max = System.DateTimeOffset.Parse(nlist.Item(0).InnerText).Date;
                    String Visits = "";
                    for (int j = nlist.Count - 1; j > 0; j--)
                    {
                        System.DateTimeOffset a = System.DateTimeOffset.Parse(nlist.Item(j).InnerText);
                        if (j < nlist.Count - 1) Visits += ", ";
                        Visits += a.ToString("dd.MM.yyyy г. H:mm");
                        if (a.Date < min) min = a.Date;
                        if (a.Date > max) max = a.Date;

                    }
                    row["DATE_IN"] = min;
                    row["DATE_OUT"] = max;
                    row["VISIT_PR"] = nlist.Count;
                  //  row["Visits"] = Visits;
                }
                row["Q_U"] = 2;

                tmp = SoapGet(itemsroot + "/resultReference/code", docMedTicket);
                System.Int16 res_trea = 4;
                System.Int16 res_ill = 1;
                if (tmp != null) res_trea = System.Int16.Parse(tmp);
                string hospitalizationItem = SoapGet(itemsroot + "/hospitalizationItem/code", docMedTicket);
                if (hospitalizationItem != null &&
                    hospitalizationItem != "" &&
                    hospitalizationItem.IndexOf("1.2.643.5.1.13.3.25.25.90") < 0)
                    res_ill = 3;
                
                if (res_trea == 6)
                    res_ill = 2;
                else if (res_trea == 10)
                    res_ill = 5;
                else if (res_trea == 11)
                    res_ill = 6;
                
                row["RESULT_ILL"] = res_ill;
                row["RESULT_TRE"] = res_trea;

                row["DS_CLIN"] = SoapGet(itemsroot + "/diagnosis/clinicalDiagnosis/diagnosisMKB", docMedTicket);
                row["CHAR_MAIN"] = SoapGet(itemsroot + "/diagnosis/clinicalDiagnosis/diagnosisCharacterCode", docMedTicket);

                tmp = SoapGet(itemsroot + "/servicePlace/code", docMedTicket);
                if(tmp=="1")
                {
                    row["VISIT_POL"] = 1;
                    row["VISIT_HOM"] = 0;
                }
                else
                {
                    row["VISIT_POL"] = 0;
                    row["VISIT_HOM"] = 1;
                }
                row["VISIT_DS"] = 0;
                row["VISIT_HS"] = 0;

                tmp = SoapGet(itemsroot + "/referralNumber", docMedTicket);
                if (tmp != null)
                    row["NSNDHOSP"] = tmp;

                if (tmp != null || hospitalizationItem != null)
                    row["TYPE_HOSP"] = 1;
                else
                    row["TYPE_HOSP"] = 0;

                row["SPECFIC"] = 48;
                tmp = SoapGet(itemsroot + "/author/doctorCode", docMedTicket);
                tmp.Replace("48", "");
                row["DOCFIC"] = tmp;

                row["TYPE_PAY"] = 1;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.identityCard.identityType.code%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                string doct = tmp;
                if (tmp == "21" && no_oms)
                    row["D_TYPE"] = row["D_TYPE"].ToString() + "1";
                if(tmp != null && tmp!="21")
                    row["D_TYPE"] = row["D_TYPE"].ToString() + "2";

                tmp = SoapGet(itemsroot + "/receptionType/code", docMedTicket);
                if (tmp != null)
                    row["K_PR"] = System.Int16.Parse(tmp);
                else
                    row["K_PR"] = 1;

                row["PRVS"] = 1128;

                row["UR_MO"] = "250160";
                row["IDDOKT"] = row["UR_MO"].ToString() + "-" + row["SPECFIC"].ToString() + row["DOCFIC"].ToString();

                if (row["P_NUM"] != null && row["P_NUM"].ToString().Length == 16)
                    row["VPOLIS"] = 3;
                else
                    row["VPOLIS"] = 1;
                row["NOVOR"] = 0;
                row["OS_SLUCH"] = 0;
                row["PROFIL"] = 60;
                row["DET"] = 0;
                row["VIDPOM"] = 1;
                row["USL_OK"] = 3;

                if (row["RESULT_TRE"].ToString() == "2")
                    row["ISHOD"] = 303;
                else if (row["RESULT_TRE"].ToString() == "3")
                    row["ISHOD"] = 305;
                else if (row["RESULT_TRE"].ToString() == "4")
                    row["ISHOD"] = 304;

                if (row["RESULT_ILL"].ToString() == "1")
                    row["ISHOD"] = 304;
                else if (row["RESULT_ILL"].ToString() == "3")
                    row["ISHOD"] = 305;

                if (doct == "21")
                    row["DOCTYPE"] = "14";

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.identityCard.series%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if(tmp!=null)
                    row["DOCSER"] = tmp;
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.identityCard.series%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if (tmp != null)
                    row["DOCSER"] = tmp;
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.identityCard.number%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if (tmp != null)
                    row["DOCNUM"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20m.history.patient.personalData.snils%20FROM%20MedicalCase%20m%20WHERE%20m.id=" + medicalCaseId, "//string");
                if (tmp != null)
                    row["SNILS"] = tmp;
                row["OKATO_OMS"] = "05000";

                //row["T"] = 0;
               // row["medicalCaseResult"] = SoapGet(itemsroot + "/medicalCaseResult/code", docMedTicket);
                
                //row["dispensary"] = SoapGet(itemsroot + "/receptionType/code", docMedTicket);
                
                
                
                


                texport.Rows.Add(row);
            }
            progressBar.Increment(17);
            sbsit.utils.DBF.DataTableSaveToDBF(texport, dbfpath.Text, "tickets");
            
            progressBar.Increment(17);
            progressBar.Visible = false;
        }

        private void btnHQLTest_Click(object sender, EventArgs e)
        {

        }
        private string nz(string val)
        {
            if (val == null) return "";
            return val;
        }
       private class ReportItem
        {
            public int alreadyexist_count;
            public int newpatients_count;
            public int out_count;
            public int free_beds;
            public ReportItem(int ex, int newc, int outc) 
            {
                alreadyexist_count = ex;
                newpatients_count = newc;
                out_count = outc;
            }
        }
        public static int m = 0;
        private void btn_csv_export_Click(object sender, EventArgs e)
       {
            while (dbfpath.Text == "")
               btnBrowse_Click(sender, e);
           if (dbfpath.Text.Length > 3 && dbfpath.Text.Last() != '\\')
               dbfpath.Text += "\\";
           if (csvto5.Checked)      // чекбокс выгрузки одного за 3 дня и 2х за день
            {
                m = 1;
                for (int i = -2; i < 1; i++)
                {
                    GenerateOneCSV(DateTime.Today.AddDays(i), dbfpath.Text);
                    m = 0;
                }
            }
            else if (csvto2.Checked) //чекбокс выгрузки за 3 дня одним файлом
            {
                m = 1;
                GenerateOneCSV(DateTime.Today, dbfpath.Text);
            }
            else if (rbMonday.Checked && !(rbMonday.Checked = false)) //чекбокс выгрузки за 3 дня;
                for (int i = -2; i < 1; i++)
                    GenerateOneCSV(DateTime.Today.AddDays(i), dbfpath.Text);
            else if (rbVoluntaryDay.Checked)                    //чекбокс выгрузки по необходимой дате;
            {
                var picker = new DateTimePicker();
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = "dd-MM-yyyy г.";
                picker.Width = 110;
                picker.Location = new Point(63, 10);

                Button buttonOk = new Button();
                buttonOk.Text = "Ok";
                buttonOk.Width = 110;
                buttonOk.Location = new Point(63, 35);
                buttonOk.DialogResult = DialogResult.OK;

                Form f = new Form();
                f.Width = 270;
                f.Height = 100;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                //f.StartPosition = FormStartPosition.CenterScreen;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.Controls.Add(picker);
                f.Controls.Add(buttonOk);

                DialogResult result;
                do
                {
                    result = f.ShowDialog();

                } while (result != DialogResult.OK);

                DateTime dvoluntary = new DateTime(picker.Value.Year, picker.Value.Month, picker.Value.Day, 9, 0, 0);

                GenerateOneCSV(dvoluntary, dbfpath.Text);
                rbVoluntaryDay.Checked = false;
            }
            else
                GenerateOneCSV(DateTime.Today, dbfpath.Text);
       }
        private void GenerateOneCSV(DateTime ctoday, string cpath)
        {

            cpath += ctoday.ToString("dd.MM.yyyy") + "\\";
            System.IO.Directory.CreateDirectory(cpath);

            progressBar.Value = 0;
            progressBar.Maximum = 100;
            progressBar.Visible = true;
            
            //0,1
            DateTime ctime = ctoday;
            DateTime xtime = new DateTime(ctime.Year, ctime.Month, ctime.Day, 20, 0, 0);
            int result = DateTime.Compare(ctime, xtime);
            
            string sbegin, send;

/*            if (result <= 0)
            {
                if (m == 1)
                {
                    DateTime yesterday = ctoday.AddDays(-1);
                    send = yesterday.ToString("dd-MM-yy 20:00");
                    DateTime daybeforeyesterday = ctoday.AddDays(-4);
                    sbegin = daybeforeyesterday.ToString("dd-MM-yy 20:00");
                }
                else
                {
                    DateTime yesterday = ctoday.AddDays(-1);
                    send = yesterday.ToString("dd-MM-yy 20:00");
                    DateTime daybeforeyesterday = ctoday.AddDays(-2);
                    sbegin = daybeforeyesterday.ToString("dd-MM-yy 20:00");
                }
            }
            else
            {*/
                if (m == 1)
                {
                    DateTime yesterday = ctoday;
                    send = yesterday.ToString("dd-MM-yy 20:00");
                    DateTime daybeforeyesterday = ctoday.AddDays(-3);
                    sbegin = daybeforeyesterday.ToString("dd-MM-yy 20:00");
                }
                else
                {
                    DateTime yesterday = ctoday;
                    send = yesterday.ToString("dd-MM-yy 20:00");
                    DateTime daybeforeyesterday = ctoday.AddDays(-1);
                    sbegin = daybeforeyesterday.ToString("dd-MM-yy 20:00");
                }
//            }
            send = send.Replace(" ", "%20");
            sbegin = sbegin.Replace(" ", "%20");
            string CSVfile="";
            string tmp="";
            string filename = "";
            progressBar.Value += 10; //Progress bar
            //0,4
            for (long jo = 0; jo < 2; jo++)
            {
                string[] common_queries = new string[2];
                string[] csv_init = new string[2];
                //planing hospitalizations
                common_queries[0] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.id%20FROM%20InPatientHistory%20p%20WHERE%20p.startDate%3E%27" + sbegin + "%27%20AND%20p.startDate%20%3C%20%27" + send + "%27%20AND%20p.receptionCondition.urgentIndication.id=2%20 AND p.departmentAppointment.id > 0 AND p.payment.paymentType.id=1 ";
                csv_init[0] = "num_dest;date_dest;form_pom;mo_src;date_fact;time_fact;vpolis;spolis;npolis;smo;ds_priem;prof_k;depart;num_hist;type_gosp;subj_rf;fam;im;ot;pol;dr\n";
                //extr hospitalizations
                common_queries[1] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.id%20FROM%20InPatientHistory%20p%20WHERE%20p.startDate%3E%27" + sbegin + "%27%20AND%20p.startDate%20%3C%20%27" + send + "%27%20AND%20p.receptionCondition.urgentIndication.id=1%20 AND p.departmentAppointment.id > 0 AND p.payment.paymentType.id=1";
                csv_init[1] = "form_pom;date_fact;time_fact;vpolis;spolis;npolis;smo;ds_priem;prof_k;depart;num_hist;type_gosp;subj_rf;fam;im;ot;pol;dr\n";

                long[] PatientsIds = { };
                SetAllEntitysId(ref PatientsIds, common_queries[jo], "//list/long");

                CSVfile = csv_init[jo];
                //open file
                tmp = "";
                
                int prbegin = progressBar.Value;
                    for (int i = 0; i < PatientsIds.Length; progressBar.Value = prbegin + (20 * (i++ + 1)) / PatientsIds.Length)
                    {
                        long hospId = PatientsIds[i];

                        string mo_src = "";
                        if (jo == 0)//planing
                        {
                            //num_dest
                            GetItemValueByQuery(ref mo_src, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.receptionCondition.referrer.code   FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                            GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.receptionCondition.referralNumber   FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                            tmp = nz(tmp);
                            string y = ctoday.Year.ToString().Substring(3);
                            if (mo_src != null && tmp.Length < 5) tmp = "0" + tmp;
                            if (mo_src != null && tmp.Length == 5) tmp = mo_src.Substring(mo_src.Length - 3, 3) + y + tmp;
                            CSVfile += tmp + ";";

                            //date_dest
                            GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.receptionCondition.referralDate   FROM InPatientHistory p WHERE p.id=" + hospId, "//sql-timestamp");
                            if (tmp != null)
                            {
                                System.DateTime min = new DateTime();
                                min = System.DateTimeOffset.Parse(tmp).Date;
                                CSVfile += min.ToString("dd.MM.yyyy;");
                            }
                            else CSVfile += ";";
                        }
                        //form_pom
                        CSVfile += (jo + 1).ToString() + ";"; //2-extr

                        if (jo == 0)//planing
                        {
                            //mo_src
                            CSVfile += nz(mo_src) + ";";
                        }
                        //date_fact
                        //time_fact
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.receptionCondition.receptionDate FROM InPatientHistory p WHERE p.id=" + hospId, "//gregorian-calendar/time");
                        if (tmp != null)
                        {
                            System.DateTime s = new DateTime(1970, 1, 1, 12, 0, 0);
                            s = s.AddMilliseconds(double.Parse(tmp));
                            CSVfile += s.ToString("dd.MM.yyyy;HH:mm;");
                        }
                        else CSVfile += ";;";

                        //vpolis
                        string ppaytype = "";
                        string pseries = "";
                        string pnumber = "";

                        GetItemValueByQuery(ref pnumber, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.number FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        GetItemValueByQuery(ref pseries, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.series FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        GetItemValueByQuery(ref ppaytype, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.paymentType.id FROM InPatientHistory p WHERE p.id=" + hospId, "//long");
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.policyType.id FROM InPatientHistory p WHERE p.id = " + hospId, "//long");

                        if (ppaytype == "1" && tmp == "2")
                            CSVfile += "2;"; //temporable policy
                        else if (ppaytype == "1" && pnumber != null && pnumber.Length > 15 && pseries == null)
                            CSVfile += "3;";//new
                        else if (ppaytype == "1" && pnumber != null && pseries != null)
                            CSVfile += "1;";//old
                        else
                            CSVfile += ";";

                        //spolis
                        CSVfile += nz(pseries) + ";";
                        //npolis
                        CSVfile += nz(pnumber) + ";";

                        //smo
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.company.code FROM InPatientHistory p WHERE p.id =" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //ds_priem
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.admissionDiagnosis.diagnosis.diagnosis.mkb.code FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //prof_k
                        string department_code = "";
                        GetItemValueByQuery(ref department_code, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.departmentAppointment.department.code FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        if (department_code == "302")//pat
                            CSVfile += "92;";
                        else if (department_code == "301")//ginek
                            CSVfile += "25;";
                        else if (department_code == "303")//aoo
                            CSVfile += "91;";
                        else
                            CSVfile += "UNKNOWN;";
                        //depart
                        CSVfile += nz(department_code) + ";";
                        //num_hist
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.code FROM InPatientHistory p WHERE p.id =" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //type_gosp
                        CSVfile += (jo + 1).ToString() + ";";
                        //subj_rf
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.payment.company.terrCode FROM InPatientHistory p WHERE p.id =" + hospId, "//string");
                        if (tmp != null)
                            CSVfile += tmp.Substring(0, 5) + ";";
                        else
                            CSVfile += nz(tmp) + ";";
                        //fam
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.patient.personalData.lastName FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //im
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.patient.personalData.firstName FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //ot
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.patient.personalData.middleName FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //pol
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.patient.personalData.sex.code FROM InPatientHistory p WHERE p.id=" + hospId, "//string");
                        CSVfile += nz(tmp) + ";";
                        //dr
                        GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.patient.personalData.birthDate FROM InPatientHistory p WHERE p.id=" + hospId, "//gregorian-calendar/time");
                        if (tmp != null)
                        {
                            System.DateTime s = new DateTime(1970, 1, 1, 12, 0, 0);
                            s = s.AddMilliseconds(double.Parse(tmp));
                            CSVfile += s.ToString("dd.MM.yyyy;");
                        }
                        else
                            CSVfile += ";";
                        CSVfile += "\n";
                    
                    
                }
                if (progressBar.Value < prbegin + 20) progressBar.Value = prbegin + 20;

                filename = cpath +"eir200_" + DateTime.Today.ToString("ddMMyyyy") + (jo + 1).ToString() + ".csv";
                File.Delete(filename);
                FileStream fs = File.OpenWrite(filename);
                Byte[] info = Encoding.UTF8.GetBytes(CSVfile);
                Byte[] info_1251 = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), info);
                fs.Write(info_1251, 0, info_1251.Length);
                fs.Close();
                
            }
            //Outcomming table
            //0,6
            long[] PatientInDepartmentAppointments = { };
            SetAllEntitysId(ref PatientInDepartmentAppointments, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.id FROM PatientInDepartmentAppointment p WHERE p.toDate>'" + sbegin + "' AND p.toDate<'" + send + "' AND p.history.payment.paymentType.id=1", "//list/long");

            CSVfile = "num_hist;date_fact;time_fact;date_out\n";
            //open file
            tmp = "";
            for (long i = 0; i < PatientInDepartmentAppointments.Length; i++)
            {
                long PatientInDepartmentAppointmentsId = PatientInDepartmentAppointments[i];

                //num_hist
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.history.code FROM PatientInDepartmentAppointment p WHERE p.id=" + PatientInDepartmentAppointmentsId, "//string");
                CSVfile += nz(tmp) + ";";
                //date_fact
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.fromDate FROM PatientInDepartmentAppointment p WHERE p.id=" + PatientInDepartmentAppointmentsId, "//sql-timestamp");
                if (tmp != null)
                {
                    System.DateTime min = new DateTime();
                    min = System.DateTimeOffset.Parse(tmp).Date;
                    CSVfile += min.ToString("dd.MM.yyyy;");
                }
                else CSVfile += ";";
                //time_fact
                CSVfile += "15:00;";
                //date_out
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT p.toDate FROM PatientInDepartmentAppointment p WHERE p.id=" + PatientInDepartmentAppointmentsId, "//sql-timestamp");
                if (tmp != null)
                {
                    System.DateTime min = new DateTime();
                    min = DateTime.Parse(tmp.Substring(0, tmp.IndexOf('.')));
                    CSVfile += min.ToString("dd.MM.yyyy;");
                }
                else CSVfile += ";";
                CSVfile += "\n";
            }

            filename = cpath + "out200_" + ctoday.ToString("ddMMyyyy") + (3).ToString() + ".csv";
            File.Delete(filename);
            FileStream fs3 = File.OpenWrite(filename);
            Byte[] info3 = Encoding.UTF8.GetBytes(CSVfile);
            Byte[] info3_1251 = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), info3);
            fs3.Write(info3_1251, 0, info3_1251.Length);
            fs3.Close();
           
            progressBar.Value += 10; //Progress bar 6
            //Volumes
            
            XmlNodeList plist = GetItemsListByQuery("/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20count%28p%29,%20p.history.payment.company.code,%20p.department.id%20FROM%20PatientInDepartmentAppointment%20p%20WHERE%20p.toDate%20is%20NULL%20AND%20p.fromDate%20%3C%20%27" + sbegin + "%27%20AND%20p.history.payment.paymentType.id=1%20group%20by%20p.history.payment.company.code,%20p.department.id", "//list/object-array");
            // Key(companyid, depid) -- (count1, count2, count 3)
            Dictionary<KeyValuePair<string, string>, ReportItem> report = new Dictionary<KeyValuePair<string, string>, ReportItem>();
            for (int k = 0; k < plist.Count; k++)
            {
                XmlNode a = plist.Item(k);
                string ccount = a.ChildNodes.Item(0).InnerText;
                string ccompany = a.ChildNodes.Item(1).InnerText;
                string cdepartment = a.ChildNodes.Item(2).InnerText;
                report.Add(new KeyValuePair<string, string>(ccompany, cdepartment), new ReportItem(Int32.Parse(ccount),0,0));
            }
            progressBar.Value += 10; //Progress bar 7
            plist = GetItemsListByQuery("/mis/rest/queries?method=getQueryXmlResult&query=SELECT count(p), p.history.payment.company.code, p.department.id FROM PatientInDepartmentAppointment p WHERE p.toDate is NULL AND p.fromDate < '" + send + "' AND p.fromDate > '" + sbegin + "' AND p.history.payment.paymentType.id=1 group by p.history.payment.company.code, p.department.id", "//list/object-array");
            for (int k = 0; k < plist.Count; k++)
            {
                XmlNode a = plist.Item(k);
                string ccount = a.ChildNodes.Item(0).InnerText;
                string ccompany = a.ChildNodes.Item(1).InnerText;
                string cdepartment = a.ChildNodes.Item(2).InnerText;
                KeyValuePair<string, string> kkey = new KeyValuePair<string, string>(ccompany, cdepartment);
                ReportItem vval;
                bool r = report.TryGetValue(kkey, out vval);
                if (r == false)
                    report.Add(kkey, new ReportItem(0, Int32.Parse(ccount), 0));
                else
                {
                    vval.newpatients_count = Int32.Parse(ccount);
                    report[kkey] = vval;
                }
            }
            plist = GetItemsListByQuery("/mis/rest/queries?method=getQueryXmlResult&query=SELECT count(p), p.history.payment.company.code, p.department.id FROM PatientInDepartmentAppointment p WHERE p.toDate  < '" + send + "' AND p.toDate > '" + sbegin + "' AND p.history.payment.paymentType.id=1 group by p.history.payment.company.code, p.department.id", "//list/object-array");
            for (int k = 0; k < plist.Count; k++)
            {
                XmlNode a = plist.Item(k);
                string ccount = a.ChildNodes.Item(0).InnerText;
                string ccompany = a.ChildNodes.Item(1).InnerText;
                string cdepartment = a.ChildNodes.Item(2).InnerText;
                KeyValuePair<string, string> kkey = new KeyValuePair<string, string>(ccompany, cdepartment);
                ReportItem vval;
                bool r = report.TryGetValue(kkey, out vval);
                if (r == false)
                    report.Add(kkey, new ReportItem(0, 0, Int32.Parse(ccount)));
                else
                {
                    vval.out_count = Int32.Parse(ccount);
                    report[kkey] = vval;
                }
            }
            plist = GetItemsListByQuery("/mis/rest/queries?method=getQueryXmlResult&query=SELECT count(p), p.department.id FROM PatientInDepartmentAppointment p WHERE p.toDate is NULL AND p.fromDate< '" + send + "'  group by p.department.id", "//list/object-array");
            XmlNodeList plist2 = GetItemsListByQuery("/mis/rest/queries?method=getQueryXmlResult&query=SELECT count(z), z.ward.department.id FROM StaffBed z WHERE z.pluggedIn=false group by z.ward.department.id", "//list/object-array");
            progressBar.Value += 10; //Progress bar 8
            Dictionary<int, int> free_b = new Dictionary<int, int>();
            for(int i=0;i<plist.Count;i++)
            {
                for (int j = 0; j < plist2.Count; j++)
                {
                    int dep_id = Int32.Parse(plist2.Item(j).ChildNodes.Item(1).InnerText);
                    if (plist.Item(i).ChildNodes.Item(1).InnerText == plist2.Item(j).ChildNodes.Item(1).InnerText) //p.department.id = z.ward.department.id
                        free_b[Int32.Parse(plist.Item(i).ChildNodes.Item(1).InnerText)] = Int32.Parse(plist2.Item(j).ChildNodes.Item(0).InnerText) - Int32.Parse(plist.Item(i).ChildNodes.Item(0).InnerText);
                    //free bed's if no man's in pepartment
                    int ccount = Int32.Parse(plist2.Item(j).ChildNodes.Item(0).InnerText);
                    if (!free_b.TryGetValue(dep_id, out ccount))
                        free_b[dep_id] = ccount;
                }
            }
            progressBar.Value += 10; //Progress bar 9
            int rc = report.Count();
            CSVfile = "date;smo;prof_k;exist_patient_count; new_patient_count;outcome_patient_count;free_beds_male;free_beds_female;free_beds_children\n";
            for(int i=0;i<rc;i++)
            {
                KeyValuePair<KeyValuePair<string, string>, ReportItem> a = report.ElementAt(i);
                int dep_id = Int32.Parse(a.Key.Value);
                free_b.TryGetValue(dep_id, out a.Value.free_beds);
                
                if (a.Value.free_beds < 0) a.Value.free_beds = 0;
                report[a.Key] = a.Value;
                //date
                CSVfile += ctoday.ToString("dd.MM.yyyy;");
                //smo
                CSVfile += a.Key.Key+";";
                //prof_k
                if (dep_id== 4)
                    CSVfile += "92;";
                else if (dep_id==6)
                    CSVfile += "25;";
                else if (dep_id==15  || dep_id == 3)
                    CSVfile += "91;";
                //exist_patient_count
                CSVfile += a.Value.alreadyexist_count + ";";
                //new_patient_count
                CSVfile += a.Value.newpatients_count + ";";
                //outcome_patient_count
                CSVfile += a.Value.out_count + ";";
                //free_beds_male
                CSVfile += "0;";
                //free_beds_female;
                CSVfile += a.Value.free_beds + ";";
                //free_beds_children
                CSVfile += "#NEED_TO_ADD_MANUAL#;\n";
            }

            filename = cpath + "volume200_" + ctoday.ToString("ddMMyyyy") + (4).ToString() + ".csv";
            File.Delete(filename);
            FileStream fs4 = File.OpenWrite(filename);
            Byte[] info4 = Encoding.UTF8.GetBytes(CSVfile);
            Byte[] info4_1251 = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), info4);
            fs4.Write(info4_1251, 0, info4_1251.Length);
            fs4.Close();
            progressBar.Value += 10; //Progress bar 10
            progressBar.Visible = false;
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\his-dv\PatientResiver",true);
            if(registryKey==null)
            {
                RegistryKey rsowt = Registry.CurrentUser.OpenSubKey(@"Software",true);
                RegistryKey his = rsowt.CreateSubKey("his-dv");
                RegistryKey pat = his.CreateSubKey("PatientResiver");
                OnClosing(sender, e);
                return;
            }
            registryKey.SetValue("filename", dbfpath.Text);
            registryKey.SetValue("mis", cmbMis.Text);
        }

        private void OnRb3RClick(object sender, EventArgs e)
        {
            //MessageBox.Show(""+rbMonday.Checked);
            
        }

        private void OnRbVoluntaryClick(object sender, EventArgs e)
        {
            //rbVoluntaryDay.Checked = !rbVoluntaryDay.Checked;
        }

        private void OnRb3RUp(object sender, MouseEventArgs e)
        {
            if(mrbMondayUncheck)
            {
                rbMonday.Checked = false;
                mrbMondayUncheck = false;
            }
        }

        private void OnRb3RDown(object sender, MouseEventArgs e)
        {
            if (rbMonday.Checked)
            {
                rbMonday.Checked = false;
                mrbMondayUncheck = true;
            }
        }

        private void OnRbVoluntaryDown(object sender, MouseEventArgs e)
        {
            if(rbVoluntaryDay.Checked)
            {
                rbVoluntaryDay.Checked = false;
                mrbMondayUncheck = true;
            }
        }

        private void OnRbVoluntaryUp(object sender, MouseEventArgs e)
        {
            if(mrbMondayUncheck)
            {
                rbVoluntaryDay.Checked = false;
                mrbMondayUncheck = false;
            }
        }

        private void btnHQLExp_Click(object sender, EventArgs e)
        {
            if (dbfpath.Text == "")
            {
                MessageBox.Show("Specify the file name plz!");
                return;
            }
            long[] patients = { };
            SetAllPatientsId(ref patients);
            ExportPatientsByIdList(ref patients, "hql_patients");
        }
        private void ExportPatientsByIdList(ref long[] patients, string dbfname)
        {
            progressBar.Visible = true;
            progressBar.Maximum = 10 + patients.Length;
            progressBar.Value = 0;

            DataTable texport = new DataTable("export");
            DataRow row;
            AddColumn(texport, "numberCard", "System.String");
            AddColumn(texport, "lastName", "System.String");
            AddColumn(texport, "firstName", "System.String");
            AddColumn(texport, "middleName", "System.String");
            AddColumn(texport, "sex", "System.Int16");
            AddColumn(texport, "birthDate", "System.DateTime");
            AddColumn(texport, "DOCSER", "System.String");
            AddColumn(texport, "DOCNUMBER", "System.String");
            AddColumn(texport, "DOCSOURCE", "System.String");
            AddColumn(texport, "DOCDATE", "System.DateTime");
            AddColumn(texport, "DOCTYPE", "System.String");
            AddColumn(texport, "snils", "System.String");
            AddColumn(texport, "IsNearPolicy", "System.String");
            AddColumn(texport, "PayCompCode", "System.String");
            AddColumn(texport, "paySeries", "System.String");
            AddColumn(texport, "payNumber", "System.String");
            AddColumn(texport, "policyType", "System.String");
            AddColumn(texport, "payType", "System.String");
            AddColumn(texport, "socialstat", "System.String");
            AddColumn(texport, "workPlace", "System.String");
            AddColumn(texport, "position", "System.String");
            AddColumn(texport, "profession", "System.String");
            AddColumn(texport, "MaritalStatus", "System.Int16");

            AddColumn(texport, "fiascode", "System.String"); //fias ,0,8
            AddColumn(texport, "cityarea", "System.String"); //area of city?
            AddColumn(texport, "cityname", "System.String"); //city name! 
            AddColumn(texport, "areaname", "System.String"); //area of region
            AddColumn(texport, "streetName", "System.String");
            AddColumn(texport, "house", "System.String");
            AddColumn(texport, "housing", "System.String");
            AddColumn(texport, "roomNumber", "System.String");
            AddColumn(texport, "zipcode", "System.String");

            AddColumn(texport, "f.fiasCode", "System.String"); //fias ,0,8
            AddColumn(texport, "f.cityarea", "System.String"); //area of city?
            AddColumn(texport, "f.cityname", "System.String"); //city name! 
            AddColumn(texport, "f.areaname", "System.String"); //area of region
            AddColumn(texport, "f.streetName", "System.String");
            AddColumn(texport, "f.house", "System.String");
            AddColumn(texport, "f.housing", "System.String");
            AddColumn(texport, "f.roomNumber", "System.String");
            AddColumn(texport, "f.zipcode", "System.String");

            AddColumn(texport, "homePhone", "System.String");
            AddColumn(texport, "datareg", "System.DateTime");
            AddColumn(texport, "mobilePhone", "System.String");
            AddColumn(texport, "Blood.Code", "System.String");
            AddColumn(texport, "Blood.Name", "System.String");
            AddColumn(texport, "rhesusFactor.code", "System.String");
            AddColumn(texport, "rhesusFactor.name", "System.String");


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            StreamWriter zlof = new StreamWriter("z_log_" + 1 + ".txt");
            zlof.AutoFlush = true;
            string tmp="";
            progressBar.Increment(5);
            foreach (int patientId in patients)
            {
            //Parallel.ForEach(patients, patientId =>
            //{
               
                Int64 ts0 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                row = texport.NewRow();
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=select%20oph.code%20from%20OutPatientHistory%20oph%20WHERE%20oph.patient.id=" + patientId, "//string");
                if (tmp != null)
                    row["numberCard"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.lastName%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["lastName"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.firstName%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["firstName"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.middleName%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["middleName"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.sex.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["sex"] = Convert.ToInt16(tmp);

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.birthDate%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//gregorian-calendar/time");
                if (tmp != null)
                {
                    System.DateTime s = new DateTime(1970, 1, 1, 12, 0, 0);
                    s = s.AddMilliseconds(double.Parse(tmp));
                    row["birthDate"] = s;
                }
                Int64 ts1 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.identityCard.series%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["DOCSER"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.identityCard.number%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["DOCNUMBER"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.identityCard.authority%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["DOCSOURCE"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.identityCard.date%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//sql-timestamp");
                if (tmp != null)
                {
                    System.DateTime min = new DateTime();
                    min = System.DateTimeOffset.Parse(tmp).Date;
                    row["DOCDATE"] = min;
                }

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.identityCard.identityType.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["DOCTYPE"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.snils%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["snils"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.territory.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["IsNearPolicy"] = tmp;
                Int64 ts2 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.company.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["PayCompCode"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.series%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["paySeries"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.number%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["payNumber"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.paymentType.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["payType"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.payment.policyType.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["policyType"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.socialStatus.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["socialstat"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.workInfo.dirWorkPlace.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["workPlace"] = tmp;
                else
                {
                    GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.workInfo.workPlace%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                    if (tmp != null)
                        row["workPlace"] = tmp;
                }
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.workInfo.position%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["position"] = tmp;
                Int64 ts3 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.workInfo.profession.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["profession"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.maritalStatus.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["MaritalStatus"] = Convert.ToInt16(tmp);

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.fiasCode%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["fiascode"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.district.name%20FROM%20Patient%20p%20WHERE%20p.id= " + patientId, "//string");
                if (tmp != null)
                    row["cityarea"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.cityCode.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["cityname"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.areaCode.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["areaname"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.streetName%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["streetName"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.house%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["house"] = tmp;
                Int64 ts4 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.building%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["housing"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.roomNumber%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["roomNumber"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.registration.zipCode%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["zipcode"] = tmp;

                //Adress 2
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.fiasCode%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.fiasCode"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.district.name%20FROM%20Patient%20p%20WHERE%20p.id= " + patientId, "//string");
                if (tmp != null)
                    row["f.cityarea"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.cityCode.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.cityname"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.areaCode.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.areaname"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.streetName%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.streetName"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.house%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.house"] = tmp;
                Int64 ts5 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.building%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.housing"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.roomNumber%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.roomNumber"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.temporaryRegistration.zipCode%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["f.zipcode"] = tmp;
                //Aditional info
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.registrationDate%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//sql-timestamp");
                if (tmp != null)
                {
                    System.DateTime min = new DateTime();
                    min = System.DateTimeOffset.Parse(tmp).Date;
                    row["datareg"] = min;
                }

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.homePhone%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["homePhone"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.personalData.mobilePhone%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["mobilePhone"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.bloodGroup.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["Blood.Code"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.bloodGroup.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["Blood.Name"] = tmp;

                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.rhesusFactor.code%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["rhesusFactor.code"] = tmp;
                Int64 ts6 = Convert.ToInt64(stopWatch.Elapsed.TotalMilliseconds);
                GetItemValueByQuery(ref tmp, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.rhesusFactor.name%20FROM%20Patient%20p%20WHERE%20p.id=" + patientId, "//string");
                if (tmp != null)
                    row["rhesusFactor.name"] = tmp;

                texport.Rows.Add(row);
                progressBar.Increment(1);
                zlof.WriteLine( (ts1-ts0) + " " + (ts2-ts1) + " "+(ts3 - ts2)+" "+(ts4-ts3)+" "+(ts5-ts4)+" "+(ts6-ts5) );
                //of.Close();
            
            }
        //);
            zlof.Close();
            stopWatch.Stop();
            sbsit.utils.DBF.DataTableSaveToDBF(texport, dbfpath.Text, dbfname);
            progressBar.Increment(5);
        }

        private void MakeDBFbyInterval(object sender, EventArgs e)
        {
            if (dbfpath.Text == "")
            {
                MessageBox.Show("Specify the file name plz!");
                return;
            }

            //make interval
            DateTime dt = new DateTime();
            dt = DateTime.Parse(txtFromDate.Text);

            string sinterval = "WHERE p.registrationDate >= TO_TIMESTAMP('" + dt.ToString("yyyy\\/MM\\/dd HH:mm:00") + "', 'YYYY/MM/DD HH24:MI:SS') ";

            long[] patients = { };
            SetAllEntitysId(ref patients, "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20p.id%20FROM%20Patient%20p%20" + sinterval, "//list/long");
            ExportPatientsByIdList(ref patients, "hql_patients_export_from" + dt.ToString("dddd dd MMMM yyyy"));

          
        }

        private void btnDubliesToDBF_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime();
            dt = DateTime.Parse(txtFromDate.Text);

            ftmSelectDubliesQueryType f = new ftmSelectDubliesQueryType();
            f.ShowDialog();
            //while (f.getType() == -1)
              //  Thread.Sleep(100);

            string[] queryes = new string[4];
            queryes[0] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20distinct%20p.id%20FROM%20Patient%20p,%20Patient%20b%20%20where%20p.personalData.lastName=b.personalData.lastName%20and%20p.id!=b.id%20";
            queryes[1] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20distinct%20p%20FROM%20Patient%20p,%20Patient%20b%20%20where%20p.personalData.lastName=b.personalData.lastName%20and%20p.id!=b.id%20and%20p.personalData.birthDate=b.personalData.birthDate";
            queryes[2] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20distinct%20p.id%20FROM%20Patient%20p,%20Patient%20b%20%20where%20(p.personalData.lastName = b.personalData.lastName  and p.personalData.firstName = b.personalData.firstName  or  p.personalData.lastName = b.personalData.lastName  and p.personalData.middleName = b.personalData.middleName or p.personalData.firstName = b.personalData.firstName and p.personalData.middleName = b.personalData.middleName)%20and%20p.id!=b.id%20";
            queryes[3] = "/mis/rest/queries?method=getQueryXmlResult&query=SELECT%20distinct%20p%20FROM%20Patient%20p,%20Patient%20b%20%20where%20p.personalData.identityCard.number=b.personalData.identityCard.number%20and%20p.id!=b.id%20";
            long[] patients = { };
            SetAllEntitysId(ref patients, queryes[f.getType()-1], "//list/long");
            ExportPatientsByIdList(ref patients, "hql_patients_dublies_export_" + dt.ToString("dddd dd MMMM yyyy hh.mm"));
        }


    }//class
}//namespace
