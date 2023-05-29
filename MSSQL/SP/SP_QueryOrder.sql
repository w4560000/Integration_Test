USE [Test]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_QueryOrder]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_QueryOrder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
    ==================================================================
    Description
         
                
    ==================================================================
    History    
        2025/05/29	Leo	Created. 

    =================================================================
    Step
    
    ==================================================================
    Result

    ==================================================================
    Example
    
        EXEC Test.dbo.SP_QueryOrder
            @OrderID = 1

            
*/


CREATE PROCEDURE [dbo].[SP_QueryOrder]
     @OrderID        INT    -- 琩高把计
AS
BEGIN    
    --ち传程舦ㄏノ
    --EXECUTE AS LOGIN = 'Executer';
    SET NOCOUNT ON;
    
    SELECT * FROM dbo.[Order]
    WHERE OrderID = @OrderID

END

GO

GRANT EXECUTE ON [SP_QueryOrder] TO [PUBLIC] AS [dbo] ;
GO