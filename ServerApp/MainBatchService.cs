using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class MainBatchService
    {
        static void Main(string[] args)
        {
            UserBatchService userBatchService = new UserBatchService();
            userBatchService.StartBatchService();
            Console.WriteLine("Batch service scheduled. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
