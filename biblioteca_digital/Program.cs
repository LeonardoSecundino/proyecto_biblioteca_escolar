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
            List<Titulo> catalogoCopia = catalogo;
            OrdenarPorISBN(catalogoCopia);
            bool salir = false;
            do
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine("      SISTEMA DE BIBLIOTECA ESCOLAR v1.0");
                Console.WriteLine("=================================================");
                Console.WriteLine("1. Mostrar catálogo ordenado (Mayor a menor préstamos)");
                Console.WriteLine("2. Registrar un préstamo");
                Console.WriteLine("3. Buscar libro por Título");
                Console.WriteLine("4. Buscar libro por ISBN (Búsqueda Binaria)");
                Console.WriteLine("5. Ejecutar prueba de rendimiento Array.BinarySearch");
                Console.WriteLine("6. Salir");
                Console.WriteLine("=================================================");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        // Se aplica el ordenamiento manual por inserción
                        OrdenamientoInsercion(catalogo);
                        Console.WriteLine("--- CATÁLOGO ORDENADO POR PRÉSTAMOS ---");
                        foreach (var libro in catalogo)
                        {
                            Console.WriteLine($"- {libro.Nombre} | Préstamos históricos: {libro.PrestamosHistoricos} | Disponibles: {libro.CalcularDisponibilidad()}");
                        }
                        break;

                    case "2":
                        Console.WriteLine("--- REGISTRAR PRÉSTAMO ---");
                        for (int i = 0; i < catalogo.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {catalogo[i].Nombre} (Disponibles: {catalogo[i].CalcularDisponibilidad()})");
                        }
                        Console.Write("\nIngrese el número del libro que desea prestar: ");

                        if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= catalogo.Count)
                        {
                            Titulo libroSeleccionado = catalogo[indice - 1];
                            int disponiblesAntes = libroSeleccionado.CalcularDisponibilidad();
                            int prestamosAntes = libroSeleccionado.PrestamosHistoricos;

                            Console.WriteLine($"\nESTADO ANTERIOR -> Disponibles: {disponiblesAntes} | Préstamos históricos: {prestamosAntes}");

                            if (libroSeleccionado.RegistrarPrestamo())
                            {
                                Console.WriteLine("¡Préstamo registrado con éxito!");
                                Console.WriteLine($"ESTADO ACTUAL   -> Disponibles: {libroSeleccionado.CalcularDisponibilidad()} | Préstamos históricos: {libroSeleccionado.PrestamosHistoricos}");
                            }
                            else
                            {
                                Console.WriteLine("Préstamo denegado: No hay ejemplares disponibles en este momento.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opción inválida. Intente de nuevo.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("--- BÚSQUEDA POR TÍTULO ---");
                        Console.Write("Ingrese el título exacto a buscar: ");
                        string tituloBuscado = Console.ReadLine();
                        Titulo resultadoTitulo = BuscarPorTitulo(catalogo, tituloBuscado);

                        if (resultadoTitulo != null)
                        {
                            Console.WriteLine($"\n¡Libro encontrado!\nNombre: {resultadoTitulo.Nombre}\nISBN: {resultadoTitulo.ISBN}\nDisponibles: {resultadoTitulo.CalcularDisponibilidad()}");
                        }
                        else
                        {
                            Console.WriteLine("\nEl libro no se encuentra en el catálogo.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("--- BÚSQUEDA POR ISBN ---");
                        Console.Write("Ingrese el ISBN a buscar (Ej. 978-0156013987): ");
                        string isbnBuscado = Console.ReadLine();

                        Titulo resultadoISBN = BuscarPorISBN(catalogoCopia, isbnBuscado, out int comparaciones);

                        if (resultadoISBN != null)
                        {
                            Console.WriteLine($"\n¡ISBN encontrado en {comparaciones} intentos!\nNombre: {resultadoISBN.Nombre}\nPréstamos Históricos: {resultadoISBN.PrestamosHistoricos}");
                        }
                        else
                        {
                            Console.WriteLine($"\nISBN no encontrado. La búsqueda tomó {comparaciones} intentos para confirmarlo.");
                        }
                        break;

                    case "5":
                        Console.WriteLine("--- PRUEBA DE RENDIMIENTO (DÍA 7) ---");
                        string[] arregloISBN = new string[catalogoCopia.Count];
                        for (int i = 0; i < catalogoCopia.Count; i++) arregloISBN[i] = catalogoCopia[i].ISBN;

                        string[] isbnConsultas = { "978-0156013987", "978-0143034902", "978-0307474728", "978-0156023487", "978-0156013677" };

                        foreach (string consulta in isbnConsultas)
                        {
                            BuscarPorISBN(catalogoCopia, consulta, out int intentosMiBusqueda);
                            int indiceNativo = Array.BinarySearch(arregloISBN, consulta);

                            Console.WriteLine($"ISBN: {consulta,-15} | Mi Algoritmo: {intentosMiBusqueda} comp. | Array.BinarySearch: {(indiceNativo >= 0 ? "Encontrado" : "No Encontrado")}");
                        }
                        break;

                    case "6":
                        salir = true;
                        Console.WriteLine("Cerrando el sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
                    Console.ReadKey();
                }

            } while (!salir);
        }

        // Ordenamiento de acuerdo a cantidad de prestamos y nombres (A-Z)
        public static void OrdenamientoInsercion(List<Titulo> catalogo)
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
                    // Console.WriteLine($"Libro {catalogo[i].Nombre} encontrado");
                    return catalogo[i]; 
                }
            }
            // Console.WriteLine("Libro no encontrado");
            return null; 
        }

        public static Titulo BuscarPorISBN(List<Titulo> catalogo, string isbnBuscado, out int comparaciones)
        {
            int inicio = 0;
            int fin = catalogo.Count - 1;
            comparaciones = 0;

            while (inicio <= fin)
            {
                comparaciones++;
                // Cálculo del punto medio
                int medio = (inicio + fin) / 2;

                // CompareTo devuelve 0 si son iguales, -1 si es menor y 1 si es mayor
                int comparacion = catalogo[medio].ISBN.CompareTo(isbnBuscado);

                if (comparacion == 0)
                {
                    // Console.WriteLine($"Coincidencia encontrada con el ISBN {isbnBuscado}");
                    return catalogo[medio];
                }
                else if (comparacion < 0)
                    inicio = medio + 1; // Buscar en la mitad derecha
                
                else
                    fin = medio - 1; // Buscar en la mitad izquierda
            }
            // Console.WriteLine("No se encontró coincidencias");
            return null; 
        }
    }
}
