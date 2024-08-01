using Patika_Hafta1_Odev.Models;

namespace Patika_Hafta1_Odev.Services
{
    public class FakeUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User{Username="admin",Password="password"}
        };
        //Login için yazılmış fake Authenticate metodu
        public bool Authenticate(string username, string password)
        {
            return _users.Any(u=>u.Username==username && u.Password==password);
        }
    }
}
