using FitnessApp.DAL.context;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.DAL.Repositories
{
    public class UserSettingRepository : IUserSettingeRepository
    {
        public UserSettingRepository(FitnessAppContext ctx)
        {
            context = ctx;
        }
        FitnessAppContext context;
        public TbUserSetting GetId(string userId)
        {
            try
            {
                return context.TbUserSettings.Where(i => i.UserId.Equals(userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("No Item with GetId in UserSetting ", ex.Message);
                throw;
            }
        }

        public bool Save(TbUserSetting tbUserSetting)
        {
            try
            {

                if (tbUserSetting.SettingId == 0)
                {
                    if (tbUserSetting.PreferredUnits == "lbs")
                    {
                        var user = context.Users.FirstOrDefault(i => i.Id == tbUserSetting.UserId);
                        if (user == null)
                            return false;
                        user.Weight = (int)(user.Weight * 0.45359237);
                        context.Users.Update(user);
                    }
                    context.TbUserSettings.Add(tbUserSetting);
                    context.SaveChanges();
                }
                else
                {

                    //context.Entry(tbUserSetting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.Entry(tbUserSetting).State = EntityState.Modified;
                    context.SaveChanges();

                }

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("No Item with Save in UserSetting ", ex.Message);
                throw;
            }
        }
    }
}
