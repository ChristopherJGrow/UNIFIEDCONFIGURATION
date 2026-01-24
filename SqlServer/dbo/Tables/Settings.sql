CREATE TABLE [dbo].[Settings] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Environment]     VARCHAR (10)  NOT NULL,
    [ApplicationName] VARCHAR (80)  NOT NULL,
    [ModuleName]      VARCHAR (80)  NULL,
    [BuildNumber]     INT           CONSTRAINT [DF_Settings_BuildNumber] DEFAULT ((1000000)) NOT NULL,
    [Section]         VARCHAR (80)  NOT NULL,
    [Variable]        VARCHAR (80)  NOT NULL,
    [Value]           VARCHAR (256) NOT NULL,
    [UserId]          VARCHAR (60)  NULL,
    [LastUpdated]     DATETIME      CONSTRAINT [DF_Settings_LastUpdated] DEFAULT (getdate()) NOT NULL
);

