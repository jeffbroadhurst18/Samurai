USE [SamuraiDataCore]
GO
/****** Object:  StoredProcedure [dbo].[FindLongestName]    Script Date: 07/05/2018 20:38:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[FindLongestName]
	-- Add the parameters for the stored procedure here
	@procResult varchar(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 select top 1 @procresult = name from dbo.Samurais order by LEN(name) desc
END
