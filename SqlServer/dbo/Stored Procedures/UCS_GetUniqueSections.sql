
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UCS_GetUniqueSections] 
	-- Add the parameters for the stored procedure here
    @Environment   VARCHAR(80),    
    @Application   VARCHAR(80),    
    @Module        VARCHAR(80)
   
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT s.Section
    FROM dbo.Settings s
    WHERE  s.Environment = @Environment         
          AND s.ApplicationName = @Application
          AND s.ModuleName = @Module          
     
END