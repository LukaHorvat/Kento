Array = MakeArray(100)
i = 0
while (i < 100) {
	Array[i] = GetTime()
	++i
}
declare temp
i = 0
while (i < 100) {
	j = i
	while (j < 100) {
		if (Array[i] < Array[j]) {
			Array[i] += Array[j]
			Array[j] = Array[i] - Array[j]
			Array[i] -= Array[j]
		}
		++j
	}
	++i
}
ConsoleOutput(GetTime())