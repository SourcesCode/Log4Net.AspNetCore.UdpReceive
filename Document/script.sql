USE [Log4Net]
GO
/****** Object:  Table [dbo].[Log4Net]    Script Date: 07/12/2012 19:12:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log4Net](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[AppName] [varchar](50) NULL,
	[Thread] [varchar](255) NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](4000) NULL,
	[Exception] [varchar](2000) NULL,
	[UserId] [varchar](50) NULL,
	[ClientIp] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogTimeCategoryInfo]    Script Date: 07/12/2012 19:12:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogTimeCategoryInfo](
	[CategoryID] [int] NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[TotalNum] [int] NOT NULL,
	[FatalNum] [int] NOT NULL,
	[ErrorNum] [int] NOT NULL,
	[WarnNum] [int] NOT NULL,
	[InfoNum] [int] NOT NULL,
	[DebugNum] [int] NOT NULL,
 CONSTRAINT [PK_LogTimeCategoryInfo] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Log4Net_CategoryLogByTime]    Script Date: 07/12/2012 19:12:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--作者：苏达鼐
--日期：2012-03-19
--功能：根据时间划分日志
--输出
      
CREATE PROCEDURE [dbo].[Log4Net_CategoryLogByTime]
	@P_StartTime datetime,
	@P_EndTime datetime
AS
 BEGIN
	SET NOCOUNT ON;
	
	declare @result table 
	(
		CategoryID int, -- 1:今天 2:昨天 3:	本周 4:本月 5：本季度 6:本年
		CategoryName nvarchar(50),
		TotalNum int,
		FatalNum int,
		ErrorNum int,
		WarnNum int,
		InfoNum int,
		DebugNum int
	)
	
	DECLARE @curDay datetime,@curBeginDay datetime,@curBeginWeek datetime,@curBeginMonth datetime,@curBeginQuarter datetime,@curBeginYear datetime
	-- @curBeginWeek(当前星期第一天)
	-- @curBeginMonth (当前月份第一天)
	-- @curBeginQuarter (当前季度第一天)
	-- @curBeginYear (当年第一天)
	set @curDay = getdate()
	SET @curBeginDay=convert(varchar(10),getdate(),120)
    set @curBeginWeek = DATEADD(Day,((DATEPART(Weekday,@curBeginDay) + @@DATEFIRST -2 )%7)*-1,@curBeginDay)
	set @curBeginMonth =  CONVERT(datetime,CONVERT(char(8),@curBeginDay,120)+'1')
	set @curBeginQuarter = CONVERT(datetime,CONVERT(char(8),DATEADD(Month,DATEPART(Quarter,@curBeginDay)*3-Month(@curBeginDay)-2,@curBeginDay),120)+'1')
	set @curBeginYear =  CONVERT(char(5),@curBeginDay,120)+'01-01'
	-- 今天
	insert into @result 
	
	select 1 As 'CategoryID','今天' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where @curBeginDay<=[Date]
		And [Date]< @curDay
	
	print '今天'+'|'+ convert(varchar,@curBeginDay,120) + '|'+convert(varchar,@curDay,120)
	
	-- 昨天
	insert into @result 
	
	select 2 As 'CategoryID','昨天' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where DATEADD(dd,-1,@curBeginDay)<=[Date]
		And [Date]< @curBeginDay
	
	print '昨天'+'|'+ convert(varchar,DATEADD(dd,-1,@curBeginDay),120) + '|'+convert(varchar,@curBeginDay,120)
	
	-- 本周
	insert into @result 
	
	select 3 As 'CategoryID','本周' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where @curBeginWeek<=[Date]
		And [Date]< @curDay
		
	print '本周'+'|'+ convert(varchar,@curBeginWeek,120) + '|'+convert(varchar,@curDay,120)
	
	-- 本月
	insert into @result 
	
	select 4 As 'CategoryID','本月' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where @curBeginMonth <=[Date]
		And [Date]< @curDay
	
	print '本月'+'|'+ convert(varchar,@curBeginMonth,120) + '|'+convert(varchar,@curDay,120)
	
	-- 本季度
	insert into @result 
	
	select 5 As 'CategoryID','本季度' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where @curBeginQuarter<=[Date]
		And [Date]< @curDay
	
	print '本季度'+'|'+ convert(varchar,@curBeginQuarter,120) + '|'+convert(varchar,@curDay,120)
	
	-- 本年
	insert into @result 
	
	select 6 As 'CategoryID','本年' As 'CategoryName',Count(*) As TotalNum
	,ISNULL(Sum(
			Case 
				When [Level]='FATAL' Then 1
				Else 0
			End
	    ),0) As 'FatalNum'
	,ISNULL(Sum(
			Case 
				When [Level]='ERROR' Then 1
				Else 0
			End
	    ),0) As 'ErrorNum'
	,ISNULL(Sum(
			Case 
				When [Level]='WARN' Then 1
				Else 0
			End
	    ),0) As 'WarnNum'
    ,ISNULL(Sum(
			Case 
				When [Level]='INFO' Then 1
				Else 0
			End
	    ),0) As 'InfoNum'
	,ISNULL(Sum(
			Case 
				When [Level]='DEBUG' Then 1
				Else 0
			End 
	    ),0) As 'DebugNum'
	from dbo.Log4Net
	where @curBeginYear<=[Date]
		And [Date]< @curDay
		
	print '本年'+'|'+ convert(varchar,@curBeginYear,120) + '|'+convert(varchar,@curDay,120)
	
	Select * from @result
 END
GO
