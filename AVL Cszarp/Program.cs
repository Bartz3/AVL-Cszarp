using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    public static string path = "test.txt";
    public static string path2 = "test.txt";
    public static string mainPath = "plik.txt";
    
    static void Main(string[] args)
    {
        AVL drzewoAVL = new AVL();
        menu(drzewoAVL);

       //tree.Delete(13);
    }
    public static void menu(AVL tree)
    {
        int wybor=-1;
        while (wybor != 0)
        {
            Console.WriteLine("1.Dodaj abonenta");
            Console.WriteLine("2.Usuń abonenta");
            Console.WriteLine("3.Znajdź numer telefonu");
            Console.WriteLine("4.Wczytaj");
            Console.WriteLine("5.Zapisz");
            Console.WriteLine("6.Wyswietl");
            Console.WriteLine("0.Zakończ program");
            wybor = Convert.ToInt32(Console.ReadLine());
            switch (wybor) {
                case 0:break;

                case 1:
                    {
                        Console.WriteLine("Podaj nazwisko do dodania.");
                        string naz = Convert.ToString(Console.ReadLine());
                        string imie = Convert.ToString(Console.ReadLine());
                        string adres = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("");
                        Console.WriteLine("Podaj numer telefonu dla {0} {1} do dodania." ,naz,imie);
                        string tel1, tel2, tel3;
                        List<string> _tel = new List<string>();
                        tel1= Convert.ToString(Console.ReadLine());
                        tel2= Convert.ToString(Console.ReadLine());
                        tel3= Convert.ToString(Console.ReadLine());
                        _tel.Add(tel1);
                        _tel.Add(tel2);
                        _tel.Add(tel3);
                        tree.Add(naz,imie,adres,_tel);
                    }
                    break;
                case 2:
                    {
                        Console.WriteLine("Podaj nazwisko do usuniecia.");
                        string x = Convert.ToString(Console.ReadLine());
                        tree.Delete(x);
                    }
                    break;
                case 3:
                    {
                        Console.WriteLine("Podaj nazwisko,imie oraz adres osoby: ");
                        string na = Convert.ToString(Console.ReadLine());
                        string im = Convert.ToString(Console.ReadLine());
                        string ad = Convert.ToString(Console.ReadLine());
                        tree.Find(na,im,ad);
                    }break;
                case 4:
                    {
                        Console.WriteLine("Wczytanie z pliku {0}.",path);
                        string line,nazwisko,imie,adres,tel1,tel2,tel3;
                        List<string> telefony = new List<string>();
                        StreamReader sr = new StreamReader(path);
                        string[] pom;
                        List<string> _tel;
                        while ((line = sr.ReadLine()) != null)
                        {
                            _tel = new List<string>();
                            pom = line.Split(',');
                            nazwisko = pom[0];
                            imie = pom[1];
                            adres = pom[2];
                            tel1 = pom[3];
                            tel2 = pom[4];
                            tel3 = pom[5];
                           //if (tel2 == " ") tel2 = null;
                           //if (tel3 == " ") tel3 = null;
                            _tel.Add(tel1);
                            _tel.Add(tel2);
                            _tel.Add(tel3);
                            tree.Add(nazwisko,imie,adres,_tel);
                        }
                        sr.Close();
                    }break;
                case 5:
                    {
                        Console.WriteLine("Zapisywanie do pliku {0}.", path2);
                        StreamWriter sw = new StreamWriter(path2);
                        tree.SaveTree(path2,sw);
                        sw.Close();
                    }
                    break;
                case 6:
                    {
                        tree.DisplayTree();
                       
                    }
                    break;
            }
            Console.WriteLine();

        }
    }
}
public class AVL
{
    public class Node
    {
        public string nazwisko;
        public string imie;
        public string adres;
        public List<string>telefony;
        public Node left;
        public Node right;
      // public Node(string _nazwisko,string _imie,string _adres) // Konstruktor z przypisaniem wartości do drzewa
      // {
      //     this.nazwisko = _nazwisko;
      //     this.imie = _imie;
      //     this.adres = _adres;
      //
      // }
        public Node(string _nazwisko, string _imie, string _adres,List<string> _tel) // Konstruktor z przypisaniem wartości do drzewa
        {
            this.nazwisko = _nazwisko;
            this.imie = _imie;
            this.adres = _adres;
            this.telefony = new List<string>(_tel);

        }
        public Node() { }
    }
    Node root;
    public AVL() // Konstruktor bezparametrowy
    {
    }
    public void Add(string nazwisko,string imie,string adres, List<string> _tel)
    {
        Node newItem = new Node(nazwisko,imie,adres,_tel);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = Insert(root, newItem);
        }
    }
    private Node Insert(Node current, Node nI)
    {
        
        if (current == null)
        {
            current = nI;
            return current;
        }
        int compareNaz = String.Compare(nI.nazwisko, current.nazwisko); // Porównanie nazwisk
        
        if (compareNaz == - 1) // nI.data < current.data
        {
            current.left = Insert(current.left, nI);
            current = balance_tree(current);
        }
        else if (compareNaz == 1)
        {
            current.right = Insert(current.right, nI);
            current = balance_tree(current);
        }
        else if (compareNaz == 0)
        {
            int compareImie = String.Compare(nI.imie, current.imie); // Porównanie imion
           

            if(compareImie == -1)
            {
                current.left = Insert(current.left, nI);
                current = balance_tree(current);
            }
            else if(compareImie == 1)
            {
                current.right = Insert(current.right, nI);
                current = balance_tree(current);
            }
            else if (compareImie == 0)
            {
                int compareAd = String.Compare(nI.adres, current.adres);  // Porównanie adresów
                if (compareAd < 1) // nI.data < current.data
                {
                    current.left = Insert(current.left, nI);
                    current = balance_tree(current);
                }
                else if (compareAd >= 1)
                {
                    current.right = Insert(current.right, nI);
                    current = balance_tree(current);
                }
            }
        }
        return current;
    }
    private Node balance_tree(Node current)
    {
        int b_factor = balance_factor(current);
        if (b_factor > 1)
        {
            if (balance_factor(current.left) > 0)
            {
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (b_factor < -1)
        {
            if (balance_factor(current.right) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
        return current;
    }
    public void Delete(string target)
    {//and here
        root = Delete(root, target);
        
    }
    private Node Delete(Node current, string target)
    {
        Node parent;
        if (current == null)
        { Console.WriteLine("Nie mozna usunac elementu {0} - nie istnieje.",target); 
            return null; }
        else
        {    
            //left subtree
            if ((String.Compare(target, current.nazwisko)) ==-1)
            {
                current.left = Delete(current.left, target);

                if (balance_factor(current) == -2)//here
                {
                    if (balance_factor(current.right) <= 0)
                    {
                        current = RotateRR(current);
                    }
                    else
                    {
                        current = RotateRL(current);
                    }
                }
            }
            //right subtree
            else if ((String.Compare(target, current.nazwisko)) == 1)
            {
                current.right = Delete(current.right, target);
                if (balance_factor(current) == 2)
                {
                    if (balance_factor(current.left) >= 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current.right != null)
                {
                    //delete its inorder successor
                    parent = current.right;
                    while (parent.left != null)
                    {
                        parent = parent.left;
                    }
                    current.nazwisko = parent.nazwisko;
                    current.right = Delete(current.right, parent.nazwisko);
                    if (balance_factor(current) == 2)//rebalancing
                    {
                        if (balance_factor(current.left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else { 
                            current = RotateLR(current); 
                        }
                    }
                }
                else
                {   //if current.left != null
                    return current.left;
                }
            }
        }
        
        return current;
    }
    public Node Find(string _nazw,string _imie,string _adres)
    {
        if (Find(_nazw, root) == null) return null;
        if ((Find2(_nazw, _imie, root).nazwisko == _nazw))// || (Find3(_adres, root).adres == _adres)
        {
            Console.WriteLine("{0} {1} - {2} znajduje się w książce telefonicznej.\nOto numery telefonow: ", _nazw,_imie,_adres);
        }
        else
        {
            Console.WriteLine("Brak {0} {1} w ksiazce telefonicznej.", _nazw,_imie);
        }
        return null;
    }
    private Node Find(string _nazw, Node current)
    {
        if (current== null) return null;
        int compare = String.Compare(_nazw, current.nazwisko);


        if (compare<1) // target < current.nazwisko
        {
            if (_nazw == current.nazwisko)
            {
                return current;
            }
            else if(current.left!=null)
                return Find(_nazw, current.left);
        }
        else // target > current.nazwisko
        {
            if (_nazw == current.nazwisko)
            {
                return current;
            }
            else if(current.right != null)
                return Find(_nazw, current.right);
        }
        return current;
    }

    private Node Find2(string _nazw, string _imie, Node current)
    {
        if (current == null) return null;
        int compareNaz = String.Compare(_nazw, current.imie);


        if (compareNaz == 1) // target < current.nazwisko
        {
            if (_nazw == current.nazwisko)
            {
                return current;
            }
            else if (current.left != null)
                return Find2(_nazw, _imie, current.left);
        }
        else if(compareNaz == -1 )// target > current.nazwisko
        {
            if (_nazw == current.nazwisko)
            {
                return current;
            }
            else if (current.right != null)
                return Find2(_nazw, _imie, current.right);
        }
        else if(compareNaz == 0)
        {
            int compareImie = String.Compare(_imie, current.imie);

            if (compareImie < 1) // target < current.nazwisko
            {
                if (_imie == current.imie)
                {
                    return current;
                }
                else if (current.left != null)
                    return Find2(_nazw,_imie,current.left);
            }
            else
            {
                if (_imie == current.imie)
                {
                    return current;
                }
                else if (current.right != null)
                    return Find2(_nazw,_imie, current.right);
            }

        }
        return current;
    }
    private Node Find3(string _adres, Node current)
    {
        if (current == null) return null;
        int compare = String.Compare(_adres, current.adres);


        if (compare < 1) // target < current.nazwisko
        {
            if (_adres == current.adres)
            {
                return current;
            }
            else if (current.left != null)
                return Find3(_adres, current.left);
        }
        else // target > current.nazwisko
        {
            if (_adres == current.adres)
            {
                return current;
            }
            else if (current.right != null)
                return Find3(_adres, current.right);
        }
        return current;
    }
    public void DisplayTree()
    {
        if (root == null)
        {
            Console.WriteLine("Tree is empty");
            return;
        }
        InOrderDisplayTree(root);
        Console.WriteLine();
        
    }
    
    private void InOrderDisplayTree(Node current)
    {
        if (current != null)
        {
            InOrderDisplayTree(current.left);
            Console.WriteLine("{0} {1} -{2} : {3} | {4} | {5}",
                current.nazwisko,current.imie,current.adres,current.telefony[0], current.telefony[1], current.telefony[2]);
            InOrderDisplayTree(current.right);
        }
        
    }
    

    public void SaveTree(string path2, StreamWriter sw)
    {
        if (root == null)
        {
            Console.WriteLine("Tree is empty");
            return;
        }
        InOrderSaveTree(root,path2,sw);
    }
    private void InOrderSaveTree(Node current,string path2, StreamWriter sw)
    {
        
        if (current != null)
        {
            InOrderSaveTree(current.left,path2,sw);
            sw.WriteLine(current.nazwisko,current.imie,",");
            InOrderSaveTree(current.right,path2,sw);
        }
    }

    private int max(int l, int r) // Funkcja zwracająca większe spośród dwóch podanych wartości
    {
        return l > r ? l : r;
    }
    private int getHeight(Node current)
    {
        int height = 0;
        if (current != null)
        {
            int l = getHeight(current.left);
            int r = getHeight(current.right);
            int m = max(l, r);
            height = m + 1;
        }
        return height;
    }
    private int balance_factor(Node current)
    {
        int l = getHeight(current.left);
        int r = getHeight(current.right);
        int b_factor = l - r;
        return b_factor;
    }
    private Node RotateRR(Node parent)  
    {
        Node pivot = parent.right;
        parent.right = pivot.left;
        pivot.left = parent;
        return pivot;
    }
    private Node RotateLL(Node parent)
    {
        Node pivot = parent.left;
        parent.left = pivot.right;
        pivot.right = parent;
        return pivot;
    }
    private Node RotateLR(Node parent)
    {
        Node pivot = parent.left;
        parent.left = RotateRR(pivot);
        return RotateLL(parent);
    }
    private Node RotateRL(Node parent)
    {
        Node pivot = parent.right;
        parent.right = RotateLL(pivot);
        return RotateRR(parent);
    }
}              
