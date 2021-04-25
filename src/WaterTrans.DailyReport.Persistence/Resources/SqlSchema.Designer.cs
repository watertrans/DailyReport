﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaterTrans.DailyReport.Persistence.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SqlSchema {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlSchema() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WaterTrans.DailyReport.Persistence.Resources.SqlSchema", typeof(SqlSchema).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;AccessToken&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE AccessToken (
        ///        TokenId NVARCHAR(100) PRIMARY KEY
        ///      , Name NVARCHAR(100) NOT NULL
        ///      , Description NVARCHAR(400) NOT NULL
        ///      , ApplicationId UNIQUEIDENTIFIER NOT NULL
        ///      , PrincipalType NVARCHAR(20) NOT NULL
        ///      , PrincipalId UNIQUEIDENTIFIER NULL
        ///      , Scopes NVARCHAR(max) NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , ExpiryTime DATETIMEOFFSET NOT NULL
        ///      ,  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateAccessToken {
            get {
                return ResourceManager.GetString("CreateAccessToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Account&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE Account (
        ///        AccountId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , PersonId UNIQUEIDENTIFIER NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , LastLoginTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///    CREATE UNIQUE INDEX ix_Account_PersonId ON Account (PersonId);
        ///
        ///END;
        ///.
        /// </summary>
        internal static string CreateAccount {
            get {
                return ResourceManager.GetString("CreateAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Application&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE Application (
        ///        ApplicationId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , ClientId NVARCHAR(100) NOT NULL
        ///      , ClientSecret NVARCHAR(100) NOT NULL
        ///      , Name NVARCHAR(100) NOT NULL
        ///      , Description NVARCHAR(400) NOT NULL
        ///      , Roles NVARCHAR(max) NOT NULL
        ///      , Scopes NVARCHAR(max) NOT NULL
        ///      , GrantTypes NVARCHAR(max) NOT NULL
        ///      , RedirectUris NVARCHAR(max) NOT NULL
        ///       [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateApplication {
            get {
                return ResourceManager.GetString("CreateApplication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;AuthorizationCode&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE AuthorizationCode (
        ///        CodeId NVARCHAR(100) PRIMARY KEY
        ///      , ApplicationId UNIQUEIDENTIFIER NOT NULL
        ///      , AccountId UNIQUEIDENTIFIER NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , ExpiryTime DATETIMEOFFSET NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , UpdateTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///END;
        ///.
        /// </summary>
        internal static string CreateAuthorizationCode {
            get {
                return ResourceManager.GetString("CreateAuthorizationCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Group&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE [Group] (
        ///        GroupId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , GroupCode NVARCHAR(20) NOT NULL
        ///      , GroupTree NVARCHAR(8) NOT NULL
        ///      , Name NVARCHAR(256) NOT NULL
        ///      , Description NVARCHAR(1024) NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , SortNo INT NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , UpdateTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///    CREATE UNIQUE INDEX ix [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateGroup {
            get {
                return ResourceManager.GetString("CreateGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;GroupPerson&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE GroupPerson (
        ///        GroupId UNIQUEIDENTIFIER NOT NULL
        ///      , PersonId UNIQUEIDENTIFIER NOT NULL
        ///      , PositionType NVARCHAR(20) NOT NULL
        ///      , PRIMARY KEY(GroupId, PersonId)
        ///    );
        ///
        ///    CREATE INDEX ix_GroupPerson_PersonId ON GroupPerson (PersonId);
        ///
        ///END;
        ///.
        /// </summary>
        internal static string CreateGroupPerson {
            get {
                return ResourceManager.GetString("CreateGroupPerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Person&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE Person (
        ///        PersonId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , PersonCode NVARCHAR(20) NOT NULL
        ///      , LoginId NVARCHAR(256) NOT NULL
        ///      , Name NVARCHAR(256) NOT NULL
        ///      , Title NVARCHAR(100) NOT NULL
        ///      , Description NVARCHAR(1024) NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , SortNo INT NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , UpdateTime DATETIMEOFFSET NOT NU [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreatePerson {
            get {
                return ResourceManager.GetString("CreatePerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Project&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE Project (
        ///        ProjectId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , ProjectCode NVARCHAR(20) NOT NULL
        ///      , Name NVARCHAR(256) NOT NULL
        ///      , Description NVARCHAR(1024) NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , SortNo INT NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , UpdateTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///    CREATE UNIQUE INDEX ix_Project_ProjectCode ON Project (P [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateProject {
            get {
                return ResourceManager.GetString("CreateProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;ProjectPerson&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE ProjectPerson (
        ///        ProjectId UNIQUEIDENTIFIER NOT NULL
        ///      , PersonId UNIQUEIDENTIFIER NOT NULL
        ///      , PRIMARY KEY(ProjectId, PersonId)
        ///    );
        ///
        ///    CREATE INDEX ix_ProjectPerson_PersonId ON ProjectPerson (PersonId);
        ///
        ///END;
        ///.
        /// </summary>
        internal static string CreateProjectPerson {
            get {
                return ResourceManager.GetString("CreateProjectPerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;Tag&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE Tag (
        ///        TagId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , TargetId UNIQUEIDENTIFIER NOT NULL
        ///      , TargetTable NVARCHAR(100) NOT NULL
        ///      , Value NVARCHAR(100) NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///    CREATE INDEX ix_Tag_TargetId ON Tag (TargetId);
        ///    CREATE INDEX ix_Tag_Value ON Tag (Value);
        ///
        ///END;
        ///.
        /// </summary>
        internal static string CreateTag {
            get {
                return ResourceManager.GetString("CreateTag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=&apos;WorkType&apos; and xtype=&apos;U&apos;)
        ///BEGIN
        ///
        ///    CREATE TABLE WorkType (
        ///        WorkTypeId UNIQUEIDENTIFIER PRIMARY KEY
        ///      , WorkTypeCode NVARCHAR(20) NOT NULL
        ///      , WorkTypeTree NVARCHAR(8) NOT NULL
        ///      , Name NVARCHAR(256) NOT NULL
        ///      , Description NVARCHAR(1024) NOT NULL
        ///      , Status NVARCHAR(20) NOT NULL
        ///      , SortNo INT NOT NULL
        ///      , CreateTime DATETIMEOFFSET NOT NULL
        ///      , UpdateTime DATETIMEOFFSET NOT NULL
        ///    );
        ///
        ///    CREATE UN [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateWorkType {
            get {
                return ResourceManager.GetString("CreateWorkType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- アプリケーション
        ///INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, RedirectUris, PostLogoutRedirectUris, Status, CreateTime, UpdateTime) VALUES (N&apos;00000000-A001-0000-0000-000000000000&apos;, N&apos;owner&apos;,        N&apos;owner-secret&apos;,        N&apos;(Debug) Owner Application&apos;,       N&apos;Owner Application for Debug&apos;,       N&apos;[&quot;Owner&quot;]&apos;,       N&apos;[&quot;full_control&quot;,&quot;read&quot;,&quot;write&quot;]&apos;, N&apos;[&quot;client_credentials&quot;]&apos;, N&apos;[]&apos;, N&apos;[]&apos;, N&apos;NORMAL&apos;, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
        ///INS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LoadInitialData {
            get {
                return ResourceManager.GetString("LoadInitialData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- アプリケーション
        ///INSERT INTO Application (ApplicationId, ClientId, ClientSecret, Name, Description, Roles, Scopes, GrantTypes, RedirectUris, PostLogoutRedirectUris, Status, CreateTime, UpdateTime) VALUES (N&apos;00000000-A001-0000-0000-000000000000&apos;, N&apos;owner-normal&apos;,        N&apos;owner-secret&apos;,       N&apos;owner-normal&apos;,         N&apos;&apos;, N&apos;[&quot;Owner&quot;]&apos;,       N&apos;[&quot;full_control&quot;,&quot;read&quot;,&quot;write&quot;]&apos;, N&apos;[&quot;client_credentials&quot;]&apos;, N&apos;[]&apos;, N&apos;[]&apos;, N&apos;NORMAL&apos;,    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
        ///INSERT INTO Application (ApplicationId [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LoadUnitTestData {
            get {
                return ResourceManager.GetString("LoadUnitTestData", resourceCulture);
            }
        }
    }
}
