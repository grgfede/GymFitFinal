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
        Task<bool> UpdateGym(string nome, string citta, string indirizzo, string telefono, string IM, string FM, string IP, string FP);
        Task UpdateAbboanmento(string uidAbbonamento);
        Task<string> DoSignUp(string email, string password, string nome, string cognome);
        Task<string> DoSignUpGym(string email, string password);
        Task DeleteSub(string uidS);
        Task AddSub(string tipoAbbonamento, double costo, string dataI, string dataF, string uidAbbonamento, string uidUtente);
        Task AddGym(string nome, string citta, string indirizzo, string telefono, string IM, string FM, string IP, string FP, string uid);
        Task addSubGym(string uid, string tipoAbbonamento, string descrizione, double costo);
        Task AddPrenotazione(TurnoPrenotato turno);
        Task<List<AbbonamentoPalestra>> getSubGym();
        Task<Gym> GetGym(string uid);
        Task<List<Gym>> GetAllGym();
        Task<List<Gym>> GetGymByName(string name);
        Task<List<Gym>> GetGymByCity(string city);
        Task<User> GetPerson(string uid);
        Task<TurnoPrenotato> GetTurnoPrenotato(string uidUtente);
        Task DeleteTurnoPrenotato(string uidS);
        Task<List<TurnoPrenotato>> GetAllTurnoPrenotato();
        Task<Abbomamento> GetSub(string uidSub);
        Task<AbbonamentoPalestra> GetSubGymSpecific(string uidSub);
        Task<string> getProfilePic();
        Task<string> getProfilePicGymIscrizione(string uid);
        Task<List<User>> GetAllPerson();
        Task<Turno> GetTurni(string uidPalestra);
        Task AddTurno (string uidTurno, string palestraIscrizione, string Capienza, bool lunMat, bool lunPom, bool marMat, bool marPom, bool merMat, bool merPom, bool gioMat, bool gioPom, bool venMat, bool venPom, bool sabMat, bool sabPom, bool domMat, bool domPom);
        Task UpdateTurno(Turno t);
        Task followGym(string uidU, string uidG);
        Task unfollowGym(string uidU);
        Task DeletePerson();
        Task refreshUser();
        bool IsUserLoggedIn();

        void Logout();
    }
}

