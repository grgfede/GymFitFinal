using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymFitFinal.Interfaces
{
    public interface IFirebaseAuthenticator
    {
        Task<string> DoLoginWithEP(string email, string password);
        Task<string> DoLoginWithEPafterSignUpGym(string email, string password);
        Task<string> ResetPassword(string email);

        Task<string> UpdateEmail(string email);
        //Task<bool> UpdatePassword(string password);

        Task<bool> UpdatePerson(string nome, string cognome);
        Task<bool> UpdateGym(string nome, string citta);
        Task UpdateAbboanmento(string uidAbbonamento);
        Task<string> DoSignUp(string email, string password, string nome, string cognome);
        Task<string> DoSignUpGym(string email, string password);

        Task AddSub(string tipoAbbonamento, double costo, DateTime dataI, DateTime dataF, string uidAbbonamento, string uidUtente);
        Task AddGym(string nome, string citta, string indirizzo, string telefono, string IM, string FM, string IP, string FP, string uid);

        Task<Gym> GetGym(string uid);
        Task<List<Gym>> GetAllGym();
        Task<List<Gym>> GetGymByName(string name);
        Task<List<Gym>> GetGymByCity(string city);
        Task<User> GetPerson(string uid);
        Task<Abbomamento> GetSub(string uidSub);
        Task<string> getProfilePic();
        Task<string> getProfilePicGymIscrizione(string uid);
        Task<List<User>> GetAllPerson();
        Task DeletePerson();
        Task refreshUser();
        bool IsUserLoggedIn();

        void Logout();
    }
}

