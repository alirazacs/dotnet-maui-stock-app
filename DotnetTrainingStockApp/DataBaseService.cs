using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DotnetTrainingStockApp
{
    class DataBaseService
    {
        private const string DatabaseName = "DotnetStockApp.db3";
        private SQLiteAsyncConnection databaseConnection;
        async Task Init()
        {
            if(databaseConnection is  null)
            {
                //databaseConnection = new SQLiteAsyncConnection(DatabaseName, SQLiteOpenFlags.Create);
                databaseConnection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DatabaseName));
                await databaseConnection.CreateTableAsync<ScannedEntity>();
            }     
        }

        public async Task<List<ScannedEntity>> GetAllScannedEntities()
        {
            await Init();
            return await databaseConnection.Table<ScannedEntity>().ToListAsync();
        }

        public async Task AddScannedEntity(ScannedEntity scannedEntity)
        {
            await Init();
            await databaseConnection.InsertAsync(scannedEntity);
        }
    }

    [Table("ScannedEntity")]
    public class ScannedEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public long Id { get; set; }
        [Column("ExpiryDate")]
        public string ExpiryDate { get; set; }
        [Column("Tags")]
        public string Tags { get; set; }
        [Column("Image")]
        public byte[] Image { get; set; }
    }

    public class ScannedEntities
    {
        public long Id { get; set; }
        public string ExpiryDate {get;set;}
        public List<string> TagsList { get; set; }
        public ImageSource EntityImageSource { get; set; }    
    }

    public class ScannedEntitiesContextModel
    {
        private DataBaseService dataBaseService;

        public ScannedEntitiesContextModel()
        {
            dataBaseService = new DataBaseService();
        }

        public async Task<List<ScannedEntities>> getItemsFromDb()
        {
            List<ScannedEntities> entities = new List<ScannedEntities>();
            foreach(ScannedEntity entity in await dataBaseService.GetAllScannedEntities())
            {
                entities.Add(new ScannedEntities
                {
                    Id = entity.Id,
                    ExpiryDate = entity.ExpiryDate,
                    TagsList = JsonConvert.DeserializeObject<List<string>>(entity.Tags),
                    EntityImageSource = ImageSource.FromStream(() => new MemoryStream(entity.Image))
                });
            }
            return entities;
        }
    }
}
