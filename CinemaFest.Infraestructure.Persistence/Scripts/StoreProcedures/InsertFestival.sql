CREATE DEFINER=`root`@`localhost` PROCEDURE `cinemafest`.`InsertFestival`(
                    CreatedAt DATE,
                    ModifiedAt DATE,
                    Name VARCHAR(45),
                    About VARCHAR(500),
                    FirstEditionYear INT ,
                    ProfileImg mediumblob,
                    CoverPageImg mediumblob,
                    Active BOOL,
                    Contact JSON,
                    Locations JSON
)
begin
	set @ProfileImageTypeId = (select Id from imagetypes where Code = 'Profile' limit 1);

	set @CoverPageImageTypeId = (select Id from imagetypes where Code = 'CoverPage' limit 1);
	
	INSERT INTO Festivals(CreatedAt,ModifiedAt,Name,About,FirstEditionYear,Active,Contact)
   		values(CreatedAt,ModifiedAt,Name,About,FirstEditionYear,Active,Contact);
   		
   	set @FestivalId = last_insert_id();
   
    if  (ProfileImg is not null) THEN 
	    INSERT INTO FestivalImages(Img, TypeId, FestivalId)
	                    VALUES(ProfileImg, @ProfileImageTypeId, @FestivalId);
	end if;
	
	    if (CoverPageImg is not null) THEN 
	    INSERT INTO FestivalImages(Img, TypeId, FestivalId)
	                    VALUES(CoverPageImg, @CoverPageImageTypeId, @FestivalId);
	end if;
    
	if (Locations is not null) then
	
	set @json_items = JSON_LENGTH(`Locations`);
    set @index = 0;

    WHILE @index < @json_items DO
         INSERT INTO cinemafest.locations (StreetAddress,City,State,ZipCode,festival_Id)
         VALUES (
        JSON_EXTRACT(`Locations`, CONCAT('$[', @index, '].StreetAddress')),
        JSON_EXTRACT(`Locations`, CONCAT('$[', @index, '].City')),
        JSON_EXTRACT(`Locations`, CONCAT('$[', @index, '].State')),
        JSON_EXTRACT(`Locations`, CONCAT('$[', @index, '].ZipCode')),
       	@FestivalId
        );
         SET @index := @index + 1;
    END WHILE;
    
	end if;
	select @FestivalId;
END