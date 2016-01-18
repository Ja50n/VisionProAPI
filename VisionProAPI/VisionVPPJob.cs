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
        public bool GetResults(List<string> resultin, ref List<double> resultout)
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
            if (null == resultin)
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
                        ResultRequest[i] = resultin[i];
                        resultout[i] = (double)tmpRecord.Content;
                    }
                }
                return false;
            }
                return true;
        }
        /// <summary>
        /// Get One Result with List string
        /// </summary>
        /// <param name="resultin"></param>
        /// <param name="numOfResult"></param>
        /// <param name="resultout"></param>
        /// <returns></returns>
        public bool GetOneResult(List<string> resultin, int numOfResult, ref List<double> resultout)
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
            if (null == resultin)
            {
                if (null == resultin[numOfResult])
                {
                    return false;
                }
                else
                {
                    ResultRequest[numOfResult] = "@\"" + resultin[numOfResult] + "\"";
                    tmpRecord = topRecord.SubRecords[ResultRequest[numOfResult]];
                    ResultRequest[numOfResult] = resultin[numOfResult];
                    resultout[numOfResult] = (double)tmpRecord.Content;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get One Result with string
        /// </summary>
        /// <param name="resultin"></param>
        /// <param name="resultout"></param>
        /// <returns></returns>
        public bool GetOneResult(string resultin, ref double resultout)
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
            if (null == resultin)
            {
                ResultRequestOnce = "@\"" + resultin + "\"";
                tmpRecord = topRecord.SubRecords[ResultRequestOnce];
                ResultRequestOnce = resultin;
                resultout = (double)tmpRecord.Content;
                return false;
            }
            return true;
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
