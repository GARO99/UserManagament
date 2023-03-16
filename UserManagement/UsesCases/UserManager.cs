using UserManagement.Models;
using UserManagement.Models.DataBaseContext;
using UserManagement.Models.ViewModels;
using UserManagement.UsesCases.Utils;

namespace UserManagement.UsesCases
{
    public class UserManager
    {
        private readonly SqlServerContext DbContext;

        public UserManager(SqlServerContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IEnumerable<UserViewModel> GetAll(bool isActive = true)
        {
            return this.DbContext.Users.Where(u => u.IsActive == isActive)
                .Select(u => Mappers.Map<UserViewModel, User>(u)).ToList();
        }

        public UserViewModel GetById(int id)
        {
            UserViewModel? userViewModel = this.DbContext.Users.Where(u => u.UserId == id).Select(u => Mappers.Map<UserViewModel, User>(u)).FirstOrDefault();

            return userViewModel ?? throw new Exception("El usuario que busca no existe");
        }

        public IEnumerable<UserViewModel> GetByFullName(UserSearchViewModel userSearch)
        {
            if (string.IsNullOrEmpty(userSearch.FullName))
            {
                return GetAll(userSearch.IsActive);
            }

            return GetAll(userSearch.IsActive).Where(u => u.FirstName.ToLower().Contains(userSearch.FullName.ToLower()) ||
                u.LastName.ToLower().Contains(userSearch.FullName.ToLower())).ToList();
        }

        public void Create(UserViewModel model)
        {
            User user = Mappers.Map<UserViewModel, User>(model);
            user.CreateDate = DateTime.Now;
            user.IsActive = true;
            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();
        }

        public void Update(UserViewModel model)
        {
            User user = Mappers.Map<UserViewModel, User>(model);
            user.UpdateDate = DateTime.Now;
            this.DbContext.Users.Update(user);
            this.DbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            User? user = this.DbContext.Users.FirstOrDefault(u => u.UserId == id) ?? throw new Exception("El usuario que intenta borrar de forma logica no existe");
            this.DbContext.Users.Remove(user);
            this.DbContext.SaveChanges();
        }

        public void LogicDelete(int id)
        {
            User? user = this.DbContext.Users.FirstOrDefault(u => u.UserId == id) ?? throw new Exception("El usuario que intenta borrar de forma logica no existe");
            user.UpdateDate = DateTime.Now;
            user.IsActive = false;
            this.DbContext.Users.Update(user);
            this.DbContext.SaveChanges();
        }

    }
}
