using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace M503_Stand_2
{
    class Vision2
    {
        #region Definition
        private CogJobManager myJobManager0;
        private CogJobManager myJobManager1;
        private CogJobManager myJobManager2;
        private CogJobManager myJobManager3;
        private CogJob myJob0;
        private CogJob myJob1;
        private CogJob myJob2;
        private CogJob myJob3;
        private CogJobIndependent myJobIndependent0;
        private CogJobIndependent myJobIndependent1;
        private CogJobIndependent myJobIndependent2;
        private CogJobIndependent myJobIndependent3;
        private CogImageFileTool myImageFile0;
        private CogImageFileTool myImageFile1;
        private CogImageFileTool myImageFile2;
        private CogImageFileTool myImageFile3;
        private Bitmap Imagein0;
        private Bitmap Imagein1;
        private Bitmap Imagein2;
        private Bitmap Imagein3;
        private CogToolGroup myToolGroup0;
        private CogToolGroup myToolGroup1;
        private CogToolGroup myToolGroup2;
        private CogToolGroup myToolGroup3;
        private CogRecordDisplay cogRecordDisplay0 = null;
        private CogRecordDisplay cogRecordDisplay1 = null;
        private CogRecordDisplay cogRecordDisplay2 = null;
        private CogRecordDisplay cogRecordDisplay3 = null;
        public struct Result0
        {
        }
        public struct Result1
        {
        }
        public struct Result2
        {
        }
        public struct Result3
        {
        }
        #endregion
        #region init
        public bool init0(string vpppath0, CogRecordDisplay cogRecordDisplayin0 = null)
        {
            updateDisplaySource0(cogRecordDisplayin0);
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
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool init1(string vpppath1, CogRecordDisplay cogRecordDisplayin1 = null)
        {
            updateDisplaySource1(cogRecordDisplayin1);
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
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool init2(string vpppath2, CogRecordDisplay cogRecordDisplayin2 = null)
        {
            updateDisplaySource2(cogRecordDisplayin2);
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
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool init3(string vpppath3, CogRecordDisplay cogRecordDisplayin3 = null)
        {
            updateDisplaySource3(cogRecordDisplayin3);
            if (null == vpppath3)
            {
                return false;
            }
            try
            {
                myJobManager3 = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath3);
                myJob3 = myJobManager3.Job(0);
                myJobIndependent3 = myJob3.OwnedIndependent;
                myJobManager3.UserQueueFlush();
                myJobManager3.FailureQueueFlush();
                myJob3.ImageQueueFlush();
                myJobIndependent3.RealTimeQueueFlush();
            }
            catch
            {
                return false;
            }
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
        public bool updateDisplaySource3(CogRecordDisplay cogRecordDisplayin)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay3 = cogRecordDisplayin;
            return true;
        }
        public bool clearAllDisplaySource()
        {
            cogRecordDisplay0.Image = null;
            cogRecordDisplay1.Image = null;
            cogRecordDisplay2.Image = null;
            cogRecordDisplay3.Image = null;
            return true;
        }
        public bool clearDisplaySource0()
        {
            cogRecordDisplay0.Image = null;
            return true;
        }
        public bool clearDisplaySource1()
        {
            cogRecordDisplay1.Image = null;
            return true;
        }
        public bool clearDisplaySource2()
        {
            cogRecordDisplay2.Image = null;
            return true;
        }
        public bool clearDisplaySource3()
        {
            cogRecordDisplay3.Image = null;
            return true;
        }
        #endregion
        #region setImage
        public bool setImage0(string pahtin = null)
        {
            if (null != cogRecordDisplay0)
            {
                if (null != pahtin)
                {
                    if (System.IO.File.Exists(pahtin))
                    {
                        Imagein0 = new Bitmap(pahtin);
                        cogRecordDisplay0.Image = new CogImage8Grey(Imagein0);
                        Imagein0.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay0.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay0.Image = null;
                }
                cogRecordDisplay0.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage0(Bitmap bitmap)
        {
            if (null != cogRecordDisplay0)
            {
                if (null != bitmap)
                {
                    if (null != Imagein0)
                    {
                        Imagein0.Dispose();
                    }
                    Imagein0 = new Bitmap(bitmap);
                    cogRecordDisplay0.Image = new CogImage8Grey(Imagein0);
                    Imagein0.Dispose();
                }
                else
                {
                    cogRecordDisplay0.Image = null;
                }
                cogRecordDisplay0.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage1(string pahtin = null)
        {
            if (null != cogRecordDisplay1)
            {
                if (null != pahtin)
                {
                    if (System.IO.File.Exists(pahtin))
                    {
                        Imagein1 = new Bitmap(pahtin);
                        cogRecordDisplay1.Image = new CogImage8Grey(Imagein1);
                        Imagein1.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay1.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay1.Image = null;
                }
                cogRecordDisplay1.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage1(Bitmap bitmap)
        {
            if (null != cogRecordDisplay1)
            {
                if (null != bitmap)
                {
                    if (null != Imagein1)
                    {
                        Imagein1.Dispose();
                    }
                    Imagein1 = new Bitmap(bitmap);
                    cogRecordDisplay1.Image = new CogImage8Grey(Imagein1);
                    Imagein1.Dispose();
                }
                else
                {
                    cogRecordDisplay1.Image = null;
                }
                cogRecordDisplay1.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage2(string pahtin = null)
        {
            if (null != cogRecordDisplay2)
            {
                if (null != pahtin)
                {
                    if (System.IO.File.Exists(pahtin))
                    {
                        Imagein2 = new Bitmap(pahtin);
                        cogRecordDisplay2.Image = new CogImage8Grey(Imagein2);
                        Imagein2.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay2.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay2.Image = null;
                }
                cogRecordDisplay2.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage2(Bitmap bitmap)
        {
            if (null != cogRecordDisplay2)
            {
                if (null != bitmap)
                {
                    if (null != Imagein2)
                    {
                        Imagein2.Dispose();
                    }
                    Imagein2 = new Bitmap(bitmap);
                    cogRecordDisplay2.Image = new CogImage8Grey(Imagein2);
                    Imagein2.Dispose();
                }
                else
                {
                    cogRecordDisplay2.Image = null;
                }
                cogRecordDisplay2.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage3(string pahtin = null)
        {
            if (null != cogRecordDisplay3)
            {
                if (null != pahtin)
                {
                    if (System.IO.File.Exists(pahtin))
                    {
                        Imagein3 = new Bitmap(pahtin);
                        cogRecordDisplay3.Image = new CogImage8Grey(Imagein3);
                        Imagein3.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay3.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay3.Image = null;
                }
                cogRecordDisplay3.AutoFit = true;
                return true;
            }
            return false;
        }
        public bool setImage3(Bitmap bitmap)
        {
            if (null != cogRecordDisplay3)
            {
                if (null != bitmap)
                {
                    if (null != Imagein3)
                    {
                        Imagein3.Dispose();
                    }
                    Imagein3 = new Bitmap(bitmap);
                    cogRecordDisplay3.Image = new CogImage8Grey(Imagein3);
                    Imagein3.Dispose();
                }
                else
                {
                    cogRecordDisplay3.Image = null;
                }
                cogRecordDisplay3.AutoFit = true;
                return true;
            }
            return false;
        }
        #endregion
        #region GetImageFromFile
        private bool GetImage0(string pathin)
        {
            try
            {
                myToolGroup0 = (CogToolGroup)(myJobManager0.Job(0).VisionTool);
                myImageFile0 = (CogImageFileTool)(myToolGroup0.Tools["CogImageFileTool1"]);
                myImageFile0.Operator.Open(pathin, CogImageFileModeConstants.Read);
                Imagein0 = new Bitmap(pathin);
                myImageFile0.InputImage = new CogImage8Grey(Imagein0);
                myImageFile0.Run();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetImage1(string pathin)
        {
            try
            {
                myToolGroup1 = (CogToolGroup)(myJobManager1.Job(0).VisionTool);
                myImageFile1 = (CogImageFileTool)(myToolGroup1.Tools["CogImageFileTool1"]);
                myImageFile1.Operator.Open(pathin, CogImageFileModeConstants.Read);
                Imagein1 = new Bitmap(pathin);
                myImageFile1.InputImage = new CogImage8Grey(Imagein1);
                myImageFile1.Run();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetImage2(string pathin)
        {
            try
            {
                myToolGroup2 = (CogToolGroup)(myJobManager2.Job(0).VisionTool);
                myImageFile2 = (CogImageFileTool)(myToolGroup2.Tools["CogImageFileTool1"]);
                myImageFile2.Operator.Open(pathin, CogImageFileModeConstants.Read);
                Imagein2 = new Bitmap(pathin);
                myImageFile2.InputImage = new CogImage8Grey(Imagein2);
                myImageFile2.Run();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetImage3(string pathin)
        {
            try
            {
                myToolGroup3 = (CogToolGroup)(myJobManager3.Job(0).VisionTool);
                myImageFile3 = (CogImageFileTool)(myToolGroup3.Tools["CogImageFileTool1"]);
                myImageFile3.Operator.Open(pathin, CogImageFileModeConstants.Read);
                Imagein3 = new Bitmap(pathin);
                myImageFile3.InputImage = new CogImage8Grey(Imagein3);
                myImageFile3.Run();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region GetResult
        private bool GetResult0(ref Result0 result)
        {
            if (null == myJobManager0)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord;
            try
            {
                topRecord = myJobManager0.UserResult();
                if (null == topRecord)
                {
                    return false;
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
                    cogRecordDisplay0.AutoFit = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetResult1(ref Result1 result)
        {
            if (null == myJobManager1)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord;
            try
            {
                topRecord = myJobManager1.UserResult();
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
                    cogRecordDisplay1.AutoFit = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetResult2(ref Result2 result)
        {
            if (null == myJobManager2)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord;
            try
            {
                topRecord = myJobManager2.UserResult();
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
                    cogRecordDisplay2.AutoFit = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetResult3(ref Result3 result)
        {
            if (null == myJobManager3)
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord;
            try
            {
                topRecord = myJobManager3.UserResult();
                if (null == topRecord)
                {
                    return false;
                }
                if (null != cogRecordDisplay3)
                {
                    tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                    tmpRecord = tmpRecord.SubRecords["LastRun"];
                    tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                    if (null != tmpRecord.Content)
                    {
                        cogRecordDisplay3.Record = tmpRecord;
                    }
                    cogRecordDisplay3.AutoFit = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Run
        public int Run0(int time, string _pathin, ref Result0 result)
        {
            Stop0();
            try
            {
                GetImage0(_pathin);
            }
            catch
            {
                return 1;
            }
            try
            {
                myJobManager0.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
            {
                return 2;
            }
            try
            {
                GetResult0(ref result);
            }
            catch
            {
                return 3;
            }
            finally
            {
                Stop0();
            }
            return 0;
        }
        public int Run1(int time, string _pathin, ref Result1 result)
        {
            Stop1();
            try
            {
                GetImage1(_pathin);
            }
            catch
            {
                return 1;
            }
            try
            {
                myJobManager1.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
            {
                return 2;
            }
            try
            {
                GetResult1(ref result);
            }
            catch
            {
                return 3;
            }
            finally
            {
                Stop1();
            }
            return 0;
        }
        public int Run2(int time, string _pathin, ref Result2 result)
        {
            Stop2();
            try
            {
                GetImage2(_pathin);
            }
            catch
            {
                return 1;
            }
            try
            {
                myJobManager2.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
            {
                return 2;
            }
            try
            {
                GetResult2(ref result);
            }
            catch
            {
                return 3;
            }
            finally
            {
                Stop2();
            }
            return 0;
        }
        public int Run3(int time, string _pathin, ref Result3 result)
        {
            Stop3();
            try
            {
                GetImage3(_pathin);
            }
            catch
            {
                return 1;
            }
            try
            {
                myJobManager3.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
            {
                return 2;
            }
            try
            {
                GetResult3(ref result);
            }
            catch
            {
                return 3;
            }
            finally
            {
                Stop2();
            }
            return 0;
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
        public bool SetColorMapPreDefined3(string type)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay3.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay3.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay3.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay3.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region CloseAndStop
        public bool Close()
        {
            bool closeS1 = false;
            bool closeS2 = false;
            bool closeS3 = false;
            bool closeS4 = false;
            if (null != myJobManager0)
            {
                myJob0.Reset();
                myJobManager0.Stop();
                myJobManager0.Shutdown();
                myJob0 = null;
                myJobManager0 = null;
                myJobIndependent0 = null;
                closeS1 = true;
            }
            else
            {
                closeS1 = false;
            }
            if (null != myJobManager1)
            {
                myJob1.Reset();
                myJobManager1.Stop();
                myJobManager1.Shutdown();
                myJob1 = null;
                myJobManager1 = null;
                myJobIndependent1 = null;
                closeS2 = true;
            }
            else
            {
                closeS2 = false;
            }
            if (null != myJobManager2)
            {
                myJob2.Reset();
                myJobManager2.Stop();
                myJobManager2.Shutdown();
                myJob2 = null;
                myJobManager2 = null;
                myJobIndependent2 = null;
                closeS3 = true;
            }
            else
            {
                closeS3 = false;
            }
            if (null != myJobManager3)
            {
                myJob3.Reset();
                myJobManager3.Stop();
                myJobManager3.Shutdown();
                myJob3 = null;
                myJobManager3 = null;
                myJobIndependent3 = null;
                closeS4= true;
            }
            else
            {
                closeS4 = false;
            }
            GC.Collect();
            if (closeS1 == true && closeS2 == true && closeS3 == true && closeS4 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Stop0()
        {
            myJobManager0.Stop();
        }
        private void Stop1()
        {
            myJobManager1.Stop();
        }
        private void Stop2()
        {
            myJobManager2.Stop();
        }
        private void Stop3()
        {
            myJobManager3.Stop();
        }
        #endregion
    }
}
