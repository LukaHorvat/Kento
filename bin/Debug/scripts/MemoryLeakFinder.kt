Update = function () {
	j = 0
	i = 0
	Mention(i, j)
	Console.Output( System.MemoryUsage )
}
Mention = function ("Var") {
	Console.Output(Var)
}
k = 0
while ( k < 1000 ) {
	Update()
	k++
}