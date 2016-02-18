# VisionProAPI 
CopyRight by Hansey
VisionProAPI For C#
---------声明-----------
VisionProAPI是基于Cognex的VisionPro所开发
使用前请确保拥有Cognex VisionPro的使用权
本程序主要用于对接机台控制程序与VisionPro视觉检测程序
---------使用-----------
1. 请使用Vision.cs 
2. 目前只提供获取本地图片之后进行图像处理的功能
3. 只针对一个VPP且VPP只含有一个Job
4. 具体函数用法:

	初始化：	public bool init(string vpppath,
								CogRecordDisplay cogRecordDisplayin = null)
			初始化init提供两个参数，参数vpppath为VPP的路径，参数cogRecordDisplayin为CogRecordDisplay类型的显示窗体句柄。初始化应用在Form_Load中，软件开启只初始化一次，同时会返回true or false可以用于判定VPP是否加载成功

	运行：	public string Run(int time = 500, 
								string _pathin, 
								ref Result result)
			运行Run提供两个参数，同时返回一个结构体。参数time为运行视觉算法时主程序的休眠时间，默认值为500，如VPP内容较多运行时间较长建议适当增加。参数_pathin为所需处理的本地图片路径。结构体Result为输出结果的结构体，可在Vision中自行定义，同时需要对应QuickBuild的发送项。

	结果获取：private bool GetResult(ref Result result)
			私有函数，当获取结果时，需要修改对应的代码
			try
            {
                tmpRecord = topRecord.SubRecords[@"发送项的名称"];
                if (tmpRecord.Content != null)
                {
                    result.结构体里的变量 = (double)tmpRecord.Content;
                    IsResult1 = true;
                }
            }
            catch
            {
                IsResult1 = false;
            }

            同时向显示控件传输数据的时候，需在如下代码中修改对应的工具名称
            try
            {
                if (null != cogRecordDisplay)
                {
                    tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                    tmpRecord = tmpRecord.SubRecords["LastRun"];
                    tmpRecord = tmpRecord.SubRecords["工具名称.OutputImage"];
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

    更新显示：public bool updateDisplaySource(CogRecordDisplay 					cogRecordDisplayin = null)
    		参数cogRecordDisplayin为CogRecordDisplay类型的显示窗体句柄，此处用于更新窗体上显示的内容

	清楚显示：public bool clearDispalySource()
			用于清楚窗体上显示的内容

	设置图像：public bool setImage(string pathin = null)
			参数pathin为本地图像路径，用于将刚拍摄到的图片显示在显示控件中

			public bool setImage(Bitmap bitmap)
			参数bitmap为内存中的图像，用于将刚拍摄到的图片显示在显示控件中

	图像类型：public bool SetColorMapPreDefined(string type)
			用于设置显示图像类型，共分为四个，
			“None"原始图像,
			"Grye"灰度图,
			"Thermal"热量图,
			"Height"高度图，
			需输入对应的string
	关闭：	public bool Close()
			用于关闭QuickBuild，应在Form_Close中使用