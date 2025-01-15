-- DATABASE

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'dev_dgdatamodel')
BEGIN
CREATE DATABASE [dev_dgdatamodel]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tst_dgdatamodeltest', FILENAME = N'/var/opt/mssql/data/dev_dgdatamodel.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'tst_dgdatamodeltest_log', FILENAME = N'/var/opt/mssql/data/dev_dgdatamodel_1.ldf' , SIZE = 13632KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Latin1_General_CI_AS
END;
ALTER DATABASE [dev_dgdatamodel] SET COMPATIBILITY_LEVEL = 100;
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dev_dgdatamodel].[dbo].[sp_fulltext_database] @action = 'enable'
end;
ALTER DATABASE [dev_dgdatamodel] SET ANSI_NULL_DEFAULT OFF;
ALTER DATABASE [dev_dgdatamodel] SET ANSI_NULLS OFF;
ALTER DATABASE [dev_dgdatamodel] SET ANSI_PADDING OFF;
ALTER DATABASE [dev_dgdatamodel] SET ANSI_WARNINGS OFF;
ALTER DATABASE [dev_dgdatamodel] SET ARITHABORT OFF;
ALTER DATABASE [dev_dgdatamodel] SET AUTO_CLOSE OFF;
ALTER DATABASE [dev_dgdatamodel] SET AUTO_SHRINK OFF;
ALTER DATABASE [dev_dgdatamodel] SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE [dev_dgdatamodel] SET CURSOR_CLOSE_ON_COMMIT OFF;
ALTER DATABASE [dev_dgdatamodel] SET CURSOR_DEFAULT  GLOBAL;
ALTER DATABASE [dev_dgdatamodel] SET CONCAT_NULL_YIELDS_NULL OFF;
ALTER DATABASE [dev_dgdatamodel] SET NUMERIC_ROUNDABORT OFF;
ALTER DATABASE [dev_dgdatamodel] SET QUOTED_IDENTIFIER OFF;
ALTER DATABASE [dev_dgdatamodel] SET RECURSIVE_TRIGGERS OFF;
ALTER DATABASE [dev_dgdatamodel] SET  DISABLE_BROKER;
ALTER DATABASE [dev_dgdatamodel] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;
ALTER DATABASE [dev_dgdatamodel] SET DATE_CORRELATION_OPTIMIZATION OFF;
ALTER DATABASE [dev_dgdatamodel] SET TRUSTWORTHY OFF;
ALTER DATABASE [dev_dgdatamodel] SET ALLOW_SNAPSHOT_ISOLATION OFF;
ALTER DATABASE [dev_dgdatamodel] SET PARAMETERIZATION SIMPLE;
ALTER DATABASE [dev_dgdatamodel] SET READ_COMMITTED_SNAPSHOT OFF;
ALTER DATABASE [dev_dgdatamodel] SET HONOR_BROKER_PRIORITY OFF;
ALTER DATABASE [dev_dgdatamodel] SET RECOVERY FULL;
ALTER DATABASE [dev_dgdatamodel] SET  MULTI_USER;
ALTER DATABASE [dev_dgdatamodel] SET PAGE_VERIFY CHECKSUM;
ALTER DATABASE [dev_dgdatamodel] SET DB_CHAINING OFF;
ALTER DATABASE [dev_dgdatamodel] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF );
ALTER DATABASE [dev_dgdatamodel] SET TARGET_RECOVERY_TIME = 0 SECONDS;
ALTER DATABASE [dev_dgdatamodel] SET DELAYED_DURABILITY = DISABLED;
ALTER AUTHORIZATION ON DATABASE::[dev_dgdatamodel] TO [sa];
ALTER DATABASE [dev_dgdatamodel] SET  READ_WRITE;
USE dev_dgdatamodel;
-- TABLES

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[blogs](
	[blogs_id] [int] IDENTITY(1,1) NOT NULL,
	[blogs_title] [varchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[blogs_description] [text] COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_blogs] PRIMARY KEY CLUSTERED 
(
	[blogs_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[blogs] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[comments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[comments](
	[comments_id] [int] IDENTITY(1,1) NOT NULL,
	[posts_id] [int] NOT NULL,
	[comments_text] [text] COLLATE Latin1_General_CI_AS NOT NULL,
	[comments_username] [varchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[comments_email] [varchar](128) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_comments] PRIMARY KEY CLUSTERED 
(
	[comments_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[comments] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[footertext]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[footertext](
	[footertext_id] [int] IDENTITY(1,1) NOT NULL,
	[footertext_title] [varchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_footertext] PRIMARY KEY CLUSTERED 
(
	[footertext_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[footertext] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[footertextdesc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[footertextdesc](
	[footertext_id] [int] NOT NULL,
	[footertext_desc] [text] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_footertextdesc] PRIMARY KEY CLUSTERED 
(
	[footertext_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[footertextdesc] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[posts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[posts](
	[posts_id] [int] IDENTITY(1,1) NOT NULL,
	[blogs_id] [int] NOT NULL,
	[posts_title] [varchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[posts_text] [text] COLLATE Latin1_General_CI_AS NOT NULL,
	[posts_username] [varchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[posts_email] [varchar](128) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_posts] PRIMARY KEY CLUSTERED 
(
	[posts_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[posts] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[poststotags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[poststotags](
	[posts_id] [int] NOT NULL,
	[tags_id] [int] NOT NULL,
	[poststotags_comments] [varchar](128) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_poststotags] PRIMARY KEY CLUSTERED 
(
	[posts_id] ASC,
	[tags_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[poststotags] TO  SCHEMA OWNER;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tags](
	[tags_id] [int] IDENTITY(1,1) NOT NULL,
	[tags_name] [varchar](128) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_tags] PRIMARY KEY CLUSTERED 
(
	[tags_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
ALTER AUTHORIZATION ON [dbo].[tags] TO  SCHEMA OWNER;
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_comments_posts]') AND parent_object_id = OBJECT_ID(N'[dbo].[comments]'))
ALTER TABLE [dbo].[comments]  WITH CHECK ADD  CONSTRAINT [FK_comments_posts] FOREIGN KEY([posts_id])
REFERENCES [dbo].[posts] ([posts_id]);
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_comments_posts]') AND parent_object_id = OBJECT_ID(N'[dbo].[comments]'))
ALTER TABLE [dbo].[comments] CHECK CONSTRAINT [FK_comments_posts];
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_footertextdesc_footertext]') AND parent_object_id = OBJECT_ID(N'[dbo].[footertextdesc]'))
ALTER TABLE [dbo].[footertextdesc]  WITH CHECK ADD  CONSTRAINT [FK_footertextdesc_footertext] FOREIGN KEY([footertext_id])
REFERENCES [dbo].[footertext] ([footertext_id]);
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_footertextdesc_footertext]') AND parent_object_id = OBJECT_ID(N'[dbo].[footertextdesc]'))
ALTER TABLE [dbo].[footertextdesc] CHECK CONSTRAINT [FK_footertextdesc_footertext];
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_posts_blogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[posts]'))
ALTER TABLE [dbo].[posts]  WITH CHECK ADD  CONSTRAINT [FK_posts_blogs] FOREIGN KEY([blogs_id])
REFERENCES [dbo].[blogs] ([blogs_id]);
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_posts_blogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[posts]'))
ALTER TABLE [dbo].[posts] CHECK CONSTRAINT [FK_posts_blogs];
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_poststotags_posts]') AND parent_object_id = OBJECT_ID(N'[dbo].[poststotags]'))
ALTER TABLE [dbo].[poststotags]  WITH CHECK ADD  CONSTRAINT [FK_poststotags_posts] FOREIGN KEY([posts_id])
REFERENCES [dbo].[posts] ([posts_id]);
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_poststotags_posts]') AND parent_object_id = OBJECT_ID(N'[dbo].[poststotags]'))
ALTER TABLE [dbo].[poststotags] CHECK CONSTRAINT [FK_poststotags_posts];
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_poststotags_tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[poststotags]'))
ALTER TABLE [dbo].[poststotags]  WITH CHECK ADD  CONSTRAINT [FK_poststotags_tags] FOREIGN KEY([tags_id])
REFERENCES [dbo].[tags] ([tags_id]);
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_poststotags_tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[poststotags]'))
ALTER TABLE [dbo].[poststotags] CHECK CONSTRAINT [FK_poststotags_tags];
