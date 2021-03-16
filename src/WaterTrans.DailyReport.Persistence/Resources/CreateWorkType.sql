IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WorkType' and xtype='U')
BEGIN

    CREATE TABLE WorkType (
        WorkTypeId UNIQUEIDENTIFIER PRIMARY KEY
      , WorkTypeCode NVARCHAR(20) NOT NULL
      , WorkTypeTree NVARCHAR(8) NOT NULL
      , Name NVARCHAR(100) NOT NULL
      , Description NVARCHAR(400) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , SortNo INT
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
      , DeleteTime DATETIMEOFFSET
    );

    CREATE UNIQUE INDEX ix_WorkType_WorkTypeCode ON WorkType (WorkTypeCode);
    CREATE INDEX ix_WorkType_WorkTypeTree ON WorkType (WorkTypeTree);
    CREATE INDEX ix_WorkTypep_Name ON WorkType (Name);

END;
