//Aliases 
out = Console.Output

OnLoad = function () {  
    shape = new ColoredShape()
	i = 0
	while ( i < Triangles.Count ) {
		tri = Triangles[i]
		shape.FilledPolygons.Add( new Polygon( false, tri.A.X, tri.A.Y, 1, 0, 0, 1,
												tri.B.X, tri.B.Y, 0, 1, 0, 1,
												tri.C.X, tri.C.Y, 0, 0, 1, 1,
												tri.A.X, tri.A.Y, 1, 0, 0, 1 ) )
		i++
	}
    shape.SetPolygons()
}
OnUpdate = function () {  
	i = 0
}
Point = class () {
	declare X
	declare Y
	Constructor = function ( "X", "Y" ) {
		X = X
		Y = Y
	}
}
Triangle = class () {
	declare A
	declare B
	declare C
	Constructor = function ( "A", "B", "C" ) {
		A = A
		B = B
		C = C
	}
}
GetMiddle = function ( "A", "B" ) {
	return new Point( (A.X + B.X) / 2, (A.Y + B.Y) / 2)
}
GetPoints = function ( "Tri" ) {
	toReturn = new Array()
	toReturn.Add( new Triangle( Tri.A, GetMiddle( Tri.A, Tri.B ), GetMiddle( Tri.A, Tri.C ) ) )
	toReturn.Add( new Triangle( GetMiddle( Tri.A, Tri.C ), GetMiddle( Tri.B, Tri.C ), Tri.C ) )
	toReturn.Add( new Triangle( GetMiddle( Tri.A, Tri.B ), Tri.B, GetMiddle( Tri.B, Tri.C ) ) )
	return toReturn
	
}
Sierpinski = function ( "Tri", "Depth" ) {
	triangles = new Array()
	triangles.Add( Tri )
	while ( Depth > 0 ) {
		i = 0
		limit = triangles.Count
		while ( i < limit ) {
			Tri = triangles.PopFirst()
			next = GetPoints( Tri )
			triangles.Add( next[0] )
			triangles.Add( next[1] )
			triangles.Add( next[2] )
			i++
		}
		Depth--
	}
	return triangles
}
Triangles = Sierpinski( new Triangle( new Point( 400, 0 ), new Point( 700, 400 ), new Point( 100, 400 ) ), 5 )

Firefly.Initialize( OnLoad, OnUpdate )