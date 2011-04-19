OnLoad = function () {	
	i = 0
	while (i < 10) {
		shape = new ColoredShape()
		shape.FilledPolygons.Add( new Polygon( false, -50, -50, 1, 0, 0, 1,
												50, -50, 0, 1, 0, 1,
												50, 50, 0, 0, 1, 1,
												-50, 50, 0, 1, 1, 1 ) )
		shape.SetPolygons()
		shapes.Add(shape)
		i++
	}
}

OnUpdate = function () {
	i = 0
	j = 0
	while (i < 10) {
		j++
		i++
	}
	Console.Output( System.MemoryUsage )
}
time = 0
shapes = new Array()
Firefly.Initialize(OnLoad, OnUpdate)