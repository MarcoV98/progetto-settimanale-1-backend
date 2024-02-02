using System;
using System.Globalization;

namespace AppFiscale
{   
    class Program
    {
        static void Main(string[] args)
        {
            bool datiCorretti = false;

            while (!datiCorretti)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine("INSERISCI LE TUE INFORMAZIONI:");
                Console.WriteLine("==================================================");

                string nome;
                do
                {
                    Console.Write("NOME: ");
                    nome = Console.ReadLine();
                    if (nome.Length <= 2 || !nome.All(char.IsLetter))
                    {
                        Console.WriteLine("Il nome deve essere superiore a 2 lettere. Riprova.");
                    }
                } while (nome.Length <= 2 || !nome.All(char.IsLetter));

                nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nome.ToLower());

                string cognome;
                do
                {
                    Console.Write("COGNOME: ");
                    cognome = Console.ReadLine();
                    if (cognome.Length <= 2 || !cognome.All(char.IsLetter))
                    {
                        Console.WriteLine("Il cognome deve essere superiore a 2 lettere. Riprova.");
                    }
                } while (cognome.Length <= 2 || !cognome.All(char.IsLetter));

                cognome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cognome.ToLower());

                string codiceFiscale;
                do
                {
                    Console.Write("CODICE FISCALE: ");
                    codiceFiscale = Console.ReadLine().ToUpper();
                    if (codiceFiscale.Length != 16 || !codiceFiscale.All(char.IsLetterOrDigit))
                    {
                        Console.WriteLine("Il codice fiscale deve essere composto da 16 caratteri alfanumerici. Riprova.");
                    }
                } while (codiceFiscale.Length != 16 || !codiceFiscale.All(char.IsLetterOrDigit));

                DateTime dataNascita;
                string[] formatiData = { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy" };
                do
                {
                    Console.Write("DATA DI NASCITA (formato: dd/MM/yyyy): ");
                    string inputDataNascita = Console.ReadLine();

                    if (!DateTime.TryParseExact(inputDataNascita, formatiData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascita) || dataNascita.Year < 1900)
                    {
                        Console.WriteLine("Data non valida o antecedente al 1900. Riprova.");
                    }
                } while (dataNascita == DateTime.MinValue || dataNascita.Year < 1900);

                char sesso;
                do
                {
                    Console.Write("SESSO (M/F/A): ");
                    string inputSesso = Console.ReadLine().ToUpper();
                    if (!char.TryParse(inputSesso, out sesso) || (sesso != 'M' && sesso != 'F' && sesso != 'A'))
                    {
                        Console.WriteLine("Sesso non valido. Riprova.");
                    }
                } while (sesso != 'M' && sesso != 'F' && sesso != 'A');

                string comuneResidenza;
                do
                {
                    Console.Write("COMUNE DI RESIDENZA: ");
                    comuneResidenza = Console.ReadLine();
                    if (comuneResidenza.Length <= 2 || !comuneResidenza.All(char.IsLetter))
                    {
                        Console.WriteLine("Il comune di residenza deve essere composto da almeno 3 lettere. Riprova.");
                    }
                } while (comuneResidenza.Length <= 2 || !comuneResidenza.All(char.IsLetter));

                comuneResidenza = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comuneResidenza.ToLower());

                decimal redditoAnnuale;
                do
                {
                    Console.Write("REDDITO ANNUALE: ");
                    string inputReddito = Console.ReadLine();

                    if (!decimal.TryParse(inputReddito, out redditoAnnuale) || redditoAnnuale < 0)
                    {
                        Console.WriteLine("Reddito non valido. Inserisci nuovamente.");
                    }
                } while (redditoAnnuale < 0);

                // Riepilogo preliminare
                Console.WriteLine("==================================================");
                Console.WriteLine("RIEPILOGO DELLE INFORMAZIONI INSERITE:");
                Console.WriteLine($"Nome: {nome}");
                Console.WriteLine($"Cognome: {cognome}");
                Console.WriteLine($"Data di nascita: {dataNascita.ToShortDateString()}");
                Console.WriteLine($"Codice fiscale: {codiceFiscale}");
                Console.WriteLine($"Sesso: {sesso}");
                Console.WriteLine($"Comune di residenza: {comuneResidenza}");
                Console.WriteLine($"Reddito annuale: {redditoAnnuale.ToString("N2")} euro");
                Console.WriteLine("==================================================");

                // Conferma dei dati
                Console.Write("I dati inseriti sono corretti? (Sì/No): ");
                string confermaDati = Console.ReadLine().Trim().ToUpper();

                while (confermaDati != "SI" && confermaDati != "SÌ" && confermaDati != "NO" && confermaDati != "N")
                {
                    Console.Write("Risposta non valida. Inserisci 'Si' o 'No': ");
                    confermaDati = Console.ReadLine().Trim().ToUpper();
                }

                if (confermaDati == "SI" || confermaDati == "SÌ")
                {
                    datiCorretti = true;

                    // Creazione dell'oggetto Contribuente e calcolo dell'imposta
                    Contribuente contribuente = new Contribuente(nome, cognome, dataNascita, codiceFiscale, sesso, comuneResidenza, redditoAnnuale);
                    decimal imposta = contribuente.CalcolaImposta();

                    // Riepilogo finale
                    Console.WriteLine("==================================================");
                    Console.WriteLine("CALCOLO DELL'IMPOSTA DA VERSARE:");
                    Console.WriteLine($"Contribuente: {contribuente.Nome} {contribuente.Cognome},");
                    Console.WriteLine($"nato il {contribuente.DataNascita.ToShortDateString()} ({contribuente.Sesso}),");
                    Console.WriteLine($"residente in {contribuente.ComuneResidenza},");
                    Console.WriteLine($"codice fiscale: {contribuente.CodiceFiscale}");
                    Console.WriteLine($"Reddito dichiarato: {contribuente.RedditoAnnuale.ToString("N2")} euro");
                    Console.WriteLine($"IMPOSTA DA VERSARE: {imposta.ToString("N2")} euro");
                    Console.WriteLine("==================================================");
                }
                else
                {
                    Console.WriteLine("Operazione annullata. Riprova inserendo i dati corretti.");
                }
            }
        }
    }


    public class Contribuente
        {
            private string _nome;
            public string Nome
            {
                get { return _nome; }
                set
                {
                    try
                    {
                        if (value != null && value.Length > 2)
                            _nome = value;
                        else
                            throw new ArgumentException("La lunghezza del nome non può essere inferiore a 2 lettere.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo Nome: {ex.Message}");
                    }
                }
            }

            private string _cognome;
            public string Cognome
            {
                get { return _cognome; }
                set
                {
                    try
                    {
                        if (value != null && value.Length > 2)
                            _cognome = value;
                        else
                            throw new ArgumentException("La lunghezza del cognome non può essere inferiore a 2 lettere.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo Cognome: {ex.Message}");
                    }
                }
            }

            private DateTime _dataNascita;
            public DateTime DataNascita
            {
                get { return _dataNascita; }
                set
                {
                    try
                    {
                        if (value <= DateTime.Now)
                            _dataNascita = value;
                        else
                            throw new ArgumentException("La data di nascita non può essere nel futuro.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo DataNascita: {ex.Message}");
                    }
                }
            }

            private string _codiceFiscale;
            public string CodiceFiscale
            {
                get { return _codiceFiscale; }
                set
                {
                    try
                    {
                        if (value != null && value.Length == 16 && value.All(char.IsLetterOrDigit))
                            _codiceFiscale = value;
                        else
                            throw new ArgumentException("Il codice fiscale inserito non è valido.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo CodiceFiscale: {ex.Message}");
                    }
                }
            }

            private char _sesso;
            public char Sesso
            {
                get { return _sesso; }
                set
                {
                    try
                    {
                        if (value == 'M' || value == 'F' || value == 'A')
                            _sesso = value;
                        else
                            throw new ArgumentException("Il sesso inserito deve essere 'M' per maschio, 'F' per femmina o 'A' per altro.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo Sesso: {ex.Message}");
                    }
                }
            }

            private string _comuneResidenza;
            public string ComuneResidenza
            {
                get { return _comuneResidenza; }
                set
                {
                    try
                    {
                        if (value != null && value.Length > 2)
                            _comuneResidenza = value;
                        else
                            throw new ArgumentException("La lunghezza del comune di residenza non può essere inferiore a 2 lettere.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo ComuneResidenza: {ex.Message}");
                    }
                }
            }

            private decimal _redditoAnnuale;
            public decimal RedditoAnnuale
            {
                get { return _redditoAnnuale; }
                set
                {
                    try
                    {
                        if (value >= 0)
                            _redditoAnnuale = value;
                        else
                            throw new ArgumentException("Il reddito annuale deve essere maggiore o uguale a 0.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Errore nel campo RedditoAnnuale: {ex.Message}");
                    }
                }
            }

            // Costruttore
            public Contribuente(string nome, string cognome, DateTime dataNascita, string codiceFiscale, char sesso, string comuneResidenza, decimal redditoAnnuale)
            {
                Nome = nome;
                Cognome = cognome;
                DataNascita = dataNascita;
                CodiceFiscale = codiceFiscale;
                Sesso = sesso;
                ComuneResidenza = comuneResidenza;
                RedditoAnnuale = redditoAnnuale;
            }

        // Costanti
        private const decimal Scaglione1 = 15000;
        private const decimal Scaglione2 = 28000;
        private const decimal Scaglione3 = 55000;
        private const decimal Scaglione4 = 75000;


        // Metodo di calcolo dell'imposta
        public decimal CalcolaImposta()
            {
                decimal imposta = 0;
                decimal reddito = RedditoAnnuale;

                if (reddito <= Scaglione1)
                {
                    imposta = reddito * 0.23m;
                }
                else if (reddito <= Scaglione2)
                {
                    imposta = 3450 + (reddito - Scaglione1) * 0.27m;
                }
                else if (reddito <= Scaglione3)
                {
                    imposta = 6960 + (reddito - Scaglione2) * 0.38m;
                }
                else if (reddito <= Scaglione4)
                {
                    imposta = 17220 + (reddito - Scaglione3) * 0.41m;
                }
                else
                {
                    imposta = 25420 + (reddito - Scaglione4) * 0.43m;
                }

                return imposta;
            }
        }
    }