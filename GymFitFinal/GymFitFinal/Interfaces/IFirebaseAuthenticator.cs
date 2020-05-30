using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymFitFinal.Interfaces
{
    public interface IFirebaseAuthenticator
    {
        Task<string> DoLoginWithEP(string email, string password);
        Task<string> ResetPassword(string email);

        Task<string> DoSignUp(string email, string password, string nome, string cognome);

        Task<User> GetPerson(string uid);
        Task<List<User>> GetAllPerson();
        Task DeletePerson();
        bool IsUserLoggedIn();

        void Logout();
    }
}

