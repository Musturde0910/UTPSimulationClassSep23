
// this doesnt work in Unity
//using MathNet.Numerics.Distributions;  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Server : MonoBehaviour {

    public float mean;
    public float std;

    public AgentQueue customers;

    Agent currCustomer = null!;
    bool serverFree = true;
    int serviceTime = 0;
    int cumulativeQSize = 0;
    int maxQSize = 0;
    Normal serviceGaussian;

    DateTime prev;
    const int updateRate = 1;
    long time;

    void Start() {
        serviceGaussian = new Normal(mean, std);
        prev = DateTime.Now;
    }

    void Update() {
        time++;
        DateTime now = DateTime.Now;
        if ((now - prev).Seconds > updateRate) {
            update(customers, time);
            prev = DateTime.Now;
        }
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

    public bool update(AgentQueue agentQueue, long Time) {

        int currQSize = agentQueue.Size();
        cumulativeQSize += currQSize;
        if (currQSize > maxQSize)
            maxQSize = currQSize;

        if (serverFree == false) {
            serviceTime --;
            Debug.Log(serviceTime+" second remaining for server "+name);


            if (serviceTime <= 0) {
                Debug.Log(name + " server done at time="+Time);
                currCustomer = agentQueue.Pop();

                serverFree = true;
                return true;    // done
            }
        }

        if (serverFree == true && agentQueue.Size() > 0) {
            serviceTime = (int) serviceGaussian.Sample();
            Debug.Log(name+" server starts. To finish in "+serviceTime);
            serverFree = false;
        }

        return false;
    }


}