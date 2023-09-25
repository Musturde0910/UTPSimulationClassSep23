class CarQueue {
    Queue<Car> carQ = new Queue<Car>();
    public void Push(Car car) {
        carQ.Enqueue(car);
    }

    public Car Pop() {
        return carQ.Dequeue();
    }

    public bool Empty() {
        return carQ.Count == 0;
    }

    public int NumCars() {
        return carQ.Count;
    }


}