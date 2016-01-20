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
        private CogJobManager myJobManager0;
        private CogJobManager myJobManager1;
        private CogJobManager myJobManager2;
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
        {
            public double ResultAngle;
            public double ResultTitle;
            public double ResultDistance;
        }
        public struct Result2
        { }
        #endregion
        #region init
        public bool init(string vpppath0,string vpppath1, string vpppath2, CogRecordDisplay cogRecordDisplayin0 = null, CogRecordDisplay cogRecordDisplayin1 = null, CogRecordDisplay cogRecordDisplayin2 = null)
        {
            if (null == vpppath0)
            {
                return false;
            }
            try
            {
                myJobManager0 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath0);
                myJob0 = myJobManager0.Job(0);
                myJobIndependent0 = myJob0.OwnedIndependent;
                myJobManager0.UserQueueFlush();
                myJobManager0.FailureQueueFlush();
                myJob0.ImageQueueFlush();
                myJobIndependent0.RealTimeQueueFlush();
                updateDisplaySource0(cogRecordDisplayin0);
            }
            catch { }
            if (null == vpppath1)
            {
                return false;
            }
            try
            {
                myJobManager1 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath1);
                myJob1 = myJobManager1.Job(0);
                myJobIndependent1 = myJob1.OwnedIndependent;
                myJobManager1.UserQueueFlush();
                myJobManager1.FailureQueueFlush();
                myJob1.ImageQueueFlush();
                myJobIndependent1.RealTimeQueueFlush();
                updateDisplaySource1(cogRecordDisplayin1);
            }
            catch { }
            if (null == vpppath2)
            {
                return false;
            }
            try
            {
                myJobManager2 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath2);
                myJob2 = myJobManager2.Job(0);
                myJobIndependent2 = myJob2.OwnedIndependent;
                myJobManager2.UserQueueFlush();
                myJobManager2.FailureQueueFlush();
                myJob2.ImageQueueFlush();
                myJobIndependent2.RealTimeQueueFlush();
                updateDisplaySource2(cogRecordDisplayin2);
            }
            catch { }
            return true;
        }
        public bool init0(string vpppath0, CogRecordDisplay cogRecordDisplayin0 = null)
        {
            if (null == vpppath0)
            {
                return false;
            }
            try
            {
                myJobManager0 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath0);
                myJob0 = myJobManager0.Job(0);
                myJobIndependent0 = myJob0.OwnedIndependent;
                myJobManager0.UserQueueFlush();
                myJobManager0.FailureQueueFlush();
                myJob0.ImageQueueFlush();
                myJobIndependent0.RealTimeQueueFlush();
                updateDisplaySource0(cogRecordDisplayin0);
            }
            catch { }
            return true;
        }
        public bool init1(string vpppath1,CogRecordDisplay cogRecordDisplayin1 = null)
        {
            if (null == vpppath1)
            {
                return false;
            }
            try
            {
                myJobManager1 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath1);
                myJob1 = myJobManager1.Job(0);
                myJobIndependent1 = myJob1.OwnedIndependent;
                myJobManager1.UserQueueFlush();
                myJobManager1.FailureQueueFlush();
                myJob1.ImageQueueFlush();
                myJobIndependent1.RealTimeQueueFlush();
                updateDisplaySource1(cogRecordDisplayin1);
            }
            catch { }
            return true;
        }
        public bool init2(string vpppath2, CogRecordDisplay cogRecordDisplayin2 = null)
        {            
            if (null == vpppath2)
            {
                return false;
            }
            try
            {
                myJobManager2 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath2);
                myJob2 = myJobManager2.Job(0);
                myJobIndependent2 = myJob2.OwnedIndependent;
                myJobManager2.UserQueueFlush();
                myJobManager2.FailureQueueFlush();
                myJob2.ImageQueueFlush();
                myJobIndependent2.RealTimeQueueFlush();
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
            myToolGroup0 = (CogToolGroup)(myJobManager0.Job(0).VisionTool);
            myImageFile0 = (CogImageFileTool)(myToolGroup0.Tools["CogImageFileTool1"]);
            myImageFile0.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein0 = new Bitmap(pathin);
            myImageFile0.InputImage = new CogImage8Grey(Imagein0);
            myImageFile0.Run();
            return true;
        }
        private bool GetImage1(string pathin)
        {
            myToolGroup1 = (CogToolGroup)(myJobManager1.Job(0).VisionTool);
            myImageFile1 = (CogImageFileTool)(myToolGroup1.Tools["CogImageFileTool1"]);
            myImageFile1.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein1 = new Bitmap(pathin);
            myImageFile1.InputImage = new CogImage8Grey(Imagein1);
            myImageFile1.Run();
            return true;
        }
        private bool GetImage2(string pathin)
        {
            myToolGroup2 = (CogToolGroup)(myJobManager2.Job(0).VisionTool);
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
            if (null == myJobManager0)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager0.UserResult();
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
        public bool GetResult1(ref Result1 result)
        {
            if (null == myJobManager1)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager1.UserResult();
            if (null == topRecord)
            {
                return false;
            }
            tmpRecord = topRecord.SubRecords[@"Angle"];
            if (null != tmpRecord.Content) if (tmpRecord.Content != null)
                {
                    result.ResultAngle = (double)tmpRecord.Content;
                }
            tmpRecord = topRecord.SubRecords[@"Title"];
            if (null != tmpRecord.Content)
            {
                result.ResultTitle = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Distance"];
            if (null != tmpRecord.Content)
            {
                result.ResultDistance = (double)tmpRecord.Content;
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
            if (null == myJobManager2)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager2.UserResult();
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
        #endregion
        #region Run
        public bool Run0(int time, string _pathin)
        {
            GetImage0(_pathin);
            try
            {
                myJobManager0.Run();
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
                myJobManager1.Run();
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
                myJobManager2.Run();
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
            if (null != myJobManager0)
            {
                myJob0.Reset();
                myJobManager0.Stop();
                myJobManager0.Shutdown();
                myJob0 = null;
                myJobManager0 = null;
                myJobIndependent0 = null;
                return true;
            }
            if (null != myJobManager1)
            {
                myJob1.Reset();
                myJobManager1.Stop();
                myJobManager1.Shutdown();
                myJob1 = null;
                myJobManager1 = null;
                myJobIndependent1 = null;
                return true;
            }
            if (null != myJobManager2)
            {
                myJob2.Reset();
                myJobManager2.Stop();
                myJobManager2.Shutdown();
                myJob2 = null;
                myJobManager2 = null;
                myJobIndependent2 = null;
                return true;
            }
            GC.Collect();
            return false;
        }
        public void Stop()
        {
            myJobManager0.Stop();
            myJobManager1.Stop();
            myJobManager2.Stop();
        }
        #endregion
    }
}
