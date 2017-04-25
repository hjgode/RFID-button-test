using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Intermec.DeviceManagement.SmartSystem;

namespace MY_SSAPI
{
    class debug
    {
        public static void dbgLog(String s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
    }

    public class SSAPI : IDisposable
    {
        //we will use a single instance of ITCSSAPI
        //init/dispose may take some time
        static Intermec.DeviceManagement.SmartSystem.ITCSSApi _ssAPI = null;
        Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI
        {
            get
            {
                if (_ssAPI == null)
                {
                    try
                    {
                        _ssAPI = new Intermec.DeviceManagement.SmartSystem.ITCSSApi();
                    }
                    catch (Exception ex)
                    {
                        debug.dbgLog("Exception init ITCSSApi " + ex.Message);
                    }
                }
                return _ssAPI;
            }
        }
        public void Dispose()
        {
            if (_ssAPI != null)
            {
                try
                {
                    _ssAPI.Dispose();
                    _ssAPI = null;
                }
                catch (Exception) { }
            }
        }
        public uint DoAction(string sXML, string GetSet, out string sResponse)
        {
            uint uRes = 0;
            sResponse = "";
            try
            {
                int iSize = 1024;
                StringBuilder sb = new StringBuilder(iSize);
                if (GetSet == "Get")
                    uRes = ssAPI.Get(sXML, sb, ref iSize, 5000);
                else
                    uRes = ssAPI.Set(sXML, sb, ref iSize, 5000);
                sResponse = sb.ToString();
                debug.dbgLog("DoAction() OK: " + sXML + " " + GetSet + " " + sResponse);
            }
            catch (Exception ex)
            {
                debug.dbgLog("Exception DoAction() GetSet=" + GetSet + " xml=" + sXML + " Exception: " + ex.Message);
            }
            return uRes;
        }
        public string GetSSAPIValue(string xmlGet, string errorText)
        {
            string response = "";
            string str1 = "";
            try
            {
                uint uRes = DoAction(xmlGet, "Get", out response);
                if ((int)uRes == 0)
                {
                    str1 = ExtractField(response, errorText);
                }
                else
                {
                    //int num2 = (int)MessageBox.Show(string.Format("Failed to get {0}", (object)errorText), "Error");
                    string message = string.Format("Failed to get {0}, status={1:X}", (object)errorText, (object)uRes);
                    System.Diagnostics.Debug.WriteLine(message);
                }
            }
            catch
            {
                string str2 = string.Format("Attempting to get {0} caused exception", (object)errorText);
                //int num = (int)MessageBox.Show(str2, "Error");
                System.Diagnostics.Debug.WriteLine(str2);
            }
            return str1;
        }
        private void SetSSAPIValue(string xmlSet, string errorText)
        {
            string response;
            uint uRes = DoAction(xmlSet, "Set", out response);
            if ((int)uRes == 0)
                return;
            //int num2 = (int)MessageBox.Show(string.Format("{0} failed", (object)errorText), "Error");
            string message = string.Format("{0} failed, status={1:X}", (object)errorText, (object)uRes);
            System.Diagnostics.Debug.WriteLine(message);
        }

        private static string ExtractField(string response, string errorText)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string str = "";
            try
            {
                xmlDocument.LoadXml(response.ToString());
                str = xmlDocument.InnerText;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Attempting to parse getting {0} caused exception", (object)errorText), "Error");
            }
            return str;
        }
    }

    //public class myPhone:IDisposable
    //{
    //    SSAPI mySSAPI;
    //    private const string xmlSetPhonePowerFormat = "<Subsystem Name=\"WWAN Radio\"><Group Name=\"Manage Radio State\"><Field Name=\"Radio Power State\">{0}</Field></Group></Subsystem>";
    //    private const string xmlGetPhonePowerFormat = "<Subsystem Name=\"WWAN Radio\"><Group Name=\"Manage Radio State\"><Field Name=\"Radio Power State\"></Field></Group></Subsystem>";
    //    /*
    //     <Subsystem Name="WWAN Radio">
    //      <Group Name="Manage Radio State">
    //       <Field Name="Radio Power State">0</Field> 
    //      </Group>
    //     </Subsystem>
    //    */

    //    /// <summary>
    //    /// some function calls may block some seconds
    //    /// for use in separate thread or process
    //    /// </summary>
    //    public myPhone()
    //    {
    //        mySSAPI = new SSAPI();
    //    }
    //    public void Dispose()
    //    {
    //        mySSAPI.Dispose();
    //        mySSAPI = null;
    //    }

    //    public bool IsPhoneOn
    //    {
    //        get
    //        {
    //            return !SystemState.PhoneRadioOff;
    //        }
    //        set
    //        {
    //            if (value)
    //                TurnOnPhone();
    //            else
    //                TurnOffPhone();
    //        }
    //    }

    //    /// <summary>
    //    /// Get the Phone power state
    //    /// </summary>
    //    /// <returns>True for powered up GSM Radio, false for not powered Radio</returns>
    //    public bool IsPhoneOnXML()
    //    {
    //        //the following may not reflect the current state, especially if tested directly after changing the state
    //        bool bPower = GetSSAPIValue(xmlGetPhonePowerFormat, "phone on") == "1";
    //        debug.dbgLog(bPower ? "Phone is ON" : "Phone is OFF");
    //        return bPower;
    //    }

    //    /// <summary>
    //    /// turns the Phone Modul off
    //    /// may take some time, as the GSM has to unregister from the network
    //    /// incorrectly powering the GSM just off may cause issues with network provider on next registering
    //    /// </summary>
    //    public void TurnOffPhone()
    //    {
    //        debug.dbgLog("TurnOffPhone()");
    //        SetSSAPIValue(string.Format(xmlSetPhonePowerFormat, (object)"0"), "Turn off phone");
    //    }

    //    /// <summary>
    //    /// turns the Phone Modul on
    //    /// may take some time, as the GSM has to register on the network
    //    /// </summary>
    //    public void TurnOnPhone()
    //    {
    //        debug.dbgLog("TurnOnPhone()");
    //        SetSSAPIValue(string.Format(xmlSetPhonePowerFormat, (object)"1"), "Turn on phone");
    //    }

    //    private static string GetSSAPIValue(string xmlGet, string errorText)
    //    {
    //        string response = "";
    //        string str1 = "";
    //        try
    //        {
    //            uint uRes = SSAPI.DoAction(xmlGet, "Get", out response);
    //            if ((int)uRes == 0)
    //            {
    //                str1 = ExtractField(response, errorText);
    //            }
    //            else
    //            {
    //                //int num2 = (int)MessageBox.Show(string.Format("Failed to get {0}", (object)errorText), "Error");
    //                string message = string.Format("Failed to get {0}, status={1:X}", (object)errorText, (object)uRes);
    //                System.Diagnostics.Debug.WriteLine(message);
    //            }
    //        }
    //        catch
    //        {
    //            string str2 = string.Format("Attempting to get {0} caused exception", (object)errorText);
    //            //int num = (int)MessageBox.Show(str2, "Error");
    //            System.Diagnostics.Debug.WriteLine(str2);
    //        }
    //        return str1;
    //    }

    //    private static void SetSSAPIValue(string xmlSet, string errorText)
    //    {
    //        string response;
    //        uint uRes = SSAPI.DoAction(xmlSet, "Set", out response);
    //        if ((int)uRes == 0)
    //            return;
    //        //int num2 = (int)MessageBox.Show(string.Format("{0} failed", (object)errorText), "Error");
    //        string message = string.Format("{0} failed, status={1:X}", (object)errorText, (object)uRes);
    //        System.Diagnostics.Debug.WriteLine(message);
    //    }

    //    private static string ExtractField(string response, string errorText)
    //    {
    //        XmlDocument xmlDocument = new XmlDocument();
    //        string str = "";
    //        try
    //        {
    //            xmlDocument.LoadXml(response.ToString());
    //            str = xmlDocument.InnerText;
    //        }
    //        catch
    //        {
    //            System.Diagnostics.Debug.WriteLine(string.Format("Attempting to parse getting {0} caused exception", (object)errorText), "Error");
    //        }
    //        return str;
    //    }

    //}
}
