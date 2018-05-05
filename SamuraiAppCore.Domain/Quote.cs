namespace SamuraiAppCore.Domain
{
	public class Quote
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public Samurai Samurai { get; set; }
		public int SamuraiId { get; set; } //add this field for it to be used as foreign key
	}
}