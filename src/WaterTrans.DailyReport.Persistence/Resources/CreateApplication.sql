IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Application' and xtype='U')
BEGIN

    CREATE TABLE Application (
        ApplicationId UNIQUEIDENTIFIER PRIMARY KEY
      , ClientId NVARCHAR(100) NOT NULL
      , ClientSecret NVARCHAR(100) NOT NULL
      , Name NVARCHAR(100) NOT NULL
      , Description NVARCHAR(400) NOT NULL
      , Roles NVARCHAR(max) NOT NULL
      , Scopes NVARCHAR(max) NOT NULL
      , GrantTypes NVARCHAR(max) NOT NULL
      , RedirectUris NVARCHAR(max) NOT NULL
      , PostLogoutRedirectUris NVARCHAR(max) NOT NULL
      , Status NVARCHAR(20) NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , UpdateTime DATETIMEOFFSET NOT NULL
    );

    CREATE UNIQUE INDEX ix_Application_ClientId ON Application (ClientId);

END;
