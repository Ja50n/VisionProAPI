using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ResultsAnalysis;
using Cognex.VisionPro.PMAlign;

namespace VisionProAPI
{
    public class Vision
    {
        private CogJobManager myJobManager;
        private CogJob myJob;
        private CogJobIndependent myJobIndependent;
        public double Result_X = 0;
        public double Result_Y = 0;
        public double Result_Angle = 0;
        private CogImageFileTool mIFTool;
        private Bitmap Imagein;
        private CogToolGroup mTGTool;

        public void init(string vpppath)
        {
            myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath);
            myJob = myJobManager.Job(0);
            myJobIndependent = myJob.OwnedIndependent;
            myJobManager.UserQueueFlush();
            myJobManager.FailureQueueFlush();
            myJob.ImageQueueFlush();
            myJobIndependent.RealTimeQueueFlush();
        }
        public bool DataIn(string pathin)
        {
            mTGTool = (CogToolGroup)(myJobManager.Job(0).VisionTool);
            mIFTool = (CogImageFileTool)(mTGTool.Tools["CogImageFileTool1"]);
            mIFTool.Operator.Open(pathin, CogImageFileModeConstants.Read);            
            Imagein = new Bitmap(pathin);
            mIFTool.InputImage = new CogImage8Grey(Imagein);
            mIFTool.Run();
            return true; 
        }
        public bool GetResult()
        {
            if (myJobManager == null)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager.UserResult();

            if (topRecord == null)
            {
                return false;
            }
            
            tmpRecord = topRecord.SubRecords[@"X"];
            if (tmpRecord.Content != null)
            {
                Result_X = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Y"];
            if (tmpRecord.Content != null)
            {
                Result_Y = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Angle"];
            if (tmpRecord.Content != null)
            {
                Result_Angle = (double)tmpRecord.Content;
            }
            return true;
        }
        public bool Run(ref string exMessage,int time, string _pathin)
        {
            DataIn(_pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch (Exception ex)
            {
                exMessage = ex.Message;
                return false;
            }
            return true;
        }
        public void Close()
        {
            myJob.Reset();
            myJobManager.Stop();
            myJobManager.Shutdown();
            myJob = null;
            myJobManager = null;
            myJobIndependent = null;
            GC.Collect();
        }
        internal bool RunContinuous(ref string exMessage)
        {
            try
            {
                myJobManager.RunContinuous();
            }
            catch (Exception ex)
            {
                exMessage = ex.Message;
                return false;
            }
            return true;
        }
        internal void Stop()
        {
            myJobManager.Stop();
        }
    }
}
