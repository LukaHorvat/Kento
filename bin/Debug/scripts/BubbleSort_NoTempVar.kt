Arr = new Array(100)
i = 0
while (i < 100) {
	Arr[i] = System.Time
	i++
}
declare temp
i = 0
while (i < 100) {
	j = i
	while (j < 100) {
		if (Arr[i] < Arr[j]) {
			Arr[i] += Arr[j]
			Arr[j] = Arr[i] - Arr[j]
			Arr[i] -= Arr[j]
		}
		j++
	}
	i++
}
Console.Output( System.Time )