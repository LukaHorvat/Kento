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
	time += 0.02
	i = 0
	while (i < shapes.Count) {
		shapes[i].X = Math.Sin(time + i * 0.2 * Math.PI) * 200 + 400
		shapes[i].Y = Math.Cos(time + i * 0.2 * Math.PI) * 200 + 250
		shapes[i].Rotation = time + i * 0.2 * Math.PI
		i++
	}
	Console.Output( System.MemoryUsage )
}

time = 0
shapes = new Array()
Firefly.Initialize(OnLoad, OnUpdate)