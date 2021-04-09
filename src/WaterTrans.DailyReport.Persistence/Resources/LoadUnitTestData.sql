﻿-- アプリケーション
INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, Status, CreateTime, UpdateTime) VALUES (N'00000000-A001-0000-0000-000000000000', N'owner-normal', N'owner-secret', N'owner-normal',   N'', N'["Owner"]', N'["full_control","read","write"]', N'["client_credentials"]', N'NORMAL', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, Status, CreateTime, UpdateTime) VALUES (N'00000000-A002-0000-0000-000000000000', N'owner-suspended', N'owner-secret', N'owner-suspended',   N'', N'["Owner"]', N'["full_control,"read","write""]', N'["client_credentials"]', N'SUSPENDED', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, Status, CreateTime, UpdateTime) VALUES (N'00000000-A003-0000-0000-000000000000', N'reader-read', N'reader-secret', N'reader-read',   N'', N'["Reader"]', N'["read"]', N'["client_credentials"]', N'NORMAL', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, Status, CreateTime, UpdateTime) VALUES (N'00000000-A004-0000-0000-000000000000', N'contributor-write', N'contributor-secret', N'contributor-write',   N'', N'["Contributor"]', N'["read","write"]', N'["client_credentials"]', N'NORMAL', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, Status, CreateTime, UpdateTime) VALUES (N'00000000-A005-0000-0000-000000000000', N'password', N'password-secret', N'password',   N'', N'["Reader"]', N'["read"]', N'["password"]', N'NORMAL', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

-- アクセストークン
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'normal', N'full_control', N'', N'APPLICATION', N'00000000-A001-0000-0000-000000000000', N'["full_control"]', N'NORMAL', N'9999-12-31T23:59:59', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'normal-read', N'read', N'', N'APPLICATION', N'00000000-A001-0000-0000-000000000000', N'["read"]', N'NORMAL', N'9999-12-31T23:59:59', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'normal-write', N'write', N'', N'APPLICATION', N'00000000-A001-0000-0000-000000000000', N'["write"]', N'NORMAL', N'9999-12-31T23:59:59', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'suspended', N'full_control', N'', N'APPLICATION', N'00000000-A001-0000-0000-000000000000', N'["full_control"]', N'SUSPENDED', N'9999-12-31T23:59:59', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'expired', N'full_control', N'', N'APPLICATION', N'00000000-A001-0000-0000-000000000000', N'["full_control"]', N'NORMAL', N'2000-01-01T00:00:00', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
INSERT INTO AccessToken (TokenId, Name, Description, PrincipalType, PrincipalId, Scopes, Status, ExpiryTime, CreateTime, UpdateTime) VALUES (N'exception', N'full_control', N'', N'EXCEPTION', N'00000000-A001-0000-0000-000000000000', N'["full_control"]', N'NORMAL', N'2000-01-01T00:00:00', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

-- 従業員
INSERT INTO Person (PersonId, PersonCode, Name, Title, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-1001-0000-0000-000000000000', N'00001', N'従業員1', N'従業員1役職', N'従業員1説明', N'NORMAL',    0, N'2020-04-01T00:00:00+09:00', N'2020-04-01T00:00:00+09:00');
INSERT INTO Person (PersonId, PersonCode, Name, Title, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-1002-0000-0000-000000000000', N'00002', N'従業員2', N'従業員2役職', N'従業員2説明', N'SUSPENDED', 1, N'2020-04-02T00:00:00+09:00', N'2020-04-02T00:00:00+09:00');
INSERT INTO Person (PersonId, PersonCode, Name, Title, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-1003-0000-0000-000000000000', N'00003', N'従業員3', N'従業員3役職', N'従業員3説明', N'NORMAL',   2, N'2020-04-03T00:00:00+09:00', N'2020-04-03T00:00:00+09:00');

-- プロジェクト
INSERT INTO Project (ProjectId, ProjectCode, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-2001-0000-0000-000000000000', N'00001', N'プロジェクト1', N'プロジェクト1説明', N'NORMAL',    0, N'2020-04-01T00:00:00+09:00', N'2020-04-01T00:00:00+09:00');
INSERT INTO Project (ProjectId, ProjectCode, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-2002-0000-0000-000000000000', N'00002', N'プロジェクト2', N'プロジェクト2説明', N'SUSPENDED', 1, N'2020-04-02T00:00:00+09:00', N'2020-04-02T00:00:00+09:00');
INSERT INTO Project (ProjectId, ProjectCode, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-2003-0000-0000-000000000000', N'00003', N'プロジェクト3', N'プロジェクト3説明', N'NORMAL' ,   2, N'2020-04-03T00:00:00+09:00', N'2020-04-03T00:00:00+09:00');

-- プロジェクト従業員
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2001-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000');
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2001-0000-0000-000000000000', N'00000000-1002-0000-0000-000000000000');
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2001-0000-0000-000000000000', N'00000000-1003-0000-0000-000000000000');
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2002-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000');
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2002-0000-0000-000000000000', N'00000000-1002-0000-0000-000000000000');
INSERT INTO ProjectPerson (ProjectId, PersonId) VALUES (N'00000000-2002-0000-0000-000000000000', N'00000000-1003-0000-0000-000000000000');

-- 部署
INSERT INTO [Group] (GroupId, GroupCode, GroupTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-3001-0000-0000-000000000000', N'00001', N'00', N'部署1', N'部署1説明', N'NORMAL',    0, N'2020-04-01T00:00:00+09:00', N'2020-04-01T00:00:00+09:00');
INSERT INTO [Group] (GroupId, GroupCode, GroupTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-3002-0000-0000-000000000000', N'00002', N'01', N'部署2', N'部署2説明', N'SUSPENDED', 1, N'2020-04-02T00:00:00+09:00', N'2020-04-02T00:00:00+09:00');
INSERT INTO [Group] (GroupId, GroupCode, GroupTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-3003-0000-0000-000000000000', N'00003', N'02', N'部署3', N'部署3説明', N'NORMAL',    2, N'2020-04-03T00:00:00+09:00', N'2020-04-03T00:00:00+09:00');

-- 部署従業員
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3001-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000', N'GENERAL_MANAGER');
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3001-0000-0000-000000000000', N'00000000-1002-0000-0000-000000000000', N'MANAGER');
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3001-0000-0000-000000000000', N'00000000-1003-0000-0000-000000000000', N'STAFF');
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3002-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000', N'STAFF');
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3002-0000-0000-000000000000', N'00000000-1002-0000-0000-000000000000', N'STAFF');
INSERT INTO GroupPerson (GroupId, PersonId, PositionType) VALUES (N'00000000-3002-0000-0000-000000000000', N'00000000-1003-0000-0000-000000000000', N'STAFF');

-- 作業分類
INSERT INTO WorkType (WorkTypeId, WorkTypeCode, WorkTypeTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-4001-0000-0000-000000000000', N'00001', N'00', N'作業分類1', N'作業分類1説明', N'NORMAL',    0, N'2020-04-01T00:00:00+09:00', N'2020-04-01T00:00:00+09:00');
INSERT INTO WorkType (WorkTypeId, WorkTypeCode, WorkTypeTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-4002-0000-0000-000000000000', N'00002', N'01', N'作業分類2', N'作業分類2説明', N'SUSPENDED', 1, N'2020-04-02T00:00:00+09:00', N'2020-04-02T00:00:00+09:00');
INSERT INTO WorkType (WorkTypeId, WorkTypeCode, WorkTypeTree, Name, Description, Status, SortNo, CreateTime, UpdateTime) VALUES (N'00000000-4003-0000-0000-000000000000', N'00003', N'02', N'作業分類3', N'作業分類3説明', N'NORMAL',    2, N'2020-04-03T00:00:00+09:00', N'2020-04-03T00:00:00+09:00');

-- タグ
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5001-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000', N'Person', N'従業員1タグ1', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5002-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000', N'Person', N'従業員1タグ2', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5003-0000-0000-000000000000', N'00000000-1001-0000-0000-000000000000', N'Person', N'従業員1タグ3', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5004-0000-0000-000000000000', N'00000000-2001-0000-0000-000000000000', N'Project', N'プロジェクト1タグ1', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5005-0000-0000-000000000000', N'00000000-2001-0000-0000-000000000000', N'Project', N'プロジェクト1タグ2', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5006-0000-0000-000000000000', N'00000000-2001-0000-0000-000000000000', N'Project', N'プロジェクト1タグ3', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5007-0000-0000-000000000000', N'00000000-3001-0000-0000-000000000000', N'Group', N'部署1タグ1', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5008-0000-0000-000000000000', N'00000000-3001-0000-0000-000000000000', N'Group', N'部署1タグ2', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-5009-0000-0000-000000000000', N'00000000-3001-0000-0000-000000000000', N'Group', N'部署1タグ3', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-500A-0000-0000-000000000000', N'00000000-4001-0000-0000-000000000000', N'WorkType', N'作業分類1タグ1', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-500B-0000-0000-000000000000', N'00000000-4001-0000-0000-000000000000', N'WorkType', N'作業分類1タグ2', N'2020-04-01T00:00:00+09:00');
INSERT INTO Tag (TagId, TargetId, TargetTable, Value, CreateTime) VALUES (N'00000000-500C-0000-0000-000000000000', N'00000000-4001-0000-0000-000000000000', N'WorkType', N'作業分類1タグ3', N'2020-04-01T00:00:00+09:00');
