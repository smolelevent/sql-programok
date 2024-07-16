using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

struct Ember
{
    public string Nev;
    public string Cim;
    public string ApjaNeve;
    public string AnyjaNeve;
    public long TelefonSzam;
    public string Nem;
    public string Email;
    public string PolgariSzam;
}

class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        Start();
    }

    static void Back()
    {
        Start();
    }

    static void Start()
    {
        Menu();
    }

    static void Menu()
    {
        Console.Clear();
        Console.WriteLine("\t\t**********ÜDVÖZLÖM A TELEFONKÖNYVBEN*************");
        Console.WriteLine("\n\n\t\t\t MENÜ\t\t\n\n");
        Console.WriteLine("\t1.Új Hozzáadása \t2.Listázás \t3.Kilépés \n\t4.Módosítás \t5.Keresés \t6.Törlés ");
        char valasz = Console.ReadKey().KeyChar;

        switch (valasz)
        {
            case '1':
                AddRecord();
                break;
            case '2':
                ListRecord();
                break;
            case '3':
                Environment.Exit(0);
                break;
            case '4':
                ModifyRecord();
                break;
            case '5':
                SearchRecord();
                break;
            case '6':
                DeleteRecord();
                break;
            default:
                Console.Clear();
                Console.WriteLine("\nAdj egy számot 1-től 6-ig");
                Console.WriteLine("\n Nyomjon meg egy billenytyűt!");
                Console.ReadKey();
                Menu();
                break;
        }
    }

    static void AddRecord()
    {
        Console.Clear();
        Ember p = new Ember();

        Console.WriteLine("\n Adja meg a nevet: ");
        p.Nev = Console.ReadLine();
        Console.WriteLine("\nAdja meg a címet: ");
        p.Cim = Console.ReadLine();
        Console.WriteLine("\nAdja meg az apja nevét: ");
        p.ApjaNeve = Console.ReadLine();
        Console.WriteLine("\nAdja meg az anyja nevét: ");
        p.AnyjaNeve = Console.ReadLine();
        Console.WriteLine("\nAdja meg a telefonszámot: ");
        p.TelefonSzam = long.Parse(Console.ReadLine());
        Console.WriteLine("Adja meg a nemét: ");
        p.Nem = Console.ReadLine();
        Console.WriteLine("\nAdja meg az e-mail-ét: ");
        p.Email = Console.ReadLine();
        Console.WriteLine("\nAdja meg a polgári számát: ");
        p.PolgariSzam = Console.ReadLine();

        using (FileStream fs = new FileStream("project.dat", FileMode.Append))
        {
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(p.Nev);
            bw.Write(p.Cim);
            bw.Write(p.ApjaNeve);
            bw.Write(p.AnyjaNeve);
            bw.Write(p.TelefonSzam);
            bw.Write(p.Nem);
            bw.Write(p.Email);
            bw.Write(p.PolgariSzam);
        }

        Console.WriteLine("\nRecord elmentve.");
        Console.WriteLine("\n\nNyomjon meg egy billenytyűt!");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }

    static void ListRecord()
    {
        Ember p;
        Console.Clear();
        using (FileStream fs = new FileStream("project.dat", FileMode.Open))
        {
            BinaryReader br = new BinaryReader(fs);
            while (fs.Position < fs.Length)
            {
                p.Nev = br.ReadString();
                p.Cim = br.ReadString();
                p.ApjaNeve = br.ReadString();
                p.AnyjaNeve = br.ReadString();
                p.TelefonSzam = br.ReadInt64();
                p.Nem = br.ReadString();
                p.Email = br.ReadString();
                p.PolgariSzam = br.ReadString();

                Console.WriteLine($"\n\n\n A TE REKORDOD\n\n ");
                Console.WriteLine($"\nNev={p.Nev}\nCim={p.Cim}\nApja neve={p.ApjaNeve}\nAnyja neve={p.AnyjaNeve}\nTelefonszáma={p.TelefonSzam}\nNem={p.Nem}\nE-mail={p.Email}\nPolgári szám={p.PolgariSzam}");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.WriteLine("\n Nyomjon meg egy billenytyűt!");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }

    static void SearchRecord()
    {
        Ember p;
        Console.Clear();
        Console.WriteLine("\nA keresésre adja meg az illető nevét: \n");
        string Nev = Console.ReadLine();
        bool talal = false;

        using (FileStream fs = new FileStream("project.dat", FileMode.Open))
        {
            BinaryReader br = new BinaryReader(fs);
            while (fs.Position < fs.Length)
            {
                p.Nev = br.ReadString();
                p.Cim = br.ReadString();
                p.ApjaNeve = br.ReadString();
                p.AnyjaNeve = br.ReadString();
                p.TelefonSzam = br.ReadInt64();
                p.Nem = br.ReadString();
                p.Email = br.ReadString();
                p.PolgariSzam = br.ReadString();

                if (p.Nev.Equals(Nev, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\n\tRészletes információ róla: {Nev}");
                    Console.WriteLine($"\nNev:{p.Nev}\nCim:{p.Cim}\nApja neve:{p.ApjaNeve}\nAnyja neve:{p.AnyjaNeve}\nTelefonszám:{p.TelefonSzam}\nNem:{p.Nem}\nEmail:{p.Email}\nPolgári szám:{p.PolgariSzam}");
                    talal = true;
                    break;
                }
            }
        }

        if (!talal)
            Console.WriteLine("A fájl nem talált");

        Console.WriteLine("\n Nyomjon meg egy billenytyűt!");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }

    static void DeleteRecord()
    {
        Ember p;
        Console.Clear();
        Console.WriteLine("Adja meg a KONTAKTOKNAK a nevét");
        string Nev = Console.ReadLine();
        bool talal = false;

        using (FileStream fs = new FileStream("project.dat", FileMode.Open))
        using (FileStream tempFs = new FileStream("temp.dat", FileMode.Create))
        {
            BinaryReader br = new BinaryReader(fs);
            BinaryWriter bw = new BinaryWriter(tempFs);

            while (fs.Position < fs.Length)
            {
                p.Nev = br.ReadString();
                p.Cim = br.ReadString();
                p.ApjaNeve = br.ReadString();
                p.AnyjaNeve = br.ReadString();
                p.TelefonSzam = br.ReadInt64();
                p.Nem = br.ReadString();
                p.Email = br.ReadString();
                p.PolgariSzam = br.ReadString();

                if (!p.Nev.Equals(Nev, StringComparison.OrdinalIgnoreCase))
                {
                    bw.Write(p.Nev);
                    bw.Write(p.Cim);
                    bw.Write(p.ApjaNeve);
                    bw.Write(p.AnyjaNeve);
                    bw.Write(p.TelefonSzam);
                    bw.Write(p.Nem);
                    bw.Write(p.Email);
                    bw.Write(p.PolgariSzam);
                }
                else
                {
                    talal = true;
                }
            }
        }

        if (talal)
        {
            File.Delete("project.dat");
            File.Move("temp.dat", "project.dat");
            Console.WriteLine("A REKORD SIKERESEN KITÖRÖLVE.");
        }
        else
        {
            File.Delete("temp.dat");
            Console.WriteLine("EGY KONTAKT ADATA SEM LETT TÖRÖLVE.");
        }

        Console.WriteLine("\n Nyomjon meg egy billenytyűt!");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }

    static void ModifyRecord()
    {
        Ember p;
        Ember s = new Ember();
        bool found = false;

        Console.Clear();
        Console.WriteLine("\nAdja meg a kontakt nevét hogy MÓDOSÍTSA\n");
        string Nev = Console.ReadLine();

        using (FileStream fs = new FileStream("project.dat", FileMode.Open))
        {
            BinaryReader br = new BinaryReader(fs);
            List<Ember> persons = new List<Ember>();

            while (fs.Position < fs.Length)
            {
                p.Nev = br.ReadString();
                p.Cim = br.ReadString();
                p.ApjaNeve = br.ReadString();
                p.AnyjaNeve = br.ReadString();
                p.TelefonSzam = br.ReadInt64();
                p.Nem = br.ReadString();
                p.Email = br.ReadString();
                p.PolgariSzam = br.ReadString();

                if (p.Nev.Equals(Nev, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\n Adja meg a nevet: ");
                    s.Nev = Console.ReadLine();
                    Console.WriteLine("\nAdja meg a címet: ");
                    s.Cim = Console.ReadLine();
                    Console.WriteLine("\nAdja meg az apja nevét: ");
                    s.ApjaNeve = Console.ReadLine();
                    Console.WriteLine("\nAdja meg az anyja nevét: ");
                    s.AnyjaNeve = Console.ReadLine();
                    Console.WriteLine("\nAdja meg a telefonszámot: ");
                    s.TelefonSzam = long.Parse(Console.ReadLine());
                    Console.WriteLine("Adja meg a nemét: ");
                    s.Nem = Console.ReadLine();
                    Console.WriteLine("\nAdja meg az e-mail-ét: ");
                    s.Email = Console.ReadLine();
                    Console.WriteLine("\nAdja meg a polgári számát: ");
                    s.PolgariSzam = Console.ReadLine();

                    persons.Add(s);
                    found = true;
                }
                else
                {
                    persons.Add(p);
                }
            }

            if (found)
            {
                fs.SetLength(0);
                fs.Position = 0;
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (var person in persons)
                {
                    bw.Write(person.Nev);
                    bw.Write(person.Cim);
                    bw.Write(person.ApjaNeve);
                    bw.Write(person.AnyjaNeve);
                    bw.Write(person.TelefonSzam);
                    bw.Write(person.Nem);
                    bw.Write(person.Email);
                    bw.Write(person.PolgariSzam);
                }

                Console.WriteLine("\nAz adatod módosítva");
            }
            else
            {
                Console.WriteLine("\nAz adatot nem találtuk");
            }
        }

        Console.WriteLine("\n Nyomjon meg egy billenytyűt!");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }
}