CREATE OR ALTER PROCEDURE [dbo].[SanctionVerificationSearch](
	@SanctionIds IdTable READONLY, 
	@Category VARCHAR(64) = NULL,
	@InsuredName VARCHAR(128) = NULL,
	@InsuredDateOfBirth VARCHAR(64) = NULL,
	@InsuredIcNumber VARCHAR(15) = NULL,
	@ResultSanctionIds VARCHAR(MAX) OUTPUT,
	@IgnoreSanctionIds BIT = 0
)

AS

SET NOCOUNT ON;

DECLARE
	@FormattedNameType1 VARCHAR(MAX),
	@FormattedNameType2 VARCHAR(MAX),
	@MatchedSanctionIds IdTable,
	@FilteredSanctionIds1 IdTable,
	@FilteredSanctionIds2 IdTable,
	@SanctionNameIds IdTable,
	@FormattedNameType3 NameTable,
	@Rule INT,
	@Query NVARCHAR(MAX),
	@TempQuery NVARCHAR(MAX)

-- SET @ResultSanctionIds = 'Category: "' + @Category + '"';
-- SET @ResultSanctionIds = 'InsuredIcNumber: "' + @InsuredIcNumber + '"';
-- SET @ResultSanctionIds = 'DOB: "' + @InsuredDateOfBirth + '"';
-- SET @ResultSanctionIds = 'InsuredName: "' + @InsuredName + '"';
-- RETURN 1;

IF (dbo.IsNull(@Category) = 0)
	BEGIN
		SET @TempQuery = 'SELECT Id FROM Sanctions WHERE Category=@Category';
		IF (@IgnoreSanctionIds = 0) 
			SET @TempQuery += ' AND Id IN (SELECT Id FROM @SanctionIds)';

		INSERT INTO @FilteredSanctionIds1 (Id) EXEC sp_executesql @TempQuery, N'@Category VARCHAR(64), @SanctionIds IdTable READONLY', 
			@Category=@Category, @SanctionIds=@SanctionIds

		-- INSERT INTO 
		-- 	@FilteredSanctionIds1 (Id)
		-- SELECT 
		-- 	Id 
		-- FROM 
		-- 	Sanctions
		-- WHERE 
		-- 	(@IgnoreSanctionIds = 1 OR Id IN (SELECT Id FROM @SanctionIds)) AND 
		-- 	Category = @Category;

		SET @IgnoreSanctionIds = 0;
	END
ELSE 
	INSERT INTO  @FilteredSanctionIds1 (Id) SELECT Id FROM @SanctionIds

IF (dbo.IsNull(@InsuredIcNumber) = 0)
	BEGIN
		SET @TempQuery = 'SELECT SanctionId FROM SanctionIdentities WHERE IdNumber = @InsuredIcNumber';
		IF (@IgnoreSanctionIds = 0) 
			SET @TempQuery += ' AND SanctionId IN (SELECT Id FROM @SanctionIds)';

		INSERT INTO @MatchedSanctionIds (Id) EXEC sp_executesql @TempQuery, N'@InsuredIcNumber VARCHAR(64), @SanctionIds IdTable READONLY', 
			@InsuredIcNumber=@InsuredIcNumber, @SanctionIds=@FilteredSanctionIds1


		-- INSERT INTO 
		-- 	@MatchedSanctionIds (Id) 
		-- SELECT 
		-- 	SanctionId 
		-- FROM 
		-- 	SanctionIdentities 
		-- WHERE 
		-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds1)) AND 
		-- 	IdNumber = @InsuredIcNumber;

		IF ((SELECT COUNT(*) FROM @MatchedSanctionIds) > 0)
			BEGIN
				SET @ResultSanctionIds = dbo.JoinIdTable(@MatchedSanctionIds, ',');
				SET @Rule = dbo.GetConstantInt('RuleIdentity');
				RETURN @Rule;
			END
	END

IF (dbo.IsNull(@InsuredDateOfBirth) = 0)
	BEGIN
		SET @TempQuery = 'SELECT SanctionId FROM SanctionBirthDates WHERE (DateOfBirth = @InsuredDateOfBirth OR
			(DateOfBirth IS NULL AND YearOfBirth = @InsuredYearOfBirth))';
		IF (@IgnoreSanctionIds = 0) 
			SET @TempQuery += ' AND SanctionId IN (SELECT Id FROM @SanctionIds)';

		DECLARE @InsuredYearOfBirth INT = Year(@InsuredDateOfBirth);
		INSERT INTO @FilteredSanctionIds2 (Id) EXEC sp_executesql @TempQuery, N'@InsuredDateOfBirth VARCHAR(64), @InsuredYearOfBirth VARCHAR(4), @SanctionIds IdTable READONLY', 
			@InsuredDateOfBirth=@InsuredDateOfBirth, @InsuredYearOfBirth=@InsuredYearOfBirth, @SanctionIds=@FilteredSanctionIds1

		-- INSERT INTO 
		-- 	@FilteredSanctionIds2 (Id)
		-- SELECT 
		-- 	SanctionId
		-- FROM 
		-- 	SanctionBirthDates
		-- WHERE 
		-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds1)) AND 
		-- 	(DateOfBirth = @InsuredDateOfBirth OR (DateOfBirth IS NULL AND YearOfBirth = Year(@InsuredDateOfBirth)))

		IF ((SELECT COUNT(*) FROM @FilteredSanctionIds2) = 0)
			RETURN 0;	

		SET @IgnoreSanctionIds = 0;
	END
ELSE 
	INSERT INTO  @FilteredSanctionIds2 (Id) SELECT Id FROM @FilteredSanctionIds1

-- Format Name
SET @FormattedNameType1 = dbo.RemoveSpecialCharacters(@InsuredName)
INSERT INTO @FormattedNameType3 SELECT * FROM STRING_SPLIT(@FormattedNameType1, ' ');

SET @FormattedNameType1 = NULL
SELECT 
	@FormattedNameType1 = COALESCE(@FormattedNameType1, '') + Name
FROM
	@FormattedNameType3

SET @FormattedNameType2 = NULL
DELETE FROM @FormattedNameType3 WHERE EXISTS (SELECT * FROM SanctionExclusions WHERE Keyword = Name)
SELECT 
	@FormattedNameType2 = COALESCE(@FormattedNameType2, '') + Name
FROM
	@FormattedNameType3

-- Search Type 1
SET @TempQuery = 'SELECT SanctionId FROM SanctionFormatNames WHERE Type = @Type AND Name = @FormattedName';
IF (@IgnoreSanctionIds = 0) 
	SET @TempQuery += ' AND SanctionId IN (SELECT Id FROM @SanctionIds)';

INSERT INTO @MatchedSanctionIds (Id) EXEC sp_executesql @TempQuery, N'@Type INT, @FormattedName VARCHAR(MAX), @SanctionIds IdTable READONLY', 
	@Type=1, @FormattedName=@FormattedNameType1, @SanctionIds=@FilteredSanctionIds2

-- INSERT INTO 
-- 	@MatchedSanctionIds (Id) 
-- SELECT 
-- 	SanctionId 
-- FROM 
-- 	SanctionFormatNames 
-- WHERE 
-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds2)) AND 
-- 	Type = 1 AND 
-- 	Name = @FormattedNameType1;

IF ((SELECT COUNT(*) FROM @MatchedSanctionIds) > 0)
	BEGIN
		SET @ResultSanctionIds = dbo.JoinIdTable(@MatchedSanctionIds, ',');
		SET @Rule = dbo.GetConstantInt('RuleNameSymbolRemoval');
		RETURN @Rule;
	END

-- Search Type 2
INSERT INTO @MatchedSanctionIds (Id) EXEC sp_executesql @TempQuery, N'@Type INT, @FormattedName VARCHAR(MAX), @SanctionIds IdTable READONLY', 
	@Type=2, @FormattedName=@FormattedNameType2, @SanctionIds=@FilteredSanctionIds2

-- INSERT INTO 
-- 	@MatchedSanctionIds (Id) 
-- SELECT 
-- 	SanctionId 
-- FROM 
-- 	SanctionFormatNames 
-- WHERE 
-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds2)) AND 
-- 	Type = 2 AND 
-- 	Name = @FormattedNameType2;

IF ((SELECT COUNT(*) FROM @MatchedSanctionIds) > 0)
	BEGIN
		SET @ResultSanctionIds = dbo.JoinIdTable(@MatchedSanctionIds, ',');
		SET @Rule = dbo.GetConstantInt('RuleNameKeywordReplacement');
		RETURN @Rule;
	END

-- Search Type 3
SET @TempQuery = 'SELECT SanctionNameId FROM SanctionFormatNames WHERE Type = @Type AND Name IN (SELECT Name FROM @FormattedNames)';
IF (@IgnoreSanctionIds = 0) 
	SET @TempQuery += ' AND SanctionId IN (SELECT Id FROM @SanctionIds)';

SET @TempQuery += ' GROUP BY SanctionNameId HAVING COUNT(SanctionNameId) >= 3';

INSERT INTO @SanctionNameIds (Id) EXEC sp_executesql @TempQuery, N'@Type INT, @FormattedNames NameTable READONLY, @SanctionIds IdTable READONLY', 
	@Type=3, @FormattedNames=@FormattedNameType3, @SanctionIds=@FilteredSanctionIds2

-- INSERT INTO 
-- 	@SanctionNameIds (Id) 
-- SELECT 
-- 	SanctionNameId 
-- FROM 
-- 	SanctionFormatNames 
-- WHERE 
-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds2)) AND 
-- 	Type = 3 AND
-- 	Name IN (SELECT Name FROM @FormattedNameType3)
-- GROUP BY 
-- 	SanctionNameId 
-- HAVING 
-- 	COUNT(SanctionNameId) >= 3;

IF ((SELECT COUNT(*) FROM @SanctionNameIds) > 0)
	BEGIN
		INSERT INTO 
			@MatchedSanctionIds (Id) 
		SELECT 
			SanctionId 
		FROM 
			SanctionFormatNames 
		WHERE 
			SanctionNameId IN (SELECT Id FROM @SanctionNameIds);
		
		SET @ResultSanctionIds = dbo.JoinIdTable(@MatchedSanctionIds, ',');
		SET @Rule = dbo.GetConstantInt('RuleNameGroupKeyword');
		RETURN @Rule;
	END

-- Search Type 4
SET @TempQuery = 'SELECT SanctionNameId FROM SanctionFormatNames WHERE Type = @Type AND Name IN (SELECT Name FROM @FormattedNames)';
IF (@IgnoreSanctionIds = 0) 
	SET @TempQuery += ' AND SanctionId IN (SELECT Id FROM @SanctionIds)';

SET @TempQuery += ' GROUP BY SanctionNameId, TypeIndex HAVING COUNT(SanctionNameId) >= 3';

INSERT INTO @SanctionNameIds (Id) EXEC sp_executesql @TempQuery, N'@Type INT, @FormattedNames NameTable READONLY, @SanctionIds IdTable READONLY', 
	@Type=4, @FormattedNames=@FormattedNameType3, @SanctionIds=@FilteredSanctionIds2

-- INSERT INTO 
-- 	@SanctionNameIds (Id) 
-- SELECT 
-- 	SanctionNameId 
-- FROM 
-- 	SanctionFormatNames 
-- WHERE 
-- 	(@IgnoreSanctionIds = 1 OR SanctionId IN (SELECT Id FROM @FilteredSanctionIds2)) AND 
-- 	Type = 4 AND
-- 	Name IN (SELECT Name FROM @FormattedNameType3)
-- GROUP BY 
-- 	SanctionNameId, TypeIndex
-- HAVING 
-- 	COUNT(SanctionNameId) >= 3;

IF ((SELECT COUNT(*) FROM @SanctionNameIds) > 0)
	BEGIN
		INSERT INTO 
			@MatchedSanctionIds (Id) 
		SELECT 
			SanctionId 
		FROM 
			SanctionFormatNames 
		WHERE 
			SanctionNameId IN (SELECT Id FROM @SanctionNameIds);
		
		SET @ResultSanctionIds = dbo.JoinIdTable(@MatchedSanctionIds, ',');
		SET @Rule = dbo.GetConstantInt('RuleNameGroupKeywordReplacement');
		RETURN @Rule;
	END

RETURN 0;