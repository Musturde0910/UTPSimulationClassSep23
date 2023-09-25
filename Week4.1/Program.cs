// See https://aka.ms/new-console-template for more information

using MathNet.Numerics.Distributions;

class Simulation {

  static void Main(string[] args)
  {
    int MaxSimTime = 720; //minutes
    int Time = 0;

    // queue starts from 0
    CarQueue washQueue= new CarQueue();
    CarQueue polishQueue= new CarQueue();    
    Random rand = new Random();

    Server washServer = new Server("Wash", 15, 5);
    Server polishServer = new Server("Polish", 45, 10);

    int timeCarArrival = rand.Next() % 10; 
    Exponential arrivalExp = new Exponential(0.1);


    // kedai buka
    // Simulation time loop
    while (Time < MaxSimTime) {
        // Possible events
        // 1) car arrive and enter the queue
        // 2) car leave the queue, server starts to wash a car; server is busy
        // 3) server finishes; server is free

        if (timeCarArrival <= 0) { // may be -ive if num generated is 0
            Console.WriteLine("Time " + Time + ": Car arriving");
            Car customer = new Car();
            washQueue.Push(customer);
            timeCarArrival = (int) arrivalExp.Sample();
            Console.WriteLine("Next car in "+timeCarArrival);
        }

        bool washDone = washServer.update(washQueue, Time);
        if (washDone == true) {
            bool doPolish = rand.Next(2) == 0;
            if (doPolish == true) {
                Car topolish = washServer.getCurrCar();
                polishQueue.Push(topolish);
            }
        }
        bool polishDone = polishServer.update(polishQueue, Time);


        timeCarArrival --;
        Time ++;
    }

    float avgQSize = washServer.getCumulativeQSize() / (float) MaxSimTime;
    int maxQSize = washServer.getMaxQSize();
    Console.WriteLine("Average: " + avgQSize);
    Console.WriteLine("Max: " + maxQSize);

    float avgPolishQSize = polishServer.getCumulativeQSize() / (float) MaxSimTime;
    int maxPolishQSize = polishServer.getMaxQSize();    
    Console.WriteLine("Average polish queue: " + avgPolishQSize);
    Console.WriteLine("Max polish queue: " + maxPolishQSize);    

  }

}