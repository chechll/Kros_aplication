namespace Kros_aplication.Dto
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public int? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }
    }
}
