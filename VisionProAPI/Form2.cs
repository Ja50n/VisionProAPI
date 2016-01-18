using System.Collections.Generic;
using System.Windows.Forms;
using Cognex.VisionPro;

namespace VisionProAPI
{
    public partial class Form2 : Form
    {
        #region Definition
        private List<CogRecordDisplay> CogRecordDisplay;
        private string vpppath = @"D:\M503\M503.vpp";
        private string imagepath1 = @"D:\M503\1.png";
        private string imagepath2 = @"D:\M503\2.tif";
        private string imagepath3 = "";
        private List<string> imagein;
        #endregion
        VisionJobs VisionJobs;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            VisionJobs = new VisionJobs();
            CogRecordDisplay = new List<Cognex.VisionPro.CogRecordDisplay>();
            CogRecordDisplay.Add(cogRecordDisplay1);
            CogRecordDisplay.Add(cogRecordDisplay2);
            CogRecordDisplay.Add(cogRecordDisplay3);
            imagein = new List<string>();
            imagein.Add(imagepath1);
            imagein.Add(imagepath2);
            imagein.Add(imagepath3);
            VisionJobs.init(vpppath,3,CogRecordDisplay);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            VisionJobs.Run(500, 0, imagein);
            VisionJobs.GetResultShow(0);
            VisionJobs.Run(500, 1, imagein);
            VisionJobs.GetResultShow(1);
        }
    }
}
