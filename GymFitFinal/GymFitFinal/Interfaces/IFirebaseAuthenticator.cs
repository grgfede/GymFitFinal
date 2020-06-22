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

        Task<string> UpdateEmail(string email);
        //Task<bool> UpdatePassword(string password);

        Task<bool> UpdatePerson(string nome, string cognome);
        Task<string> DoSignUp(string email, string password, string nome, string cognome);
        Task<string> DoSignUpGym(string email, string password, string nome, string citta);

        Task<User> GetPerson(string uid);
        Task<List<User>> GetAllPerson();
        Task DeletePerson();
        Task refreshUser();
        bool IsUserLoggedIn();

        void Logout();
    }
}

