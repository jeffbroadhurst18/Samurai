using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiAppCore.Data;
using SamuraiAppCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreUI
{
	class Program
	{
		private static SamuraiContext _context = new SamuraiContext();

		static void Main(string[] args)
		{
			_context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
			//InsertSamurai();
			//InsertMultipleSamurai();
			//SimpleSamuraiQuery();
			//MoreQueries();
			//RetrieveAndUpdateSamurai();
			RetrieveAndUpdateMultipleSamurai();
			Console.ReadLine();
		}

		private static void RetrieveAndUpdateMultipleSamurai()
		{
			var samurais = _context.Samurais.Where(s => !s.Name.Contains("San")).ToList();
			samurais.ForEach(s => s.Name += "San");
			_context.SaveChanges();
		}

		private async static void RetrieveAndUpdateSamurai()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Name += "San";
			await _context.SaveChangesAsync();
		}

		private static void MoreQueries()
		{
			var name = "Julie";
			//var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
			var samurais = _context.Samurais.Find(1); //if the record is already in memory then EF won't go to
			//the database to get the value.
		}

		private static void SimpleSamuraiQuery()
		{
			var samurais = _context.Samurais.ToList();
		}

		private static void InsertMultipleSamurai()
		{
			var samurai = new Samurai { Name = "Bill" };
			var samuraiSam = new Samurai { Name = "Sampson" };
			_context.AddRange(new List<Samurai>() { samurai, samuraiSam });//Adds multiple
			_context.SaveChanges(); //Does single SQL to insert batch of data.
		}

		private static void InsertSamurai()
		{
			var samurai = new Samurai { Name = "Julie" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}


	}
}
