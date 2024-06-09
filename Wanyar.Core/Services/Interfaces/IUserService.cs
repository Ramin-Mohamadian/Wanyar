using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.DTOs;
using Wanyar.DataLayer.Entities.Users;
using Wanyar.DataLayer.Entities.Wallet;

namespace Wanyar.Core.Services.Interfaces
{
    public interface IUserService
    {
        #region User
        bool IsExistUser(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string code);
        void UpdateUser(User user);
        bool ActiveAccount(string activecode);
        User GetUserByName(string name);
        UserInformationViewModel GetUserInformation(string username);
        SideBarViewModel GetInfoSidebar(string name);

        EditeUserProfileViewModel GetDataUserProfile(string username);
        void EditeUserProfile(string username, EditeUserProfileViewModel profile);

        bool CompareOldPassword(string username, string oldpassword);

        void ChangePassword(string username, string newpassword);
        int GetUserIdByUserName(string userName);
        string GetUserNameByUserId(int id);

        #endregion

        #region wallet
        int TotalWallet(string username);
        List<WalletViewModel>ShowWallet(string username);
        int ChargeWallet(string userName, int amount, string description, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region UserAdmin
        UserForAdminViewModel GetUserForAdmin(int pageId=1,string FilterUserName="",string FilterEmail = "");
         UserForAdminViewModel GetDeleteUserForAdmin(int pageId=1,string FilterUserName="",string FilterEmail = "");

        int AddUserAdmin(CreateUserViewModel user);

        EditeUserViewModel GetUserForShowInEditeMode(int userId);

        void UpdateUserAdmin(EditeUserViewModel editeUserViewModel);
        User GetUserById(int userId);
        UserInformationViewModel GetUserInformationById(int uaerId);


        void DeleteUser(int userId);
        #endregion
    }
}
