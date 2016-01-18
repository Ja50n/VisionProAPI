using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ResultsAnalysis;

namespace VisionProAPI
{
    public class Vision
    {
        #region Definition
        private CogJobManager myJobManager;
        private CogJob myJob;
        private CogJobIndependent myJobIndependent;
        private CogImageFileTool mIFTool;
        private Bitmap Imagein;
        private CogToolGroup mTGTool;
        private CogRecordDisplay cogRecordDisplay = null;
        private List<CogJobManager> myJobManagerList;
        private List<CogJob> myJobList;
        private List<CogJobIndependent> myJobIndependentList;
		private List<CogJob> myJobsList;
        public struct Result
        {
            public double ResultX;
            public double ResultY;
            public double ResultAngle;
        }
        #endregion
        #region init
		/// <summary>
		/// One VPP with One Job
		/// </summary>
		/// <param name="vpppath">Vpppath.</param>
		/// <param name="cogRecordDisplayin">Cog record displayin.</param>
        public bool init(string vpppath,CogRecordDisplay cogRecordDisplayin = null)
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
            catch
            {
 
            }
            return true;           
        }
		/// <summary>
		/// Any VPP with List and Each VPP contains any Jobs With List
		/// </summary>
		/// <param name="vpppath">Vpppath.</param>
		/// <param name="cogRecordDisplayin">Cog record displayin.</param>
		public bool init(List<string> vpppath,CogRecordDisplay cogRecordDisplayin = null)
		{
			if (null == vpppath)
			{
				return false;
			}
			try
			{
				for(int i =0; i<vpppath.Count;i++)
				{
					myJobManagerList[i] = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath[i]);
					//Problem: How to get the number of Jobs of each VPP, and if each VPP has different number of Jobs.
					for(int j =0; j<1000 ;j++)
					{
						myJobsList[j] = myJobManagerList[i].Job(j);
						myJobIndependentList[j] = myJobsList[j].OwnedIndependent;
						myJobsList[j].ImageQueueFlush();
						myJobIndependentList[j].RealTimeQueueFlush();
						if(null == myJobsList[j])
						{
							break;
						}
					}
					myJobManagerList[i].UserQueueFlush();
					myJobManagerList[i].FailureQueueFlush();
					updateDisplaySource(cogRecordDisplayin);
				}
			}
			catch
			{

			}
			return true;           
		}
		/// <summary>
		/// Any VPPS with List and each VPP contains One Job.
		/// </summary>
		/// <param name="count">Count.</param>
		/// <param name="vpppath">Vpppath.</param>
		/// <param name="cogRecordDisplayin">Cog record displayin.</param>
		public bool init(List<string> vpppath, CogRecordDisplay cogRecordDisplayin = null)
        {
			if (null == vpppath)
            {
                return false;
            }
            for (int i = 0; i < vpppath.Count; i++)
            {
                if (vpppath[i] == null)
                {
                    return false;
                }
                try
                {
                    myJobManagerList[i] = (CogJobManager)CogSerializer.LoadObjectFromFile(vpppath[i]);
                    myJobList[i] = myJobManagerList[i].Job(0);
                    myJobIndependentList[i] = myJobList[i].OwnedIndependent;
                    myJobManagerList[i].UserQueueFlush();
                    myJobManagerList[i].FailureQueueFlush();
                    myJobList[i].ImageQueueFlush();
                    myJobIndependentList[i].RealTimeQueueFlush();
					updateDisplaySource(cogRecordDisplayin);
                }
                catch
                {

                }
            }
            return true;
        }
        #endregion
        #region updateDisplaySource
		/// <summary>
		/// Update the display source of the Display tools of Main User Interface
		/// </summary>
		/// <returns><c>true</c>, if display source was updated, <c>false</c> otherwise.</returns>
		/// <param name="cogRecordDisplayin">Cog record displayin.</param>
        public bool updateDisplaySource(CogRecordDisplay cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay = cogRecordDisplayin;
            return true;
        }
        #endregion
        #region GetImageFromFile
		/// <summary>
		/// Get the Image source (One VPP with One Job)
		/// </summary>
		/// <returns><c>true</c>, if in was dataed, <c>false</c> otherwise.</returns>
		/// <param name="pathin">Pathin.</param>
        private bool DataIn(string pathin)
        {
            mTGTool = (CogToolGroup)(myJobManager.Job(0).VisionTool);
            mIFTool = (CogImageFileTool)(mTGTool.Tools["CogImageFileTool1"]);
            mIFTool.Operator.Open(pathin, CogImageFileModeConstants.Read);            
            Imagein = new Bitmap(pathin);
            mIFTool.InputImage = new CogImage8Grey(Imagein);
            mIFTool.Run();
            return true; 
        }
		/// <summary>
		/// Get the Image source (some VPP with One Job)
		/// </summary>
		/// <returns><c>true</c>, if in was dataed, <c>false</c> otherwise.</returns>
		/// <param name="pathin">Pathin.</param>
        private bool DataIn(List<string> pathin)
        {
            for (int i = 0; i < pathin.Count; i++)
            {
                mTGTool = (CogToolGroup)(myJobManagerList[i].Job(0).VisionTool);
                mIFTool = (CogImageFileTool)(mTGTool.Tools["CogImageFileTool1"]);
                mIFTool.Operator.Open(pathin[i], CogImageFileModeConstants.Read);
                Imagein = new Bitmap(pathin[i]);
                mIFTool.InputImage = new CogImage8Grey(Imagein);
                mIFTool.Run();
            }                
            return true;
        }
        #endregion
        #region GetResult
        public bool GetResult(ref Result result)
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
                result.ResultX = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Y"];
            if (tmpRecord.Content != null)
            {
                result.ResultY = (double)tmpRecord.Content;
            }
            tmpRecord = topRecord.SubRecords[@"Angle"];
            if (tmpRecord.Content != null)
            {
                result.ResultAngle = (double)tmpRecord.Content;
            }
            if (null != cogRecordDisplay)
            {
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                if (null != tmpRecord.Content)
                {
                    cogRecordDisplay.Record = tmpRecord;
                }
                cogRecordDisplay.Fit(true);
            }
            return true;
        }
        public bool GetResult(int numOfVpp ,ref Result result)
        {
            if (null == myJobManagerList[numOfVpp])
            {
                return false;
            }
            ICogRecord tmpRecord;
            ICogRecord topRecord = myJobManagerList[numOfVpp].UserResult();
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
            if (null != cogRecordDisplay)
            {
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                if (null != tmpRecord.Content)
                {
                    cogRecordDisplay.Record = tmpRecord;
                }
                cogRecordDisplay.Fit(true);
            }
            return true;
        }
        #endregion
        #region SetDifferentColorMap
        public bool SetColorMapPreDefined(int num)
        {
            if (num == 0)
            {
                cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
                return true;
            }
            if (num == 1)
            {
                cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Grey;
                return true;
            }
            if (num == 2)
            {
                cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Thermal;
                return true;
            }
            if (num == 3)
            {
                cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Height;
                return true;
            }
            if (num == 4)
            {
                cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.Custom;
                return true;
            }
            return false;
        }
        #endregion
        #region Run
        public bool Run(int time, string _pathin)
        {
            DataIn(_pathin);
            try
            {
                myJobManager.Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
            { 
			}
            return true;
        }
        public bool Run(int time, List<string> _pathin, int numOfVpp)
        {
            DataIn(_pathin[numOfVpp]);
            try
            {
                myJobManagerList[numOfVpp].Run();
                System.Threading.Thread.Sleep(time);
            }
            catch
			{ 
			}
            return true;
        }
        #endregion
        #region CloseAndStop
        public void Close()
        {
            if (null != myJobManager)
            {
                myJob.Reset();
                myJobManager.Stop();
                myJobManager.Shutdown();
                myJob = null;
                myJobManager = null;
                myJobIndependent = null;
            }
            for (int i = 0; i < myJobManagerList.Count; i++)
            {
                if (null != myJobManagerList[i])
                {
                    myJobList[i].Reset();
                    myJobManagerList[i].Stop();
                    myJobManagerList[i].Shutdown();
                    myJobList[i] = null;
                    myJobManagerList[i] = null;
                    myJobIndependentList[i] = null;
                }
            }
                GC.Collect();
        }
        internal void Stop()
        {
            myJobManager.Stop();
        }
        #endregion
    }
}
