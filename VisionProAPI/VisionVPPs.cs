using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    class VisionVPPs
    {
        #region Definition
        private List<CogJobManager> myJobManager;
        private List<CogJob> myJob;
        private List<CogJobIndependent> myJobIndependent;
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
                        myJob[i] = myJobManager[i].Job(0);
                        myJobIndependent[i] = myJob[i].OwnedIndependent;
                        myJobManager[i].UserQueueFlush();
                        myJobManager[i].FailureQueueFlush();
                        myJobIndependent[i].RealTimeQueueFlush();
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
        #endregion  
        #region GetImageFromFile
        private bool GetImage(int numOfVpps, List<string> pathin)
        {
            if (null == pathin[numOfVpps])
            {
                return false;
            }
            else
            {
                myToolGroup = (CogToolGroup)(myJobManager[numOfVpps].Job(numOfVpps).VisionTool);
                myImageFile = (CogImageFileTool)(myToolGroup.Tools["CogImageFileTool1"]);
                myImageFile.Operator.Open(pathin[numOfVpps], CogImageFileModeConstants.Read);
                Imagein = new Bitmap(pathin[numOfVpps]);
                myImageFile.InputImage = new CogImage8Grey(Imagein);
                myImageFile.Run();
                Imagein = null;
            }
            return true;
        }
        #endregion
    }
}
