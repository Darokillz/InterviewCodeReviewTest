using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InterviewCodeReviewTest
{
	public class Test1
	{
		// Called by web API and returns list of strongly typed customer address for given status
            	// CustomerAddress is populated by external import and could be dirty
		
		// - GetCustomerNumbers name does not refect the methods funcionality. Consider another name.
		public IEnumerable<Address> GetCustomerNumbers(string status)
		{
			// - Use a configuration file for connection string for better readability 
			// i.e Var connection = new SqlConnection(System.Configuration. ConfigurationManager.ConnectionStrings["ExampleName"].ConnectionString
			// - For scalabilty, instead of using varibles for connections it could be turned into a class
			
			var connection = new SqlConnection("data source=TestServer;initial catalog=CustomerDB;Trusted_Connection=True");
			var cmd = new SqlCommand($"SELECT CustomerAddress FROM dbo.Customer WHERE Status = '{status}'", connection);
			
			
			// - Consider using the keyword "Using" to handle with closing connections
			// i.e Using(var connection....)
			//     Using(var cmd...)
			//     Using(var reader....)
			
			// - Could consider having two try-catch block, one to handle inital sql connection and later for executing sql query
			// this will allow a better control over debug and adding logs for observabillity.
			try
			{
				var addressStrings = new List<string>();

				connection.Open();
				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					addressStrings.Add(reader.GetString(0));
				}

				return addressStrings
					.Select(StringToAddress)
					.Where(x => x != null)
					.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static Address StringToAddress(string addressString)
		{
			return new Address(addressString);
		}
	}
	
	// Consider puting this class in a seperate file 
	public class Address
	{
		// Some members...

		public Address(string addressString)
		{
			// Assume there are logic here to parse address and return strongly typed object
		}
	}
}
