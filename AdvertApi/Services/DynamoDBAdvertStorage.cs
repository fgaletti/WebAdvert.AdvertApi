using AdvertApi.Models.Fg;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    //17
    public class DynamoDBAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper _mapper;
        public DynamoDBAdvertStorage(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> Add(AdvertModel model)
        {
            // optimistic just for the example
            var dbMoled = _mapper.Map<AdvertDbModel>(model);
            dbMoled.Id = Guid.NewGuid().ToString(); //new Guid().ToString();
            dbMoled.CreationDateTime = DateTime.UtcNow;
            dbMoled.Status = AdvertStatus.Pending;


            using (var client = new  AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(dbMoled);
                }
            }

            return dbMoled.Id;
        }

        // 20
        public async Task<bool> CheckHealthAsync()
        {
           using (var client = new AmazonDynamoDBClient())
            {
                var tableData = await client.DescribeTableAsync("Adverts");
                return string.Compare(tableData.Table.TableStatus, "active", true) == 0;
            }
        }

        // 18
        public async Task Confirm(ConfirmAdvertModel model)
        {
            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var record = await context.LoadAsync<AdvertDbModel>(model.Id);
                    if (record == null)
                    {
                        throw new KeyNotFoundException($"A record with ID= {model.Id } was not found.");
                    }
                    if (model.Status == AdvertStatus.Active)
                    {
                        record.Status = AdvertStatus.Active;
                        await context.SaveAsync(record);
                    }
                    else
                    {
                        await context.DeleteAsync(record);
                    }
                }
            }
        }

        //public Task<AdvertModel> GetById(string id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<AdvertModel> GetById(string id)
        {
            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var dbModel = await context.LoadAsync<AdvertDbModel>(id);
                    if (dbModel != null) return _mapper.Map<AdvertModel>(dbModel);
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
