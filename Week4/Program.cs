// See https://aka.ms/new-console-template for more information
class Simulation {

  static void Main(string[] args)
  {
    int MaxSimTime = 720; //minutes
    int Time = 0;

    // queue starts from 0
    CarQueue carQueue= new CarQueue();
    CarQueue polishQueue= new CarQueue();    
    Random rand = new Random();

    Car towash = null!;
    Car topolish = null!;

    bool serverFree = true;
    bool polishFree = true;

    int serviceTime = 0;
    int polishTime = 0;

    int qSize = 0;
    int polishQSize = 0;

    int maxQSize = 0;
    int maxPolishQSize = 0;

    int timeCarArrival = rand.Next() % 10; 



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
            towash = carQueue.Pop();
            serviceTime = rand.Next() % 10;
            Console.WriteLine("Server start. To finish in "+serviceTime);

            serverFree = false;
        }
        else if (serverFree == false) {
            serviceTime --;

            if (serviceTime <= 0) {
                Console.WriteLine("Server done at time="+Time);
                serverFree = true;

                bool doPolish = rand.Next(2) == 0;
                if (doPolish == true) {
                    polishQueue.Push(towash);
                    polishTime = rand.Next() % 10;
                }
            }
        }

        if (polishFree == true && polishQueue.NumCars() > 0) {
            topolish = polishQueue.Pop();
            polishTime = rand.Next() % 10;
            Console.WriteLine("Polish server start. To finish in "+polishTime);

            polishFree = false;
        }
        else if (polishFree == false) {
            polishTime --;

            if (polishTime <= 0) {
                Console.WriteLine("Polish server done at time="+Time);
                polishFree = true;
            }
        }        

        int currQSize = carQueue.NumCars();
        qSize += currQSize;
        if (currQSize > maxQSize)
            maxQSize = currQSize;

        int currPolishQSize = polishQueue.NumCars();
        polishQSize += currPolishQSize;
        if (currPolishQSize > maxPolishQSize)
            maxPolishQSize = currPolishQSize;


        timeCarArrival --;
        serviceTime --;
        polishTime --;

        Time ++;
    }

    float avgQSize = qSize / (float) MaxSimTime;
    Console.WriteLine("Average: " + avgQSize);
    Console.WriteLine("Max: " + maxQSize);

    float avgPolishQSize = polishQSize / (float) MaxSimTime;
    Console.WriteLine("Average polish queue: " + avgPolishQSize);
    Console.WriteLine("Max polish queue: " + maxPolishQSize);    

  }

}