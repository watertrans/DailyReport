IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Group' and xtype='U')
BEGIN

    CREATE TABLE [Group] (
        GroupId UNIQUEIDENTIFIER PRIMARY KEY
      , GroupCode NVARCHAR(20) NOT NULL
      , GroupTree NVARCHAR(8) NOT NULL
      , Name NVARCHAR(256) NOT NULL
      , Description NVARCHAR(1024) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , SortNo INT NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
    );

    CREATE UNIQUE INDEX ix_Group_GroupCode ON [Group] (GroupCode);
    CREATE UNIQUE INDEX ix_Group_GroupTree ON [Group] (GroupTree);
    CREATE INDEX ix_Group_Name ON [Group] (Name);

END;
