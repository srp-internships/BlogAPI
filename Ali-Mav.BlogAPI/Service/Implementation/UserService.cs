using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Reflection;

namespace Ali_Mav.BlogAPI.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse<List<User>>> CreateAll()
        {
            var serviceResponse = new BaseResponse<List<User>>();

            try
            {
                var url = "https://jsonplaceholder.typicode.com/users";
                var client = new HttpClient();

                var response = await client.GetAsync(url);
                var db = await _userRepository.GetAll().AnyAsync();

                if (response.IsSuccessStatusCode && db == false)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    List<UserViewModel> userViewModels = JsonConvert.DeserializeObject<List<UserViewModel>>(json);

                    foreach (UserViewModel userViewModel in userViewModels)
                    {
                        var Users = _mapper.Map<User>(userViewModel);
                        await _userRepository.Create(Users);


                    }

                    serviceResponse.Data = _userRepository.GetAll().ToList();
                    serviceResponse.success = true;
                }

                serviceResponse.Description = "there is data in the database";
                serviceResponse.success = false;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.Description = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<BaseResponse<User>> GetById(int userId)
        {
            var serviceResponse = new BaseResponse<User>();

            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(c => c.Id == userId);
                if (user == null)
                {
                    serviceResponse.success = true;
                    serviceResponse.Description = "User not found";
                }
                else
                {
                    serviceResponse.success = true;
                    serviceResponse.Data = user;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Description = ex.Message;
                serviceResponse.success = false;
            }
            return serviceResponse;
        }

        public async Task<BaseResponse<User>> Search(string word)
        {
            var serviceResponse = new BaseResponse<User>();
            try
            {

                var user = await GetUser(word);
                if (user == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.Description = "User not found";
                }
                serviceResponse.success = true;
                serviceResponse.Data = user;
            }
            catch (Exception ex)
            {
                serviceResponse.Description = ex.Message;
                serviceResponse.success = false;
            }
            return serviceResponse;
        }

        private async Task<User> GetUser(string word)
        {
            var users = await _userRepository.GetAll().ToListAsync();
            foreach (var user in users)
            {
                string wordUp = word.ToUpper();

                string last = user.LastName.ToUpper();
                string full = user.FullName.ToUpper();
                string firt = user.FirstName.ToUpper();

                bool lastname = last.Contains(wordUp);
                bool fullName = full.Contains(wordUp);
                bool firtName = firt.Contains(wordUp);

                if (lastname || fullName || firtName)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
