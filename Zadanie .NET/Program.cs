using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadanie.NET.Models;


namespace Zadanie.NET
{
    internal class Program
    {
        //Główna klasa programu
        //Stworzenie obiektu biblioteka
        //Wyświetlenie menu
        //Wybór opcji
        //Wywołanie odpowiedniej metody
        //Program wykożystuje pętle do while do wyboru opcji
        //W zależności od wyboru użytkownika wywołuje odpowiednią metodę
        //W przypadku niepoprawnego wyboru wyświetla komunikat
        //Program kończy działanie po wybraniu opcji 5
        static void Main(string[] args)
        {
            Biblioteka biblioteka = new Biblioteka();
            string command;

            do
            {
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Dodaj książkę");
                Console.WriteLine("2. Usuń książkę");
                Console.WriteLine("3. Aktualizuj książkę");
                Console.WriteLine("4. Wyświetl książki");
                Console.WriteLine("5. Koniec programu");
                Console.Write("\nTwój wybór: ");
                command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        DodajKsiazke(biblioteka);
                        break;
                    case "2":
                        UsunKsiazke(biblioteka);
                        break;
                    case "3":
                        ZaktualizujKsiazke(biblioteka);
                        break;
                    case "4":
                        biblioteka.WyswitelKsiazki();
                        break;
                    case "5":
                        Console.WriteLine("Koniec programu.");
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                        break;
                }
            } while (command != "5");
        }

        //Metoda dodająca książkę do biblioteki
        //Pobiera od użytkownika dane książki
        //Sprawdza czy rok wydania książki jest liczbą całkowitą
        //Weryfikacja czy dane nie są puste następuje w metodzie DodajKsiazke w klasie Biblioteka
        //Tworzy nowy obiekt książki i dodaje go do listy książek
        static void DodajKsiazke(Biblioteka biblioteka)
        {
            Console.Write("Podaj tytuł: ");
            string tytul = Console.ReadLine();
            Console.Write("Podaj autora: ");
            string autor = Console.ReadLine();
            Console.Write("Podaj rok wydania: ");
            if (!int.TryParse(Console.ReadLine(), out int rokWydania))
            {
                Console.WriteLine("Rok wydania musi być liczbą całkowitą.");
                return;
            }
            Console.Write("Podaj ISBN: ");
            string isbn = Console.ReadLine();

            biblioteka.DodajKsiazke(tytul, autor, rokWydania, isbn);
        }

        //Metoda usuwająca książkę z biblioteki
        //Wyświetla listę książek
        //Pobiera od użytkownika ID książki do usunięcia
        //Sprawdza czy ID jest liczbą całkowitą
        //Jeśli książka o podanym ID istnieje to zostaje usunięta
        //W przeciwnym wypadku wyświetlony zostaje komunikat (zaimplementowane w metodzie UsunKsiazke w klasie Bibloteka)
        static void UsunKsiazke(Biblioteka biblioteka)
        {
            Console.WriteLine("Lista książek:\n");
            biblioteka.WyswitelKsiazki();

            Console.Write("\nPodaj ID książki do usunięcia: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID musi być liczbą całkowitą.");
                return;
            }
            biblioteka.UsunKsiazke(id);
        }


        //Metoda aktualizująca dane książki
        //Wyświetla listę książek
        //Pobiera od użytkownika ID książki do aktualizacji
        //Pobiera nowe dane książki
        //Weryfikuje czy rok wydania jest liczbą całkowitą
        //Jeśli książka o podanym ID istnieje to zostaje zaktualizowana
        //W przeciwnym wypadku wyświetlony zostaje komunikat (zaimplementowane w metodzie ZaktualizujKsiazke w klasie Bibloteka)
        static void ZaktualizujKsiazke(Biblioteka biblioteka)
        {
            Console.WriteLine("Lista książek:\n");
            biblioteka.WyswitelKsiazki();

            Console.Write("Podaj ID książki do aktualizacji: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID musi być liczbą całkowitą.");
                return;
            }

            Ksiazka bookToUpdate = biblioteka.PobierzDaneKsiazki(id);
            if (bookToUpdate == null)
            {
                Console.WriteLine("Nie znaleziono książki o ID: {0}", id);
                return;
            }

            Console.Write("Podaj nowy tytuł (pozostaw puste, aby nie zmieniać): ");
            string tytul = Console.ReadLine();
            Console.Write("Podaj nowego autora (pozostaw puste, aby nie zmieniać): ");
            string autor = Console.ReadLine();
            Console.Write("Podaj nowy rok wydania (pozostaw puste, aby nie zmieniać): ");
            string rokWydaniaStr = Console.ReadLine();
            int? rokWydania = null;
            if (!string.IsNullOrEmpty(rokWydaniaStr))
            {
                if (int.TryParse(rokWydaniaStr, out int rokWydaniaVal))
                {
                    rokWydania = rokWydaniaVal;
                }
                else
                {
                    Console.WriteLine("Rok wydania musi być liczbą całkowitą.");
                    return;
                }
            }
            Console.Write("Podaj nowy ISBN (pozostaw puste, aby nie zmieniać): ");
            string isbn = Console.ReadLine();

            biblioteka.ZaktualizujKsiazke(id, tytul, autor, rokWydania, isbn);
        }
    }


        //Klasa Biblioteka
        //Zawiera całą implementację operacji na książkach
        //Dodawanie, usuwanie, aktualizacja i wyświetlanie książek
        //Książki przechowywane są w liście ksiazki
        //Każda książka ma unikalne ID
        //Klasa Biblioteka jest odpowiedzialna za weryfikację danych
        //W przypadku niepoprawnych danych wyświetla odpowiednie komunikaty
        //Obiekt książki jest bazonwany na modelu Ksiazka
        public class Biblioteka
        {
            //Stworzenie listy książek oraz iteratora ID
            private List<Ksiazka> ksiazki = new List<Ksiazka>();
            private int nextId = 1;

            //Metoda dodająca książkę do listy
            //Sprawdza czy wszystkie pola są wypełnione
            //Tworzy nowy obiekt książki i dodaje go do listy
            //Wyświetla komunikat o dodaniu książki
            public void DodajKsiazke(string tytul, string autor, int rokWydania, string isbn)
            {
                if (string.IsNullOrEmpty(tytul) || string.IsNullOrEmpty(autor) || string.IsNullOrEmpty(isbn))
                {
                    Console.WriteLine("Wszystkie pola muszą być wypełnione.");
                    return;
                }

                Ksiazka newBook = new Ksiazka
                {
                    Id = nextId++,
                    Tytul = tytul,
                    Autor = autor,
                    RokWydania = rokWydania,
                    ISBN = isbn
                };
                ksiazki.Add(newBook);
                Console.WriteLine("Dodano książkę: {0}", newBook.Tytul);
            }

            //Metoda usuwająca książkę z listy
            //Sprawdza czy książka o podanym ID istnieje
            //Jeśli tak to usuwa książkę z listy
            //Wyświetla komunikat o usunięciu książki
            //W przeciwnym wypadku wyświetla komunikat o braku książki o podanym ID
            public void UsunKsiazke(int id)
            {
                Ksiazka bookToRemove = ksiazki.FirstOrDefault(b => b.Id == id);
                if (bookToRemove != null)
                {
                    ksiazki.Remove(bookToRemove);
                    Console.WriteLine("Usunięto książkę: {0}", bookToRemove.Tytul);
                }
                else
                {
                    Console.WriteLine("Nie znaleziono książki o ID: {0}", id);
                }
            }

            //Metoda aktualizująca dane książki
            //Sprawdza czy książka o podanym ID istnieje
            //Jeśli tak to aktualizuje dane książki
            //Wyświetla komunikat o zaktualizowaniu książki
            //W przeciwnym wypadku wyświetla komunikat o braku książki o podanym ID
            //W przypadku pustych danych nie aktualizuje ich
            public void ZaktualizujKsiazke(int id, string tytul, string autor, int? rokWydania, string isbn)
            {
            //Tworzy nowy obiekt książki o podanym ID
            //Aby móc później zaktualizować dane lub pozostawić orginalne
                Ksiazka bookToUpdate = ksiazki.FirstOrDefault(b => b.Id == id);
                if (bookToUpdate == null)
                {
                    Console.WriteLine("Nie znaleziono książki o ID: {0}", id);
                    return;
                }

                if (!string.IsNullOrEmpty(tytul))
                {
                    bookToUpdate.Tytul = tytul;
                }
                if (!string.IsNullOrEmpty(autor))
                {
                    bookToUpdate.Autor = autor;
                }
                if (rokWydania.HasValue)
                {
                    bookToUpdate.RokWydania = rokWydania.Value;
                }
                if (!string.IsNullOrEmpty(isbn))
                {
                    bookToUpdate.ISBN = isbn;
                }
                Console.WriteLine("Zaktualizowano książkę: {0}", bookToUpdate.Tytul);
            }

            //Metoda wyświetlająca książki
            //Jeśli lista książek jest pusta wyświetla komunikat
            //W przeciwnym wypadku wyświetla dane każdej książki
            public void WyswitelKsiazki()
            {
                if (ksiazki.Count == 0)
                {
                    Console.WriteLine("Brak książek w bibliotece.");
                }
                else
                {
                    foreach (var ksiazka in ksiazki)
                    {
                        Console.WriteLine("ID: {0}, Tytuł: {1}, Autor: {2}, Rok Wydania: {3}, ISBN: {4}",
                            ksiazka.Id, ksiazka.Tytul, ksiazka.Autor, ksiazka.RokWydania, ksiazka.ISBN);
                    }
                }
            }

            //Metoda pobierająca dane książki o podanym ID
            //Zwraca obiekt książki o podanym ID
            //Jeśli książka o podanym ID nie istnieje zwraca null
            public Ksiazka PobierzDaneKsiazki(int id)
            {
                return ksiazki.FirstOrDefault(b => b.Id == id);
            }
    }
    }

