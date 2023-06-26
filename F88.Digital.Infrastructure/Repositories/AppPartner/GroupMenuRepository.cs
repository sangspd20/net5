using AutoMapper;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using F88.Digital.Application.DTOs.Settings;
using Microsoft.Extensions.Options;
using F88.Digital.Application.Features.AppPartner.LocationShop.Queries;
using F88.Digital.Domain.Entities.Share;
using F88.Digital.Application.DTOs.AppPartner.GroupManagment;
using AspNetCoreHero.Results;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class GroupMenuRepository : IGroupMenuRepository
    {
        private readonly IRepositoryAsync<Group> _repository;
        private readonly AppPartnerDbContext _context;
        private readonly AffiliateDbContext _contextAffiliate;
        public GroupMenuRepository(IRepositoryAsync<Group> repository,
            AppPartnerDbContext context, AffiliateDbContext contextAffiliate)
        {
            _repository = repository;
            _context = context;
            _contextAffiliate = contextAffiliate;
        }

        public GetGroupMenuResponse GetListGroupMenuByUserId(int userId)
        {
            var userGroups = _context.UserGroups.Where(x => x.UserProfileId == userId).AsQueryable();
            var rs = new GetGroupMenuResponse();
            rs.Menus = new List<Menu>();
            rs.Groups = new List<Group>();
            foreach(var userGroup in userGroups)
            {
                var group = _context.Groups.FirstOrDefault(x => x.Id == userGroup.GroupId);
                var groupMenus = _context.GroupMenus.Where(x => x.GroupId == userGroup.GroupId);
                foreach(var groupMenu in groupMenus)
                {
                    var menu = _context.Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
                    rs.Menus.Add(menu);
                }    
                rs.Groups.Add(group);
            }
            return rs;
        }

        public async Task<Group> AddGroup(Group group)
        {
            group.CreatedOn = DateTime.Now;
            var item = await _context.Groups.AddAsync(group);
            _context.SaveChanges();
            return item.Entity;
        }
        public async Task<bool> UpdateGroup(UpdateGroupRequest updateGroupRequest)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == updateGroupRequest.groupId);
            if (group != null)
            {
                group.LastModifiedOn = DateTime.Now;
                group.Name = updateGroupRequest.Name;
                var item = _context.Groups.Update(group);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<Menu> AddMenu(Menu menu)
        {
            menu.CreatedOn = DateTime.Now;
            var item = await _context.Menus.AddAsync(menu);
            _context.SaveChanges();
            return item.Entity;
        }

        public async Task<GroupMenu> AddGroupMenu(GroupMenu groupMenu)
        {
            groupMenu.CreatedOn = DateTime.Now;
            var item = await _context.GroupMenus.AddAsync(groupMenu);
            _context.SaveChanges();
            return item.Entity;
        }

        public async Task<UserGroup> AddUserGroup(UserGroup userGroup)
        {
            userGroup.CreatedOn = DateTime.Now;
            var item = await _context.UserGroups.AddAsync(userGroup);
            _context.SaveChanges();
            return item.Entity;
        }
        public void DeteleUserGroupByGroup(int groupId)
        {
            var userGroups = _context.UserGroups.Where(x => x.GroupId == groupId);
            if(userGroups.Any())
            {
                _context.UserGroups.RemoveRange(userGroups);
                _context.SaveChanges();
            }    
        }
        public void DeteleGroupMenuByGroup(int groupId)
        {
            var groupMenus = _context.GroupMenus.Where(x => x.GroupId == groupId);
            if (groupMenus.Any())
            {
                _context.GroupMenus.RemoveRange(groupMenus);
                _context.SaveChanges();
            }
        }
        public bool DeteleGroup(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);
            if (group != null)
            {
                var userGroups = _context.UserGroups.Where(x => x.GroupId == groupId);
                if (userGroups.Any())
                {
                    _context.UserGroups.RemoveRange(userGroups);
                }
                _context.Groups.Remove(group);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public void DeteleUserGroupByUser(int userId)
        {
            var userGroups = _context.UserGroups.Where(x => x.UserProfileId == userId);
            if (userGroups.Any())
            {
                _context.UserGroups.RemoveRange(userGroups);
                _context.SaveChanges();
            }
        }
        public Group GetGroupByName(string name)
        {
            return _context.Groups.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
        }

        public Menu GetMenuByName(string name)
        {
            return _context.Menus.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
        }

        public GroupMenu GetGroupMenuByMenuGroupId(int menuId, int groupId)
        {
            return _context.GroupMenus.FirstOrDefault(x => x.MenuId == menuId && x.GroupId == groupId);
        }

        public UserGroup GetUserGroupByUserId(int userProfileId)
        {
            return _context.UserGroups.FirstOrDefault(x => x.UserProfileId == userProfileId);
        }

        public async Task<Result<List<GetAllGroupMenuResponse>>> GetAllGroupMenu()
        {
            var result = _context.Groups.ToList().Select(x => new GetAllGroupMenuResponse
            {
                Group = x,
                Menu =_context.GroupMenus.Where(a => a.GroupId == x.Id).Select(s => _context.Menus.FirstOrDefault(w => w.Id == s.MenuId)).ToList()
            }).ToList();
            return await Result<List<GetAllGroupMenuResponse>>.SuccessAsync(result);
        }

        public async Task<Result<List<GetAllUserGroupResponse>>> GetAllUserGroup()
        {
            var result = _context.UserProfiles.ToList().Select(x => new GetAllUserGroupResponse
            {
                UserId = x.Id,
                UserPhone = x.UserPhone,
                FullName = x.FirstName + x.LastName,
                Group = GetUserGroupByUserId(x.Id) != null ? _context.Groups.FirstOrDefault(w => w.Id == GetUserGroupByUserId(x.Id).GroupId) : new Group()
            }).ToList();
            return await Result<List<GetAllUserGroupResponse>>.SuccessAsync(result);
        }

        public async Task<Result<List<Menu>>> GetAllMenu()
        {
            var result = _context.Menus.ToList();
            return await Result<List<Menu>>.SuccessAsync(result);
        }

        public async Task<Result<List<GetAllUserGroupResponse>>> GetUserGroup(int groupId)
        {
            var result = _context.UserProfiles.ToList().Select(x => new GetAllUserGroupResponse
            {
                UserId = x.Id,
                UserPhone = x.UserPhone,
                FullName = x.FirstName + x.LastName,
                Group = GetUserGroupByUserId(x.Id) != null ? _context.Groups.FirstOrDefault(w => w.Id == GetUserGroupByUserId(x.Id).GroupId) : new Group()
            }).Where(x => x.Group != null && x.Group.Id == groupId).ToList();
            return await Result<List<GetAllUserGroupResponse>>.SuccessAsync(result);
        }

        public bool DeleteUserGroup(DeleteUserGroupRequest deleteUserGroupRequest)
        {
            var userGroup = _context.UserGroups.FirstOrDefault(x => x.GroupId == deleteUserGroupRequest.GroupId && x.UserProfileId == deleteUserGroupRequest.UserProfileId);
            if(userGroup != null)
            {
                _context.UserGroups.Remove(userGroup);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<Group> Groups => _repository.Entities;
    }
}
