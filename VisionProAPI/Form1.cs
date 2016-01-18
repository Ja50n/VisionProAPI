using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisionProAPI;

namespace VisionProAPI
{
    public partial class Form1 : Form
    {
        Vision vision;
        public double X = 0;
        public double Y = 0;
        public double Angle = 0;
        public string pathin = @"D:\M503_Photo\1.bmp";
        public string pathout = @"D:\M503_Photo\2.bmp";
        public string vpppath = @"D:\M503_Photo\M503.vpp";
        public Form1()
        {
            InitializeComponent();
        }
        Vision.Result result; 
        private void button1_Click(object sender, EventArgs e)
        {
            vision.Run(500,pathin);
            vision.GetResult(ref result);
            Angle = result.ResultAngle;
            X = result.ResultX;
            Y = result.ResultY;
            label1.Text = X.ToString();
            label2.Text = Y.ToString();
            label3.Text = Angle.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            vision = new Vision();
            vision.init(vpppath, cogRecordDisplay1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            vision.SetColorMapPreDefined(3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vision.SetColorMapPreDefined(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vision.SetColorMapPreDefined(1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            vision.SetColorMapPreDefined(2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
        
    }
}
