
using MathNet.Numerics.Distributions;

class Server {
    string name = null!;
    Car currcar = null!;
    bool serverFree = true;
    int serviceTime = 0;
    int cumulativeQSize = 0;
    int maxQSize = 0;
    Normal serviceGaussian;

    public Server(string name, float mean, float var) {
        this.name = name;
        serviceGaussian = new Normal(mean, var);
    }

    public Car getCurrCar() {
        return currcar;
    }

    public int getCumulativeQSize() {
        return cumulativeQSize;
    }

    public int getMaxQSize() {
        return maxQSize;
    }

    public bool update(CarQueue carQueue, int Time) {

        int currQSize = carQueue.NumCars();
        cumulativeQSize += currQSize;
        if (currQSize > maxQSize)
            maxQSize = currQSize;

        if (serverFree == false) {
            serviceTime --;

            if (serviceTime <= 0) {
                Console.WriteLine(name + " server done at time="+Time);
                serverFree = true;
                return true;    // done
            }
        }

        if (serverFree == true && carQueue.NumCars() > 0) {
            currcar = carQueue.Pop();
            serviceTime = (int) serviceGaussian.Sample();
            Console.WriteLine(name+" server starts. To finish in "+serviceTime);
            serverFree = false;
        }

        return false;
    }


}