IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AccessToken' and xtype='U')
BEGIN

    CREATE TABLE AccessToken (
        TokenId NVARCHAR(100) PRIMARY KEY
      , Name NVARCHAR(100) NOT NULL
      , Description NVARCHAR(400) NOT NULL
      , ApplicationId UNIQUEIDENTIFIER NOT NULL
      , PrincipalType NVARCHAR(20) NOT NULL
      , PrincipalId UNIQUEIDENTIFIER NULL
      , Scopes NVARCHAR(max) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , ExpiryTime DATETIMEOFFSET NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
    );

END;
