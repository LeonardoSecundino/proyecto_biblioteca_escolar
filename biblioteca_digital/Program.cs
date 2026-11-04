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

            List<Titulo> catalogoCopia = new List<Titulo>
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

            // Mostrar catálogo inicial
            Console.WriteLine("CATÁLOGO INICIAL");
            foreach (var libro in catalogo)
            {
                Console.WriteLine($"{libro.Nombre} - Disponibles: {libro.CalcularDisponibilidad()}");
            }

            // Simular tres préstamos
            catalogo[0].RegistrarPrestamo();
            catalogo[1].RegistrarPrestamo();
            catalogo[4].RegistrarPrestamo();

            // Mostrar catálogo después de los préstamos
            Console.WriteLine("\nCATÁLOGO DESPUÉS DE PRÉSTAMOS");
            foreach (var libro in catalogo)
            {
                Console.WriteLine($"{libro.Nombre} - Disponibles: {libro.CalcularDisponibilidad()}");
            }

            // Ordenar catálogo principal por número de préstamos
            OrdenamientoInsercíon(catalogo);
            Console.WriteLine("\nCATÁLOGO ORDENADO POR PRÉSTAMOS (MAYOR A MENOR)");
            foreach (var libro in catalogo)
            {
                Console.WriteLine($"{libro.Nombre} - Préstamos: {libro.PrestamosHistoricos}");
            }

            foreach (var libro in catalogo)
            {
                if (libro.Ejemplares == null || libro.Ejemplares.Length == 0)
                    Console.WriteLine($"Error de estructura en: {libro.Nombre}");
            }

            // Integración de Búsquedas
            // Crear copia y ordenarla por ISBN para la búsqueda binaria
           
            OrdenarPorISBN(catalogoCopia);

            Console.WriteLine("\nRESULTADOS DE BÚSQUEDA");
            Titulo busqueda1 = BuscarPorISBN(catalogoCopia, "978-0156013987");
            Console.WriteLine(busqueda1 != null ? $"ISBN Encontrado: {busqueda1.Nombre}" : "ISBN no encontrado");

            Titulo busqueda2 = BuscarPorTitulo(catalogo, "1984");
            Console.WriteLine(busqueda2 != null ? $"Título Encontrado: {busqueda2.ISBN}" : "Título no encontrado");


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

        public static void OrdenarPorISBN(List<Titulo> catalogo)
        {
            for (int i = 1; i < catalogo.Count; i++)
            {
                Titulo actual = catalogo[i];
                int j = i - 1;

                // Se compara el string del ISBN alfabéticamente
                while (j >= 0 && string.Compare(catalogo[j].ISBN, actual.ISBN) > 0)
                {
                    catalogo[j + 1] = catalogo[j];
                    j--;
                }
                catalogo[j + 1] = actual;
            }
        }

        public static Titulo BuscarPorTitulo(List<Titulo> catalogo, string tituloBuscado)
        {
            for (int i = 0; i < catalogo.Count; i++)
            {
                if (catalogo[i].Nombre.ToLower() == tituloBuscado.ToLower())
                {
                    Console.WriteLine($"Libro {catalogo[i].Nombre} encontrado");
                    return catalogo[i]; 
                }
            }
            Console.WriteLine("Libro no encontrado");
            return null; 
        }

        public static Titulo BuscarPorISBN(List<Titulo> catalogo, string isbnBuscado)
        {
            int inicio = 0;
            int fin = catalogo.Count - 1;

            while (inicio <= fin)
            {
                // Cálculo del punto medio
                int medio = (inicio + fin) / 2;

                // CompareTo devuelve 0 si son iguales, -1 si es menor y 1 si es mayor
                int comparacion = catalogo[medio].ISBN.CompareTo(isbnBuscado);

                if (comparacion == 0)
                {
                    Console.WriteLine($"Coincidencia encontrada con el ISBN {isbnBuscado}");
                    return catalogo[medio];
                }
                else if (comparacion < 0)
                    inicio = medio + 1; // Buscar en la mitad derecha
                
                else
                    fin = medio - 1; // Buscar en la mitad izquierda
            }
            Console.WriteLine("No se encontró coincidencias");
            return null; 
        }
    }
}
