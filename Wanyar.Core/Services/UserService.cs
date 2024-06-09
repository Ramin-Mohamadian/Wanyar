using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.Convertors;
using Wanyar.Core.DTOs;
using Wanyar.Core.Generator;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Context;
using Wanyar.DataLayer.Entities.Users;
using Wanyar.DataLayer.Entities.Wallet;

namespace Wanyar.Core.Services
{
    public class UserService : IUserService
    {
        private WanyarContext _context;
        public UserService(WanyarContext context)
        {
            _context = context;
        }

        #region User
        public bool ActiveAccount(string activecode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
            if (user == null||user.IsActive)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode=NameGenerator.GenerateUniqeCode();
            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User GetUserByActiveCode(string code)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == code);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(user => user.Email == email);
        }


        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUser(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User LoginUser(LoginViewModel login)
        {
            var email = FixedText.FixedEmail(login.Email);
            var hashpassword = PasswordHelper.EncodePasswordMd5(login.Password);
            return _context.Users.FirstOrDefault(u => u.Email==email&&u.Password==hashpassword);
        }
        public User GetUserByName(string name)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == name);
        }
        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }



        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        #endregion

        #region UserPanel
        public UserInformationViewModel GetUserInformation(string username)
        {
            User user = GetUserByName(username);
            UserInformationViewModel information = new UserInformationViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Wallet=TotalWallet(username)
            };
            return information;
        }



        public SideBarViewModel GetInfoSidebar(string name)
        {
            return _context.Users.Where(u => u.UserName==name).Select(u => new SideBarViewModel()
            {
                UserName = u.UserName,
                ImageName=u.UserAvatar,
                RegisterDate=u.RegisterDate,
            }).Single();
        }

        public EditeUserProfileViewModel GetDataUserProfile(string username)
        {
            return _context.Users.Where(u => u.UserName==username).Select(u => new EditeUserProfileViewModel()
            {
                Email=u.Email,
                UserName=u.UserName,
                AvatarName=u.UserAvatar,
            }).Single();
        }

        public void EditeUserProfile(string username, EditeUserProfileViewModel profile)
        {

            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "Defult.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                profile.AvatarName = NameGenerator.GenerateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }

            }
            var user = GetUserByName(username);
            user.UserName=profile.UserName;
            user.Email=profile.Email;
            user.UserAvatar=profile.AvatarName;


            UpdateUser(user);

        }

        public bool CompareOldPassword(string username, string oldpassword)
        {
            string hasholdpassword = PasswordHelper.EncodePasswordMd5(oldpassword);
            return _context.Users.Any(u => u.UserName==username && u.Password==hasholdpassword);
        }

        public void ChangePassword(string username, string newpassword)
        {
            User user = GetUserByName(username);
            user.Password=PasswordHelper.EncodePasswordMd5(newpassword);
            UpdateUser(user);
        }






        #endregion

        #region Wallet
        public int TotalWallet(string username)
        {
            int user = GetUserIdByUserName(username);

            var deposit = _context.Wallets.Where(w => w.UserId==user&&w.TypeId==1&&w.IsPay).Select(w => w.Amount).ToList();

            var withdrow = _context.Wallets.Where(w => w.UserId==user&&w.TypeId==2).Select(w => w.Amount).ToList();

            var Balance = (deposit.Sum()-withdrow.Sum());

            return Balance;
        }

        public List<WalletViewModel> ShowWallet(string username)
        {
            var userid = GetUserIdByUserName(username);
            return _context.Wallets.Where(w => w.UserId==userid&&w.IsPay).Select(w => new WalletViewModel()
            {
                Amount = w.Amount,
                DateTime=w.CreateDate,
                Description = w.Description,
                TypeId=w.TypeId,
            }).ToList();
        }

        public int ChargeWallet(string userName, int amount, string description, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName)
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }


        #endregion

        #region AdminUser
        public UserForAdminViewModel GetUserForAdmin(int pageId, string FilterUserName, string FilterEmail)
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(FilterUserName))
            {
                result=result.Where(u => u.UserName.Contains(FilterUserName));
            }

            if (!string.IsNullOrEmpty(FilterEmail))
            {
                result=result.Where(u => u.Email.Contains(FilterEmail));

            }

            //Show Item In Page
            int take = 30;
            int skipe = (pageId-1)*take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage=pageId;
            list.PageCount=result.Count()/take;
            list.GetUser=result.OrderBy(u => u.RegisterDate).Skip(skipe).Take(take).ToList();

            return list;
        }

        public int AddUserAdmin(CreateUserViewModel user)
        {
            User Adduser = new User();
            Adduser.UserName=user.UserName;
            Adduser.Email=user.Email;
            Adduser.Password=PasswordHelper.EncodePasswordMd5(user.Password);
            Adduser.ActiveCode=NameGenerator.GenerateUniqeCode();
            Adduser.RegisterDate=DateTime.Now;
            Adduser.IsActive=true;

            #region Save Image
            if (user.UserAvatar!=null)
            {
                string imagePath = "";
                Adduser.UserAvatar = NameGenerator.GenerateUniqeCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", Adduser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }

            }
            #endregion

            return AddUser(Adduser);

        }

        public EditeUserViewModel GetUserForShowInEditeMode(int userId)
        {
           
            return _context.Users.Where(u => u.UserId == userId).Select(u => new EditeUserViewModel()
            {
                UserId = u.UserId,
                AvatarName=u.UserAvatar,
                Email=u.Email,             
                UserName=u.UserName,
                UserRoles=u.UserRoles.Select(r=>r.RoleId).ToList()
            }).Single();

        }

        public void UpdateUserAdmin(EditeUserViewModel editeUserViewModel)
        {
            User user = GetUserById(editeUserViewModel.UserId);
            user.Email = editeUserViewModel.Email;
            if(!string.IsNullOrEmpty(editeUserViewModel.Password))
            {
                user.Password=PasswordHelper.EncodePasswordMd5(editeUserViewModel.Password);
            }

            if(editeUserViewModel.UserAvatar!=null)
            {
                string imagePath = "";
                if (editeUserViewModel.AvatarName != "Defult.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editeUserViewModel.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                user.UserAvatar = NameGenerator.GenerateUniqeCode() + Path.GetExtension(editeUserViewModel.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editeUserViewModel.UserAvatar.CopyTo(stream);
                }
            }
            _context.Users.Update(user);
            _context.SaveChanges();


        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public UserInformationViewModel GetUserInformationById(int uaerId)
        {
            var user=_context.Users.Find(uaerId);          
            UserInformationViewModel information = new UserInformationViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Wallet=TotalWallet(user.UserName)
            };
            return information;

        }

        public void DeleteUser(int userId)
        {
            User user=GetUserById(userId);

            user.IsDelete=true;
            UpdateUser(user);
        }

        public UserForAdminViewModel GetDeleteUserForAdmin(int pageId = 1, string FilterUserName = "", string FilterEmail = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);

            if (!string.IsNullOrEmpty(FilterUserName))
            {
                result=result.Where(u => u.UserName.Contains(FilterUserName));
            }

            if (!string.IsNullOrEmpty(FilterEmail))
            {
                result=result.Where(u => u.Email.Contains(FilterEmail));

            }

            //Show Item In Page
            int take = 30;
            int skipe = (pageId-1)*take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage=pageId;
            list.PageCount=result.Count()/take;
            list.GetUser=result.OrderBy(u => u.RegisterDate).Skip(skipe).Take(take).ToList();

            return list;
        }

        public string GetUserNameByUserId(int id)
        {
            return _context.Users.Find(id).UserName;
        }
    }
    #endregion
}

