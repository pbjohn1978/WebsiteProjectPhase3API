/**********************************************************************************************************************************
				Create Database script

				this script WILL DROP THE DATABASE!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				USE WITH CAUTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				Oasis - Be Here Now (album) + alittle Elton John - Yellow Brick Road + Frampton Comes Alive
************************************************************************************************************************************/
use master
go
if exists(select * from sys.databases where name = 'WebSiteMember')
	Drop Database [WebSiteMember]
go

create database [WebSiteMember]
go

use [WebSiteMember]
go

/*************************************************************************************
				Create Tables...
*************************************************************************************/

create table colors(
	[Color]					varchar(50)				primary key
)
go
create table WebSiteMembers(
	[Member_ID]				int						identity(1,1)			primary key
	,[MemberName]			varchar(75)				not null				unique
	,[MemberPassword]		varchar(75)				not null
	,[MemberBGcolor]		varchar(50)				foreign key references colors(Color)
	,[MemberTextColor]		varchar(50)				foreign key references colors(Color)
	,[MemberText]			varchar(8000)
	,[MemberAccess]			tinyint					--this will be a number 1 2 or 3... we could add a check constraint later... maybe
	,[GuidGoesHereJohn]		varchar(75)
)
go

--this is the allowed colors for the website.
INSERT INTO [dbo].[colors]
           ([Color])
     values('aqua'), ('black'), ('blue'), ('fuchsia'), ('gray'), ('green'), ('lime'), ('maroon'), ('navy'), ('olive'), ('purple'), ('red'), ('silver'), ('teal'), ('white'), ('yellow')
go