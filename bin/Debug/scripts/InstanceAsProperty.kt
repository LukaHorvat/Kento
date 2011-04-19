out = Console.Output

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
	Constructor = function ( "Aa" ) {
		A = Aa
	}
}

first = new Point( 5, 2 )
tri = new Triangle( &first )

Console.Output(tri.A.X)