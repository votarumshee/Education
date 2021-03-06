﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTestClient
{
    public class TestConductor
    {
        //private const int MaxClientsCount = 1_000;
        private const int MaxClientsCount = 500;

        //private List<TestClient> TestClients { get; }

        public TestConductor()
        {
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                List<TestClient> testClients = PrepareTestClients(httpClient);
                List<Task> promises = new List<Task>(MaxClientsCount);

                foreach (var client in testClients)
                {
                    Console.WriteLine($"Running client, Id: {client.ClientId}");
                    promises.Add(client.Run(cancellationToken));
                }

                await Task.WhenAll(promises);
            }
        }

        private List<TestClient> PrepareTestClients(HttpClient httpClient)
        {
            var testClients = new List<TestClient>(MaxClientsCount);

            for (long i = 1; i <= MaxClientsCount; i++)
            {
                var client = new TestClient(httpClient, i);
                testClients.Add(client);
            }

            return testClients;
        }
    }
}