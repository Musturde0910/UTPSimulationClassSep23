
// this doesnt work in Unity
//using MathNet.Numerics.Distributions;  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Server {
    string name = null!;
    Agent currCustomer = null!;
    bool serverFree = true;
    int serviceTime = 0;
    int cumulativeQSize = 0;
    int maxQSize = 0;
    Normal serviceGaussian;

    

    public Server(string name, float mean, float std) {
        this.name = name;
        serviceGaussian = new Normal(mean, std);
    }

    public Agent getCurrCustomer() {
        return currCustomer;
    }

    public int getCumulativeQSize() {
        return cumulativeQSize;
    }

    public int getMaxQSize() {
        return maxQSize;
    }

    public bool update(AgentQueue agentQueue, int Time) {

        int currQSize = agentQueue.Size();
        cumulativeQSize += currQSize;
        if (currQSize > maxQSize)
            maxQSize = currQSize;

        if (serverFree == false) {
            serviceTime --;

            if (serviceTime <= 0) {
                Debug.Log(name + " server done at time="+Time);
                serverFree = true;
                return true;    // done
            }
        }

        if (serverFree == true && agentQueue.Size() > 0) {
            currCustomer = agentQueue.Pop();
            serviceTime = (int) serviceGaussian.Sample();
            Debug.Log(name+" server starts. To finish in "+serviceTime);
            serverFree = false;
        }

        return false;
    }


}