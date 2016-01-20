using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    class VisionVPPsJobs
    {
        #region Definition
        private List<CogJobManager> myJobManager;
        private List<List<CogJob>> myJob;
        private List<List<CogJobIndependent>> myJobIndependent;
        private CogImageFileTool myImageFile;
        private CogToolGroup myToolGroup;
        private Bitmap Imagein;
        private List<CogRecordDisplay> cogRecordDisplay = null;
        private List<string> ResultRequest;
        #endregion
        #region init
        public bool init(int amountOfVpps,List<string> vpppath, List<CogRecordDisplay> cogRecordDisplayin = null)
        {
            if (null == vpppath)
            {
                return false;
            }
            try
            {
                for (int i = 0; i < amountOfVpps; i++)
                {
                    if (null == vpppath[i])
                    {
                        return false;
                    }
                    else
                    {
                        myJobManager[i] = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath[i]);
                        for (int j = 0; j < 1000; j++)
                        {
                            if (null == myJobManager[i].Job(j))
                            {
                                break;
                            }
                            myJob[i][j] = myJobManager[i].Job(j);
                            myJobIndependent[i][j] = myJob[i][j].OwnedIndependent;
                            myJobIndependent[i][j].RealTimeQueueFlush();
                        }
                        myJobManager[i].UserQueueFlush();
                        myJobManager[i].FailureQueueFlush();
                    }
                }
            }
            catch{}
            return true;
        }
        #endregion
        #region updateDisplaySource and clearDisplaySource
        public bool updateAllDispalySource(List<CogRecordDisplay> cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                for (int i = 0; i < cogRecordDisplayin.Count; i++)
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
        private bool GetImage(int numOfVpps, int numOfJobs, string pathin)
        {
            if (null == pathin[numOfVpps])
            {
                return false;
            }
            else
            {
                myToolGroup = (CogToolGroup)(myJobManager[numOfVpps].Job(numOfJobs).VisionTool);
                myImageFile = (CogImageFileTool)(myToolGroup.Tools["CogImageFileTool1"]);
                myImageFile.Operator.Open(pathin, CogImageFileModeConstants.Read);
                Imagein = new Bitmap(pathin);
                myImageFile.InputImage = new CogImage8Grey(Imagein);
                myImageFile.Run();
                Imagein = null;
            }
            return true;
        }
        #endregion
        #region GetResult
        public bool GetResult(int numOfVpps, List<string> resultin, ref List<double> resultout)
        {
            if (null == myJobManager)
            {
                return false;
            }
            if (null == myJobManager[numOfVpps])
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManager[numOfVpps].UserResult();
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
        public bool Run(int numOfVpps,int amountOfJobs,int time,List<List<string>> _pathin)
        {
            for (int i=0; i<=numOfVpps; i++)
            {
                for(int j=0; j<amountOfJobs;j++)
                {
                    GetImage(numOfImages,j, pathin[i][j]);
                }                
            }
            try
            {
                myJobManager[numOfVpps].Run();
                System.Threading.Thread.Sleep(time);
            }
            catch { }
            return true;
        }
        #endregion
        #region SetColorMapPreDefined
        public bool SetColorMapPreDefined(int numOfSource,string type)
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
                for (int i = 0; i < 1000; i++)
                {
                    for(int j=0;j<1000;j++)
                    {
                        myJob[i][j].Reset();
                        myJob = null;
                        myJobIndependent[i][j] = null;
                    }
                    myJobManager[i].Stop();
                    myJobManager[i].Shutdown();
                    myJobManager = null;
                }
                return true;
            }
            GC.Collect();
            return false;
        }
        public void Stop()
        {
            for (int i = 0; i < 1000; i++)
            {
                myJobManager[i].Stop();
            }
        }
        #endregion
    }
}

