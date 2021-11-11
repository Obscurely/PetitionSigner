using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS4014

namespace PetitionSpammer
{
    class Program
    {
        static int totalSigns = 0;

        /// <summary>
        /// Main method with the dialog of asking for the petition name and where the threads with loops are created.
        /// IMPORTANT NOTICE!!! --->
        /// THIS IS TESTED UNDER LINUX, TEST YOURSELF UNDER WINDOWS IF YOU USE IT, I'M TOO LAZY TO REBOOT INTO WINDOWS JUST TO TRY THIS, BUT PROBABLY WORKS THE SAME IN WINDOWS.
        /// IF YOU HAVE 16 THREADS (NOT CORES, THREADS, YOUR CPU CAN HAVE 8 CORES BUT 16 THEADS) FOR EXAMPLE YOU WOULD USE THE DOUBLE TO MAKE THE REQUEST, MEANING 32 THREADS IN THE APP.
        /// ANOTHER EXAMPLE... IF YOU HAVE 8 THREADS YOU WOULD USE 16 THREADS IN THE APP.
        /// FROM MY TESTING ON MY MAIN WORKSTATION THAT 16 THREADS USING 32 THREADS IN THE APP WORKS FINE (UNDER LINUX), BUT WHEN TRYING 32 THREADS IN MY SERVER WHICH HAS ONLY 
        /// 8 THREADS IT FREEZES AFTER A FEW REQUESTS (DOESN'T GIVE AN ERROR).
        /// AFTER LIKE 30 MINUTES OR MORE YOU DON'T SEE ANY ACTION IN THE TERMINAL THAN YOU SHOULD RECOMPILE APP TO USE LESS THREADS. COMMENT/DELETE THREAD INIT LINES LIKE DONE BELOW.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            // Prompt for setting the petition name (from the url)
            string petitionName = "";
            while (true)
            {
                System.Console.WriteLine("Please enter the petition name in the url, like \"nu-vrem-teze-anul-asta\" in https://campaniamea.declic.ro/petitions/nu-vrem-teze-anul-asta :");
                petitionName = Console.ReadLine();

                System.Console.WriteLine($"You chose {petitionName}, are you sure this is right? (y/n)");
                string input = Console.ReadLine();
                if (input.ToLower().Equals("y") || input.ToLower().Equals("yes"))
                    break;
            }

            PetitionSpammer petitionSpammer = await PetitionSpammer.AsyncCreatePetitionSpammer(petitionName);
            ProxyManager proxyManager = new();

            Thread signPetitionThread1 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 1)));
            Thread signPetitionThread2 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 2)));
            Thread signPetitionThread3 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 3)));
            Thread signPetitionThread4 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 4)));
            Thread signPetitionThread5 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 5)));
            Thread signPetitionThread6 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 6)));
            Thread signPetitionThread7 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 7)));
            Thread signPetitionThread8 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 8)));
            Thread signPetitionThread9 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 9)));
            Thread signPetitionThread10 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 10)));
            Thread signPetitionThread11 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 11)));
            Thread signPetitionThread12 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 12)));
            Thread signPetitionThread13 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 13)));
            Thread signPetitionThread14 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 14)));
            Thread signPetitionThread15 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 15)));
            Thread signPetitionThread16 = new(new ThreadStart(() => LoopSignPetition(petitionSpammer, proxyManager, 16)));
            // Thread signPetitionThread17 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 17)));
            // Thread signPetitionThread18 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 18)));
            // Thread signPetitionThread19 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 19)));
            // Thread signPetitionThread20 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 20)));
            // Thread signPetitionThread21 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 21)));
            // Thread signPetitionThread22 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 22)));
            // Thread signPetitionThread23 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 23)));
            // Thread signPetitionThread24 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 24)));
            // Thread signPetitionThread25 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 25)));
            // Thread signPetitionThread26 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 26)));
            // Thread signPetitionThread27 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 27)));
            // Thread signPetitionThread28 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 28)));
            // Thread signPetitionThread29 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 29)));
            // Thread signPetitionThread30 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 30)));
            // Thread signPetitionThread31 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 31)));
            // Thread signPetitionThread32 = new(new ThreadStart(() => LoopSignPetitions(petitionSpammer, proxyManager, 32)));

            signPetitionThread1.Start();
            signPetitionThread2.Start();
            signPetitionThread3.Start();
            signPetitionThread4.Start();
            signPetitionThread5.Start();
            signPetitionThread6.Start();
            signPetitionThread7.Start();
            signPetitionThread8.Start();
            signPetitionThread9.Start();
            signPetitionThread10.Start();
            signPetitionThread11.Start();
            signPetitionThread12.Start();
            signPetitionThread13.Start();
            signPetitionThread14.Start();
            signPetitionThread15.Start();
            signPetitionThread16.Start();
            // signPetitionThread17.Start();
            // signPetitionThread18.Start();
            // signPetitionThread19.Start();
            // signPetitionThread20.Start();
            // signPetitionThread21.Start();
            // signPetitionThread22.Start();
            // signPetitionThread23.Start();
            // signPetitionThread24.Start();
            // signPetitionThread25.Start();
            // signPetitionThread26.Start();
            // signPetitionThread27.Start();
            // signPetitionThread28.Start();
            // signPetitionThread29.Start();
            // signPetitionThread30.Start();
            // signPetitionThread31.Start();
            // signPetitionThread32.Start();

            System.Console.WriteLine("Press any key to stop!");
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// The infinite loop os signing petition with proxies and checking for any errors.
        /// </summary>
        /// <param name="petitionSpammer">The PetitionSpammer object.</param>
        /// <param name="proxyManager">The ProxyManager object.</param>
        /// <param name="threadId">The thread if of the loop to be ran in.</param>
        /// <returns>A task that you can use to check shit if you need to.</returns>
        static async Task LoopSignPetition(PetitionSpammer petitionSpammer, ProxyManager proxyManager, int threadId)
        {
            await proxyManager.DownloadProxies(3000, threadId);
            HttpClient client = await petitionSpammer.NewHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
            
            int i = 1;
            while (true)
            {
                try
                {
                    HttpResponseMessage response = await petitionSpammer.SignPetition(client);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    HttpStatusCode successCode = (HttpStatusCode)200;
                    if (response.StatusCode == successCode && responseContent.Split("\"action\":\"signed\"").Length >= 2 && responseContent.Split("'Signed Petition'").Length >= 2)
                    {
                        i++;
                        totalSigns++;
                        System.Console.WriteLine($"Signed petition, in thread {threadId}, number {i} in thread, total number: {totalSigns}");
                    }
                    else 
                    {
                        await proxyManager.DownloadProxies(3000, threadId);
                        client = await petitionSpammer.NewHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
                    }
                }
                catch (TaskCanceledException)
                {
                    client = petitionSpammer.RecreateHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
                }
                catch (HttpRequestException)
                {
                    client = petitionSpammer.RecreateHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
                }
                catch (UriFormatException)
                {
                    client = petitionSpammer.RecreateHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
                }
                catch (Exception)
                {
                    await proxyManager.DownloadProxies(3000, threadId);
                    client = petitionSpammer.RecreateHttpClientForSign(await proxyManager.PickProxy(3000, threadId));
                }
            }
        }
    }
}
