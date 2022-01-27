using System.Threading.Tasks;
using EasyCooking.Auth.Models;

namespace EasyCooking.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}