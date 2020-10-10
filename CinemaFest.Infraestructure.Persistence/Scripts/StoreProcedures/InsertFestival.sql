CREATE PROCEDURE `cinemafest`.`InsertFestival`(
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
    
	select @FestivalId;
END