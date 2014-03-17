using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.Entity;
using CustomerDataLayer;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Reflection;
using EDXInterface;
using System.Data.EntityClient;

namespace CustomerDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string ObjectsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Objects");
            foreach (string filename in Directory.GetFiles(ObjectsDir, "*.dll"))
            {
                Assembly custObj = Assembly.LoadFile(filename);
                Type[] CustType = custObj.GetTypes();
                if (CustType == null) continue;
                foreach (Type t in CustType)
                {
                    if (t.GetInterface("IEdx") != null)
                    {
                        var Obj = custObj.CreateInstance("CustomerObject.CustomerObj");
                        object[] argstopass = new object[] { (object)textBox1.Text };
                        Obj.GetType().GetMethod("ProcessPacket").Invoke(Obj, argstopass);
                    }
                }
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            textBox1.Text = ts.ToString();
        }


    }
}
