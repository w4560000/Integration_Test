﻿CREATE DATABASE Test;
GO

USE master;
CREATE LOGIN [TestLogin] WITH PASSWORD = 'Aa123456';
GO

USE Test;

CREATE USER [TestUser] FOR LOGIN [TestLogin] WITH DEFAULT_SCHEMA=[dbo]

ALTER ROLE [db_owner] ADD MEMBER [TestUser]

GO