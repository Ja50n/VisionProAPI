using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    public class VisionVPPJob
    {
        #region Definition
        private CogJobManager myJobManager;
        private CogJob myJob;
        private CogJobIndependent myJobIndependent;
        private CogImageFileTool myImageFile;
        private Bitmap Imagein;
        private CogToolGroup myToolGroup;
        private CogRecordDisplay cogRecordDisplay = null;
        private List<string> ResultRequest;
        private string ResultRequestOnce;
        #endregion
        #region init
        public bool init(string vpppath, CogRecordDisplay cogRecordDisplayin = null)
        {
            if (null == vpppath)
            {
                return false;
            }
            try
            {
                myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath);
                myJob = myJobManager.Job(0);
                myJobIndependent = myJob.OwnedIndependent;
                myJobManager.UserQueueFlush();
                myJobManager.FailureQueueFlush();
                myJob.ImageQueueFlush();
                myJobIndependent.RealTimeQueueFlush();
                updateDisplaySource(cogRecordDisplayin);
            }
            catch { }
            return true;
        }
        #endregion
        #region updateDisplaySource and clearDisplaySource
        public bool updateDisplaySource(CogRecordDisplay cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            else
            {
                cogRecordDisplay = cogRecordDisplayin;
                return true;
            } 
        }
        public bool clearDisplaySource()
        {
            cogRecordDisplay = null;
            return true;
        }
        public bool setImage(Bitmap bitmap)
        {
            if (null != cogRecordDisplay)
            {
                if (null != bitmap)
                {
                    if (null != Imagein)
                    {
                        Imagein.Dispose();
                    }
                    Imagein = new Bitmap(bitmap);
                    cogRecordDisplay.Image = new CogImage8Grey(Imagein);
                }
                else
                {
                    cogRecordDisplay.Image = null;
                }
                cogRecordDisplay.Fit(true);
                return true;
            }
            return false;
        }
        #endregion
        #region GetImageFromFile
        private bool GetImage(string pathin)
        {
            myToolGroup = (CogToolGroup)(myJobManager.Job(0).VisionTool);
            myImageFile = (CogImageFileTool)(myToolGroup.Tools["CogImageFileTool1"]);
            myImageFile.Operator.Open(pathin, CogImageFileModeConstants.Read);
            Imagein = new Bitmap(pathin);
            myImageFile.InputImage = new CogImage8Grey(Imagein);
            myImageFile.Run();
            return true;
        }
        #endregion
        #region GetResult
        /// <summary>
        /// Get All Results together
        /// </summary>
        /// <param name="resultin"></param>
        /// <param name="resultout"></param>
        /// <returns></returns>
        public bool GetResult(List<string> resultin, ref List<double> resultout)
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
            if (null != resultin)
            {
                for (int i = 0; i < resultin.Count; i++)
                {
                    if (null == resultin[i])
                    {
                        return false;
                    }
                    else
                    {
                        ResultRequest[i] = "@\"" + resultin[i] + "\"";
                        tmpRecord = topRecord.SubRecords[ResultRequest[i]];
                        resultout[i] = (double)tmpRecord.Content;
                        ResultRequest[i] = null;
                    }
                }
                return true;
            }
                return false;
        }
        #endregion
        #region Run
        public bool Run(int time, string _pathin)
        {
            GetImage(_pathin);
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
        public bool SetColorMapPreDefined(string type)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
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
                myJob.Reset();
                myJobManager.Stop();
                myJobManager.Shutdown();
                myJob = null;
                myJobManager = null;
                myJobIndependent = null;
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
    }
}
