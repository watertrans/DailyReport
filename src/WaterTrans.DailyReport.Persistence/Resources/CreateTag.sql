IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tag' and xtype='U')
BEGIN

    CREATE TABLE Tag (
        TagId UNIQUEIDENTIFIER PRIMARY KEY
      , TargetId UNIQUEIDENTIFIER NOT NULL
      , TargetTable NVARCHAR(100) NOT NULL
      , Value NVARCHAR(100) NOT NULL
      , CreateTime DATETIMEOFFSET NOT NULL
    );

    CREATE INDEX ix_Tag_TargetId ON Tag (TargetId);
    CREATE INDEX ix_Tag_Value ON Tag (Value);

END;
