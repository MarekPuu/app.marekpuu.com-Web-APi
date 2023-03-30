using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Data;
using System.Security.Claims;
using portfolio_api.Models.HouseHold;
using portfolio_api.Contracts;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace portfolio_api.Controllers
{
    [Route("api/household")]
    [ApiController]
    [Authorize]
    public class HouseholdsController : ControllerBase
    {
        private readonly IHouseholdRepository _householdRepository;
        private readonly IHouseholdUserRepository _householdUserRepository;


        public HouseholdsController(IHouseholdRepository householdRepository, IHouseholdUserRepository householdUserRepository)
        {
            this._householdRepository = householdRepository;
            this._householdUserRepository = householdUserRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<Household>>> GetMyHouseholds()
        {
            string userid = getUserID();

            var households = await _householdUserRepository.GetHouseholdsByUser(userid);

            return Ok(households);
        }


        [HttpPost]
        public async Task<ActionResult> CreateNewHousehold(CreateHouseholdRequestDto household)
        {
            string userId = getUserID();

            var householdExists = await _householdRepository.DuplicatedHouseholdNameToUser(household.name, userId);

            if (householdExists) return BadRequest($"Käyttäjällä on jo '{household.name}' niminen talous");

            var newHousehold = new Household
            {
                name = household.name,
                ownerId = userId

            };

            var addedHousehold = await _householdRepository.AddAsync(newHousehold);


            var newHouseHoldUser = new HouseholdUser
            {
                HouseholdId = addedHousehold.HouseholdId,
                AuthServerUserId = userId,
                RoleId = 1
            };

            await _householdUserRepository.AddAsync(newHouseHoldUser);

            return StatusCode(201);
        }

        [HttpDelete("{householdId}")]
        public async Task<ActionResult> DeleteHousehold([FromRoute] Guid householdId)
        {
            string userId = getUserID();

            bool canPerformOwnerTask = await _householdUserRepository.CanPerformOwnerTask(householdId, userId);
            if (!canPerformOwnerTask) return BadRequest("Käyttäjällä ei ole oikeutta poistaa taloutta");

            var household = await _householdRepository.GetAsyncByGuid(householdId);
            if (household == null) return BadRequest("Taloutta ei löytynyt");

            var delete = await _householdRepository.DeleteAsyncByGuid(householdId);
            if (!delete) return BadRequest("Talouden poistaminen epäonnistui");

            return Ok();
        }

        private string getUserID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
