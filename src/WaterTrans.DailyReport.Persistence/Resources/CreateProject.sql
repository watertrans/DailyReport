IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Project' and xtype='U')
BEGIN

    CREATE TABLE Project (
        ProjectId UNIQUEIDENTIFIER PRIMARY KEY
      , ProjectCode NVARCHAR(20) NOT NULL
      , Name NVARCHAR(100) NOT NULL
      , Description NVARCHAR(400) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , SortNo INT NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
      , DeleteTime DATETIMEOFFSET
    );

    CREATE UNIQUE INDEX ix_Project_ProjectCode ON Project (ProjectCode);
    CREATE INDEX ix_Project_Name ON Project (Name);

END;
