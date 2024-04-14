using Bogus;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Tests;

public static class GoalGenerator
{
    private static readonly string[] IsSatisfactory = { "S", "PS", "U" };

    public static Goal GenerateOne() =>
        GetFaker().Generate();

    public static List<Goal> GenerateMany(int quantity = 5) =>
        GetFaker().Generate(quantity);

    private static Faker<Goal> GetFaker() =>
        new Faker<Goal>()
            .RuleFor(g => g.Name, f => f.Random.Word())
            //.RuleFor(g => g.IsSatisfactory, f => f.PickRandom(IsSatisfactory))
            .RuleFor(g => g.AtendedDescription, f => f.Lorem.Paragraph())
            .RuleFor(g => g.ParciallyAtendedDescription, f => f.Lorem.Paragraph())
            .RuleFor(g => g.UnatendedDescription, f => f.Lorem.Paragraph());
}