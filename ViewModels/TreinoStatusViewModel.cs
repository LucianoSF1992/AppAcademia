namespace AppAcademia.ViewModels
{
    public class TreinoStatusViewModel
    {
        public int TreinoId { get; set; }
        public DayOfWeek DiaSemana { get; set; }

        public int TotalExercicios { get; set; }
        public int ExerciciosConcluidos { get; set; }

        public bool NaoIniciado => ExerciciosConcluidos == 0;
        public bool EmAndamento =>
            ExerciciosConcluidos > 0 && ExerciciosConcluidos < TotalExercicios;
        public bool Concluido =>
            TotalExercicios > 0 && ExerciciosConcluidos == TotalExercicios;
        public bool PodeExcluir => ExerciciosConcluidos == 0;
    }
}
