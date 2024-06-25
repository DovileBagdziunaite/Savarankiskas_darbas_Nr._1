using System;
using System.Collections.Generic;
using System.Linq;

namespace Savarankiskas_darbas_Nr._1
{
    public class Program
    {
        static Dictionary<string, User> users = new Dictionary<string, User>();
        static User currentUser = null;
        static string askedQuestions;

        static void Main(string[] args)
        {
            while (true)
            {
                ShowLoginScreen();
                if (currentUser != null)
                {
                    ShowMenu();
                }
            }
        }

        static void ShowMenuLogo()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|###################################################################|");
            Console.WriteLine("|########################     PROTMUSIS     ########################|");
            Console.WriteLine("|###################################################################|");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("");
        }

        static void ShowAndPrintMenu()
        {
            Console.WriteLine("Sveikinu prisijungus prie protmusio zaidimo.");
            Console.WriteLine("Jei jau atvykote cia, nepraleiskite progos sudalyvauti zaidime ");
            Console.WriteLine("ir isbandyti savo jegas.");
            Console.WriteLine("");
        }

        static void ShowLoginScreen()
        {
            Console.WriteLine("Sveiki! Prasome prisijungti, noredami uzeiti i pagrindini puslapi.");

            string firstName;
            do
            {
                Console.Write("Iveskite savo varda: ");
                firstName = Console.ReadLine().ToUpper().Trim();
            } while (string.IsNullOrWhiteSpace(firstName) || ContainsInvalidCharacters(firstName));

            string lastName;
            do
            {
                Console.Write("Iveskite pavarde: ");
                lastName = Console.ReadLine().ToUpper().Trim();
            } while (string.IsNullOrWhiteSpace(lastName) || ContainsInvalidCharacters(lastName));

            string fullName = $"{firstName} {lastName}";
            if (users.ContainsKey(fullName))
            {
                currentUser = users[fullName];
                Console.WriteLine($"Sveiki sugrize, {fullName}!");
            }
            else
            {
                Console.Clear();
                ShowMenuLogo();
                Console.WriteLine("");
                currentUser = new User(fullName);
                users.Add(fullName, currentUser);
                Console.WriteLine("Sveiki!");
                Console.WriteLine($"Sekmingai prisijungete ir susikurete nauja paskyra, {fullName}.");
            }

            WaitForEnter();
        }

        static bool ContainsInvalidCharacters(string input)
        {
            char[] invalidChars = { ' ', '"', '\'', ',', '.', '-' };
            foreach (char c in invalidChars)
            {
                if (input.Contains(c))
                {
                    Console.WriteLine("Ivedete netinkama simboli. Bandykite dar karta.");
                    return true;
                }
            }
            return false;
        }

        static void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowMenuLogo();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
                Console.WriteLine("");
                ShowAndPrintMenu();
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Pasirinkite is galimu variantu, ka norite toliau daryti - spauskite: ");
                Console.WriteLine("1 - jei norite susipazinti su ZAIDIMO TAISYKLEMIS;");
                Console.WriteLine("2 - jei norite pamatyti DALYVIUS IR JU REZULTATUS;");
                Console.WriteLine("3 - jei norite PRADETI ZAIDIMA (Start game);");
                Console.WriteLine("4 - jei norite ATSIJUNTI;");
                Console.WriteLine("5 - jei norite ISEITI.");
                Console.WriteLine("");
                Console.Write("Jusu pasirinkimas yra: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowRules();
                        break;
                    case "2":
                        ShowResults();
                        break;
                    case "3":
                        StartGame();
                        break;
                    case "4":
                        Logout();
                        return;
                    case "5":
                        Console.Clear();
                        ShowMenuLogo();
                        Console.WriteLine("Iki kitu susitikimu PROTMUSYJE!");
                        Console.WriteLine("Tikimes neprailgo laikas su mumis ir patyreti idomiu nuotykiu.");
                        Console.WriteLine($"Lauksime sugriztant taves, {currentUser.Name}.");
                        Console.WriteLine("Pakviesk ir savo draugus.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neteisingas pasirinkimas. Bandykite dar karta.");
                        break;
                }
            }
        }

        static void CallButtonQ()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Paspauskite 'q', jei norite grizti i MENIU langa");
            if (Console.ReadLine() == "q")
            {
                ShowMenu();
            }
            Console.ResetColor();
            Console.WriteLine("");
        }

        static void WaitForEnter()
        {
            string input;
            do
            {
                Console.WriteLine("");
                Console.WriteLine("Spauskite 'enter' noredami testi...");
                input = Console.ReadLine().ToLower().Trim();
            } while (input != "enter");
        }

        static void ShowRules()
        {
            Console.Clear();
            ShowMenuLogo();
            Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Zaidimo taisykles:");
            Console.WriteLine("Sis protmusis jums leidzia pasirinkti iš klausimu kategoriju.");
            Console.WriteLine("Pasirinkus kategorija pradesite zaidima ir turesite pasirinkti iš 4 galimu variantu, kuris yra jusu klausimui teisingas atsakymas.");

            CallButtonQ();
            while (Console.ReadLine() != "q") { }
        }

        static bool IsValidCategoryChoice(string input)
        {
            return input == "1" || input == "2" || input == "3";
        }

        static bool IsValidAnswerChoice(string input, int numberOfOptions)
        {
            if (input.Length != 1) return false;
            char choice = input[0];
            return choice >= 'a' && choice < 'a' + numberOfOptions;
        }

        static void HandlePostGameOptions(List<Question> askedQuestions)
        {
            Console.WriteLine("Paspauskite 'q', jei norite grizti i meniu, arba paspauskite 'pp', jei norite pasitikrinti, ar nebuvo pasikartojanciu klausimu.");
            string userInput;
            do
            {
                userInput = Console.ReadLine().ToLower().Trim();
                if (userInput == "q")
                {
                    ShowMenu();
                    return;
                }
                else if (userInput == "pp")
                {
                    CheckForDuplicateQuestions(askedQuestions);
                    return;
                }
                else
                {
                    Console.WriteLine("Neteisinga ivestis. Paspauskite 'q' arba 'pp'.");
                }
            } while (true);
        }

        static void CheckForDuplicateQuestions(List<Question> askedQuestions)
        {
            var duplicateQuestions = askedQuestions.GroupBy(q => q.Text)
                                                   .Where(g => g.Count() > 1)
                                                   .Select(g => g.Key)
                                                   .ToList();

            if (duplicateQuestions.Any())
            {
                Console.WriteLine("Rasti pasikartojantys klausimai:");
                foreach (var question in duplicateQuestions)
                {
                    Console.WriteLine($"- {question}");
                }
            }
            else
            {
                Console.WriteLine("Pasikartojanciu klausimu nebuvo.");
            }

            Console.WriteLine("Paspauskite bet kuri klavisa, kad griztumete i meniu.");
            Console.ReadKey();
            ShowMenu();
        }
        static void StartGame()
        {
            Console.Clear();
            ShowMenuLogo();
            Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Pasirinkite kategorija is triju galimu:");
            Console.WriteLine("1. MATEMATIKA");
            Console.WriteLine("2. INFORMATIKA IR PROGRAMAVIMAS");
            Console.WriteLine("3. APIMA ABI ANKSCIAU MINETAS TEMAS");

            string categoryChoice;
            do
            {
                Console.Write("Pasirinkite kategorija (1, 2 arba 3) arba paspauskite 'q', jei norite grizti i MENIU langa: ");
                categoryChoice = Console.ReadLine().Trim();
                if (categoryChoice.ToLower() == "q")
                {
                    ShowMenu();
                    return;
                }
                if (!IsValidCategoryChoice(categoryChoice))
                {
                    Console.WriteLine("Neteisinga ivestis, bandykite dar karta: pasirinkite viena is galimu variantu");
                }
            } while (!IsValidCategoryChoice(categoryChoice));

            List<Question> questions = GetQuestionsForCategory(categoryChoice);

            int score = 0;
            int questionCount = 5;
            Random random = new Random();

            for (int i = 1; i <= questionCount; i++)
            {
                Question question = questions[random.Next(questions.Count)];
                questions.Remove(question);

                Console.Clear();
                ShowMenuLogo();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"<< {i}/{questionCount} klausimas >>");
                Console.WriteLine("");
                Console.WriteLine(question.Text);

                for (int j = 0; j < question.Options.Length; j++)
                {
                    Console.WriteLine($"{(char)('a' + j)}. {question.Options[j]}");
                }
                Console.WriteLine();
                string answer;
                do
                {
                    Console.WriteLine("Jusu pasirinkimas: ");
                    answer = Console.ReadLine().ToLower().Trim();
                    Console.WriteLine("");
                } while (!IsValidAnswerChoice(answer, question.Options.Length));

                if (answer.ToLower() == question.CorrectOption)
                {
                    score += question.Score;
                    Console.WriteLine("Teisingas atsakymas!");
                }
                else
                {
                    Console.WriteLine($"Neteisingas atsakymas! Teisingas atsakymas buvo {question.CorrectOption}");
                }

                Console.WriteLine($"Siuo metu turite {score} tasku.");
                WaitForEnter();
            }
            currentUser.Score = score;

            Console.Clear();
            ShowMenuLogo();
            Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"Zaidimas baigtas! Surinkote is viso: {score} tasku.");
            //CallButtonQ();
            //while (Console.ReadLine() != "q") { }
            HandlePostGameOptions(questions);
        }

        static void ShowResults()
        {
            Console.Clear();
            ShowMenuLogo();
            Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Zaidimo rezultatu ir dalyviu perziura:");

            Console.WriteLine("1. Perziureti dalyvius");
            Console.WriteLine("2. Perziureti rezultatus");
            Console.Write("Jusu pasirinkimas: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Clear();
                Console.WriteLine("Dalyviai:");
                foreach (var user in users.Values)
                {
                    Console.WriteLine(user.Name);
                }
            }
            else if (choice == "2")
            {
                Console.Clear();
                ShowMenuLogo();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.Name}");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Rezultatai:");
                var sortedUsers = new List<User>(users.Values);
                sortedUsers.Sort((u1, u2) => u2.Score.CompareTo(u1.Score));
                int rank = 1;
                foreach (var user in sortedUsers)
                {
                    string stars = rank <= 3 ? new string('*', rank) : "";
                    Console.WriteLine($"{rank}. {user.Name} - {user.Score} {stars}");
                    rank++;
                }
            }
            else
            {
                Console.WriteLine("Neteisingas pasirinkimas. Griztama i meniu.");
            }

            CallButtonQ();
            while (Console.ReadLine() != "q") { }
        }

        static List<Question> GetQuestionsForCategory(string categoryChoice)
        {
            List<Question> questions = new List<Question>();

            if (categoryChoice == "1")
            {
                questions.Add(new Question("Kas yra matematika?", new string[] { "žinių sistema, siejanti dydžio ir kitimo sąvokas", "žinių sistema, siejanti dydžio, erdvės, struktūros ir kitimo sąvokas", "žinių sistema, siejanti dydžio, erdvės ir kitimo sąvokas", "nė vienas iš aukščiau minėtų" }, "b", 10));
                questions.Add(new Question("Kiek bus 10 * 10 ?", new string[] { "100", "10", "20", "nė vienas iš aukščiau minėtų" }, "a", 10));
                questions.Add(new Question("Žodis „matematika“ kilęs iš? ", new string[] { "Italų kalbos ", "Slavų kalbos", "Graikų kalbos", "Armėnų kalbos" }, "c", 10));
                questions.Add(new Question("Kiek bus 4 / 2 ?", new string[] { "8", "2", "6", "0" }, "b", 10));
                questions.Add(new Question("Skritulio ploto formulė?", new string[] { "S = 2r2", "S = r * r", "S = π*r2", "S = r2" }, "c", 10));
                questions.Add(new Question("Kas yra aritmetika?", new string[] { "seniausia matematikos sritis, nagrinėjanti veiksmus su skaičiais bei jų savybes", "seniausia matematikos sritis, nagrinėjanti atimties veiksmus", "seniausia matematikos sritis, nagrinėjanti veiksmus su skaičiais", "nė vienas iš aukščiau minėtų" }, "a", 10));
                questions.Add(new Question("Kas yra sudėtis?", new string[] { "suma", "du skaičius (dėmenis) paverčia vienu – suma", "skirtumas", "dalmuo" }, "b", 10));
                questions.Add(new Question("Kiek bus 6*5?", new string[] { "11", "15", "30", "1" }, "c", 10));
                questions.Add(new Question("Kas yra daugyba?", new string[] { "skirtumas", "daugiklis padaugintas is daugiklio", "dauginys padaugintas is dauginio", "būlinė aritmetinė operacija" }, "d", 10));
                questions.Add(new Question("Kiek bus 8*9?", new string[] { "17", "72", "89", "98" }, "b", 10));
                questions.Add(new Question("Kiek bus 1 * 10 ?", new string[] { "100", "10", "11", "110" }, "b", 10));
                questions.Add(new Question("Kiek bus 7 + 8?", new string[] { "15", "78", "14", "16" }, "a", 10));
                questions.Add(new Question("Kas yra skaičiaus kvadratas?", new string[] { "skaičiaus sandauga iš jo paties", "menamojo skaičiaus kvadratas yra visada yra mažesnis už nulį", "nelyginių skaičių suma eilės tvarka sudaro tobulą lyginio arba nelyginio skaičiaus kvadratą", "antrojo laipsnio daugianarė lygtis" }, "a", 10));
                questions.Add(new Question("Kas yra kvadratinė šaknis?", new string[] { "skaičius, kurį padauginus iš savęs gaunamas x.;", "skaičiaus sandauga iš jo paties", "dauginys padaugintas is dauginio", "antrojo laipsnio daugianarė lygtis" }, "a", 10));
                questions.Add(new Question("15.\tKiek bus 2 * 2?", new string[] { "5", "4", "22", "12" }, "b", 10));
            }
            else if (categoryChoice == "2")
            {
                questions.Add(new Question("Kas yra informatika?", new string[] { "mokslas bei technikos šaka, panaudojant kompiuterius", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą bei jos saugojimą, panaudojant kompiuterius", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą bei jos saugojimą" }, "b", 10));
                questions.Add(new Question("Kas yra programų kūrimas?", new string[] { "mokslas", "mokslas bei technikos šaka", "sudėtingas procesas ir programavimas tėra nedidelė šio proceso dalis", "sudėtingas procesas" }, "c", 10));
                questions.Add(new Question("Kas buvo pirmoji programuotoja?", new string[] { "Ala Lovela", "Charle Babagge", "Clara Zetkin", "Ada Lovelace" }, "d", 10));
                questions.Add(new Question("Kas yra ‘int’?", new string[] { "duomenų tipas", "kintamasis", "double", "boolean" }, "a", 10));
                questions.Add(new Question("Kas yra ‘switch’?", new string[] { "duomenų tipas", "valdymo struktūra programavimo kalboje C#, leidžianti vykdyti skirtingas kodo dalis pagal kintamojo reikšmę", "kintamasis", "trumpinys nuo „integer“, kas reiškia sveikąjį skaičių. int tipas saugo sveikus skaičius" }, "b", 10));
                questions.Add(new Question("Kas yra ‘double'?", new string[] { "vienas iš bazinių duomenų tipų programavimo kalboje C#. Tai yra trumpinys nuo „integer“, kas reiškia sveikąjį skaičių", "vienas iš bazinių duomenų tipų programavimo kalboje C#. Tai yra trumpinys nuo 'integer'", "vienas iš duomenų tipų programavimo kalboje C#, naudojamas kintamiesiems, kuriems reikia saugoti dešimtainius skaičius", "valdymo struktūra programavimo kalboje C#, leidžianti vykdyti skirtingas kodo dalis pagal kintamojo reikšmę" }, "c", 10));
                questions.Add(new Question("Kas yra metodai?", new string[] { "blokai kintamųjų, kurie yra sukurti atlikti tam tikrą funkciją", "blokai skaiciu, kurie yra sukurti atlikti tam tikrą funkciją", "blokai listu, kurie yra sukurti atlikti tam tikrą funkciją", "blokai kodo, kurie yra sukurti atlikti tam tikrą funkciją" }, "d", 10));
                questions.Add(new Question("Kas yra ‘double'?", new string[] { "duomenų tipas;", "boolean", "integer;", "kintamasis" }, "a", 10));
                questions.Add(new Question("Kas yra ref raktažodis?", new string[] { "ref raktažodis C# kalboje naudojamas, kad perduoti kintamąjį kaip nuorodą į metodą, o ne kaip reikšmę", "ref raktinis žodis yra speciali C# kalbos ypatybė, naudojama parametrams, kuriem atiduos reikšmę įgautą metode", "skirtingai nuo out raktinio žodžio, ref parametras nereikalauja pradinės reikšmės prieš perduodant jį į funkciją", "funkcija, naudojanti ref parametrą, privalo priskirti reikšmę šiam parametrui prieš išeinant iš funkcijos" }, "a", 10));
                questions.Add(new Question("Kas yra List?", new string[] { "double", "kintamasis", "duomenų tipas", "boolean" }, "c", 10));
                questions.Add(new Question("Kam lygu HEX 7 dvejetainėje?", new string[] { "1110", "1011", "1101", "0111" }, "d", 10));
                questions.Add(new Question("Kam lygu HEX 8 dvejetainėje?", new string[] { "1000", "0001", "0010", "0100" }, "a", 10));
                questions.Add(new Question("Kam lygu HEX 10 dvejetainėje?", new string[] { "1001", "0011", "1010", "1100" }, "c", 10));
                questions.Add(new Question("Kokia funkciją atlieka String metodas Trim?", new string[] { "patikrina, ar stringe yra nurodytas simbolis arba tekstas ir grąžina rezultatą", "patikrina, ar du stringai yra lygūs ir grąžina rezultatą (true arba false)", "pašalina pradžioje ir pabaigoje esančius tarpus (arba nurodytus simbolius) iš stringo", "paverčia visus stringosimbolius mažosiomis raidėmis" }, "c", 10));
                questions.Add(new Question("Kokia funkciją atlieka String metodas Replace?", new string[] { "pašalina nurodytą simbolių arba teksto fragmentą iš stringo", "pakeičia visus pasitaikančius nurodytus simbolius arba tekstus stringe naujais simboliais arba tekstu", "paverčia visus stringo simbolius didžiosiomis raidėmis", "grąžina stringo simbolių skaičių" }, "b", 10));
            }
            else if (categoryChoice == "3")
            {
                questions.Add(new Question("Kas yra matematika?", new string[] { "žinių sistema, siejanti dydžio ir kitimo sąvokas", "žinių sistema, siejanti dydžio, erdvės, struktūros ir kitimo sąvokas", "žinių sistema, siejanti dydžio, erdvės ir kitimo sąvokas", "nė vienas iš aukščiau minėtų" }, "b", 10));
                questions.Add(new Question("Kas yra informatika?", new string[] { "mokslas bei technikos šaka, panaudojant kompiuterius", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą bei jos saugojimą, panaudojant kompiuterius", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą", "mokslas bei technikos šaka, kuri nagrinėja informacijos apdorojimą bei jos saugojimą" }, "b", 10));
                questions.Add(new Question("Žodis „matematika“ kilęs iš? ", new string[] { "Italų kalbos ", "Slavų kalbos", "Graikų kalbos", "Armėnų kalbos" }, "c", 10));
                questions.Add(new Question("Kas buvo pirmoji programuotoja?", new string[] { "Ala Lovela", "Charle Babagge", "Clara Zetkin", "Ada Lovelace" }, "d", 10));
                questions.Add(new Question("Kiek bus 4 / 2 ?", new string[] { "8", "2", "6", "0" }, "b", 10));
                questions.Add(new Question("Kas yra ‘int’?", new string[] { "duomenų tipas", "kintamasis", "double", "boolean" }, "a", 10));
                questions.Add(new Question("Kiek bus 6*5?", new string[] { "11", "15", "30", "1" }, "c", 10));
                questions.Add(new Question("Kiek bus 8*9?", new string[] { "17", "72", "89", "98" }, "b", 10));
                questions.Add(new Question("Kas yra metodai?", new string[] { "blokai kintamųjų, kurie yra sukurti atlikti tam tikrą funkciją", "blokai skaiciu, kurie yra sukurti atlikti tam tikrą funkciją", "blokai listu, kurie yra sukurti atlikti tam tikrą funkciją", "blokai kodo, kurie yra sukurti atlikti tam tikrą funkciją" }, "d", 10));
                questions.Add(new Question("Kas yra List?", new string[] { "double", "kintamasis", "duomenų tipas", "boolean" }, "c", 10));
                questions.Add(new Question("Kas yra skaičiaus kvadratas?", new string[] { "skaičiaus sandauga iš jo paties", "menamojo skaičiaus kvadratas yra visada yra mažesnis už nulį", "nelyginių skaičių suma eilės tvarka sudaro tobulą lyginio arba nelyginio skaičiaus kvadratą", "antrojo laipsnio daugianarė lygtis" }, "a", 10));
                questions.Add(new Question("Kam lygu HEX 8 dvejetainėje?", new string[] { "1000", "0001", "0010", "0100" }, "a", 10));
                questions.Add(new Question("Kam lygu HEX 10 dvejetainėje?", new string[] { "1001", "0011", "1010", "1100" }, "c", 10));
                questions.Add(new Question("Kokia funkciją atlieka String metodas Trim?", new string[] { "patikrina, ar stringe yra nurodytas simbolis arba tekstas ir grąžina rezultatą", "patikrina, ar du stringai yra lygūs ir grąžina rezultatą (true arba false)", "pašalina pradžioje ir pabaigoje esančius tarpus (arba nurodytus simbolius) iš stringo", "paverčia visus stringosimbolius mažosiomis raidėmis" }, "c", 10));
                questions.Add(new Question("Kokia funkciją atlieka String metodas Replace?", new string[] { "pašalina nurodytą simbolių arba teksto fragmentą iš stringo", "pakeičia visus pasitaikančius nurodytus simbolius arba tekstus stringe naujais simboliais arba tekstu", "paverčia visus stringo simbolius didžiosiomis raidėmis", "grąžina stringo simbolių skaičių" }, "b", 10));
            }

            return questions;
        }

        static void Logout()
        {
            Console.Clear();
            ShowMenuLogo();
            Console.WriteLine("");
            Console.WriteLine("");
            currentUser = null;
            Console.WriteLine("Iki kitu susitikimu PROTMUSYJE!");
            Console.WriteLine("Tikimes neprailgo laikas su mumis ir patyreti idomiu nuotykiu.");
            Console.WriteLine("Lauksime sugriztant taves, mielas dalyvi.");
            Console.WriteLine("Pakviesk ir savo draugus.");
            Console.WriteLine("");
            WaitForEnter();
            Console.Clear();
            ShowLoginScreen();
            ShowMenu();
        }
    }

    class User
    {
        public string Name { get; }
        public int Score { get; set; }

        public User(string name)
        {
            Name = name;
            Score = 0;
        }
    }

    class Question
    {
        public string Text { get; }
        public string[] Options { get; }
        public string CorrectOption { get; }
        public int Score { get; }

        public Question(string text, string[] options, string correctOption, int score)
        {
            Text = text;
            Options = options;
            CorrectOption = correctOption;
            Score = score;
        }
    }
}
