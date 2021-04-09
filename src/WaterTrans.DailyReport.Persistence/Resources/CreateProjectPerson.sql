IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProjectPerson' and xtype='U')
BEGIN

    CREATE TABLE ProjectPerson (
        ProjectId UNIQUEIDENTIFIER NOT NULL
      , PersonId UNIQUEIDENTIFIER NOT NULL
      , PRIMARY KEY(ProjectId, PersonId)
    );

    CREATE INDEX ix_ProjectPerson_PersonId ON ProjectPerson (PersonId);

END;
