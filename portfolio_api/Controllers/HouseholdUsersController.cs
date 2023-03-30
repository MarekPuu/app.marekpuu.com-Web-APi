using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Contracts;
using portfolio_api.Data;
using portfolio_api.Models.HouseholdUsers;

namespace portfolio_api.Controllers
{
    [Route("api/household")]
    [ApiController]
    [Authorize]
    public class HouseholdUsersController : ControllerBase
    {
        private readonly IHouseholdRepository _householdRepository;
        private readonly IHouseholdUserRepository _householdUserRepository;
        private readonly IAuthServerUserRepository _authServerUserRepository;

        public HouseholdUsersController(IHouseholdRepository householdRepository, IHouseholdUserRepository householdUserRepository, IAuthServerUserRepository authServerUserRepository)
        {
            this._householdRepository = householdRepository;
            this._householdUserRepository = householdUserRepository;
            this._authServerUserRepository = authServerUserRepository;
        }

        [HttpGet("{householdId}/users")]
        public async Task<ActionResult<List<GetHouseholdUsersDto>>> GetHouseholdUsers([FromRoute] Guid householdId)
        {

            string userid = getUserID();

            bool canRequestHousehold = await _householdRepository.CanRequestHousehold(householdId, userid);

            if (!canRequestHousehold) return BadRequest("Käyttäjällä ei ole oikeutta talouteen tai sitä ei ole");

            var householdUsers = await _householdUserRepository.GetHouseholdUsers(householdId);

            return Ok(householdUsers);
        }

        [HttpPost("{householdId}/users")]
        public async Task<ActionResult<List<GetHouseholdUsersDto>>> AddUserToHousehold([FromBody] AddUserToHouseholdDto data, [FromRoute] Guid householdId)
        {
            string userid = getUserID();

            bool canRequestHousehold = await _householdRepository.CanRequestHousehold(householdId, userid);

            if (!canRequestHousehold) return BadRequest("Käyttäjällä ei ole oikeutta lisätä jäseniä");

            var user = await _authServerUserRepository.GetUserByEmail(data.Email);

            if (user == null) return BadRequest("Käyttäjää ei löytynyt");

            var isInHousehold = await _householdUserRepository.IsInHousehold(householdId, user.AuthServerUserId);

            if (isInHousehold) return BadRequest("Käyttäjä on jo taloudessa");

            var newHouseholdUser = new HouseholdUser
            {
                HouseholdId = householdId,
                AuthServerUserId = user.AuthServerUserId,
                RoleId = 3
            };

            await _householdUserRepository.AddAsync(newHouseholdUser);

            return StatusCode(201);
        }


        [HttpDelete("{householdId}/users/{id}")]
        public async Task<ActionResult> DeleteUserFromHousehold([FromRoute] Guid householdId, string id)
        {
            string userId = getUserID();

            if (userId == id) return BadRequest("Et voi poistaa itseäsi");

            var user = await _householdUserRepository.GetHouseholdUser(householdId, id);

            if (user.RoleId == 1) return BadRequest("Omistajaa ei voi poistaa taloudesta");

            bool canPerformAdminTask = await _householdUserRepository.CanPerformAdminTask(householdId, userId);

            if (!canPerformAdminTask) return BadRequest("Ei oikeuksia poistaa jäsentä");

            var userRemoved = await _householdUserRepository.RemoveUserFromHousehold(householdId, id);

            if (!userRemoved) return BadRequest("Jotain meni pieleen");

            return Ok();
        }

        [HttpPatch("{householdId}/users")]
        public async Task<ActionResult> EditUserFromHousehold([FromBody] UpdateUserInHouseholdDto data, [FromRoute] Guid householdId)
        {
            string requestUserId = getUserID();

            if (data.userId == requestUserId) return BadRequest("Et voi muokata itseäsi");

            if (data.roleId == 1) return BadRequest("Käyttäjälle ei voi antaa Omistajan roolia");

            bool canPerformAdminTask = await _householdUserRepository.CanPerformAdminTask(householdId, requestUserId);

            if (!canPerformAdminTask) return BadRequest("Ei oikeuksia muokata käyttäjää");

            var user = await _householdUserRepository.GetHouseholdUser(householdId, data.userId);

            if (user == null) return BadRequest("Käyttäjää ei löytynyt");

            user.RoleId = data.roleId;

            await _householdUserRepository.UpdateAsync(user);

            return Ok();
        }

        private string getUserID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
