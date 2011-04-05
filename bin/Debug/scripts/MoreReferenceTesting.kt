Random = class () {
	declare Prop
}
first = new Random
first.Prop = 5

second = first
second.Prop = 10
ConsoleOutput("Normal assignment")
ConsoleOutput(first.Prop)
ConsoleOutput(second.Prop)

second = &first
second.Prop = 10
ConsoleOutput("Referential assignment")
ConsoleOutput(first.Prop)
ConsoleOutput(second.Prop)