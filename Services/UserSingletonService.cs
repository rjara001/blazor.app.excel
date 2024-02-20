using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;

namespace BlazorAppExcel.Services
{
    public class UserSingletonService : IUserSingletonService
    {
        public UserSingletonService() { 
            this.User = new User();
        }

        public User User { get; set; }
    }
}