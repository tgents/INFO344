class Main {
	static void Main(String[] args) {
		eBayProduct p = new eBayProduct();
		Console.WriteLine(p.Name + ": " + p.Price);
		p.SetName("Macbook Pro Retina");
		p.Price = 10.00;
		p.Name = "doesn't work";
		Console.WriteLine(p.Name + ": " + p.Price);
		Console.Read();
	}
}