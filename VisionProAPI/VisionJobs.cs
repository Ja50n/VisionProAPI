using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    class VisionJobs
    {
        #region Definition
        private CogJobManager myJobManager;
        private List<CogJob> myJob;
        private List<CogJobIndependent> myJobIndependent;
        private CogImageFileTool myImageFile;
        private Bitmap Imagein;
        private List<CogRecordDisplay> cogRecordDisplay = null;
        private CogToolGroup myToolGroup;
        private List<string> ResultRequest;
        #endregion
        #region init
        public bool init(string vpppath, int amountOfJobs, List<CogRecordDisplay> cogRecordDisplayin = null)
        {
            if (null == vpppath)
            {
                return false;
            }
            try
            {
                myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath);
                for (int i = 0; i < amountOfJobs; i++)
                {
                    myJob[i] = myJobManager.Job(i);
                    myJobIndependent[i] = myJob[i].OwnedIndependent;
                    myJob[i].ImageQueueFlush();
                    myJobIndependent[i].RealTimeQueueFlush();
                    
                }
                myJobManager.UserQueueFlush();
                myJobManager.FailureQueueFlush();
                updateAllDisplaySource(cogRecordDisplayin);
            }
            catch { }
            return true;
        }
        #endregion
        #region updateDisplaySource and clearDisplaySource
        public bool updateAllDisplaySource(List<CogRecordDisplay> cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                for(int i =0; i<cogRecordDisplayin.Count;i++)
                {
                    if (null == cogRecordDisplayin[i])
                    {
                        return false;
                    }
                    else
                    {
                        cogRecordDisplay[i] = cogRecordDisplayin[i];
                    }
                }
                return true;
            }
            return false;
        }
        public bool updateDisplaySource(int numOfSource, List<CogRecordDisplay> cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                if (null == cogRecordDisplayin[numOfSource])
                {
                    return false;
                }
                else
                {
                    cogRecordDisplay[numOfSource] = cogRecordDisplayin[numOfSource];
                }
                return true;
            }
            return false;
        }
        public bool clearAllDisplaySource()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (null == cogRecordDisplay[i])
                {
                    break;
                }
                else
                {
                    cogRecordDisplay[i] = null;
                }
            }
            return true;
        }
        public bool clearDisplaySource(int numOfSource)
        {
            cogRecordDisplay[numOfSource] = null;
            return true;
        }
        public bool setImage(Bitmap bitmap, int numOfSource)
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
                    cogRecordDisplay[numOfSource].Image = new CogImage8Grey(Imagein);
                }
                else
                {
                    cogRecordDisplay[numOfSource].Image = null;
                }
                cogRecordDisplay[numOfSource].Fit(true);
                return true;
            }
            return false;
        }
        #endregion
        #region GetImageFromFile
        private bool GetImage(int numOfJobs, List<string> pathin)
        {

            if (null == pathin[numOfJobs])
            {
                return false;
            }
            else
            {
                myToolGroup = (CogToolGroup)(myJobManager.Job(numOfJobs).VisionTool);
                myImageFile = (CogImageFileTool)(myToolGroup.Tools["CogImageFileTool1"]);
                myImageFile.Operator.Open(pathin[numOfJobs], CogImageFileModeConstants.Read);
                Imagein = new Bitmap(pathin[numOfJobs]);
                myImageFile.InputImage = new CogImage8Grey(Imagein);
                myImageFile.Run();
                Imagein = null;
            }
            return true;
        }
        #endregion
        #region GetResult
        // public bool GetResultShow(int numOfSource)
        // {
        //     if (myJobManager == null)
        //     {
        //         return false;
        //     }
        //     ICogRecord tmpRecord;
        //     ICogRecord topRecord = myJobManager.UserResult();
        //     if (null != cogRecordDisplay)
        //     {
        //         tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
        //         tmpRecord = tmpRecord.SubRecords["LastRun"];
        //         tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
        //         if (null != tmpRecord.Content)
        //         {
        //             cogRecordDisplay[numOfSource].Record = tmpRecord;
        //         }
        //         cogRecordDisplay[numOfSource].Fit(true);
        //     }
        //     return true;
        // }
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
                        tmpRecord = topRecord.SubRecords[ResultRequest[i]];                            resultout[i] = (double)tmpRecord.Content;
                        ResultRequest[i] = null;
                    }
                }
                return true;
            }
            return false;
        }
        #endregion
        #region Run
        public bool Run(int time, int numOfJobs, List<string> _pathin)
        {
            GetImage(numOfJobs, _pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch { }
            return true;
        }
        #endregion
        #region SetColorMapPreDefined
        public bool SetColorMapPreDefined(string type,int numOfSource)
        {
            if (null == type)
            {
                return false;
            }
            switch (type)
            {
                case "None":
                    cogRecordDisplay[numOfSource].ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                    return true;
                case "Grey":
                    cogRecordDisplay[numOfSource].ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                    return true;
                case "Thermal":
                    cogRecordDisplay[numOfSource].ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                    return true;
                case "Height":
                    cogRecordDisplay[numOfSource].ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
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
                for (int i = 0; i < myJob.Count; i++)
                {
                    myJob[i].Reset();
                    myJobManager.Stop();
                    myJobManager.Shutdown();
                    myJob[i] = null;
                    myJobManager = null;
                    myJobIndependent[i] = null;
                }
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
