using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;
using BlazorAppExcel.Pages.Work;
using Blazored.LocalStorage;

namespace BlazorAppExcel.Services
{
    public class SessionSingletonService : ISessionSingletonService
    {
   
        public SessionSingletonService() { 
            this.User = new User();
        }

        public User User { get; private set; }

        public async Task SetUser(User user)
        {
            this.User = user;
        }

        public void SetTableActive(TableExcel table)
        {
            this.User.TableActive = table;
        }
    }
}