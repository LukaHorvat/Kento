Animal = class () {
	Live = function () {
		Console.Output("The animal called " + Name + " lives")
	}
}
Fish = class Animal {
	Name = "Goldy" 
	Swim = function () {
		Console.Output("The fish swims")
	}
}
Bird = class Animal {
	Name = "Tweety"
	Fly = function () {
		Console.Output("The bird flies")
	}
}
bird = new Bird
fish = new Fish

bird.Live()
fish.Live()
bird.Fly()
fish.Swim()