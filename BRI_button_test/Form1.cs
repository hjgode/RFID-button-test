using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Intermec.DataCollection.RFID;
using MY_SSAPI;
namespace BRI_button_test
{
    public partial class Form1 : Form, IDisposable
    {
        static bool doNotUpdate = true;

        BRIReader _BRIReader;
        SSAPI _SSAPI;
        
        BRIReader.RFIDButtonIDs[] bIDs = new BRIReader.RFIDButtonIDs[]{
                BRIReader.RFIDButtonIDs.CENTER,
                BRIReader.RFIDButtonIDs.LEFT,
                BRIReader.RFIDButtonIDs.MIDDLE,
                BRIReader.RFIDButtonIDs.RIGHT};
        Dictionary<BRIReader.RFIDButtonIDs, CheckBox> chkBoxes = new Dictionary<BRIReader.RFIDButtonIDs, CheckBox>();
        /*
        CENTER  The center trigger button on a portable reader.
        LEFT    The left scan button on a mobile computer.
        MIDDLE  The center scan button on a mobile computer.
        RIGHT   The right scan button on a mobile computer.
        UNKNOWN Unknown button ID.
         * 
        The registry changes
         * ie BriReaderButonEnable( RFIDButtonID.Left ) changes event2 from "ITC_RFID_TRIGGER_EVENT" to "RFIDDCE_LEFT_DELTA" (HKEY_LOCAL_MACHINE\Drivers\HID\ClientDrivers\ITCKeyboard\Layout\A-Numeric\0001\Events\Delta)
         * MIDDLE changes event1 between "RFIDDCE_MIDDLE_DELTA" and "DeltaLeftScan"
         * 
         * ITC_RFID_TRIGGER_EVENT is for general RFID and used by IntermecSettings for RFID, that does not include Button Event Firing, which is only done for "Button Remapping" settings to "BRI" (which is RFIDDCE_ in registry).
         * RFIDDCE_... is for BRI
         * so, the ITC_RFID_TRIGGER_EVENT will trigger a RFID TAG read but this button press will not be reported to BRIReader
        */
        public Form1()
        {
            InitializeComponent();
            _SSAPI = new SSAPI();
            BRIReader.LoggerOptionsAdv logOpts=new BRIReader.LoggerOptionsAdv();
            logOpts.LogEnable=true;
            logOpts.LogFilePath=@"\RFIDLog1.txt";
            logOpts.IDEConsoleEnable = false; //send to debugOut?
            logOpts.TimeStampEnable=true;
            logOpts.TraceSeverity=BasicBRIReader.LoggerOptions.TraceSeverityLevels.DEBUG;
            logOpts.ShowNonPrintableChars = true;

            _BRIReader = new BRIReader(this, "tcp://127.0.0.1:2189", logOpts);
            
            _BRIReader.EventHandlerRFIDButton += new DCE_BUTTON_EventHandlerAdv(_BRIReader_EventHandlerRFIDButton);

            //for reading TAGs
            _BRIReader.EventHandlerTag += new Tag_EventHandlerAdv(_BRIReader_EventHandlerTag);

            //will only be called for Trigger presses on RFID Handle (if any)
            _BRIReader.EventHandlerTriggerAction += new TriggerAction_EventHandler(_BRIReader_EventHandlerTriggerAction);

            textBox1.Text = getButtonSetting().ToString();

            //if (!this._BRIReader.IsRFIDButtonEnabled(BRIReader.RFIDButtonIDs.MIDDLE))
            //    this._BRIReader.RFIDButtonEnable(BRIReader.RFIDButtonIDs.MIDDLE);

            chkLeftLower.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkLeftUpper.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkLeftLower.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkRightLower.CheckStateChanged += new EventHandler(CheckStateChanged);

            chkBoxes.Add(BRIReader.RFIDButtonIDs.CENTER, chkCenterScan);
            chkBoxes.Add(BRIReader.RFIDButtonIDs.LEFT,chkLeftLower);
            chkBoxes.Add(BRIReader.RFIDButtonIDs.MIDDLE, chkButtonMiddle);
            chkBoxes.Add(BRIReader.RFIDButtonIDs.RIGHT, chkRightLower);
            
            chkRightUpper.Enabled=false;
            chkLeftUpper.Enabled=false;

            CheckBox ChkBox=null;
            foreach(BRIReader.RFIDButtonIDs id in bIDs){
                if(_BRIReader.IsRFIDButtonEnabled(id)){
                    if (chkBoxes.TryGetValue(id, out ChkBox))
                        ChkBox.Checked = true;
                }
                else{
                    if (chkBoxes.TryGetValue(id, out ChkBox))
                        ChkBox.Checked = false;
                }
            }
            updateCheckBoxes();
            doNotUpdate = false;
        }

        void _BRIReader_EventHandlerTriggerAction(object sender, TriggerAction_EventArgs EvtArgs)
        {
            addLog("EventHandlerTriggerAction: " + EvtArgs.TriggerName);
        }

        void _BRIReader_EventHandlerTag(object sender, EVTADV_Tag_EventArgs EvtArgs)
        {
            addLog("EventHandlerTag: "+ Encoding.ASCII.GetString(EvtArgs.Data, 0, EvtArgs.Data.Length));
        }

        void updateCheckBoxes()
        {
            doNotUpdate = true;
            CheckBox ChkBox=null;
            foreach(BRIReader.RFIDButtonIDs id in bIDs){
                if(_BRIReader.IsRFIDButtonEnabled(id)){
                    if (chkBoxes.TryGetValue(id, out ChkBox))
                        ChkBox.Checked = true;
                }
                else{
                    if (chkBoxes.TryGetValue(id, out ChkBox))
                        ChkBox.Checked = false;
                }
            }
        }

        void _BRIReader_EventHandlerRFIDButton(object sender, EVTADV_RFIDButton_EventArgs EvtArgs)
        {
            try
            {
                switch (EvtArgs.ButtonID)
                {
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonIDs.LEFT:
                        //
                        // The host computer’s left button was pressed or released.
                        addLog("EvtArgs.ButtonID: "+"LEFT");
                        break;
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonIDs.RIGHT:
                        //
                        // The host computer’s right button was pressed or released.
                        addLog("EvtArgs.ButtonID: " + "RIGHT");
                        break;
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonIDs.MIDDLE:
                        //
                        // The host computer’s center button was pressed or released.
                        addLog("EvtArgs.ButtonID: " + "MIDDLE");
                        break;
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonIDs.CENTER:
                        //
                        // The RFID reader’s center-trigger was pressed or released.
                        addLog("EvtArgs.ButtonID: " + "CENTER");
                        break;
                }

                switch (EvtArgs.ButtonState)
                {
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonStates.PRESSED:
                        //
                        // The button or trigger has been pressed by the operator...
                        addLog("EvtArgs.ButtonState: " + "PRESSED");
                        _BRIReader.StartReadingTags(BRIReader.TagReportOptions.EVENT);
                        break;
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonStates.RELEASED:
                        //
                        // The button or trigger has been released by the operator...
                        addLog("EvtArgs.ButtonState: " + "RELEASED");
                        _BRIReader.StopReadingTags();
                        break;
                }
                // Perform any action you intend to associate with the particular 
                // button pressed or released, such as reading or writing RFID tags.
            }
            catch (Exception ex)
            {
                addLog("_BRIReader_EventHandlerRFIDButton exception: " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_BRIReader != null)
            {
                _BRIReader.Dispose();
                _BRIReader = null;
            }
            if (_SSAPI != null)
            {
                _SSAPI.Dispose();
                _SSAPI = null;
            }
            base.Dispose();
        }
        private void btnEnableBRI_Click(object sender, EventArgs e)
        {
            if (_BRIReader.RFIDButtonEnable(BRIReader.RFIDButtonIDs.MIDDLE))
                addLog("setting MIDDLE enable OK");
            else
                addLog("setting MIDDLE enable FAILED");
            //foreach (BRIReader.RFIDButtonIDs id in bIDs)
            //{
            //    //enableButtonX(id);
            //    this._BRIReader.RFIDButtonEnable(id);
            //}
        }

        void enableButtonX(BRIReader.RFIDButtonIDs btnId)
        {
            if (!_BRIReader.RFIDButtonEnable(BRIReader.RFIDButtonIDs.MIDDLE))
            {
                textBox1.Text = "enable FAILED";
                addLog("enable FAILED");
            }
            else
            {
                addLog("enable OK");
                textBox1.Text = getButtonSetting().ToString();
            }
        }

        private void btnDisableBRI_Click(object sender, EventArgs e)
        {
            if (_BRIReader.RFIDButtonEnable(BRIReader.RFIDButtonIDs.MIDDLE))
                addLog("setting MIDDLE enable OK");
            else
                addLog("setting MIDDLE enable FAILED");
        }

        CenterMapped getButtonSetting()
        {
            String sResp = "";
            addLog("getButtonSetting()");
            //SSAPI.DoAction(sXMLgetCenterButtonSetting, "Get", out sResp);
            int iCenterButton = -1;
            try
            {
                iCenterButton = int.Parse(_SSAPI.GetSSAPIValue(sXMLgetCenterButtonSetting, "3"));
                addLog("iCenterButton=" + iCenterButton.ToString());
            }
            catch (FormatException ex)
            {
            }
            CenterMapped mapped = CenterMapped.ERROR;
            if (iCenterButton > 0)
            {
                mapped = (CenterMapped)iCenterButton;
            }
            addLog("mapped=" + mapped.ToString());
            return mapped;
        }
        // 1=SCAN, 2=RFID, 3=BRI, 4=Camera
        enum CenterMapped
        {
            ERROR = -1,
            UNKNOWN = 0,
            SCANNER = 1,
            RFID,
            BRI,
            CAMERA
        };
        string sXMLgetCenterButtonSetting =
            @"<Subsystem Name=""Device Settings""> 
                <Group Name=""Keypad"">
                  <Group Name=""Scan Button Remapping"">
                    <Field Name=""3""></Field> 
                  </Group>
                </Group>
              </Subsystem>";
        private void btnGet_Click(object sender, EventArgs e)
        {
            textBox1.Text = getButtonSetting().ToString();
        }
        /*
        - <Subsystem Name="Device Settings">
        - <Group Name="Keypad">
        - <Group Name="Scan Button Remapping">
          <Field Name="3">1</Field> 
          </Group>
          </Group>
          </Subsystem>
         */
        delegate void SetTextCallback(string text);
        public void addLog(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(addLog);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(text);
                if (txtLog.Text.Length > 2000)
                    txtLog.Text = "";
                txtLog.Text += text + "\r\n";
                txtLog.SelectionLength = 0;
                txtLog.SelectionStart = txtLog.Text.Length - 1;
                txtLog.ScrollToCaret();
            }
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Dispose();
        }

        private void CheckStateChanged(object sender, EventArgs e)
        {
            if (doNotUpdate)
                return;
            CheckBox cb = (CheckBox)sender;
            BRIReader.RFIDButtonIDs btn = BRIReader.RFIDButtonIDs.CENTER;
            if (cb.Name == "chkLeftUpper")
                btn = BRIReader.RFIDButtonIDs.LEFT;
            if (cb.Name == "chkLeftLower")
                btn = BRIReader.RFIDButtonIDs.LEFT;

            if (cb.Name == "chkRightLower")
                btn = BRIReader.RFIDButtonIDs.RIGHT;
            if (cb.Name == "chkRightUpper")
                btn = BRIReader.RFIDButtonIDs.RIGHT;
            
            if (cb.Name == "chkCenterScan")
                btn = BRIReader.RFIDButtonIDs.CENTER;

            if (cb.Name == "chkButtonMiddle")
                btn = BRIReader.RFIDButtonIDs.MIDDLE;

            if (cb.Checked)
            {
                if (!_BRIReader.RFIDButtonEnable(btn))
                    addLog("Enable button " + btn.ToString() + " FAILED");
                else
                    addLog("Enable button " + btn.ToString() + " OK");
            }
            else
            {
                if (!_BRIReader.RFIDButtonDisable(btn))
                    addLog("Disable button " + btn.ToString() + " FAILED");
                else
                    addLog("Disable button " + btn.ToString() + " FAILED");
            }
        }

        private void btnGetRFIDMapping_Click(object sender, EventArgs e)
        {
            doNotUpdate = true;
            updateCheckBoxes();
            doNotUpdate = false;
        }
    }
}