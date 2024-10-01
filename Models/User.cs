using System;
namespace DailyPlanManager.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public ICollection<DailyPlan>? DailyPlans { get; set; }
	}
}

