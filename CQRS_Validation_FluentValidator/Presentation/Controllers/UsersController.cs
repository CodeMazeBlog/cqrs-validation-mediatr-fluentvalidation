using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Users;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUsers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    /// <summary>
    /// The users controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="sender"></param>
        public UsersController(ISender sender) => _sender = sender;

        /// <summary>
        /// Gets all of the users.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery();

            var users = await _sender.Send(query, cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Gets the user with the specified identifier, if it exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The user with the specified identifier, if it exists.</returns>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(userId);

            var user = await _sender.Send(query, cancellationToken);

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user based on the specified request.
        /// </summary>
        /// <param name="request">The create user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateUserCommand>();

            var user = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
        }

        /// <summary>
        /// Updates the user with the specified identifier based on the specified request, if it exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The update user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content.</returns>
        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<UpdateUserCommand>() with
            {
                UserId = userId
            };

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
