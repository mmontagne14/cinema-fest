using CinemaFest.Application.Interfaces;
using CinemaFest.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using CinemaFest.Infraestructure.Persistence;

namespace CinemaFest.Persistence.Dapper.Repositories
{
    public class FestivalRepository : IFestivalRepository
    {
        private readonly IDbTransaction dbTransaction;
        private readonly IDbConnection dbConnection;
        private readonly string insertRowRetrievalQuery;

        public FestivalRepository(IDbTransaction dbTransaction, string insertRowRetrievalQuery)
        {
            this.dbTransaction = dbTransaction;
            this.insertRowRetrievalQuery = insertRowRetrievalQuery;
            this.dbConnection = dbTransaction.Connection;

            SqlMapper.AddTypeHandler(typeof(Contact), new JsonTypeHandler());
            //SqlMapper.AddTypeHandler(typeof(ICollection<Address>), new JsonTypeHandler());
            //SqlMapper.AddTypeHandler(typeof(ICollection<FestivalImage>), new JsonTypeHandler());
        }

        public async Task<int> CreateFestivalWithImagesAndLocationsAsync(Festival festival)
        {
            festival = await AddFestivalToDatabase(festival);
            await AddFestivalImagesToDatabase(festival);
            await AddFestivalLocationsToDatabase(festival);
            
            return festival.Id;
        }
        public async Task<IEnumerable<Festival>> RetrieveAllFestivalsWithImagesAndLocationsAsync()
        {
            var allFestivalImages = await GetAllFestivalImages();
            var allFestivalLocations = await GetAllFestivalLocations();
            var allFestivals = await GetAllFestivals();

            foreach (Festival festival in allFestivals)
            {
                festival.Images = allFestivalImages.Any(i => i.Festival_Id == festival.Id) ? allFestivalImages.Where(i => i.Festival_Id == festival.Id).ToList() : null;
                festival.Locations = allFestivalLocations.Any(l => l.Festival_Id == festival.Id) ? allFestivalLocations.Where(l => l.Festival_Id == festival.Id).ToList() : null;
            }

            return allFestivals;
        }
        public async Task<Festival> RetrieveFestivalWithImagesAndLocationsByIdAsync(int id)
        {
            var festival = await GetFestival(id); //TODO check if this can be a parameter coming from validation

            festival.Images = await GetFestivalImagesFromFestival(id);
            festival.Locations = await GetFestivalLocationsFromFestival(id);

            return festival;
        }
        public async Task<int> UpdateFestivalWithImagesAndLocationsAsync(Festival festival)
        {
            await UpdateFestivalInDatabase(festival);
     
            var currentDbImages = await GetFestivalImagesFromFestival(festival.Id); //TODO see if can bring it from validation
            var currentDbLocations = await GetFestivalLocationsFromFestival(festival.Id); //TODO see if can bring it from validation

            await DeleteFestivalImagesInDatabase(currentDbImages, festival.Images);
            await UpsertFestivalImagesInDatabase(festival);

            await DeleteLocationsInDatabase(currentDbLocations, festival.Locations);
            await UpsertLocationsInDatabase(festival);

            //var actualImages = festival.Images;

            //var updateImagesSql = @"INSERT INTO FestivalImages(Id,Img,Type_Id,Festival_Id, Filepath)
            //    VALUES(@Id,@Img,@Type_id,@Festival_Id, @Filepath) 
            //    ON DUPLICATE KEY UPDATE Img = @Img , FilePath = @FilePath;";

            //foreach(FestivalImage image in currentDbImages)
            //{
            //    if(!actualImages.Select(x => x.Id).Contains(image.Id))
            //    {
            //        await DeleteFestivalImageFromDatabaseById(image.Id);
            //    }
            //};



            //foreach (FestivalImage image in actualImages)
            //{
            //    //await dbConnection.ExecuteAsync(@"INSERT INTO FestivalImages(Id,Img,Type_Id,Festival_Id, Filepath)
            //    //VALUES(@Id,@Img,@Type_id,@Festival_Id, @Filepath) 
            //    //ON DUPLICATE KEY UPDATE Img = @Img , FilePath = @FilePath;",
            //    //new
            //    //{
            //    //    image.Id,
            //    //    image.Img,
            //    //    Type_Id = image.Type,
            //    //    Festival_Id = festival.Id,
            //    //    image.FilePath
            //    //}, dbTransaction);
            //    await UpsertFestivalImageInDatabase(image,festival.Id);
            //}

            return festival.Id;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        #region private methods
        private async Task<Festival> AddFestivalToDatabase(Festival festival)
        {
           var insertFestivalSql = new StringBuilder(@"INSERT INTO Festivals(CreatedAt,ModifiedAt,Name,About,FirstEditionYear,Active,Contact)
           values(@CreatedAt, @ModifiedAt, @Name, @About, @FirstEditionYear, @Active, @Contact)");

           var festivalId =  await dbConnection.ExecuteScalarAsync<int>(insertFestivalSql.Append(insertRowRetrievalQuery).ToString(),
                new
                {
                    festival.CreatedAt,
                    festival.ModifiedAt,
                    festival.Name,
                    festival.About,
                    festival.FirstEditionYear,
                    festival.Active,
                    festival.Contact
                }, dbTransaction);

            festival.Id = festivalId;
            return festival;
        }
        private async Task AddFestivalImagesToDatabase(Festival festival)
        {
            if (festival.Images.Any())
            {
                var insertImagesSql = @"INSERT INTO FestivalImages(Img,Type_id,Festival_Id, FilePath) 
                VALUES(@img,@type_id,@festival_id,@filepath)";

                foreach (FestivalImage image in festival.Images)
                {
                    await dbConnection.ExecuteAsync(insertImagesSql,
                      new
                      {
                          image.Img,
                          Type_Id = image.Type,
                          Festival_id = festival.Id,
                          image.FilePath
                      }, dbTransaction);
                }
            }
        }
        private async Task AddFestivalLocationsToDatabase(Festival festival)
        {
            if (festival.Locations.Any())
            {
                var insertLocationsSql = @"INSERT INTO Locations(StreetAddress,City,State,ZipCode,Festival_Id) 
                VALUES(@StreetAddress,@City,@State,@ZipCode,@Festival_Id)";

                foreach (Address location in festival.Locations)
                {
                    await dbConnection.ExecuteAsync(insertLocationsSql,
                      new
                      {
                          location.StreetAddress,
                          location.City,
                          location.State,
                          location.ZipCode,
                          Festival_id = festival.Id
                      }, dbTransaction);
                }
            }
        }

        private async Task<ICollection<FestivalImage>> GetAllFestivalImages()
        {
            return (await dbConnection.QueryAsync<FestivalImage>(
                            @"SELECT Id, TO_BASE64(Img) as Img,Type_id as Type,Festival_Id, FilePath FROM FestivalImages",
                            dbTransaction)).ToList();
        }
        private async Task<ICollection<Location>> GetAllFestivalLocations()
        {
            return (await dbConnection.QueryAsync<Location>(
                @"SELECT * FROM Locations",
                dbTransaction)).ToList();
        }
        private async Task<ICollection<Festival>> GetAllFestivals()
        {
            return (await dbConnection.QueryAsync<Festival>(
                @"SELECT * FROM Festivals",
                dbTransaction)).ToList();
        }

        private async Task<ICollection<FestivalImage>> GetFestivalImagesFromFestival(int id)
        {
            return (await dbConnection.QueryAsync<FestivalImage>(
                @"SELECT Id, TO_BASE64(Img) as Img,Type_id as Type,Festival_Id, FilePath FROM FestivalImages WHERE Festival_Id = @Id",
                new { Id = id },
                dbTransaction)).ToList();
        }
        private async Task<ICollection<Location>> GetFestivalLocationsFromFestival(int id)
        {
            return (await dbConnection.QueryAsync<Location>(
                @"SELECT * FROM Locations WHERE Festival_Id = @Id",
                new { Id = id },
                dbTransaction)).ToList();
        }
        private async Task<Festival> GetFestival(int id)
        {
            return (await dbConnection.QueryAsync<Festival>(
                @"SELECT * FROM Festivals f WHERE f.Id = @Id",
                new { Id = id },
                dbTransaction)).FirstOrDefault();
        }
        private async Task UpdateFestivalInDatabase(Festival festival)
        {
            await dbConnection.ExecuteAsync(
                    @"	UPDATE Festivals SET
                        ModifiedAt = @ModifiedAt,
                        Name = @Name,
                        About = @About,
                        FirstEditionYear = @FirstEditionYear,
                        Active = @Active,
                        Contact = @Contact
                        WHERE Id = @Id;",
                    new
                    {
                        festival.ModifiedAt,
                        festival.Name,
                        festival.About,
                        festival.FirstEditionYear,
                        festival.Active,
                        festival.Contact,
                        festival.Id
                    },
                    dbTransaction);
        }

        private async Task DeleteFestivalImageInDatabaseById(int id)
        {
            await dbConnection.ExecuteAsync(
                     @"	DELETE FROM FestivalImages 
                        WHERE Id = @Id;",
                     new
                     {
                         id
                     },
                     dbTransaction);
        }

        private async Task DeleteFestivalImagesInDatabase(ICollection<FestivalImage> currentDatabaseList, ICollection<FestivalImage> updateList)
        {
            foreach (FestivalImage image in currentDatabaseList)
            {
                if (!updateList.Select(x => x.Id).Contains(image.Id))
                {
                    await DeleteFestivalImageInDatabaseById(image.Id);
                }
            };
        }

        private async Task DeleteLocationInDatabaseById(int id)
        {
            await dbConnection.ExecuteAsync(
                     @"	DELETE FROM Locations 
                        WHERE Id = @Id;",
                     new
                     {
                         id
                     },
                     dbTransaction);
        }

        private async Task DeleteLocationsInDatabase(ICollection<Location> currentDatabaseList, ICollection<Location> updateList)
        {
            foreach (Location location in currentDatabaseList)
            {
                if (!updateList.Select(x => x.Id).Contains(location.Id))
                {
                    await DeleteLocationInDatabaseById(location.Id);
                }
            };
        }

        private async Task UpsertFestivalImageInDatabase(FestivalImage image, int festivalId)
        {
            await dbConnection.ExecuteAsync(@"INSERT INTO FestivalImages(Id,Img,Type_Id,Festival_Id, Filepath)
                VALUES(@Id,@Img,@Type_id,@Festival_Id, @Filepath) 
                ON DUPLICATE KEY UPDATE Img = @Img , FilePath = @FilePath;",
                new
                {
                    image.Id,
                    image.Img,
                    Type_Id = image.Type,
                    Festival_Id = festivalId,
                    image.FilePath
                }, dbTransaction);
        }

        private async Task UpsertLocationInDatabase(Location location, int festivalId)
        {
            await dbConnection.ExecuteAsync(@"INSERT INTO Locations(StreetAddress,City,State,ZipCode,Festival_Id) 
                VALUES(@StreetAddress,@City,@State,@ZipCode,@Festival_Id)
                ON DUPLICATE KEY UPDATE 
                                StreetAddress = @StreetAddress , 
                                City = @City,
                                State = @State , 
                                ZipCode = @ZipCode;",
                new
                {
                    location.Id,
                    location.StreetAddress,
                    location.City,
                    location.State,
                    location.ZipCode,
                    Festival_Id = festivalId
                }, dbTransaction);
        }
        #endregion
        private async Task UpsertFestivalImagesInDatabase(Festival festival)
        {
            foreach (FestivalImage image in festival.Images)
            {
                await UpsertFestivalImageInDatabase(image, festival.Id);
            }
        }
        private async Task UpsertLocationsInDatabase(Festival festival)
        {
            foreach (Location image in festival.Locations)
            {
                await UpsertLocationInDatabase(image, festival.Id);
            }
        }


    }
}
