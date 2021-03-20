IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Person' and xtype='U')
BEGIN

    CREATE TABLE Person (
        PersonId UNIQUEIDENTIFIER PRIMARY KEY
      , PersonCode NVARCHAR(20) NOT NULL
      , Name NVARCHAR(100) NOT NULL
      , Title NVARCHAR(100) NOT NULL
      , Description NVARCHAR(400) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , SortNo INT NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
      , DeleteTime DATETIMEOFFSET
    );

    CREATE UNIQUE INDEX ix_Person_PersonCode ON Person (PersonCode);
    CREATE INDEX ix_Person_Name ON Person (Name);

END;
