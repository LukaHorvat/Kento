Geometry = class () {
	Circumference = function ("Circle") {
		return 2 * Circle.Radius * 3.1415
	}
}
Circle = class () {
	Radius = 0
}
geo = new Geometry
circle = new Circle
circle.Radius = 10
ConsoleOutput(geo.Circumference(circle))