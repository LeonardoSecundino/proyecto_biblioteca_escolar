using System.Security.Cryptography.X509Certificates;

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
                new Titulo("Ficciones", "978-0307950925", 20, 4),
                new Titulo("Rayuela", "978-8420471832", 7, 2),
                new Titulo("Pedro Páramo", "978-0802133908", 10, 3),
                new Titulo("La Sombra del Viento", "978-0143034902", 2, 3)
            };

            OrdenamientoInsercíon(catalogo);
            foreach (var libro in catalogo)
            {
                Console.WriteLine(libro.Nombre);
            }
        
            foreach (var libro in catalogo)
            {
                if (libro.Ejemplares == null || libro.Ejemplares.Length == 0)
                    Console.WriteLine($"Error de estructura en: {libro.Nombre}");
            }
        }

        // Ordenamiento de acuerdo a cantidad de prestamos y nombres (A-Z)
        public static void OrdenamientoInsercíon(List<Titulo> catalogo)
        {
            for (int i = 1; i < catalogo.Count; i++)
            {
                Titulo actual = catalogo[i];
                int j = i - 1;

                while (j >= 0 && (catalogo[j].PrestamosHistoricos < actual.PrestamosHistoricos || (catalogo[j].PrestamosHistoricos == actual.PrestamosHistoricos && string.Compare(catalogo[j].Nombre, actual.Nombre) > 0)))
                {
                    catalogo[j + 1] = catalogo[j];
                    j--;
                }
                catalogo[j + 1] = actual;
            }
        }
    }
}
