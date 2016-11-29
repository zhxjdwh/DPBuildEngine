using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using BuildTest.Config;
using BuildTest.Database;
using BuildTest.Repository;
using Oracle.DataAccess.Client;
using IsolationLevel = System.Data.IsolationLevel;

namespace BuildTest
{
    class Program
    {

        public static void Main(string[] args)
        {
            StartUp.Start();


            Console.Read();
        }

    }
}
