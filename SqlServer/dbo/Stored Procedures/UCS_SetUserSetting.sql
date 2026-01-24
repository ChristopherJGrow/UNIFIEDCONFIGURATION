
CREATE PROCEDURE [dbo].[UCS_SetUserSetting]
    @Environment   VARCHAR(80),
    @Section       VARCHAR(80),
    @Variable      VARCHAR(80),
    @Application   VARCHAR(80),
    @AppVersion    INT,
    @Module        VARCHAR(80) = NULL,
    @UserId        VARCHAR(60),
    @Value         VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Safety: user override must have a user id
    IF @UserId IS NULL
    BEGIN
        RAISERROR('UCS_SetUserSetting requires a non-NULL @UserId.', 16, 1);
        RETURN;
    END;

    -- If a user-specific row already exists for this exact key, update it.
    IF EXISTS
    (
        SELECT 1
        FROM   dbo.Settings s
        WHERE  s.Environment     = @Environment
          AND  s.ApplicationName = @Application
          AND  s.Section         = @Section
          AND  s.Variable        = @Variable
          AND  s.BuildNumber     = @AppVersion
          AND  ISNULL(s.ModuleName, '') = ISNULL(@Module, '')
          AND  s.UserId         = @UserId
    )
    BEGIN
        UPDATE dbo.Settings
        SET    Value       = @Value,
               LastUpdated = GETDATE()
        WHERE  Environment     = @Environment
          AND  ApplicationName = @Application
          AND  Section         = @Section
          AND  Variable        = @Variable
          AND  BuildNumber     = @AppVersion
          AND  ISNULL(ModuleName, '') = ISNULL(@Module, '')
          AND  UserId         = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Settings
        (
            Environment,
            ApplicationName,
            ModuleName,
            BuildNumber,
            Section,
            Variable,
            Value,
            UserId,
            LastUpdated
        )
        VALUES
        (
            @Environment,
            @Application,
            @Module,
            @AppVersion,
            @Section,
            @Variable,
            @Value,
            @UserId,
            GETDATE()
        );
    END
END;
