using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using eaep;

namespace eaep.generator
{
	public partial class EAEPGenerator : Form
	{
        Timer timer = new Timer();

		EAEPNode eaepNode = new EAEPNode();

		public EAEPGenerator()
		{
			InitializeComponent();
			InitialiseForm();
			eaepNode.Start();
		}

		private void InitialiseForm()
		{
			this.timestampBox.Text = DateTime.Now.ToString(EAEPMessage.TIMESTAMP_FORMAT);
		}

		private void sendButton_Click(object sender, EventArgs e)
		{
            EAEPMessage message = BuildMessage();
            message.TimeStamp = DateTime.ParseExact(timestampBox.Text, EAEPMessage.TIMESTAMP_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
            eaepNode.SendMessage(message);
            InitialiseForm();
		}

        private EAEPMessage BuildMessage()
        {
            EAEPMessage message = new EAEPMessage(hostBox.Text, appBox.Text, eventBox.Text);

            StringReader reader = new StringReader(paramsBox.Text);
            {
                while (reader.Peek() != -1)
                {
                    message.AddParamAVP(reader.ReadLine());
                }
            }
            return message;
        }

        private void multiSendButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
                SetFormEnablement(true);
                multiSendButton.Text = "Multi Send";
            }
            else
            {
                timer.Interval = 100;
                timer.Tick += new EventHandler(timer_Tick);
                SetFormEnablement(false);
                timer.Start();
                multiSendButton.Text = "Stop Multi";
                
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            EAEPMessage message = BuildMessage();
            message.TimeStamp = DateTime.Now;
            eaepNode.SendMessage(message);
        }

        private void SetFormEnablement(bool enable)
        {
            timestampBox.Enabled = enable;
            hostBox.Enabled = enable;
            appBox.Enabled = enable;
            eventBox.Enabled = enable;
            paramsBox.Enabled = enable;
            sendButton.Enabled = enable;
        }
	}
}
