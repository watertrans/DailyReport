IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AuthorizationCode' and xtype='U')
BEGIN

    CREATE TABLE AuthorizationCode (
        CodeId NVARCHAR(100) PRIMARY KEY
      , ApplicationId UNIQUEIDENTIFIER NOT NULL
      , AccountId UNIQUEIDENTIFIER NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , ExpiryTime DATETIMEOFFSET NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
    );

END;
