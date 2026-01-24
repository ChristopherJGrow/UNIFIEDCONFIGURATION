
CREATE PROCEDURE [dbo].[UCS_GetSectionSettings]
    @Environment   VARCHAR(80),
    @Section       VARCHAR(80),
    @Application   VARCHAR(80),
    @AppVersion    INT,
    @Module        VARCHAR(80),
    @UserId        VARCHAR(60)
AS
BEGIN
    SET NOCOUNT ON;

    WITH Variables AS
    (
        SELECT DISTINCT s.Variable
        FROM   dbo.Settings s
        WHERE  s.Environment     = @Environment
          AND  s.Section         = @Section
          AND  s.ApplicationName = @Application
          AND  s.BuildNumber     <= @AppVersion
          AND (s.ModuleName = @Module OR s.ModuleName IS NULL)
          AND (s.UserId = @UserId OR s.UserId IS NULL)
    )
    SELECT
        v.Variable,
        best.Value,
        best.UserId      AS OverridingUserId,
        CASE WHEN best.UserId IS NULL THEN 0 ELSE 1 END AS IsUserOverride,
        best.BuildNumber AS EffectiveBuildNumber,
        best.ModuleName  AS EffectiveModule
    FROM Variables v
    -- First, figure out the newest default build for this variable
    CROSS APPLY
    (
        SELECT MAX(sd.BuildNumber) AS MaxDefaultBuild
        FROM   dbo.Settings sd
        WHERE  sd.Environment     = @Environment
          AND  sd.Section         = @Section
          AND  sd.ApplicationName = @Application
          AND  sd.Variable        = v.Variable
          AND  sd.BuildNumber     <= @AppVersion
          AND  (sd.ModuleName = @Module OR sd.ModuleName IS NULL)
          AND  sd.UserId IS NULL              -- defaults only
    ) def
    -- Then pick the best candidate, applying the new user/default rule
    CROSS APPLY
    (
        SELECT TOP (1)
               s.Value,
               s.BuildNumber,
               s.ModuleName,
               s.UserId,
               s.LastUpdated
        FROM   dbo.Settings s
        WHERE  s.Environment     = @Environment
          AND  s.Section         = @Section
          AND  s.ApplicationName = @Application
          AND  s.Variable        = v.Variable
          AND  s.BuildNumber     <= @AppVersion
          AND (s.ModuleName = @Module OR s.ModuleName IS NULL)
          AND (s.UserId = @UserId OR s.UserId IS NULL)
          -- 🔴 New rule: user overrides only valid if not older than latest default
          AND (
                s.UserId IS NULL
                OR s.BuildNumber >= ISNULL(def.MaxDefaultBuild, 0)
              )
        ORDER BY
               CASE WHEN s.UserId = @UserId THEN 0 ELSE 1 END,
               CASE WHEN s.ModuleName = @Module THEN 0 ELSE 1 END,
               s.BuildNumber DESC,
               s.LastUpdated DESC
    ) AS best;
END
GO
