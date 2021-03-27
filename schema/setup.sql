USE [master]
GO

-- データベース作成
CREATE DATABASE [DailyReport] COLLATE Japanese_XJIS_100_CI_AS
GO

USE [DailyReport]
GO

-- 開発用ログイン作成
CREATE LOGIN [daily_report_dev] WITH PASSWORD='ZXcvbNM#1'
GO

-- 開発用ユーザー作成
CREATE USER [daily_report_dev] FOR LOGIN [daily_report_dev] WITH DEFAULT_SCHEMA=[dev]
GO

-- 開発用スキーマ作成
CREATE SCHEMA [dev] AUTHORIZATION [daily_report_dev]
GO

-- 開発用ユーザーへの権限付与
GRANT CREATE TABLE TO [daily_report_dev]
GO

-- ユニットテスト用ログイン作成
CREATE LOGIN [daily_report_ut] WITH PASSWORD='ZXcvbNM#2'
GO

-- ユニットテスト用ユーザー作成
CREATE USER [daily_report_ut] FOR LOGIN [daily_report_ut] WITH DEFAULT_SCHEMA=[ut]
GO

-- ユニットテスト用スキーマ作成
CREATE SCHEMA [ut] AUTHORIZATION [daily_report_ut]
GO

-- ユニットテスト用ユーザーへの権限付与
GRANT CREATE TABLE TO [daily_report_ut]
GO

