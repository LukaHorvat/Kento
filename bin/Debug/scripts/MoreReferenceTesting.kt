Random = class () {
	declare Prop
}
first = new Random
first.Prop = 5

second = first
second.Prop = 10
Console.Output("Normal assignment")
Console.Output(first.Prop)
Console.Output(second.Prop)

second = &first
second.Prop = 10
Console.Output("Referential assignment")
Console.Output(first.Prop)
Console.Output(second.Prop)