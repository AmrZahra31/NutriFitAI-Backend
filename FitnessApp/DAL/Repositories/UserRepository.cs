using FitnessApp.DAL.context;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.DAL.Repositories
{

    public class UserRepository : IUserRepository
    {
        public UserRepository(FitnessAppContext ctx)
        {
            _context = ctx;
        }
        FitnessAppContext _context;

        public bool UpdateCurentState(string userId,bool _CurrentState)
        {
            
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                    return false;
                user.CurrentState = _CurrentState;
                _context.Update(user);
                _context.SaveChanges();
                return true;
            
            
        }

        public bool updateUser(ApplicationUsers user)
        {
            if (user == null)
                return false;

            _context.Update(user);
            _context.SaveChanges();
            return true;



        }

        public ApplicationUsers GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);

        }
    }
}
