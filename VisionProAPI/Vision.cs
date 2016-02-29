using System;
using System.Collections.Generic;
using System.Drawing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ImageFile;

namespace VisionProAPI
{
    public class Vision
    {
        #region Definition
        /// <summary>
        /// 定义
        /// </summary>
        private CogJobManager myJobManager;
        private CogJob myJob;
        private CogJobIndependent myJobIndependent;
        private CogImageFileTool mIFTool;
        private Bitmap Imagein;
        private CogToolGroup mTGTool;
        private CogRecordDisplay cogRecordDisplay = null;
        public struct Result
        {
            public double X;
            public double Y;
            public double Angle;
        }
        #endregion
        #region init
        /// <summary>
        /// 初始化
        /// </summary>
        ///
        /// <param name="vpppath">< VPP路径 ></param>
        /// <param name="cogRecordDisplayin">< VisionPro显示控件句柄 ></param>
        ///
        /// <returns>< bool判定是否初始化成功 ></returns>
        public bool init(string vpppath,CogRecordDisplay cogRecordDisplayin = null)
        {
            updateDisplaySource(cogRecordDisplayin);
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
                return true;
            }
            catch
            {
                return false;
            }          
        }
        #endregion
        #region updateDisplaySource and clearDispalySource
        /// <summary>
        /// 更新显示源
        /// </summary>
        ///
        /// <param name="cogRecordDisplayin">< VisionPro显示控件句柄 ></param>
        ///
        /// <returns>< bool判定是否更新成功 ></returns>
        public bool updateDisplaySource(CogRecordDisplay cogRecordDisplayin = null)
        {
            if (null == cogRecordDisplayin)
            {
                return false;
            }
            cogRecordDisplay = cogRecordDisplayin;
            return true;
        }
        /// <summary>
        /// 清除显示源
        /// </summary>
        ///
        /// <returns>< bool判定是否清除成功 ></returns>
        public bool clearDispalySource()
        {
            try
            {
                cogRecordDisplay.Image = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region setImage
        /// <summary>
        /// 设置图像至VisionPro显示控件
        /// </summary>
        ///
        /// <param name="pathin">< 图片路径 ></param>
        ///
        /// <returns>< bool判定是否设置成功 ></returns>
        public bool setImage(string pathin = null)
        {
            if(null != cogRecordDisplay)
            {
                if(null != pathin)
                {
                    if(System.IO.File.Exists(pathin))
                    {
                        Imagein = new Bitmap(pathin);
                        cogRecordDisplay.Image = new CogImage8Grey(Imagein);
                        Imagein.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay.Image = null;
                }
                cogRecordDisplay.AutoFit = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置图像至VisionPro显示控件
        /// </summary>
        ///
        /// <param name="bitmap">< Bitmap 图片 ></param>
        ///
        /// <returns>< bool判定是否设置成功 ></returns>
        public bool setImage(Bitmap bitmap)
        {
            if(null != cogRecordDisplay)
            {
                if(null != bitmap)
                {
                    if(null != Imagein)
                    {
                        Imagein = bitmap;
                        cogRecordDisplay.Image = new CogImage8Grey(bitmap);
                        Imagein.Dispose();
                    }
                    else
                    {
                        cogRecordDisplay.Image = null;
                    }
                }
                else
                {
                    cogRecordDisplay.Image = null;
                }
                cogRecordDisplay.AutoFit = true;
                return true;
            }
            return false;
        }
        #endregion
        #region GetImageFromFile
        /// <summary>
        /// 获取图像
        /// </summary>
        ///
        /// <param name="pathin">< 图片路径 ></param>
        ///
        /// <returns>< bool判定是否获取成功 ></returns>
        private bool GetImage(string pathin)
        {
            bool step1 = false;
            bool step2 = false;
            bool step3 = false;
            try
            {
                mTGTool = (CogToolGroup)(myJobManager.Job(0).VisionTool);
                step1 = true;
            }
            catch
            {
                step1 = false;
            }
            try
            {   
                mIFTool = (CogImageFileTool)(mTGTool.Tools["CogImageFileTool1"]);
                mIFTool.Operator.Open(pathin, CogImageFileModeConstants.Read); 
                step2 = true;
            }
            catch
            {
                step2 = false;
            }
            try
            {
                Imagein = new Bitmap(pathin);
                mIFTool.InputImage = new CogImage8Grey(Imagein);
                step3 = true;
            }
            catch
            {
                step3 = false;
            }
            if(step1 && step2 && step3)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region GetResult
        /// <summary>
        /// 获取结果
        /// </summary>
        ///
        /// <param name="result">< 结构体用于储存结果 ></param>
        ///
        /// <returns>< bool判定是否获取成功 ></returns>
        private bool GetResult(ref Result result)
        {
            bool IsResult1 = false;
            bool IsResult2 = false;
            bool IsResult3 = false;
            bool IsDispaly = false;
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
            try
            {
                tmpRecord = topRecord.SubRecords[@"X"];
                if (tmpRecord.Content != null)
                {
                    result.X = (double)tmpRecord.Content;
                    IsResult1 = true;
                }
            }
            catch
            {
                IsResult1 = false;
            }
            try
            {
                tmpRecord = topRecord.SubRecords[@"Y"];
                if (tmpRecord.Content != null)
                {
                    result.Y = (double)tmpRecord.Content;
                    IsResult2 = true;
                }
            }
            catch
            {
                IsResult2 = false;
            }
            try
            {
                tmpRecord = topRecord.SubRecords[@"Angle"];
                if (tmpRecord.Content != null)
                {
                    result.Angle = (double)tmpRecord.Content;
                    IsResult3 = true;
                }
            }
            catch
            {
                IsResult3 = false;
            }
            try
            {
                if (null != cogRecordDisplay)
                {
                    tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                    tmpRecord = tmpRecord.SubRecords["LastRun"];
                    tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
                    if (null != tmpRecord.Content)
                    {
                        cogRecordDisplay.Record = tmpRecord;
                        IsDispaly = true;
                    }
                    cogRecordDisplay.AutoFit = true;
                }
            }
            catch
            {
                IsDispaly = false;
            }
            if(IsResult1 && IsResult2 && IsResult3 && IsDispaly)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Run
        /// <summary>
        /// 运行Job
        /// </summary>
        ///
        /// <param name="time">< 运行等待时间 ></param>
        /// <param name="_pathin">< 图片路径 ></param>
        /// <param name="result">< 结果结构体 ></param>
        ///
        /// <returns>< 字符串显示错误信息 ></returns>
        public string Run(string _pathin, ref Result result, int time = 500)
        {
            Stop();
            if(GetImage(_pathin))
            {
                try
                {
                    myJobManager.Run();
                    System.Threading.Thread.Sleep(time);
                }
                catch
                {
                    return "Error: JobManager run fail.";
                }
                if(GetResult(ref result))
                {
                    Stop();
                    return "Success!";
                }
                else
                {
                    return "Get result fail";
                }
            }
            else
            {
                return "Error: Get image fail.";
            }
        }
        #endregion
        #region CloseAndStop
        /// <summary>
        /// 关闭
        /// </summary>
        public bool Close()
        {
            if (null != myJobManager)
            {
                try
                {
                    myJob.Reset();
                    myJobManager.Stop();
                    myJobManager.Shutdown();
                    myJob = null;
                    myJobManager = null;
                    myJobIndependent = null;
                    Imagein.Dispose();
                    return true;
                }
                catch
                {
                    return false;
                }
                GC.Collect();
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        ///
        /// <returns>< bool判定是否停止成功 ></returns>
        private bool Stop()
        {
            try
            {
                myJobManager.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region SetDifferentColorMap
        /// <summary>
        /// 设置显示图像格式
        /// </summary>
        ///
        /// <param name="type">< 格式类型 ></param>
        ///
        /// <returns>< bool判定是否显示成功 ></returns>
        public bool SetColorMapPreDefined(string type)
        {
            if(null == type)
            {
                return false;
            }
            else
            {
                switch(type)
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
        }
        #endregion
    }
}
