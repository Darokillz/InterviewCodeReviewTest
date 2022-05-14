using System.Net.Mail;
using System.Threading;

namespace InterviewCodeReviewTest
{
	public class Test3
	{
		// This class represents a queue for email sending.
		// There are multiple active queues at any given time and they have activities all the time.
		// Each queue can be handled by multiple threads.
		public class EmailSendQueue
		{
			public int SentCount { get; private set; }
			public int FailedCount { get; private set; }

			// Assign each email to different thread for performance
			public void SendNextEmail()
			{
				// - Consider using threadpool due to a posibility of high volumne of requests
				//  problem may arise such as context swtiching, thread creation and large overhead
				//  use ThreadPool.QueueUserWorkItem(SendEmail);
				var thread = new Thread(SendEmail);
				thread.Start();
			}

			private void SendEmail()
			{
				var client = new SmtpClient();
				// Send email via Smtp and returns Result object...
				UpdateStatistics(result);
			}

			private void UpdateStatistics(Result result)
			{
				// - lock(typeof(EmailSendQueue) is a bad way of locking.
				// This is due to the object EmailSendQueue is public thus widely accesible with the possibility of introducing deadlocks.
				// A solution will be creating a private object in the emailSendQueue class ie. Private object _updateLock = new object();
				// and change lock(typeof(EmailSendQueue) into lock(_updateLock). 
				lock (typeof(EmailSendQueue))
				{
					if (result.IsSuccessful)
					{
						SentCount++;
					}
					else
					{
						FailedCount++;
					}
				}
			}
		}
	}
}
