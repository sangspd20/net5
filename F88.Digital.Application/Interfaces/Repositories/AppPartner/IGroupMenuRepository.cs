using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Domain.Entities.Share;
using F88.Digital.Application.DTOs.AppPartner.GroupManagment;
using AspNetCoreHero.Results;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IGroupMenuRepository
    {
        GetGroupMenuResponse GetListGroupMenuByUserId(int userId);
        IQueryable<Group> Groups { get; }
        Task<Group> AddGroup(Group group);
        Task<Menu> AddMenu(Menu menu);
        Task<GroupMenu> AddGroupMenu(GroupMenu groupMenu);
        Task<UserGroup> AddUserGroup(UserGroup userGroup);
        Group GetGroupByName(string name);
        Menu GetMenuByName(string name);
        GroupMenu GetGroupMenuByMenuGroupId(int menuId, int groupId);
        UserGroup GetUserGroupByUserId(int userProfileId);
        Task<Result<List<GetAllGroupMenuResponse>>> GetAllGroupMenu();
        Task<Result<List<GetAllUserGroupResponse>>> GetAllUserGroup();
        Task<Result<List<Menu>>> GetAllMenu();
        void DeteleUserGroupByGroup(int groupId);
        void DeteleGroupMenuByGroup(int groupId);
        void DeteleUserGroupByUser(int userId);
        Task<bool> UpdateGroup(UpdateGroupRequest updateGroupRequest);
        Task<Result<List<GetAllUserGroupResponse>>> GetUserGroup(int groupId);
        bool DeleteUserGroup(DeleteUserGroupRequest deleteUserGroupRequest);
        bool DeteleGroup(int groupId);
    }
}
