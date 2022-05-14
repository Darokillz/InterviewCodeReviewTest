using System;
using System.Data.SqlClient;

namespace InterviewCodeReviewTest
{
	public class Test2
	{
		// Record customer purchase and update customer reward programme
		
		// Consider a better name to reflect the method functionality 
		public Result UpdateCustomerHistory(Purchase customerPurchase)
		{
			// - Consider a connection class to handle with connections instead of variables for better scalability
			// - Consider using a configuration file for the sql connection strings
			// - Spelling error for the variable "connPruchase" instead it should be "connPurchase"
			// - Consider using the keyword "using" for sql connection, commands and transaction to ensure objects are closed and disposed.
			// - Consider two try-catch block to handle with sql connections and sql query
			
			
			var connPruchase = new SqlConnection("data source=TestPurchaseServer;initial catalog=PurchaseDB;Trusted_Connection=True");
			var connReward = new SqlConnection("data source=TestRewardServer;initial catalog=RewardDB;Trusted_Connection=True");

			var cmdPurchase = new SqlCommand("INSERT INTO dbo.Purchase..."); // omitted the columns
			var cmdReward = new SqlCommand("INSERT INTO dbo.Reward..."); // omitted the columns

			SqlTransaction tranPurchase = null;
			SqlTransaction tranReward = null;

			try
			{
				// - Spelling error for the variable "connPruchase" instead it should be "connPurchase"
				connPruchase.Open();
				// - Could consider giving a name in BeginTransaction parameter for better clarity and can be used for later calls to rollback if needed
				tranPurchase = connPruchase.BeginTransaction(); 
				cmdPurchase.ExecuteNonQuery();

				connReward.Open();
				tranReward = connReward.BeginTransaction();
				cmdReward.ExecuteNonQuery();

				tranPurchase.Commit();
				tranReward.Commit();

				return Result.Success();
			}
			catch (Exception ex)
			{
				tranPurchase.Rollback();
				tranReward.Rollback();

				return Result.Failed();
			}
		}
	}

	public class Purchase
	{
		// Some members
	}

	public class Result
	{
		public bool IsSuccessful { get; private set; }

		public static Result Success()
		{
			return new Result { IsSuccessful = true };
		}

		public static Result Failed()
		{
			return new Result { IsSuccessful = false };
		}
	}
}
