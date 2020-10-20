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

        public async Task<int> CreateFestivalAsync(Festival festival)
        {
            festival = await AddFestivalToDatabase(festival);
            await AddFestivalImagesToDatabase(festival);
            await AddFestivalLocationsToDatabase(festival);
            await AddFestivalTaxonomiesToDatabase(festival);


            return festival.Id;
        }
        public async Task<IEnumerable<Festival>> RetrieveAllFestivalsWithImagesAndLocationsAsync()
        {
            var allFestivalImages = await GetAllFestivalImages();
            var allFestivalLocations = await GetAllFestivalLocations();
            var allFestivalTaxonomies = await GetAllFestivalTaxonomies();
            var allFestivals = await GetAllFestivals();

            foreach (Festival festival in allFestivals)
            {
                festival.Images = allFestivalImages.Any(i => i.Festival_Id == festival.Id) ? allFestivalImages.Where(i => i.Festival_Id == festival.Id).ToList() : null;
                festival.Locations = allFestivalLocations.Any(l => l.Festival_Id == festival.Id) ? allFestivalLocations.Where(l => l.Festival_Id == festival.Id).ToList() : null;
                festival.Taxonomies = allFestivalTaxonomies.Any(t => t.Festival_Id == festival.Id) ? allFestivalTaxonomies.Where(t => t.Festival_Id == festival.Id).ToList() : null;
            }

            return allFestivals;
        }
        public async Task<Festival> RetrieveFestivalByIdAsync(int id)
        {
            var festival = await GetFestival(id); //TODO check if this can be a parameter coming from validation

            festival.Images = await GetFestivalImagesFromFestival(id);
            festival.Locations = await GetFestivalLocationsFromFestival(id);
            festival.Taxonomies = await GetFestivalTaxonomiesFromFestival(id);

            return festival;
        }
        public async Task<int> UpdateFestivalWithImagesAndLocationsAsync(Festival festival)
        {
            await UpdateFestivalInDatabase(festival);
     
            var currentDbImages = await GetFestivalImagesFromFestival(festival.Id); //TODO see if can bring it from validation
            var currentDbLocations = await GetFestivalLocationsFromFestival(festival.Id); //TODO see if can bring it from validation
            var currentDbTaxonomies = await GetFestivalTaxonomiesFromFestival(festival.Id);

            await DeleteFestivalImagesInDatabase(currentDbImages, festival.Images);
            await UpsertFestivalImagesInDatabase(festival);

            await DeleteFestivalLocationsInDatabase(currentDbLocations, festival.Locations);
            await UpsertFestivalLocationsInDatabase(festival);

            await DeleteFestivalTaxonomiesInDatabase(currentDbTaxonomies, festival);
            await UpsertFestivalTaxonomiesInDatabase(festival);

            return festival.Id;
        }

        public bool FestivalExistsById(int Id)
        {
            return dbConnection.ExecuteScalar<bool>(@"SELECT count(1) from Festivals where Id=@Id", 
                new { Id },
                dbTransaction);
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
                var insertLocationsSql = @"INSERT INTO Locations(Street,Number,Apartment,Floor,Locality_Id,Festival_Id) 
                VALUES(@Street,@Number,@Apartment,@Floor,@Locality_Id,@Festival_Id)";

                foreach (Address location in festival.Locations)
                {
                    await dbConnection.ExecuteAsync(insertLocationsSql,
                      new
                      {
                          location.Street,
                          location.Number,
                          location.Apartment,
                          location.Floor,
                          Locality_Id = location.Locality_Id,
                          Festival_id = festival.Id
                      }, dbTransaction);
                }
            }
        }

        private async Task AddFestivalTaxonomiesToDatabase(Festival festival)
        {
            if (festival.Taxonomies.Any())
            {
                var insertTaxonomiesSql = @"INSERT INTO FestivalTaxonomies(Taxonomy_Id,Festival_Id) 
                VALUES(@Taxonomy_Id,@Festival_Id)";

                foreach (Taxonomy taxonomies in festival.Taxonomies)
                {
                    await dbConnection.ExecuteAsync(insertTaxonomiesSql,
                      new
                      {
                          Taxonomy_Id = taxonomies.Id,
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

        private async Task<ICollection<Taxonomy>> GetAllFestivalTaxonomies()
        {
            return (await dbConnection.QueryAsync<Taxonomy>(
                @"SELECT t.*,Festival_Id FROM FestivalTaxonomies ft JOIN Taxonomies t 
                                                ON ft.Taxonomy_Id = t.Id",
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
            var locations = (await dbConnection.QueryAsync<Location>(
                @"SELECT * FROM Locations WHERE Festival_Id = @Id",
                new { Id = id },
                dbTransaction)).ToList();

            //foreach(Location location in locations)
            //{
            //    location.Locality = (await dbConnection.QueryAsync<Locality>(
            //    @"SELECT * FROM Localities WHERE Id = @Id",
            //    new { Id = location.Locality_Id },
            //    dbTransaction)).FirstOrDefault();
            //}

            return locations;
        }
        private async Task<ICollection<Taxonomy>> GetFestivalTaxonomiesFromFestival(int id)
        {
            return (await dbConnection.QueryAsync<Taxonomy>(
                @"SELECT t.* FROM FestivalTaxonomies ft JOIN Taxonomies t 
                                                ON ft.Taxonomy_Id = t.Id WHERE Festival_Id = @Id",
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

        private async Task DeleteFestivalLocationInDatabaseById(int id)
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

        private async Task DeleteFestivalLocationsInDatabase(ICollection<Location> currentDatabaseList, ICollection<Location> updateList)
        {
            foreach (Location location in currentDatabaseList)
            {
                if (!updateList.Select(x => x.Id).Contains(location.Id))
                {
                    await DeleteFestivalLocationInDatabaseById(location.Id);
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

        private async Task UpsertFestivalLocationInDatabase(Location location, int festivalId)
        {
            await dbConnection.ExecuteAsync(@"INSERT INTO Locations(Id,Street,Number,Apartment,Floor,Locality_Id,Festival_Id) 
                VALUES(@Id, @Street,@Number,@Apartment,@Floor,@Locality_Id,@Festival_Id)
                ON DUPLICATE KEY UPDATE 
                                Street = @Street, 
                                Number = @Number,
                                Apartment = @Apartment, 
                                Floor = @Floor,
                                Locality_Id = @Locality_Id;",
                new
                {
                    location.Id,
                    location.Street,
                    location.Number,
                    location.Apartment,
                    location.Floor,
                    Locality_Id = location.Locality_Id,
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
        private async Task UpsertFestivalLocationsInDatabase(Festival festival)
        {
            foreach (Location image in festival.Locations)
            {
                await UpsertFestivalLocationInDatabase(image, festival.Id);
            }
        }

        private async Task UpsertFestivalTaxonomyInDatabase(Taxonomy taxonomy, int festivalId)
        {
            await dbConnection.ExecuteAsync(@"INSERT INTO FestivalTaxonomies(Taxonomy_Id,Festival_Id) 
                VALUES(@Taxonomy_Id,@Festival_Id) 
                ON DUPLICATE KEY UPDATE Taxonomy_Id = Taxonomy_Id , Festival_Id = Festival_Id;",
                new
                {
                    Taxonomy_Id = taxonomy.Id,
                    Festival_id = festivalId
                }, dbTransaction);
        }

        private async Task UpsertFestivalTaxonomiesInDatabase(Festival festival)
        {
            foreach(Taxonomy taxonomy in festival.Taxonomies)
            {
                UpsertFestivalTaxonomyInDatabase(taxonomy, festival.Id);
            }
        }

        private async Task DeleteFestivalTaxonomyInDatabase(Taxonomy taxonomy,int festivalId)
        {
            await dbConnection.ExecuteAsync(
         @"	DELETE FROM FestivalTaxonomies 
                        WHERE Taxonomy_Id = @Taxonomy_Id AND Festival_Id = @Festival_Id;",
         new
         {
             Taxonomy_Id = taxonomy.Id,
             Festival_Id = festivalId
         },
         dbTransaction);
        }

        private async Task DeleteFestivalTaxonomiesInDatabase(ICollection<Taxonomy> currentDatabaseList, Festival festival)
        {
            var updateList = festival.Taxonomies;
            foreach (Taxonomy taxonomy in currentDatabaseList)
            {
                if (!updateList.Select(x => x.Id).Contains(taxonomy.Id))
                {
                    await DeleteFestivalTaxonomyInDatabase(taxonomy,festival.Id);
                }
            };
        }
    }
}
