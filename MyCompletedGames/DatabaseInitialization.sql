CREATE TABLE Platforms (ID int IDENTITY(1,1) PRIMARY KEY, Name varchar(255) NOT NULL, Type int NOT NULL, Image varBinary(MAX) NOT NULL)
CREATE TABLE Games (ID int IDENTITY(1,1) PRIMARY KEY, Name varchar(255) NOT NULL, Completed varchar(255) NOT NULL, PlatformID int FOREIGN KEY REFERENCES Platforms(ID) NOT NULL)
GO
CREATE PROCEDURE CountPlatforms (@Search varchar(255)) AS SELECT COUNT(*) FROM Platforms WHERE Name LIKE '%' + @Search + '%'
GO
CREATE PROCEDURE DeletePlatformById (@ID int) AS DELETE FROM Platforms WHERE ID = @ID
GO
CREATE PROCEDURE EditPlatform (@ID int, @Name varchar(255), @Type int, @Image varBinary(MAX)) AS UPDATE Platforms SET Name = @Name, Type = @Type, Image = @Image WHERE ID = @ID
GO
CREATE PROCEDURE GetPlatformById (@ID int) AS SELECT * FROM Platforms WHERE ID = @ID
GO
CREATE PROCEDURE GetPlatformGames (@PlatformId int) AS SELECT * FROM Games WHERE PlatformId = @PlatformId
GO
CREATE PROCEDURE InsertGame (@Name varchar(255), @Completed varchar(255), @PlatformID int) AS INSERT INTO Games(Name,Completed,PlatformID) VALUES(@Name, @Completed,@PlatformID)
GO
CREATE PROCEDURE InsertPlatform (@Name varchar(255), @Type int, @Image varBinary(MAX)) AS INSERT INTO Platforms(Name,Type,Image) VALUES(@Name, @Type, @Image)
GO
CREATE PROCEDURE SearchPlatforms(@Search varchar(255),@Min int, @Max int) AS
	WITH Results AS
	(
		SELECT ID, Name, Type, ROW_NUMBER() OVER (ORDER BY ID) AS Row
		FROM Platforms
		WHERE Name LIKE '%' + @Search + '%'
	)

	SELECT * 
	FROM Results
	WHERE Row BETWEEN @Min AND @Max;