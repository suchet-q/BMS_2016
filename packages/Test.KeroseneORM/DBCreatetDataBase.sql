--- =============================================
PRINT 'CREATING THE TEST DATABASE...'
--- =============================================

USE [master];

CREATE DATABASE [KeroseneDB]
ON PRIMARY (
	NAME = N'KeroseneDB',
	FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\KeroseneDB.mdf',
	SIZE = 3072KB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1024KB )
LOG ON (
	NAME = N'KeroseneDB_log',
	FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\KeroseneDB_log.ldf',
	SIZE = 1024KB,
	MAXSIZE = 2048GB,
	FILEGROWTH = 10% );

ALTER DATABASE [KeroseneDB] SET COMPATIBILITY_LEVEL = 100; /* SQL SERVER 2008 */

ALTER DATABASE [KeroseneDB] SET ANSI_NULL_DEFAULT OFF;

ALTER DATABASE [KeroseneDB] SET ANSI_NULLS OFF;

ALTER DATABASE [KeroseneDB] SET ANSI_PADDING OFF;

ALTER DATABASE [KeroseneDB] SET ANSI_WARNINGS OFF;

ALTER DATABASE [KeroseneDB] SET ARITHABORT OFF;

ALTER DATABASE [KeroseneDB] SET AUTO_CLOSE OFF;

ALTER DATABASE [KeroseneDB] SET AUTO_CREATE_STATISTICS ON;

ALTER DATABASE [KeroseneDB] SET AUTO_SHRINK OFF;

ALTER DATABASE [KeroseneDB] SET AUTO_UPDATE_STATISTICS ON;

ALTER DATABASE [KeroseneDB] SET CURSOR_CLOSE_ON_COMMIT OFF;

ALTER DATABASE [KeroseneDB] SET CURSOR_DEFAULT GLOBAL;

ALTER DATABASE [KeroseneDB] SET CONCAT_NULL_YIELDS_NULL OFF;

ALTER DATABASE [KeroseneDB] SET NUMERIC_ROUNDABORT OFF;

ALTER DATABASE [KeroseneDB] SET QUOTED_IDENTIFIER OFF;

ALTER DATABASE [KeroseneDB] SET RECURSIVE_TRIGGERS OFF;

ALTER DATABASE [KeroseneDB] SET DISABLE_BROKER;

ALTER DATABASE [KeroseneDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF

ALTER DATABASE [KeroseneDB] SET DATE_CORRELATION_OPTIMIZATION OFF;

ALTER DATABASE [KeroseneDB] SET TRUSTWORTHY OFF;

ALTER DATABASE [KeroseneDB] SET ALLOW_SNAPSHOT_ISOLATION OFF;

ALTER DATABASE [KeroseneDB] SET PARAMETERIZATION SIMPLE;

ALTER DATABASE [KeroseneDB] SET READ_COMMITTED_SNAPSHOT OFF;

ALTER DATABASE [KeroseneDB] SET HONOR_BROKER_PRIORITY OFF;

ALTER DATABASE [KeroseneDB] SET READ_WRITE;

ALTER DATABASE [KeroseneDB] SET RECOVERY FULL;

ALTER DATABASE [KeroseneDB] SET  MULTI_USER;

ALTER DATABASE [KeroseneDB] SET PAGE_VERIFY CHECKSUM;

ALTER DATABASE [KeroseneDB] SET DB_CHAINING OFF;

PRINT 'DATABASE CREATED'
