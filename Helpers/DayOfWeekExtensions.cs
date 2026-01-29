namespace AppAcademia.Helpers
{
    public static class DayOfWeekExtensions
    {
        public static string ToPtBr(this DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Sunday => "Domingo",
                DayOfWeek.Monday => "Segunda-feira",
                DayOfWeek.Tuesday => "Terça-feira",
                DayOfWeek.Wednesday => "Quarta-feira",
                DayOfWeek.Thursday => "Quinta-feira",
                DayOfWeek.Friday => "Sexta-feira",
                DayOfWeek.Saturday => "Sábado",
                _ => dia.ToString()
            };
        }
    }
}
