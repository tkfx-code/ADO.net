-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Marta
-- Create date: 
-- Description:	Search and filter by Actor First Name
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[uspSearchActorsActorId]
	-- Add the parameters for the stored procedure here
	@ActorId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		f.title as Title,
		f.release_year as Released,
		a.actor_id as Actor_Id
	from film f
	inner join film_actor fa on f.film_id=fa.film_id
	inner join actor a on fa.actor_id=a.actor_id
	where a.actor_id = @ActorId
END
