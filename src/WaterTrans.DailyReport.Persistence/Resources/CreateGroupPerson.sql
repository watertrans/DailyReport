IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GroupPerson' and xtype='U')
BEGIN

    CREATE TABLE GroupPerson (
        GroupId UNIQUEIDENTIFIER NOT NULL
      , PersonId UNIQUEIDENTIFIER NOT NULL
      , PositionType NVARCHAR(20) NOT NULL
      , PRIMARY KEY(GroupId, PersonId)
    );

    CREATE INDEX ix_GroupPerson_PersonId ON GroupPerson (PersonId);

END;
