using GameServer.Models.Infrastructure;

namespace GameServer.Models
{
    public class Question : CreationAuditedEntity
    {
        public Question()
        {

            Random random = new Random();
            List<decimal> operands = new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<string> operations = new List<string> { "+", "-", "*", @"\" };
            var firstOperandIndex = random.Next(operands.Count);
            var secondOperandIndex = random.Next(operands.Count);
            var operationIndex = random.Next(operations.Count);
            QuestionBody = $"{operands[firstOperandIndex]} {operations[operationIndex]} {operands[secondOperandIndex]}";

            switch (operations[operationIndex])
            {
                case "+":
                    QuestionAnswer = operands[firstOperandIndex] + operands[secondOperandIndex];
                    break;
                case "-":
                    QuestionAnswer = operands[firstOperandIndex] - operands[secondOperandIndex];
                    break;
                case "*":
                    QuestionAnswer = operands[firstOperandIndex] * operands[secondOperandIndex];
                    break;
                case "/":
                    QuestionAnswer = operands[firstOperandIndex] / operands[secondOperandIndex];
                    break;
                default:
                    QuestionAnswer = 0;
                    break;

            }
        }


        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        public string QuestionBody { get; set; }
        public decimal QuestionAnswer { get; set; }
    }
}
