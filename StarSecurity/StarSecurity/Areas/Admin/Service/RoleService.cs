//using System.Net.Http;
//using Api_StarSecurity.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;

//namespace StarSecurity.Areas.Admin.Service
//{
//    public class RoleService
//    {
//        private readonly IConfiguration _config;
//        private readonly ILogger<RoleService> _logger;

//        private readonly string _roleUrl;

//        public RoleService(IConfiguration config, ILogger<RoleService> logger)
//        {
//            _config = config;
//            _logger = logger;

//            _roleUrl = _config.GetValue<string>("StarApi:Role");
//        }

//        public async Task<IActionResult> GetRolesAsync()
//        {
//            var endpoint = $"{_roleUrl}/List";
//            try
//            {

//                using (var client = new HttpClient())
//                {
//                    client.BaseAddress = new Uri(endpoint);
//                    HttpResponseMessage data = await client.GetAsync(endpoint);

//                    var result = data.IsSuccessStatusCode ? await data.Content.ReadAsStringAsync() : "[]";

//                    var model = JsonConvert.DeserializeObject<Role>(result);
//                }
//            }
//            catch (Exception ex)
//            {
//                //log error 
//                _logger.LogError(ex.Message);
//            }
//            return ;
//        }
//    }
//}
