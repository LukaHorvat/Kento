Geometry = class () {
	static Circumference = function ("Circle") {
		return 2 * Circle.Radius * Math.PI
	}
}
Circle = class () {
	declare Radius
	Constructor = function ("R") {
		Radius = R
	}
}
circle = new Circle(10)
Console.Output( Geometry.Circumference( circle ) )