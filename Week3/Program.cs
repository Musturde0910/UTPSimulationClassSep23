// See https://aka.ms/new-console-template for more information
using MathNet.Numerics.Distributions;



class Simulation {

  static void Main(string[] args)
  {
    int MaxSimTime = 720; //minutes
    int Time = 0;

    // queue starts from 0
    CarQueue carQueue= new CarQueue();
    Random rand = new Random();
    Normal serviceGaussian = new Normal(10, 4);
    Exponential carArrivalExp = new Exponential(0.1);

    bool serverFree = true;
    int serviceTime = 0;
    int timeCarArrival = rand.Next() % 10; 

    int qSize = 0;
    int maxQSize = 0;


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
            carQueue.Push(customer);
            timeCarArrival = (int) carArrivalExp.Sample();
            Console.WriteLine("Next car in "+timeCarArrival+" minutes");
        }

        if (serverFree == true && carQueue.NumCars() > 0) {
            Car towash = carQueue.Pop();
            serviceTime = (int) serviceGaussian.Sample();
            Console.WriteLine("Server start. To finish in "+serviceTime+" minutes");

            serverFree = false;
        }
        else if (serverFree == false) {
            serviceTime --;

            if (serviceTime <= 0) {
                Console.WriteLine("Server done at time="+Time);
                serverFree = true;
            }
        }

        int currQSize = carQueue.NumCars();
        qSize += currQSize;
        if (currQSize > maxQSize)
            maxQSize = currQSize;
        timeCarArrival --;
        Time ++;
    }

    float avgQSize = qSize / (float) MaxSimTime;
    Console.WriteLine("Average: " + avgQSize);
    Console.WriteLine("Max: " + maxQSize);

  }

}