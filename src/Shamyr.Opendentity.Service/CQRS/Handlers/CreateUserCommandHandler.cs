using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, CreatedModel>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<CreatedModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if ((await userManager.FindByNameAsync(request.Model.UserName)) != null)
                throw new ConflictException($"User with username '{request.Model.UserName}' already exist.");

            if ((await userManager.FindByEmailAsync(request.Model.Email)) != null)
                throw new ConflictException($"User with email '{request.Model.Email}' already exist.");

            var user = new ApplicationUser(request.Model.UserName)
            {
                Email = request.Model.Email,
                FirstName = request.Model.FirstName,
                LastName = request.Model.LastName,
                ImageUrl = request.Model.ImageUrl
            };

            var result = await userManager.CreateAsync(user, request.Model.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

            return new CreatedModel { Id = user.Id };
        }
    }
}
