using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using portfolio_api.Contracts;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using portfolio_api.Data;
using portfolio_api.Models.AuthServerUser;

namespace portfolio_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthServerUsersController : ControllerBase
    {
        private readonly IAuthServerUserRepository _authServerUserRepository;

        public AuthServerUsersController(IAuthServerUserRepository authServerUserRepository)
        {
            this._authServerUserRepository = authServerUserRepository;
        }

        [HttpPost("new-registration/{userId}")]
        public async Task<ActionResult> PostAuthServerUser([FromRoute] string userId)
        {
            var user = await GetAuth0UserData(userId);
            if (user == null) return BadRequest();

            var authServerUser = new AuthServerUser
            {
                AuthServerUserId = user.User_id,
                Email = user.Email,

            };

            var userExists = await _authServerUserRepository.ExistsById(user.User_id);

            if (userExists) return BadRequest("Käyttäjä on jo olemassa");

            await _authServerUserRepository.AddAsync(authServerUser);

            return Ok("Käyttäjä tallennettu ja talous luotu");
        }

        private async Task<string> GetAccessToken()
        {
            string domain = $"{Environment.GetEnvironmentVariable("domain")}/oauth/token";

            using HttpClient httpClient = new HttpClient();

            Console.WriteLine("GetEnviromentVariable" + Environment.GetEnvironmentVariable("client_id").ToString());

            var tokenRequestBody = new
            {
                client_id = Environment.GetEnvironmentVariable("client_id"),
                client_secret = Environment.GetEnvironmentVariable("client_secret"),
                audience = Environment.GetEnvironmentVariable("audience"),
                grant_type = "client_credentials"
            };

            HttpResponseMessage tokenResponse = await httpClient.PostAsJsonAsync(domain, tokenRequestBody);
            tokenResponse.EnsureSuccessStatusCode();

            string tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();

            TokenResponseDto tokenResponseJson = JsonConvert.DeserializeObject<TokenResponseDto>(tokenResponseBody);

            return tokenResponseJson.access_token;
        }

        private async Task<ParseAuthServerUserDto> GetAuth0UserData(string userId)
        {
            string accessToken = await GetAccessToken();
            string domain = $"{Environment.GetEnvironmentVariable("domain")}/api/v2/users/{userId}";

            using HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage userResponse = await httpClient.GetAsync(domain);

            if (!userResponse.IsSuccessStatusCode)
            {
                return null;
            }

            string userResponseBody = await userResponse.Content.ReadAsStringAsync();
            ParseAuthServerUserDto userResponseJson = JsonConvert.DeserializeObject<ParseAuthServerUserDto>(userResponseBody);

            return userResponseJson;
        }
    }
}
