using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database.Entities;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.CQRS.Users;

public record GetUsersQuery(UsersFilterModel Model): IRequest<UsersModel>;

public class GetUsersQueryHandler: IRequestHandler<GetUsersQuery, UsersModel>
{
    private readonly UserManager<ApplicationUser> manager;

    public GetUsersQueryHandler(UserManager<ApplicationUser> manager)
    {
        this.manager = manager;
    }

    public async Task<UsersModel> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = TrySearch(request, manager.Users);

        var count = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip(request.Model.Offset)
            .Take(request.Model.Limit)
            .ToArrayAsync(cancellationToken);

        return new UsersModel
        {
            Total = count,
            Users = data.ToModel()
        };
    }

    private static IQueryable<ApplicationUser> TrySearch(GetUsersQuery request, IQueryable<ApplicationUser> query)
    {
        if (!string.IsNullOrWhiteSpace(request.Model.Search) && request.Model.Search.Length > 3)
        {
            var parts = GetParts(request.Model.Search);
            var likes = parts.Select(x => $"%{x}%");

            Expression<Func<ApplicationUser, bool>>? exp = null;

            foreach (var like in likes)
            {
                Expression<Func<ApplicationUser, bool>>? newExp = e => EF.Functions.ILike(e.FirstName + " " + e.LastName + " " + e.NormalizedEmail + " " + e.NormalizedUserName, like);
                if (exp == null)
                    exp = newExp;
                else
                {
                    var parameter = Expression.Parameter(typeof(ApplicationUser));

                    var binary = (BinaryExpression)new ReplaceParametersVisitor(parameter, exp.Parameters[0], newExp.Parameters[0])
                        .Visit(Expression.OrElse(exp.Body, newExp.Body));

                    exp = Expression.Lambda<Func<ApplicationUser, bool>>(binary, parameter);
                }
            }

            if (exp != null)
            {
                query = query.Where(exp);
            }
        }

        return query;
    }

    private static List<string> GetParts(string search)
    {
        var parts = search
            .Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        var longEnoughParts = new List<string>();
        var current = string.Empty;

        foreach (var part in parts)
        {
            if (part.Length <= 3)
            {
                current = current == string.Empty ? part : current + " " + part;
            }
            else
            {
                longEnoughParts.Add(part);
                current = string.Empty;
            }

            if (current.Length > 3)
            {
                longEnoughParts.Add(current);
                current = string.Empty;
            }
        }

        return longEnoughParts;
    }
}
