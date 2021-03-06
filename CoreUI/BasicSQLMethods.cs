﻿using Microsoft.EntityFrameworkCore;
using SamuraiAppCore.Data;
using SamuraiAppCore.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CoreUI
{
    public class BasicSQLMethods
    {
		private static SamuraiContext _context;

		public BasicSQLMethods(SamuraiContext context)
		{
			_context = context;
		}

		public static void RawSQlCommandWithOutput()
		{
			var procResult = new SqlParameter
			{
				ParameterName = "@ProcResult",
				SqlDbType = System.Data.SqlDbType.VarChar,
				Direction = System.Data.ParameterDirection.Output,
				Size = 50
			};
			// use _context.Database as we aren't operating on a specific table
			//SP gives output parameter
			_context.Database.ExecuteSqlCommand(
				"exec FindLongestName @procResult OUT", procResult);
			Console.WriteLine($"Longest Name {procResult.Value}");
		}

		public static void QueryWithNonSql()
		{
			var samurais = _context.Samurais
				.Select(s => new { newName = ReverseString(s.Name), name = s.Name }).
				ToList();
			samurais.ForEach(s => Console.Write(s.name + " " + s.newName + Environment.NewLine));
		}

		public static string ReverseString(string value)
		{
			var stringChar = value.AsEnumerable();
			return string.Concat(stringChar.Reverse());
		}

		public static void StoredProcQuery()
		{
			var namePart = "J";
			//var samurais = _context.Samurais.FromSql("EXEC FilterSamuraiByNamePart {0}", namePart)
			var samurais = _context.Samurais.FromSql("EXEC FilterSamuraiByNamePart @namepart",
				new SqlParameter("@namepart", namePart))
				.OrderByDescending(s => s.Name).ToList(); //won't amend the SP

			samurais.ForEach(s => Console.WriteLine(s.Name));
			Console.WriteLine();
		}

		public static void RawSQLQuery()
		{
			//EF works out that the orderby can be done by sql on database.
			var samurais = _context.Samurais.FromSql("Select * from Samurais")
				.Where(s => s.Name != "Julie") //Does filetr on db, so don't return complete dataset to here
				.OrderByDescending(s => s.Name)
				.ToList();

			samurais.ForEach(s => Console.WriteLine(s.Name));
			Console.WriteLine();
		}

		public static void DeleteMany()
		{
			var samurais = _context.Samurais.Where(s => s.Id > 5);
			_context.RemoveRange(samurais);
			_context.SaveChanges(); //removes a range of objects in one batch
		}

		public static void DeleteWhileTracked()
		{
			var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Wendy");
			_context.Samurais.Remove(samurai); //You must pass in the whole of the class.

			//alternatives
			//_context.Remove(samurai);
			//_context.Entry(samurai).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			//_context.Remove(_context.Samurais.Find(8)); //means you only have to pass in id.
			_context.SaveChanges(); //
		}

		public static void QueryAndUpdateSamuraiDisconnectedBattle()
		{
			var battle = _context.Battles.FirstOrDefault();
			var battle2 = _context.Battles.LastOrDefault();
			//battle2.EndDate = new DateTime(1759, 12, 31);
			_context.Entry(battle2).State = Microsoft.EntityFrameworkCore.EntityState.Modified; //will be updated by Save changes
			_context.SaveChanges(); //updates even though no data has actually changed.
			using (var contextNewAppInstance = new SamuraiContext()) //different context
			{
				contextNewAppInstance.Battles.Update(battle); //EF works out which battle to update from the data
															  //in the battle object. Updates all the properties as it doesn't know which have changed.
				contextNewAppInstance.SaveChanges();
				//EF doesn't know which property has been updated so sends them all in. 
			}
		}

		public static void QueryAndUpdateSamuraiDisconnected()
		{
			var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Jeff");
			samurai.Name += "San";
			using (var contextNewAppInstance = new SamuraiContext()) //different context
			{
				contextNewAppInstance.Samurais.Update(samurai);
				contextNewAppInstance.SaveChanges();
			}
		}

		public static void RetrieveAndUpdateMultipleSamurai()
		{
			var samurais = _context.Samurais.Where(s => !s.Name.Contains("San")).ToList();
			samurais.ForEach(s => s.Name += "San"); //note the syntax for foreach on a list
			_context.SaveChanges();
		}

		public async static void RetrieveAndUpdateSamurai()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Name += "San";
			await _context.SaveChangesAsync();
		}

		public static void MoreQueries()
		{
			var name = "Julie";
			//var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
			var samurais = _context.Samurais.Find(1); //if the record is already in memory then EF won't go to
													  //the database to get the value.
		}

		public static void SimpleSamuraiQuery()
		{
			var samurais = _context.Samurais.ToList();
		}

		public static void InsertMultipleSamurai()
		{
			var samurai = new Samurai { Name = "Bill" };
			var samuraiSam = new Samurai { Name = "Sampson" };
			_context.AddRange(new List<Samurai>() { samurai, samuraiSam });//Adds multiple
			_context.SaveChanges(); //Does single SQL to insert batch of data.
		}

		public static void InsertSamurai()
		{
			var samurai = new Samurai { Name = "Julie" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

	}
}
