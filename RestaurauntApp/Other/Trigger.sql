CREATE TRIGGER DeleteExpiredDiscountCode
ON DiscountCodes
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentDate DATETIME = GETDATE();

    DELETE FROM DiscountCodes
    WHERE ValidTo < @CurrentDate;
END;
