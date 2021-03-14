USE [master]
GO

-- データベース作成
CREATE DATABASE [DailyReport] COLLATE Japanese_XJIS_100_CI_AS
GO

-- 開発用ログイン作成
CREATE LOGIN [dev] WITH PASSWORD='dev'
GO

-- ユニットテスト用ログイン作成
CREATE LOGIN [unittest] WITH PASSWORD='unittest'
GO

USE [DailyReport]
GO

-- 開発用ユーザー作成
CREATE USER [dev] FOR LOGIN [dev] WITH DEFAULT_SCHEMA=[dev]
GO

-- 開発用スキーマ作成
CREATE SCHEMA [dev] AUTHORIZATION [dev]
GO

-- 開発用ユーザーへの権限付与
GRANT CREATE TABLE TO [dev]
GO

-- ユニットテスト用ユーザー作成
CREATE USER [unittest] FOR LOGIN [unittest] WITH DEFAULT_SCHEMA=[unittest]
GO

-- ユニットテスト用スキーマ作成
CREATE SCHEMA [unittest] AUTHORIZATION [unittest]
GO

-- ユニットテスト用ユーザーへの権限付与
GRANT CREATE TABLE TO [unittest]
GO

