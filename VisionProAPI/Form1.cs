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
        public string waring = "";
        public string pathin = @"D:\M503_Photo\1.bmp";
        public string vpppath = @"D:\M503_Photo\M503.vpp";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vision.Run(ref waring,500,pathin);
            vision.GetResult();
            Angle = vision.Result_Angle;
            X = vision.Result_X;
            Y = vision.Result_Y;
            label1.Text = X.ToString();
            label2.Text = Y.ToString();
            label3.Text = Angle.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            vision = new Vision();
            vision.init(vpppath);
        }
        
    }
}
