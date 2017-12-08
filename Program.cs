using System;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
using System.Text;
using RestSharp.Extensions.MonoHttp;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GA;

namespace FunctionOptimizationWithGeneticSharp
{

    public class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string fileName;
            int i = 0;
            int count = 0;
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "JSON File Browser";
            fd.Filter = "Json files (*.json)|*.json";
            fd.ShowDialog();
            fileName = fd.FileName;
            var path = fileName;
            string contents = File.ReadAllText(path);
            dynamic rootObj = JsonConvert.DeserializeObject<InputJson.RootObject>(contents);
            foreach (var row in rootObj.resources)
            {
                foreach (var element in row.shifts)
                {
                    count++;
                }
            }
            double[] min_p = new double[count];
            double[] max_p = new double[count];
            foreach (var row in rootObj.resources)
            {
                foreach (var element in row.shifts)
                {
                    if (element.start < 0)
                    {
                        min_p[i] = (float)element.curProviders;
                        max_p[i] = (float)element.curProviders;
                    }
                    else
                    { 
                    min_p[i] = (float)element.minProviders;
                    max_p[i] = (float)element.maxProviders;
                    }
                    i++;
                }
            }
            int[] five = new int[count];
            int[] zero = new int[count];
            for (int j = 0; j < count; j++)
            {
                five[j] = 5;
                zero[j] = 0;
            }
            var chromosome = new FloatingPointChromosome(min_p, max_p, five, zero);
            var population = new Population(25,50,chromosome);

               var fitness = new FuncFitness((f) =>
                {
                    var serviceproviders = f as FloatingPointChromosome;
                    int val = 0;
                    var values = serviceproviders.ToFloatingPoints();
                    
                    foreach (var row in rootObj.resources)
                    {
                        if (val == count)
                            break;
                        else
                        {
                            foreach (var element in row.shifts)
                            {
                                element.curProviders = (int)values[val];
                            }
                            val++;
                        }
                    }
         
                    string output = JsonConvert.SerializeObject(rootObj, Formatting.Indented);
                    string encoded = HttpUtility.UrlEncode(output);
                    string final = "s=" + encoded;
                    string responseContent = null;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    WebRequest request = WebRequest.Create("https://backend.hera.potentiaco.com/simavg");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(final);
                    request.ContentLength = data.Length;

                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    
                    using (WebResponse response = request.GetResponse())
                     {
                         using (Stream stream = response.GetResponseStream())
                         {
                             using (StreamReader sr = new StreamReader(stream))
                             {
                                 responseContent = sr.ReadToEnd();
                             }
                         }
                     }
                    

                    dynamic outputObj = JsonConvert.DeserializeObject<OutputJson.RootObject>(responseContent);
                        double LOS = (1 / (1 + outputObj.LOS_avg));
                        return (LOS);
                    
                });
               var selection = new RouletteWheelSelection();
               var crossover = new UniformCrossover(0.5f);
               var mutation = new FlipBitMutation();
               var termination = new FitnessStagnationTermination(15);
               var ga = new GeneticAlgorithm(population,fitness,selection,crossover,mutation);
               ga.Termination = termination;
               Console.WriteLine("Genetic Algorithm for Optimization");
               var latestFitness = 0.0;
               ga.GenerationRan += (sender, e) =>
               {
                   var optimalProviders = ga.BestChromosome as FloatingPointChromosome;
                   var LengthOfStay = ((1/optimalProviders.Fitness.Value)-1);

                   if (LengthOfStay != latestFitness)
                   {
                       latestFitness = LengthOfStay;
                       var x = 0;
                       var finalChromosome = optimalProviders.ToFloatingPoints();
                       foreach (var row in rootObj.resources)
                       {

                               foreach (var element in row.shifts)
                               {
                                   element.curProviders = (int)finalChromosome[x];
                                    x++;
                               }
                               
                          
                       }
                       Console.WriteLine(
                           "Generation {0,2}:",ga.GenerationsNumber);
                       foreach (var row in rootObj.resources)
                       {
                           Console.WriteLine("ID: {0}", row.id);
                           Console.WriteLine("Name: {0}", row.name);
                           Console.WriteLine("Set of providers:");
                           foreach (var element in row.shifts)
                           {
                               Console.Write(element.curProviders);
                               Console.Write(" ");
                           }
                           Console.WriteLine();
                       }
                       Console.WriteLine(
                           "Length of stay: {0}", LengthOfStay);
                       Console.WriteLine();
                   }
               };
               ga.Start();
            Console.WriteLine("The Genetic Algorithm has completed it's execution");
            Console.ReadKey();
         }
     }
 }
 