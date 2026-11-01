namespace biblioteca_digital
{
    public class Titulo
    {
        public string Nombre { get; set; }
        public string ISBN { get; set; }
        public int PrestamosHistoricos { get; set; }
        public bool[] Ejemplares { get; set; } // true = disponible, false = prestado

        public Titulo(string nombre, string isbn, int prestamos, int ejemplares)
        {
            Nombre = nombre;
            ISBN = isbn;
            PrestamosHistoricos = prestamos;
            Ejemplares = new bool[ejemplares];
            for (int i = 0; i < ejemplares; i++) 
                Ejemplares[i] = true;
        }

        public int CalcularDisponibilidad()
        {
            int disponibles = 0;
            foreach (bool estado in Ejemplares)
                if (estado) disponibles++;
          
            return disponibles;
        }

        public bool RegistrarPrestamo()
        {
            for (int i = 0; i < Ejemplares.Length; i++)
            {
                if (Ejemplares[i])
                {
                    Ejemplares[i] = false; // Marca el ejemplar como prestado
                    PrestamosHistoricos++;
                    return true;
                }
            }
            return false; // No hay ejemplares disponibles
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Titulo> catalogo = new List<Titulo> 
            {
                new Titulo("Cien Años de Soledad", "978-0307474728", 15, 5),
                new Titulo("Don Quijote", "978-8424922498", 8, 2),
                new Titulo("El Principito", "978-0156013987", 20, 4),
                new Titulo("1984", "978-0451524935", 5, 3),
                new Titulo("Ficciones", "978-0307950925", 12, 4),
                new Titulo("Rayuela", "978-8420471832", 7, 2),
                new Titulo("Pedro Páramo", "978-0802133908", 10, 0),
                new Titulo("La Sombra del Viento", "978-0143034902", 2, 3)
            };

            // Prueba de tres prestamos
            catalogo[0].RegistrarPrestamo();
            catalogo[1].RegistrarPrestamo();
            catalogo[1].RegistrarPrestamo();
            catalogo[2].RegistrarPrestamo();
            catalogo[2].RegistrarPrestamo();
            catalogo[2].RegistrarPrestamo();
            Console.WriteLine(catalogo[0].PrestamosHistoricos + ", " + catalogo[1].PrestamosHistoricos + ", " + catalogo[2].PrestamosHistoricos); 

            // Verificación de que ningún título queda con celdas vacías
            foreach (var libro in catalogo)
            {
                if (libro.Ejemplares == null || libro.Ejemplares.Length == 0)
                    Console.WriteLine($"Error de estructura en: {libro.Nombre}");
            }
        }
    }
}
