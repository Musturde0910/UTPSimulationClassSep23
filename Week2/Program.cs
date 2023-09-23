// See https://aka.ms/new-console-template for more information
class Simulation {

  static void Main(string[] args)
  {
    int MaxSimTime = 720; //minutes
    int Time = 0;

    // queue starts from 0
    CarQueue carQueue= new CarQueue();
    Random rand = new Random();

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
            timeCarArrival = rand.Next() % 10;
            Console.WriteLine("Next car in "+timeCarArrival);
        }

        if (serverFree == true && carQueue.NumCars() > 0) {
            Car towash = carQueue.Pop();
            serviceTime = rand.Next() % 10;
            Console.WriteLine("Server start. To finish in "+serviceTime);

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