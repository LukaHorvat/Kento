Milkshake = class () {
	Bring = function ("Boys", "Yard") {
		Yard &= Yard
		i=0
		while (i<3) {
			Yard[i]=Boys[i]
			i++
		}
	}
}
Boys = "Boy1","Boy2","Boy3"
Yard = "", "", ""

myMilkshake = new Milkshake
myMilkshake.Bring(Boys, &Yard)
i=0
while (i<3) {
	ConsoleOutput(Yard[i] + " " + typeof Yard[i])
	i++
}