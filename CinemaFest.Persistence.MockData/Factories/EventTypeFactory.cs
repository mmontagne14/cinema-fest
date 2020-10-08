using CinemaFest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Persistence.MockData.Factories
{
    public static class EventTypeFactory
    {
        public static IEnumerable<EventType> GetEventTypeList()
        {
            return new List<EventType>()
            {
                new EventType(){Id=1,Code="FilmFestival", Name="Film Festival", SingularDescription = "Festival de Cine", PluralDescription = "Festivales de Cine" },
                new EventType(){Id=2,Code="ScreenwritingContest", Name="Screenwriting Contest", SingularDescription = "Concurso de escritura guíones", PluralDescription = "Concursos de escritura guíones" },
                new EventType(){Id=3,Code="OnlineFestival", Name="Online Festival", SingularDescription = "Festival Online", PluralDescription = "Festivales Online" }
            };
        }

        public static EventType GetByEventTypeCode(string code)
        {
            return GetEventTypeList().Where(x => x.Code.ToUpper() == code.ToUpper()).FirstOrDefault();
        }
    }
}
