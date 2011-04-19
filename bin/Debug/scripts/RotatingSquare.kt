OnLoad = function () {  
    shape = new ColoredShape()
    shape.FilledPolygons.Add( new Polygon( false, 0, 0, 1, 0, 0, 1,
                                            100, 0, 0, 1, 0, 1,
                                            100, 100, 0, 0, 1, 1,
                                            0, 100, 0, 1, 1, 1 ) )
    shape.SetPolygons()
}
OnUpdate = function () {
    time += 0.03
	Console.Output( "Before " + System.MemoryUsage )
    shape.X = (Math.Sin( time ) + 1) * 350
    shape.Y = (Math.Cos( time ) + 1) * 200
	Console.Output( "After " + System.MemoryUsage )
}
 
time = 0
declare shape
Firefly.Initialize(OnLoad, OnUpdate)