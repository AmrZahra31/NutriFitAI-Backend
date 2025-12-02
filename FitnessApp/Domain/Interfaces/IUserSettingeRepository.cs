using FitnessApp.Domain.Entities;

namespace FitnessApp.Domain.Interfaces
{
    public interface IUserSettingeRepository
    {
        TbUserSetting GetId(string userId);
        public bool Save(TbUserSetting tbUserSetting);
    }
}
