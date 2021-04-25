IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Account' and xtype='U')
BEGIN

    CREATE TABLE Account (
        AccountId UNIQUEIDENTIFIER PRIMARY KEY
      , PersonId UNIQUEIDENTIFIER NOT NULL
      , Roles NVARCHAR(max) NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
      , LastLoginTime DATETIMEOFFSET NOT NULL
    );

    CREATE UNIQUE INDEX ix_Account_PersonId ON Account (PersonId);

END;
