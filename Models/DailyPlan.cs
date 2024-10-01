using System;
namespace DailyPlanManager.Models
{
	public class DailyPlan
	{
		public int Id { get; set; }
		public int? User_Id { get; set; }
		public User? User { get; set; }
		public string? Title{ get; set; }
		public string? Description { get; set; }
		public DateOnly Date { get; set; }
	}
}

