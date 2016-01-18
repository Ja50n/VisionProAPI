using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    class Vision2
    {
        #region Definition
        private CogJobManager myJobManager;
        private CogJob myJob0;
        private CogJob myJob1;
        private CogJob myJob2;
        private CogJobIndependent myJobIndependent0;
        private CogJobIndependent myJobIndependent1;
        private CogJobIndependent myJobIndependent2;
        private CogImageFileTool myImageFile0;
        private CogImageFileTool myImageFile1;
        private CogImageFileTool myImageFile2;
        private Bitmap Imagein0;
        private Bitmap Imagein1;
        private Bitmap Imagein2;
        private CogToolGroup myToolGroup0;
        private CogToolGroup myToolGroup1;
        private CogToolGroup myToolGroup2;
        private CogRecordDisplay cogRecordDisplay0 = null;
        private CogRecordDisplay cogRecordDisplay1 = null;
        private CogRecordDisplay cogRecordDisplay2 = null;
        public struct Result0
        {
            public double ResultX;
            public double ResultY;
            public double ResultAngle;
        }
        public struct Result1
        { }
        public struct Result2
        { }
        #endregion
        #region init
        public bool init(string vpppath, CogRecordDisplay cogRecordDisplayin0 = null, CogRecordDisplay cogRecordDisplayin1 = null, CogRecordDisplay cogRecordDisplayin2 = null)
        {
            if (null == vpppath)
            {
                return false;
            }
            try
            {
                myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath);
                myJob0 = myJobManager.Job(0);
                myJobIndependent0 = myJob0.OwnedIndependent;
                myJob1 = myJobManager.Job(1);
                myJobIndependent1 = myJob1.OwnedIndependent;
                myJob2 = myJobManager.Job(2);
                myJobIndependent2 = myJob2.OwnedIndependent;
                myJobManager.UserQueueFlush();
                myJobManager.FailureQueueFlush();
                myJob0.ImageQueueFlush();
                myJobIndependent0.RealTimeQueueFlush();
                myJob1.ImageQueueFlush();
                myJobIndependent1.RealTimeQueueFlush();
                myJob2.ImageQueueFlush();
                myJobIndependent2.RealTimeQueueFlush();
                updateDisplaySource0(cogRecordDisplayin0);
                updateDisplaySource1(cogRecordDisplayin1);
                updateDisplaySource2(cogRecordDisplayin2);
            }
            catch { }
            return true;
        }
        #endregion
        #region updateDisplaySource and clearDisplaySource
        public bool updateDisplaySource0(CogRecordDisplay cogRecordDisplayin)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay0 = cogRecordDisplayin;
            return true;
        }
        public bool updateDisplaySource1(CogRecordDisplay cogRecordDisplayin)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay1 = cogRecordDisplayin;
            return true;
        }
        public bool updateDisplaySource2(CogRecordDisplay cogRecordDisplayin)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay2 = cogRecordDisplayin;
            return true;
        }
        public bool clearDisplaySource0()
        {
            cogRecordDisplay0 = null;
            return true;
        }
        public bool clearDisplaySource1()
        {
            cogRecordDisplay1 = null;
            return true;
        }
        public bool clearDisplaySource2()
        {
            cogRecordDisplay2 = null;
            return true;
        }
        #endregion
        #region GetImageFromFile
        private bool GetImage0(string pathin)
        {
            myToolGroup0 = (CogToolGroup)(myJobManager.Job(0).VisionTool);
            myImageFile0 = (CogImageFileTool)(myToolGroup0.Tools["CogImageFileTool1"]);
            myImageFile0.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein0 = new Bitmap(pathin);
            myImageFile0.InputImage = new CogImage8Grey(Imagein0);
            myImageFile0.Run();
            return true;
        }
        private bool GetImage1(string pathin)
        {
            myToolGroup1 = (CogToolGroup)(myJobManager.Job(1).VisionTool);
            myImageFile1 = (CogImageFileTool)(myToolGroup1.Tools["CogImageFileTool1"]);
            myImageFile1.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein1 = new Bitmap(pathin);
            myImageFile1.InputImage = new CogImage8Grey(Imagein1);
            myImageFile1.Run();
            return true;
        }
        private bool GetImage2(string pathin)
        {
            myToolGroup2 = (CogToolGroup)(myJobManager.Job(2).VisionTool);
            myImageFile2 = (CogImageFileTool)(myToolGroup2.Tools["CogImageFileTool1"]);
            myImageFile2.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein2 = new Bitmap(pathin);
            myImageFile2.InputImage = new CogImage8Grey(Imagein2);
            myImageFile2.Run();
            return true;
        }
        #endregion
        #region GetResult
        public bool GetResult0(ref Result0 result)
        {
            if (null == myJobManager)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager.UserResult();
            if (null == topRecord)
            {
                return false;
            }
            if (null == topRecord)
            {
                return false;
            }
            tmpRecord = topRecord.SubRecords[@"X"];
            if (null != tmpRecord.Content) if (tmpRecord.Content != null)
                {
                    result.ResultX = (double)tmpRecord.Content;
                }
            tmpRecord = topRecord.SubRecords[@"Y"];
            if (null != tmpRecord.Content)
            {
                result.ResultY = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Angle"];
            if (null != tmpRecord.Content)
            {
                result.ResultAngle = (double)tmpRecord.Content;
            }
            if (null != cogRecordDisplay0)
            {
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                if (null != tmpRecord.Content)
                {
                    cogRecordDisplay0.Record = tmpRecord;
                }
                cogRecordDisplay0.Fit(true);
            }
            return true;
        }
        public bool GetResult1(ref Result1 reuslt)
        {
            if (null == myJobManager)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager.UserResult();
            if (null == topRecord)
            {
                return false;
            }
            if (null != cogRecordDisplay1)
            {
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                if (null != tmpRecord.Content)
                {
                    cogRecordDisplay1.Record = tmpRecord;
                }
                cogRecordDisplay1.Fit(true);
            }
            return true;
        }
        public bool GetResult2(ref Result2 result)
        {
            if (null == myJobManager)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager.UserResult();
            if (null == topRecord)
            {
                return false;
            }
            if (null != cogRecordDisplay2)
            {
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                if (null != tmpRecord.Content)
                {
                    cogRecordDisplay2.Record = tmpRecord;
                }
                cogRecordDisplay2.Fit(true);
            }
            return true;
        }
        #region Run
        public bool Run0(int time, string _pathin)
        {
            GetImage0(_pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch { }
            return true;
        }
        public bool Run1(int time, string _pathin)
        {
            GetImage1(_pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch { }
            return true;
        }
        public bool Run2(int time, string _pathin)
        {
            GetImage2(_pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch { }
            return true;
        }
        #endregion
        #region
        public bool SetColorMapPreDefined0(string type)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay0.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay0.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay0.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay0.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
                    return true;
                default:
                    return false;
            }
        }
        public bool SetColorMapPreDefined1(string type)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
                    return true;
                default:
                    return false;
            }
        }
        public bool SetColorMapPreDefined2(string type)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region CloseAndStop
        public bool Close()
        {
            if (null != myJobManager)
            {
                myJob0.Reset();
                myJob1.Reset();
                myJob2.Reset();
                myJobManager.Stop();
                myJobManager.Shutdown();
                myJob0 = null;
                myJob0 = null;
                myJob0 = null;
                myJobManager = null;
                myJobIndependent0 = null;
                myJobIndependent1 = null;
                myJobIndependent2 = null;
                return true;
            }
            GC.Collect();
            return false;
        }
        public void Stop()
        {
            myJobManager.Stop();
        }
        #endregion
        #endregion
    }
}
