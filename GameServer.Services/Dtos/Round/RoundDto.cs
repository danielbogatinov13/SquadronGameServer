namespace GameServer.Services.Dtos.Round
{
    public class RoundDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public decimal AnswerBody { get; set; }
        public int GameId { get; set; }
        public bool? IsAnswered { get; set; }
        public bool IsAnswerCorrect { get; set; }
        public string? AnswerMessage { get; set; }
    }
}
