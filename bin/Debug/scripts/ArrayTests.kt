Arr = new Array()
Arr.Add([2,4,1,5,6])
Arr.Add([3,7,12,11])
a = 5
b = 11
c = 18
Arr.Add(&a)
Arr.Add(&b)
Arr.Add(&c)
Arr.Remove(&a)
Arr.RemoveAt(1)
Arr.Insert(2,"Luka")
Console.Output(Arr)