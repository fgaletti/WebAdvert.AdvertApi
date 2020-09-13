using AdvertApi.Models.Fg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    //17
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel model);
        Task Confirm(ConfirmAdvertModel model);

       // Task<AdvertModel> GetByIdAsync(string id);
        Task<AdvertModel> GetById(string id); // 30  not found 

        Task<bool> CheckHealthAsync();
    }
}
