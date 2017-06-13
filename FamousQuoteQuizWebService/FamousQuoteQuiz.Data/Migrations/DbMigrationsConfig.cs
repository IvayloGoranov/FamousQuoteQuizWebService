using System.Data.Entity.Migrations;
using System.Linq;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Data.Migrations
{
    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<QuoteQuizContext>
    {
        public DbMigrationsConfig()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(QuoteQuizContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (!context.Authors.Any() && !context.Quotes.Any())
            {
                var kevinKruse = new Author
                {
                    Name = "Kevin Kruse"
                };

                context.Authors.Add(kevinKruse);

                var quote1 = new Quote
                {
                    Content = "Life is about making an impact, not making an income.",
                    Author = kevinKruse
                };

                context.Quotes.Add(quote1);

                var napoleonHill = new Author
                {
                    Name = "Napoleon Hill"
                };

                context.Authors.Add(napoleonHill);

                var quote2 = new Quote
                {
                    Content = "Whatever the mind of man can conceive and believe, it can achieve.",
                    Author = napoleonHill
                };

                context.Quotes.Add(quote2);

                var bayGanyo = new Author
                {
                    Name = "Bay Ganyo"
                };

                context.Authors.Add(napoleonHill);

                var quote3 = new Quote
                {
                    Content = "You had money, you gave money",
                    Author = bayGanyo
                };

                context.Quotes.Add(quote3);

                var sunTsu = new Author
                {
                    Name = "Sun Tsu"
                };

                context.Authors.Add(sunTsu);

                var quote4 = new Quote
                {
                    Content = "All warfare is based on deception",
                    Author = sunTsu
                };

                context.Quotes.Add(quote4);

                var stoichkov = new Author
                {
                    Name = "Stoichkov"
                };

                context.Authors.Add(stoichkov);

                var quote5 = new Quote
                {
                    Content = "First half very good, second half I no like...",
                    Author = stoichkov
                };

                context.Quotes.Add(quote5);

                var luboslavPenev = new Author
                {
                    Name = "Luboslav Penev"
                };

                context.Authors.Add(luboslavPenev);

                var quote6 = new Quote
                {
                    Content = "If you don't like me, blow on the soup.",
                    Author = luboslavPenev
                };

                context.Quotes.Add(quote6);

                context.SaveChanges();
            }

            if (!context.QuizModes.Any())
            {
                var binaryMode = new QuizMode
                {
                    Type = QuizModeType.Binary,
                    IsSelected = true
                };

                context.QuizModes.Add(binaryMode);

                var multipleChoiceMode = new QuizMode
                {
                    Type = QuizModeType.MultipleChoice,
                    IsSelected = false
                };

                context.QuizModes.Add(multipleChoiceMode);

                context.SaveChanges();
            }
        }
    }
}
