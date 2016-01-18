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
        private List<CogImageFileTool> myImageFile;
        private List<Bitmap> Imagein;
        private List<CogRecordDisplay> cogRecordDisplay = null;
        private List<CogToolGroup> myToolGroup;
        private List<string> ResultRequest;
        private string ResultRequestOnce;
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
        #endregion
        #region GetImageFromFile
        private bool GetImage(int amountOfJobs, List<string> pathin)
        {
            for(int i=0;i<amountOfJobs;i++)
            {
                if (null == pathin[i])
                {
                    return false;
                }
                else
                {
                    myToolGroup[i] = (CogToolGroup)(myJobManager.Job(i).VisionTool);
                    myImageFile[i] = (CogImageFileTool)(myToolGroup[i].Tools["CogImageFileTool1"]);
                    myImageFile[i].Operator.Open(pathin[i], CogImageFileModeConstants.Read);
                    Imagein[i] = new Bitmap(pathin[i]);
                    myImageFile[i].InputImage = new CogImage8Grey(Imagein[i]);
                    myImageFile[i].Run();
                }
            }
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
    }
}
