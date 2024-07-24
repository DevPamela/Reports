//using Bogus;
//using Bogus.DataSets;
//using Reports.Domain.Models;

//namespace Reports.Infrastructure.Tests;

//public static class StudentGenerator
//{
//    private static readonly string[] Genders = { "F", "M" };

//    public static Student GenerateOne() =>
//        GetFaker().Generate();

//    public static List<Student> GenerateMany(int quantity = 5) =>
//        GetFaker().Generate(quantity);

//    private static Faker<Student> GetFaker() =>
//        new Faker<Student>()
//            .RuleFor(s => s.Genre, f => f.PickRandom(Genders))
//            .RuleFor(s => s.Name, (f, s) =>
//            {
//                var gender = s.Genre == "F" ? Name.Gender.Female : Name.Gender.Male;
//                return f.Name.FullName(gender);
//            })
//            //RuleFor(s => s.Birthday, f => f.Date.Past().ToShortDateString())
//            .RuleFor(s => s.Absences, f => f.Random.Number(10).ToString());
//}