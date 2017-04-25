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
        BRIReader _BRIReader;
        SSAPI _SSAPI;
        /*
        CENTER  The center trigger button on a portable reader.
        LEFT    The left scan button on a mobile computer.
        MIDDLE  The center scan button on a mobile computer.
        RIGHT   The right scan button on a mobile computer.
        UNKNOWN Unknown button ID.
        */
        public Form1()
        {
            InitializeComponent();
            _SSAPI = new SSAPI();
            _BRIReader = new BRIReader(this);
            _BRIReader.EventHandlerRFIDButton += new DCE_BUTTON_EventHandlerAdv(_BRIReader_EventHandlerRFIDButton);
            textBox1.Text = getButtonSetting().ToString();

            //if (!this._BRIReader.IsRFIDButtonEnabled(BRIReader.RFIDButtonIDs.MIDDLE))
            //    this._BRIReader.RFIDButtonEnable(BRIReader.RFIDButtonIDs.MIDDLE);

            chkLeftLower.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkLeftUpper.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkRightLower.CheckStateChanged += new EventHandler(CheckStateChanged);
            chkRightLower.CheckStateChanged += new EventHandler(CheckStateChanged);
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
                        break;
                    case EVTADV_RFIDButton_EventArgs.RFIDButtonStates.RELEASED:
                        //
                        // The button or trigger has been released by the operator...
                        addLog("EvtArgs.ButtonState: " + "RELEASED");
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
        }
        private void btnEnableBRI_Click(object sender, EventArgs e)
        {
            BRIReader.RFIDButtonIDs[] bIDs = new BRIReader.RFIDButtonIDs[]{
                BRIReader.RFIDButtonIDs.CENTER,
                BRIReader.RFIDButtonIDs.LEFT,
                BRIReader.RFIDButtonIDs.MIDDLE,
                BRIReader.RFIDButtonIDs.RIGHT};
            foreach (BRIReader.RFIDButtonIDs id in bIDs)
            {
                //enableButtonX(id);
                this._BRIReader.RFIDButtonEnable(id);
            }
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
            if (!_BRIReader.RFIDButtonDisable(BRIReader.RFIDButtonIDs.MIDDLE))
            {
                textBox1.Text = "disable FAILED";
                addLog("disable FAILED");
            }
            else
            {
                addLog("disable OK");
                textBox1.Text = getButtonSetting().ToString();
            }
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

            if (cb.Checked)
                _BRIReader.RFIDButtonEnable(btn);
            else
                _BRIReader.RFIDButtonDisable(btn);
        }
    }
}