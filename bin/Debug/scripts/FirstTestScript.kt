Animal = class () {
	Live = function () {
		ConsoleOutput("The animal called " + Name + " lives")
	}
}
Fish = class Animal {
	Name = "Goldy" 
	Swim = function () {
		ConsoleOutput("The fish swims")
	}
}
Bird = class Animal {
	Name = "Tweety"
	Fly = function () {
		ConsoleOutput("The bird flies")
	}
}
bird = new Bird
fish = new Fish

bird.Live()
fish.Live()
bird.Fly()
fish.Swim()