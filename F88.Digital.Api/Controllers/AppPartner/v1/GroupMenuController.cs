using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.GroupManagment;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Application.DTOs.Identity;
using F88.Digital.Application.Interfaces;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Application.Logging;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [Route("api/app-partner/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupMenuController : ControllerBase
    {
        private readonly IGroupMenuRepository _groupMenuRepository;
        private readonly IMapper _mapper;

        public GroupMenuController(IGroupMenuRepository groupMenuRepository, IMapper mapper)
        {
            _groupMenuRepository = groupMenuRepository;
            _mapper = mapper;
        }

        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup(AddGroupRequest addGroupRequest)
        {

            if (_groupMenuRepository.GetGroupByName(addGroupRequest.Name) == null)
            {
                var entity = _mapper.Map<Group>(addGroupRequest);
                var data = _groupMenuRepository.AddGroup(entity);
                return Ok(await Result<Group>.SuccessAsync(data.Result, "Thêm thành công"));
            }
            return Ok(await Result<int>.FailAsync("Thêm thất bại, Group đã tồn tại."));
        }
        [HttpPost("UpdateGroup")]
        public async Task<IActionResult> UpdateGroup(UpdateGroupRequest updateGroupRequest)
        {

            var data = _groupMenuRepository.UpdateGroup(updateGroupRequest);
            if (data.Result == true)
            {
                return Ok(await Result<int>.SuccessAsync("Cập nhật thành công"));
            }
            return Ok(await Result<int>.FailAsync("Cập nhật thất bại, Group không tồn tại."));
        }
        [HttpPost("DeleteGroup")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {

            var data = _groupMenuRepository.DeteleGroup(groupId);
            if (data == true)
            {
                return Ok(await Result<int>.SuccessAsync("Xóa thành công"));
            }
            return Ok(await Result<int>.FailAsync("Xóa thất bại, Group không tồn tại."));
        }
        [HttpPost("DeleteUserGroup")]
        public async Task<IActionResult> DeleteUserGroup(DeleteUserGroupRequest deleteUserGroupRequest)
        {

            var data = _groupMenuRepository.DeleteUserGroup(deleteUserGroupRequest);
            if (data == true)
            {
                return Ok(await Result<int>.SuccessAsync("Xóa thành công"));
            }
            return Ok(await Result<int>.FailAsync("Xóa thất bại, Group không tồn tại."));
        }
        [HttpPost("AddMenu")]
        public async Task<IActionResult> AddMenu(AddMenuRequest addMenuRequest)
        {
            if (_groupMenuRepository.GetMenuByName(addMenuRequest.Name) == null)
            {
                var entity = _mapper.Map<Menu>(addMenuRequest);
                var data = _groupMenuRepository.AddMenu(entity);
                return Ok(await Result<int>.SuccessAsync("Thêm thành công"));
            }
            return Ok(await Result<int>.FailAsync("Thêm thất bại, Menu đã tồn tại."));
        }
        [HttpPost("AddOrUpdateGroupMenu")]
        public async Task<IActionResult> AddOrUpdateGroupMenu(AddOrUpdateGroupMenuRequest addOrUpdateGroupMenuRequest)
        {
            _groupMenuRepository.DeteleGroupMenuByGroup(addOrUpdateGroupMenuRequest.GroupId);
            foreach (var menuId in addOrUpdateGroupMenuRequest.ListMenuId)
            {
                var entity = _mapper.Map<GroupMenu>(new GroupMenu
                {
                    GroupId = addOrUpdateGroupMenuRequest.GroupId,
                    MenuId = menuId
                });
                await _groupMenuRepository.AddGroupMenu(entity);
            }
            return Ok(await Result<int>.SuccessAsync("Thêm thành công"));
        }        
        [HttpPost("AddOrUpdateUserGroup")]
        public async Task<IActionResult> AddOrUpdateUserGroup(AddOrUpdateUserGroupRequest addOrUpdateUserGroupRequest)
        {
            _groupMenuRepository.DeteleUserGroupByGroup(addOrUpdateUserGroupRequest.GroupId);
            foreach (var userProfileId in addOrUpdateUserGroupRequest.ListUserProfileId)
            {
                _groupMenuRepository.DeteleUserGroupByUser(userProfileId);
                var entity = _mapper.Map<UserGroup>(new UserGroup
                {
                    UserProfileId = userProfileId,
                    GroupId = addOrUpdateUserGroupRequest.GroupId
                });
                await _groupMenuRepository.AddUserGroup(entity);
            }
            return Ok(await Result<int>.SuccessAsync("Thêm thành công"));
        }
        [HttpGet("GetAllGroupMenu")]
        public async Task<IActionResult> GetAllGroupMenu()
        {
            return Ok(await _groupMenuRepository.GetAllGroupMenu());
        }
        [HttpGet("GetAllUserGroup")]
        public async Task<IActionResult> GetAllUserGroup()
        {
            return Ok(await _groupMenuRepository.GetAllUserGroup());
        }
        [HttpGet("GetUserGroup")]
        public async Task<IActionResult> GetUserGroup(int groupId)
        {
            return Ok(await _groupMenuRepository.GetUserGroup(groupId));
        }
        [HttpGet("GetAllMenu")]
        public async Task<IActionResult> GetAllMenu()
        {
            return Ok(await _groupMenuRepository.GetAllMenu());
        }
        [HttpPost("AddUserGroup")]
        public async Task<IActionResult> AddUserGroup(AddUserGroupRequest addUserGroupRequest)
        {
            _groupMenuRepository.DeteleUserGroupByUser(addUserGroupRequest.UserProfileId);
            var entity = _mapper.Map<UserGroup>(new UserGroup
            {
                UserProfileId = addUserGroupRequest.UserProfileId,
                GroupId = addUserGroupRequest.GroupId
            });
            await _groupMenuRepository.AddUserGroup(entity);
            return Ok(await Result<int>.SuccessAsync("Thêm thành công"));
        }
    }
}
