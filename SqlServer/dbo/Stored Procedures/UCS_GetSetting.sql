
CREATE PROCEDURE [dbo].[UCS_GetSetting]
    @Environment   VARCHAR(80),
    @Section       VARCHAR(80),
    @Variable      VARCHAR(80),
    @Application   VARCHAR(80),
    @AppVersion    INT,
    @Module        VARCHAR(80) = NULL,
    @UserId        VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Find the newest default build (no user) for this key up to @AppVersion
    ;WITH DefaultBuild AS
    (
        SELECT MAX(s.BuildNumber) AS MaxDefaultBuild
        FROM   dbo.Settings s
        WHERE  s.Environment     = @Environment
          AND  s.ApplicationName = @Application
          AND  s.Section         = @Section
          AND  s.Variable        = @Variable
          AND  s.BuildNumber     <= @AppVersion
          AND  (s.ModuleName = @Module OR s.ModuleName IS NULL)
          AND  s.UserId IS NULL              -- defaults only
    ),
    CandidateSettings AS
    (
        SELECT
            s.Value,
            s.BuildNumber,
            s.ModuleName,
            s.UserId,
            s.LastUpdated,
            -- precedence scores
            CASE WHEN s.UserId = @UserId THEN 0 ELSE 1 END     AS UserScore,
            CASE WHEN s.ModuleName = @Module THEN 0 ELSE 1 END AS ModuleScore,
            d.MaxDefaultBuild
        FROM   dbo.Settings s
        CROSS JOIN DefaultBuild d
        WHERE  s.Environment     = @Environment
          AND  s.ApplicationName = @Application
          AND  s.Section         = @Section
          AND  s.Variable        = @Variable
          AND  s.BuildNumber     <= @AppVersion
          AND (s.ModuleName = @Module OR s.ModuleName IS NULL)
          AND (s.UserId = @UserId OR s.UserId IS NULL)
          -- 🔴 New rule:
          -- If it's a user override, only keep it if it's as new or newer than the default.
          AND (
                s.UserId IS NULL
                OR s.BuildNumber >= ISNULL(d.MaxDefaultBuild, 0)
              )
    )
    SELECT TOP (1)
           cs.Value,
           cs.UserId      AS OverridingUserId,
           CASE WHEN cs.UserId IS NULL THEN 0 ELSE 1 END AS IsUserOverride,
           cs.BuildNumber AS EffectiveBuildNumber,
           cs.ModuleName  AS EffectiveModule
    FROM   CandidateSettings cs
    ORDER BY
           cs.UserScore   ASC,          -- user-specific first (if allowed)
           cs.ModuleScore ASC,
           cs.BuildNumber DESC,
           cs.LastUpdated DESC;
END
