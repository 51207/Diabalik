using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiaBalik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //**** pour le jeu: ****

        public int n = 7;
        //definition tableau à deux dimensions
        PictureBox[,] PictureBoxes;
        PictureBox[,] pions1;
        PictureBox[,] pions2;
        bool[,] caseoccupe;
        public int sizecircle= 60;
        public bool ballpassing = false;
        //ces deux booleen permettent de faire une passe seulement lorque q'un des joueur joue
        public bool ballpassingOneforennemi = false;
        public bool ballpassingOneforplayer = true;

        //permet de savoir s'il existe un pion ennemi ou joueur dans le mouvement diagonale
        //si c'est le cas, le mouvement diagonal est interdit
        public bool blocagepionennemi = false;
        public bool blocagepion_ennemi_joueur_VH = false;

        //le compteur permet de gerer le tour de jeu des joueurs . Chaque joueur peut effectuer 2 tours ensuite faire une passe .

        public int compteurdetours = 0;
        public int compteurde2deplacementparjoueur = 0;
        public int compteurde2deplacementparennemi = 0;


        List<int> Stockvalue = new List<int>();
        List<int> stocvalueEnd = new List<int>();
        List<PictureBox> deselectedcolorPlayer = new List<PictureBox>();
        List<PictureBox> deselectedcolorEnnemi = new List<PictureBox>();
        //List<PictureBox> deselectedcolorSolution = new List<PictureBox>();
        List<int> deselectedcolorSolution = new List<int>();
        List<int >deselectedcolorPionPrincipale = new List<int>();
       
        
        List<int> deletetags = new List<int>();


        string[,] caseoccupeInt = new string[7, 7];



        //**** pour le min max : ****

        private List<List<int>> allpostionjoueurs = new List<List<int>>();
        private List<List<int>> allpostionennemi = new List<List<int>>();

        private List<int> Stockvalueballennemi = new List<int>();
        private List<int> Stockvalueballballjoueur = new List<int>();

        private bool makepasse_IA = false;
        private int nbredetours_IA = 0;
        //stocke chaque etat etat avec son score qui lui correspond lors du calucl in-max
        //Dictionary<State, int> all_get_score = new Dictionary<State, int>();
       // public int scoretest = 0;

        //permet de stocker les 4 mouvement qu'on veut reutiliser au min -max
        private List<int> stock_four_mvt = new List<int>();



       




        //creer un tableau de pictureBox
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Ennemi";

            int left = 2; int tops = 2;
           
            PictureBoxes = new PictureBox[n, n];
            caseoccupe = new  bool[n, n];
           

            for ( int i =0; i < n; i++)
            {
                left = 2;
                for (int j=0; j<n; j++)
                {
                  
                    PictureBoxes[i, j] = new PictureBox();
                   
                    PictureBoxes[i,j].Image = Properties.Resources.icons8_circle_32;

                   
                    PictureBoxes[i, j].Location = new Point(left, tops);
                    PictureBoxes[i, j].Size = new Size(60, 60);
                    PictureBoxes[i, j].AccessibleName = i.ToString()+","+j.ToString();

                    caseoccupe[i, j] = false;
                    left += 60;

                 

                    if (i == 0) {

                     
                            panel.Controls.Remove(PictureBoxes[i, j]);

                            PictureBoxes[i, j].Image = Properties.Resources.icons8_blue_circle_32;
                            PictureBoxes[i, j].Name = "joueur";
                            caseoccupe[i, j] = true;

                        //lié au min max 
                        List<int> position = new List<int>();
                        position.Add(i);
                        position.Add(j);
                        allpostionjoueurs.Add(position);

                        if ( j ==3)
                        {
                           
                            PictureBoxes[i, j].AccessibleDescription = "J";
                           
                           
                            //lié au min -max 
                            Stockvalueballballjoueur.Add(i);
                            Stockvalueballballjoueur.Add(j);
                           



                        }
                       
                    }
                    if (i == (n - 1))
                    {
                        
                            panel.Controls.Remove(PictureBoxes[i, j]);
                            PictureBoxes[i, j].Image = Properties.Resources.icons8_orange_circle_32;
                            PictureBoxes[i, j].Name = "ennemi";
                            caseoccupe[i, j] = true;

                            if (j == 3)
                            {
                             
                                PictureBoxes[i, j].AccessibleDescription = "E";


                            //lié au min-max 
                            Stockvalueballennemi.Add(i);
                            Stockvalueballennemi.Add(j);

                        }



                        //lié au min max 
                        List<int> position = new List<int>();
                        position.Add(i);
                        position.Add(j);
                        allpostionennemi.Add(position);
                    }



                   //les picture box cache le panel ducoup, la selection du panel est impossible
                   //la solution est donc de recupere la position de chaque picture box 
                    this.PictureBoxes[i,j].MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseClick);
                    panel.Controls.Add(PictureBoxes[i, j]);

                }
                tops += 60;
            }
            
          
        }







        //movement des joueurs  (avec la position du joueur au debut et la position du joueur à l'etat finale)


        private void moving(int antx, int anty, int nextx, int nexty, PictureBox box)
        {
            //pour se deplacer on supprimer les deux pictures et on remplace par les picturebox qui correspond
            //les pions oranges
            //la condition Tag = "C" 

            if (caseoccupe[nextx, nexty] == false && (string)PictureBoxes[nextx,nexty].Tag=="C")
            {
                // (string)pictureox[positionxdanslalistedeletags,positionydanslalistedeletags] == "C"
                //grace à ce "C" le joueur reconnait que, seul les position qui ont comme caracteristique "C" sont des solutions pour bouger et ont donc la position nextx et nexty


                deletetagsname();
                //cette liste permet de stocker aussi les solutions du joueur, son contenu ne serait pas supprimer dès le debut du click
                deletetags.Clear();



                if (PictureBoxes[antx, anty].Name =="ennemi" && compteurdetours <3)
                {
                    
                   panel.Controls.Remove(PictureBoxes[antx, anty]);
                   panel.Controls.Remove(PictureBoxes[nextx, nexty]);
                   

                    PictureBoxes[nextx, nexty].Image = Properties.Resources.icons8_orange_circle_32;
                    PictureBoxes[nextx, nexty].Name = "ennemi";
                    panel.Controls.Add(PictureBoxes[nextx, nexty]);
                   

                    PictureBoxes[antx, anty].Image = Properties.Resources.icons8_circle_32;
                    PictureBoxes[antx, anty].Name = " ";
                    panel.Controls.Add(PictureBoxes[antx, anty]);

                    caseoccupe[nextx, nexty] = true;
                    caseoccupe[antx, anty] = false;

                    

                    //pour le nombre de tour
                    compteurdetours++;
                    
                    //compteur de deplacement des pions qui ne doit pas etre superieur de 2
                    
                    compteurde2deplacementparennemi++;

                    //probleme
                   for( int i = 0; i< allpostionennemi.Count; i++)
                    {
                        if( allpostionennemi[i][0]==antx && allpostionennemi[i][1] == anty)
                        {
                            allpostionennemi[i][0] = nextx;
                            allpostionennemi[i][1] = nexty;
                        }
                      
                    }
                  

                }





                //on supprimer les deux pictures et on remplace par les picturebox qui correspond
                // less pions bleu
                else if (PictureBoxes[antx, anty].Name== "joueur" && (compteurdetours>=3 || compteurdetours<6) )
                {
                    panel.Controls.Remove(PictureBoxes[antx, anty]);
                    panel.Controls.Remove(PictureBoxes[nextx, nexty]);

                    PictureBoxes[nextx, nexty].Image = Properties.Resources.icons8_blue_circle_32;
                    PictureBoxes[nextx, nexty].Name = "joueur";
                    panel.Controls.Add(PictureBoxes[nextx, nexty]);
                    caseoccupe[nextx, nexty] = true;


                    PictureBoxes[antx, anty].Image = Properties.Resources.icons8_circle_32;
                    PictureBoxes[antx, anty].Name = " ";
                    panel.Controls.Add(PictureBoxes[antx, anty]);
                    caseoccupe[antx, anty] = false;
                    compteurdetours++;
                    compteurde2deplacementparjoueur++;
                }

              
                
               if (compteurdetours == 3 )
                {
                    ballpassingOneforplayer = false;
                    label1.Text = "joueur";


                    Playmin_max();
                    compteurdetours++;
                    Playmin_max();
                    compteurdetours++;
                    Playmin_max();
                    compteurdetours++;


                    compteurdetours = 0;

                    ballpassingOneforennemi = false;
                    ballpassingOneforplayer = true;
                    compteurde2deplacementparennemi = 0;
                    compteurde2deplacementparjoueur = 0;
                    label1.Text = "Ennemi";
                    makepasse_IA = false;

                }






                if (compteurdetours == 6)
                {
                    compteurdetours = 0;

                    ballpassingOneforennemi = false;
                    ballpassingOneforplayer = true;
                    compteurde2deplacementparjoueur = 0;
                    label1.Text = "Ennemi";
                }
            }
        }



        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            // methode qui permet de changer le backcolor du joueur 
            unselectedPlayerColor();
          

           
         
           //permet de recuperer la valeur de chaque picture box  selectionner
            PictureBox box = (PictureBox)sender;
            string[] msg = box.AccessibleName.Split(',');
            int valueI = Int32.Parse(msg[0]);
            int valueJ = Int32.Parse(msg[1]);

            //stockvalue est une  liste qui stocke les valeur x et y du pion (permet donc de stocker la position du joueur du depart)
            // lorsqu'on clique sur un pion on veut l'identifier avec une couleur 
            // on stocke le picturebox du pion selectionner dans une liste pour pouvoir mettre à jour ses mouvements
            //box.AccessibleDescription permet d'identifier soit le pion du joueur (AccessibleDescription=null) soit la balle du joueur (avec AccessibleDescription='avec une lettre
            //stocvalueEnd permet de stocker la position des destinations
            
            
            
            if ( box.Name == "joueur" && compteurdetours>=3 )
            {
                //pour differencier les pions normaux et le pion qui detient la balle :
                //-si un pion a comme propriété AccessibleDescription == null cvd c'est un pion normal
                //-si un pion a comme propriété AccessibleDescription == "E" ou "J" cvd c'est un pion qui detient la balle



                if (box.AccessibleDescription == null)
                {
                    // compteurdetours < 5
                   
                        if (ballpassing == false)
                        {
                            if (compteurde2deplacementparjoueur <2)
                            {
                                Stockvalue.Clear();
                                Stockvalue.Add(valueI);
                                Stockvalue.Add(valueJ);

                                box.BackColor = Color.Green;
                                deselectedcolorPlayer.Add(box);

                                compteurde2deplacementparennemi = 0;
                              
                            }
                        }
                        else if (ballpassing == true)
                        {
                            if (caseoccupe[valueI, valueJ] == true)
                            {
                                stocvalueEnd.Clear();
                                stocvalueEnd.Add(valueI);
                                stocvalueEnd.Add(valueJ);

                            }
                        }
                    //ballpassing gere le movement des pions normaux et des pions qui possede la balle 
                    // il est mis a true lorsque on clique sur le joueur  qui possede la balle.
                    //et permet donc au pion princpale (qui possede la balle)  de bouger vers un autre pions




                }
                else if (box.AccessibleDescription == "J") {
                    Stockvalue.Clear();
                    Stockvalue.Add(valueI);
                    Stockvalue.Add(valueJ);

                    box.BackColor = Color.Black;
                    deselectedcolorPlayer.Add(box);
                    //ce boolean permettra de faire une condition pour eviter la selection des joueurs et de leur solution
                    ballpassing = true;
                  
                }

            }
            else if(box.Name == "ennemi" && compteurdetours<3)
            {
                //pour differencier les pions normaux et le pion qui detient la balle :
                //-si un pion a comme propriété AccessibleDescription == null cvd c'est un pion normal
                //-si un pion a comme propriété AccessibleDescription == "E" ou "J" cvd c'est un pion qui detient la balle


                if (box.AccessibleDescription == null )
                {//
                    if (ballpassing ==false)
                    {
                        if (compteurde2deplacementparennemi < 2)
                        {
                            Stockvalue.Clear();
                            Stockvalue.Add(valueI);
                            Stockvalue.Add(valueJ);

                            box.BackColor = Color.Red;
                            //je stocke ce picture box pour mettre ensuite de le reutiliser pour en lenver les différentes couleurs
                            deselectedcolorEnnemi.Add(box);

                            compteurde2deplacementparjoueur = 0;
                        }
                    }//compteurdetours == 2
                    else if(ballpassing ==true) {
                        if (caseoccupe[valueI, valueJ] == true)
                        {
                            stocvalueEnd.Clear();
                            stocvalueEnd.Add(valueI);
                            stocvalueEnd.Add(valueJ);
                           

                        }
                    }
                  
                }
                else if (box.AccessibleDescription == "E") {

                    Stockvalue.Clear();
                    Stockvalue.Add(valueI);
                    Stockvalue.Add(valueJ);

                    box.BackColor = Color.Orange;
                    deselectedcolorEnnemi.Add(box);
                    //ce boolean permettra de faire une condition pour eviter la selection des joueurs et de leur solution
                    ballpassing = true;
                  
                }
              
            }
           else
            {
                //objectif de movement
                //pour toutes les cases où caseoccupe est false

                if (caseoccupe[valueI, valueJ] == false)
                {
                    stocvalueEnd.Clear();
                    stocvalueEnd.Add(valueI);
                    stocvalueEnd.Add(valueJ);
                }

            }
           

            //if (compteurdetours != 2 && compteurdetours != 5)
            if(ballpassing ==false)
            {   
                //cette methode  permet de montrer les possibilités(solution en affichant des couleur) pour jouer 
                selectsolution(box, valueI, valueJ, caseoccupe);
                
            }






            if (Stockvalue.Count ==2 && stocvalueEnd.Count ==2)
            {// on supprime les deux derniers element de la list
             //in verse les valeur parceque le premier element qu'on va ajouté sera lesecond elemnt lorsqu'on rajoute un nouveau element
             //  if (compteurdetours != 2 && compteurdetours != 5)
             
                
                if(ballpassing ==false )
                {
                    //ces deux compteurs permettent de gerer le nombre de tours qu'un joueur ou ennemi qui n'a pas la balle effetuer

                    if (compteurde2deplacementparennemi < 2  && compteurde2deplacementparjoueur ==0)
                    {

                        
                        moving(Stockvalue[0], Stockvalue[1], stocvalueEnd[0], stocvalueEnd[1], box);
                       // check_condition_tours();
                       // compteurde2deplacementparjoueur++;
                    }
                    else if (compteurde2deplacementparennemi == 0 && compteurde2deplacementparjoueur < 2)
                        {


                            moving(Stockvalue[0], Stockvalue[1], stocvalueEnd[0], stocvalueEnd[1], box);
                      //  check_condition_tours();
                            // compteurde2deplacementparjoueur++;
                        }

                }
                else
                {
                    //ballpassingOneforennemi && ballpassingOneforplayer permet le nombre de fois qu'on fait une passe
                    // au debut je innitialiser ballpassingOneforennemi à false et  ballpassingOneforplayer à true
                    //lorsque l'ennemi effectue une passe ballpassingOneforennemi se met à true et on peut plus faire de passe
                    //lorsque l'ennemi aura effectué ces 3 mouvements (cad le compteurs de tours =3) alors ballpassingOneforplayer se met à false
                    //il fonctionne de la meme manière que ballpassingOneforennemi. lorsque le compteur est à 6 , on reinitialise ballpassingOneforennemi=false et ballpassingOneforplayer=true
                    //


                    if (ballpassingOneforennemi == false || ballpassingOneforplayer ==false)
                    {
                      

                        makePasse(Stockvalue[0], Stockvalue[1], stocvalueEnd[0], stocvalueEnd[1], box);
                   
                                              
                    }
                    ballpassing = false;
                    deletetags.Clear();
                }
                stocvalueEnd.Clear();

            }
            //les tag sont mis à = " " apres avoir  selectionné un joueur et qu'on a mis sont tag = " " 
            unselectedTags();

        }


   

        private bool[] GetpossibleMoov()
        {
            return null;
        }

        private bool[] GetpossiblePossiblePass()
        {
            return null;
        }


        public bool Checking_if_we_have_not_ennemi_in_the_diagonal_road(int nextx, int nexty, int antx, int anty, bool blocagepionennemi)
        {

            if (nexty > anty && nextx < antx)
            {
                int j = nexty - 1; int i = nextx + 1;

                while (j > anty && i < antx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i += 1;
                        j -= 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        j -= 1;
                    }

                }

            }
            else if (nexty < anty && nextx < antx)
            {
                int j = nexty + 1; int i = nextx + 1;
                while (j < anty && i < antx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i += 1;
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        j += 1;
                    }
                }

            }
            else if (nexty < anty && nextx > antx)
            {
                int j = nexty + 1; int i = nextx - 1;
                while (j < anty && i > antx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i -= 1;
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i -= 1;
                        j += 1;
                    }
                }

            }
            else if (nexty > anty && nextx > antx)
            {
                int j = nexty - 1; int i = nextx - 1;
                while (j > anty && i > antx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i -= 1;
                        j -= 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i -= 1;
                        j -= 1;
                    }
                }
            }
            return blocagepionennemi;
        }

            public  bool Checking_if_we_have_not_player_in_the_diagonal_road(int nextx , int nexty, int antx ,int anty, bool blocagepionennemi)
        {

            if (nexty > anty && nextx < antx)
            {
                int j = nexty - 1; int i = nextx + 1;

                while (j > anty && i < antx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i += 1;
                        j -= 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        j -= 1;
                    }

                }

            }
            else if (nexty < anty && nextx < antx)
            {
                int j = nexty + 1; int i = nextx + 1;
                while (j < anty && i < antx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i += 1;
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        j += 1;
                    }
                }

            }
            else if (nexty < anty && nextx > antx)
            {
                int j = nexty + 1; int i = nextx - 1;
                while (j < anty && i > antx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i -= 1;
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i -= 1;
                        j += 1;
                    }
                }

            }
            else if (nexty > anty && nextx > antx)
            {
                int j = nexty - 1; int i = nextx - 1;
                while (j > anty && i > antx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i -= 1;
                        j -= 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i -= 1;
                        j -= 1;
                    }
                }
            }
            return blocagepionennemi;
        
        }

        public bool Checking_if_we_have_not_ennemi_in_vertical_Horizontal_road(int nextx, int nexty, int antx, int anty, bool blocagepionennemi)
        {

            if (nexty == anty && nextx < antx)
            {
                 int i = nextx + 1;
               int  j = nexty;
                while ( i < antx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i += 1;
                        
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        break;
                    }

                }

            }
            else if (nexty == anty && nextx > antx)
            {
                int j = nexty; int i = antx + 1;
                while ( i < nextx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                        i += 1;
                       
                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        break;
                    }
                }

            }
            else if (nexty > anty && nextx == antx)
            {
                int j = anty + 1; int i = nextx;
                while (j < nexty )
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                       
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                      
                        j += 1;
                        break;
                    }
                }

            }
            else if (nexty < anty && nextx == antx)
            {
                int j = antx + 1; int i = nextx;
                while (j < nextx)
                {
                    if (PictureBoxes[i, j].Name != "ennemi")
                    {
                      
                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;
                      
                        j+= 1;
                        break;
                    }
                }
            }
            return blocagepionennemi;
        }

        public bool Checking_if_we_have_not_player_in_vertical_Horizontal_road(int nextx, int nexty, int antx, int anty, bool blocagepionennemi)
        {

            if (nexty == anty && nextx < antx)
            {
                int i = nextx + 1;
                int j = nexty;
                while (i < antx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i += 1;

                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        break;
                    }

                }

            }
            else if (nexty == anty && nextx > antx)
            {
                int j = nexty; int i = antx + 1;
                while (i < nextx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {
                        i += 1;

                    }
                    else
                    {
                        blocagepionennemi = true;
                        i += 1;
                        break;
                    }
                }

            }
            else if (nexty > anty && nextx == antx)
            {
                int j = anty + 1; int i = nextx;
                while (j < nexty)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {

                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;

                        j += 1;
                        break;
                    }
                }

            }
            else if (nexty < anty && nextx == antx)
            {
                int j = antx + 1; int i = nextx;
                while (j < nextx)
                {
                    if (PictureBoxes[i, j].Name != "joueur")
                    {

                        j += 1;
                    }
                    else
                    {
                        blocagepionennemi = true;

                        j += 1;
                        break;
                    }
                }
            }
            return blocagepionennemi;
        }

        private void selectPlayer(int actux, int actuy) { 
        
        }
       private void unselectedTags()
        {
            for( int i = 0; i< n; i++)
            {

                for (int j= 0; j < n; j++)
                {
                    PictureBoxes[i, j].Tag = " ";
                }

                }
        }


        //methode qui permet d'enlever la couleur des solutions mais aussi de stocker pic[,].tag ="c" qui permettra de savoir que ce sont  des solutions eventuelles
        private void unselectedPlayerColor()
        {
            // to deselected backColor

            PictureBox antboxplayer;
            PictureBox antboxennemi;
            PictureBox antboxSolution;


            if (deselectedcolorPlayer.Count != 0)
            {
                antboxplayer = deselectedcolorPlayer[0];


                antboxplayer.BackColor = Color.Transparent;

            }

            if (deselectedcolorEnnemi.Count != 0)
            {
                antboxennemi = deselectedcolorEnnemi[0];
                antboxennemi.BackColor = Color.Transparent;

            }

            if (deselectedcolorSolution.Count != 0)
            {
                

                if (deselectedcolorSolution.Count == 2)
                {
                    PictureBoxes[deselectedcolorSolution[0], deselectedcolorSolution[1]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[0], deselectedcolorSolution[1]].Tag = "C";
                }
                
             
                else if (deselectedcolorSolution.Count == 4)
                    {
                    
                    for (int j = 0; j < 3; j++)
                    {
                        int k = j % 2;
                        if (k == 0 )
                        {
                            PictureBoxes[deselectedcolorSolution[j], deselectedcolorSolution[j + 1]].BackColor = Color.Transparent;
                            PictureBoxes[deselectedcolorSolution[j], deselectedcolorSolution[j + 1]].Tag = "C";
                        }
                    }

                    /* PictureBoxes[deselectedcolorSolution[0], deselectedcolorSolution[1]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[2], deselectedcolorSolution[3]].BackColor = Color.Transparent;*/
                }
                
                else if (deselectedcolorSolution.Count == 6)
                {

                    for (int i = 0; i < 5; i++)
                    {
                        if (i % 2 == 0)
                        {
                            PictureBoxes[deselectedcolorSolution[i], deselectedcolorSolution[i + 1]].BackColor = Color.Transparent;
                            PictureBoxes[deselectedcolorSolution[i], deselectedcolorSolution[i + 1]].Tag = "C";
                        }
                    }
                    /* PictureBoxes[deselectedcolorSolution[0], deselectedcolorSolution[1]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[2], deselectedcolorSolution[3]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[4], deselectedcolorSolution[5]].BackColor = Color.Transparent;*/
                }
                
                else if (deselectedcolorSolution.Count == 8)
                {

                    for (int i = 0; i < 7; i++)
                    {
                        if (i % 2 == 0)
                        {
                            PictureBoxes[deselectedcolorSolution[i], deselectedcolorSolution[i + 1]].BackColor = Color.Transparent;
                            PictureBoxes[deselectedcolorSolution[i], deselectedcolorSolution[i + 1]].Tag = "C";
                        }
                    }
                    /*
                    PictureBoxes[deselectedcolorSolution[0], deselectedcolorSolution[1]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[2], deselectedcolorSolution[3]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[4], deselectedcolorSolution[5]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorSolution[6], deselectedcolorSolution[7]].BackColor = Color.Transparent;*/
                }

            }
            //pour le pion principale (balle)
            if (deselectedcolorPionPrincipale.Count != 0)
            {

                if (deselectedcolorPionPrincipale.Count == 12)
                {
                    for( int i =0; i<11; i++)
                    {
                        if (i % 2 == 0 )
                        {
                            PictureBoxes[deselectedcolorPionPrincipale[i], deselectedcolorPionPrincipale[i+1]].BackColor = Color.Transparent;
                          //  PictureBoxes[deselectedcolorPionPrincipale[i], deselectedcolorPionPrincipale[i + 1]].Tag = "C";
                        }
                    }


                    /*
                    PictureBoxes[deselectedcolorPionPrincipale[0], deselectedcolorPionPrincipale[1]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorPionPrincipale[2], deselectedcolorPionPrincipale[3]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorPionPrincipale[4], deselectedcolorPionPrincipale[5]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorPionPrincipale[6], deselectedcolorPionPrincipale[7]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorPionPrincipale[8], deselectedcolorPionPrincipale[9]].BackColor = Color.Transparent;
                    PictureBoxes[deselectedcolorPionPrincipale[10], deselectedcolorPionPrincipale[11]].BackColor = Color.Transparent;*/

                }
                }






                deselectedcolorEnnemi.Clear();
            deselectedcolorPlayer.Clear();
            deselectedcolorSolution.Clear();
            deselectedcolorPionPrincipale.Clear();

        }


        public void deletetagsname()
        {
            //j'ai crée la list deletetags et j'ai fait en sorte que seulement les solutions proposer pour bouger seront prise en compte par le joueur pour bouger 
            //ducoup selon la refexion prise ici, lorsque le joueur clique sur une des solution pour bouger , la methode unselect permet de rendre les backcolor transparents en
            // à la fin de cette methode toute la liste qui stockait la position des differente solution pour rendre leeur backolor transparent sont supprimer 
            //l'idée que j'ai eu est d'ajouter dans cette methode unselected une des caracteristique tags ="C" qui permettrait que l'orsque la methode
            //move  serait appelé  , cette methode permettra au joueur de bouger selon les solution qui ont été proposé en ajoutant la condition que
            // (string)pictureox[positionxdanslalistedeletags,positionydanslalistedeletags] == "C"
            //grace à ce "C" le joueur reconnait que, seul les position qui on comme caracteristique "C" sont des solution pour bouger
            if (deletetags.Count != 0)
            {


                if (deletetags.Count == 2)
                {
                   
                    PictureBoxes[deletetags[0], deletetags[1]].Tag = "";
                }


                else if (deletetags.Count == 4)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        int k = j % 2;
                        if (k == 0)
                        {
                        
                            PictureBoxes[deletetags[j], deletetags[j + 1]].Tag = "";
                        }
                    }

                   }

                else if (deletetags.Count == 6)
                {

                    for (int i = 0; i < 5; i++)
                    {
                        if (i % 2 == 0)
                        {
                        
                            PictureBoxes[deletetags[i], deletetags[i + 1]].Tag = "";
                        }
                    }
                   }

                else if (deletetags.Count == 8)
                {

                    for (int i = 0; i < 7; i++)
                    {
                        if (i % 2 == 0)
                        {
                          
                            PictureBoxes[deletetags[i], deletetags[i + 1]].Tag = "";
                        }
                    }
                    
                   
                }

            }
            //pour le pion principale
          
        }
        private void selectsolution(PictureBox box, int valueI, int valueJ, bool[,] solution)
        {


            //permet de montrer les solutions proposer au joueur sur lequel on a cliqué dessus

            if ((box.Name == "joueur" || box.Name == "ennemi") && box.AccessibleDescription == null)
           //if ((box.Name == "ennemi") && box.AccessibleDescription == null)
               {

                if ((valueI <= 6 && valueI >= 0) && valueJ == 0)
                {
                    if (valueI == 6)
                    {//on ne peut que monter et aller à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                            deselectedcolorSolution.Add(valueI - 1);
                            deselectedcolorSolution.Add(valueJ);


                            //on ajoute les coord dans cette liste pour plutard  permettre de reconnaitre les vrai positions qui etait des solutions
                            deletetags.Add(valueI - 1);
                            deletetags.Add(valueJ);
                        }

                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                            
                            deselectedcolorSolution.Add(valueI);
                            deselectedcolorSolution.Add(valueJ + 1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ + 1);
                        }
                    }
                    else if (valueI == 0)
                    {//on ne peut que descendre et aller à droite
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                        
                             deselectedcolorSolution.Add(valueI + 1);
                            deselectedcolorSolution.Add(valueJ);

                            deletetags.Add(valueI + 1);
                            deletetags.Add(valueJ);
                        }
                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                         

                            deselectedcolorSolution.Add(valueI);
                            deselectedcolorSolution.Add(valueJ + 1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ + 1);
                        }
                    }
                    else if (valueI > 0 && valueI < 6)
                    {  //monter -descendre-aler à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                        


                            deselectedcolorSolution.Add(valueI - 1);
                            deselectedcolorSolution.Add(valueJ);


                            deletetags.Add(valueI - 1);
                            deletetags.Add(valueJ);
                        }
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                          

                            deselectedcolorSolution.Add(valueI + 1);
                            deselectedcolorSolution.Add(valueJ);

                            deletetags.Add(valueI + 1);
                            deletetags.Add(valueJ);

                        }
                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                         

                            deselectedcolorSolution.Add(valueI);
                            deselectedcolorSolution.Add(valueJ + 1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ + 1);
                        }
                    }
                }



                else if ((valueI >= 0 && valueI <= 6) && valueJ == 6)
                {
                    if (valueI == 6)
                    {//on ne peut que monter et aller à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                      
                            deselectedcolorSolution.Add(valueI - 1);
                            deselectedcolorSolution.Add(valueJ);

                            deletetags.Add(valueI - 1);
                            deletetags.Add(valueJ);
                        }

                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                           

                            deselectedcolorSolution.Add(valueI);
                            deselectedcolorSolution.Add(valueJ - 1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ - 1);
                        }
                    }
                    else if (valueI == 0)
                    {//on ne peut que descendre et aller à droite
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                           

                            deselectedcolorSolution.Add(valueI + 1);
                            deselectedcolorSolution.Add(valueJ);


                            deletetags.Add(valueI + 1);
                            deletetags.Add(valueJ);
                        }
                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                          

                            deselectedcolorSolution.Add(valueI);
                            deselectedcolorSolution.Add(valueJ - 1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ - 1);
                        }
                    }
                    else if (valueI > 0 && valueI < 6)
                    {  //monter -descendre-aler à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                           

                            deselectedcolorSolution.Add(valueI - 1);
                            deselectedcolorSolution.Add(valueJ);

                            deletetags.Add(valueI - 1);
                            deletetags.Add(valueJ);
                        }
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                       
                            deselectedcolorSolution.Add(valueI + 1);
                            deselectedcolorSolution.Add(valueJ);

                            deletetags.Add(valueI + 1);
                            deletetags.Add(valueJ);

                        }
                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                          
                            deselectedcolorSolution.Add(valueI );
                            deselectedcolorSolution.Add(valueJ-1);

                            deletetags.Add(valueI);
                            deletetags.Add(valueJ - 1);
                        }
                    }
                }



                else if ((valueJ < 6 && valueJ > 0) && valueI == 0)
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI + 1, valueJ] == false)
                    {
                        PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                        
                        deselectedcolorSolution.Add(valueI + 1);
                        deselectedcolorSolution.Add(valueJ);

                        deletetags.Add(valueI + 1);
                        deletetags.Add(valueJ);
                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                       
                        deselectedcolorSolution.Add(valueI );
                        deselectedcolorSolution.Add(valueJ+1);

                        deletetags.Add(valueI);
                        deletetags.Add(valueJ + 1);
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                       

                        deselectedcolorSolution.Add(valueI);
                        deselectedcolorSolution.Add(valueJ-1);

                        deletetags.Add(valueI);
                        deletetags.Add(valueJ - 1);
                    }

                }
                else if ((valueJ < 6 && valueJ > 0) && valueI == 6)
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI - 1, valueJ] == false)
                    {
                        PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                      
                        deselectedcolorSolution.Add(valueI - 1);
                        deselectedcolorSolution.Add(valueJ);


                        deletetags.Add(valueI - 1);
                        deletetags.Add(valueJ);
                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                    
                        deselectedcolorSolution.Add(valueI );
                        deselectedcolorSolution.Add(valueJ+1);


                        deletetags.Add(valueI);
                        deletetags.Add(valueJ + 1);
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                      
                        deselectedcolorSolution.Add(valueI );
                        deselectedcolorSolution.Add(valueJ-1);

                        deletetags.Add(valueI);
                        deletetags.Add(valueJ - 1);
                    }

                }
                else
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI - 1, valueJ] == false)
                    {
                        PictureBoxes[valueI - 1, valueJ].BackColor = Color.Yellow;
                       
                        deselectedcolorSolution.Add(valueI - 1);
                        deselectedcolorSolution.Add(valueJ);

                        deletetags.Add(valueI - 1);
                        deletetags.Add(valueJ);
                    }
                    if (caseoccupe[valueI + 1, valueJ] == false)
                    {
                        PictureBoxes[valueI + 1, valueJ].BackColor = Color.Yellow;
                     
                        deselectedcolorSolution.Add(valueI + 1);
                        deselectedcolorSolution.Add(valueJ);


                        deletetags.Add(valueI + 1);
                        deletetags.Add(valueJ);
                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        PictureBoxes[valueI, valueJ + 1].BackColor = Color.Yellow;
                      
                        deselectedcolorSolution.Add(valueI );
                        deselectedcolorSolution.Add(valueJ+1);

                        deletetags.Add(valueI);
                        deletetags.Add(valueJ + 1);
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        PictureBoxes[valueI, valueJ - 1].BackColor = Color.Yellow;
                  
                        deselectedcolorSolution.Add(valueI );
                        deselectedcolorSolution.Add(valueJ-1);

                        deletetags.Add(valueI);
                        deletetags.Add(valueJ - 1);
                    }

                }
             // deletetags=  deselectedcolorSolution;
            }

            //colorer les solutions pour les pions principale de l'equipe 
            if ((box.Name == "joueur" || box.Name == "ennemi") && (box.AccessibleDescription == "J" || box.AccessibleDescription == "E"))
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                      
                            if (caseoccupe[valueI, valueJ] == true && box.Name == "ennemi")
                            {
                                if (PictureBoxes[i, j].AccessibleDescription == null && PictureBoxes[i, j].Name == "ennemi")
                                {
                                    PictureBoxes[i, j].BackColor = Color.Red;

                                    deselectedcolorPionPrincipale.Add(i);
                                    deselectedcolorPionPrincipale.Add(j);
                                }

                            }
                            else if (caseoccupe[valueI, valueJ] == true && box.Name == "joueur")
                            {
                                if (PictureBoxes[i, j].AccessibleDescription == null && PictureBoxes[i, j].Name == "joueur")
                                {
                                    PictureBoxes[i, j].BackColor = Color.Green;

                                    deselectedcolorPionPrincipale.Add(i);
                                    deselectedcolorPionPrincipale.Add(j);
                                }
                            }
                        
                    }
                }
            }

            
              
                /*
                if (solution[caseIselected,caseJselected] == false)
                {
                    box.BackColor = Color.Yellow;
                    deselectedcolorSolution.Add(box);

                }*/
            }


        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox newpic = new PictureBox();
            newpic.Location = new Point(80, 80);
            newpic.Size = new Size(15, 15);
            newpic.BackColor = Color.Black;
            panel.Controls.Add(newpic);
        }



        public void Playmin_max() {
            bool turnplayer = true;
            State state = new State(turnplayer, caseoccupeInt, caseoccupe, allpostionennemi, allpostionjoueurs, Stockvalueballennemi, Stockvalueballballjoueur);
            Min_Max min_Max = new Min_Max();
            int deep = 2;
            //min_Max.recusive_method(state, deep);
            // min_Max.get_mvt_we_need(state, deep, min_Max.scoretest);
            int globaldepp = deep;
             
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            
            //alpha beta
            int stock = min_Max.alpha_beta(state, deep, globaldepp,alpha,beta);
            //min max
            //int stock =min_Max.recusive_method(state, deep, globaldepp);
            
            
            List<int> Mvment_minMax= min_Max.get_mvt_we_need(state,deep,stock,compteurde2deplacementparjoueur,makepasse_IA, Stockvalueballballjoueur[0],Stockvalueballballjoueur[1], PictureBoxes);
            //List<int> Mvment_minMax = min_Max.get_mvt_we_need(state, deep, min_Max.scoretest); ;
            if (Mvment_minMax.Count != 0 )
            {
                moveIA(Mvment_minMax[0], Mvment_minMax[1], Mvment_minMax[2], Mvment_minMax[3]);
            }
            else
            {

            }
            Mvment_minMax.Clear();

        }



        public void moveIA(int antx , int anty, int nextx,int nexty )
        {   //on verifie s'il faut faire une passe ou jouer . on verifie donc si les position antx et anty ne correspondent pas au coordonnées de la balle

            int count = 0;

            if (antx == Stockvalueballballjoueur[0]) {
               count++;
            }
            if (anty == Stockvalueballballjoueur[1])
            {
                count++;
            }


            if(count <2) { 

                if (PictureBoxes[antx, anty].Name == "joueur" && caseoccupe[nextx, nexty] == false && compteurde2deplacementparjoueur <2)
                {
                    // if (PictureBoxes[antx, anty].Name == "joueur" && caseoccupe[nextx, nexty] == false && compteurde2deplacementparjoueur<2)

                    panel.Controls.Remove(PictureBoxes[antx, anty]);
                    panel.Controls.Remove(PictureBoxes[nextx, nexty]);

                    PictureBoxes[nextx, nexty].Image = Properties.Resources.icons8_blue_circle_32;
                    PictureBoxes[nextx, nexty].Name = "joueur";
                    panel.Controls.Add(PictureBoxes[nextx, nexty]);
                    caseoccupe[nextx, nexty] = true;


                    PictureBoxes[antx, anty].Image = Properties.Resources.icons8_circle_32;
                    PictureBoxes[antx, anty].Name = " ";
                    panel.Controls.Add(PictureBoxes[antx, anty]);
                    caseoccupe[antx, anty] = false;

                    compteurde2deplacementparjoueur++;


                    //probleme
                    for (int i = 0; i < allpostionjoueurs.Count; i++)
                    {
                        if (allpostionjoueurs[i][0] == antx && allpostionjoueurs[i][1] == anty)
                        {
                            allpostionjoueurs[i][0] = nextx;
                            allpostionjoueurs[i][1] = nexty;
                            break;
                        }

                    }
                }
            }
            else if( makepasse_IA ==false)
            {//mouvement de la ball
                PictureBoxes[nextx, nexty].Image = Properties.Resources.icons8_blue_circle_32;
                PictureBoxes[nextx, nexty].BackColor = Color.Black;
                PictureBoxes[nextx, nexty].AccessibleDescription = "J";



            
                PictureBoxes[antx, anty].BackColor = Color.Transparent;
                PictureBoxes[antx, anty].AccessibleDescription = null;

                //on verifie si la balle reste sur la meme place
                if (antx == nextx && anty==nexty)
                {
                    PictureBoxes[nextx, nexty].BackColor = Color.Black;
                }

                //attention la case à laquelle on veut faire la passe doit etre à true ,et la position antécedente de la case aussi
                caseoccupe[nextx, nexty] = true;
                caseoccupe[antx, anty] = true;

                Stockvalueballballjoueur[0] = nextx;
                Stockvalueballballjoueur[1] = nexty;
                makepasse_IA = true;

            }

            if(  Stockvalueballballjoueur[0]==6 )
            {
               
                    string message = "L'IA a gagné la partie";
                    string title = "Winner";
                    MessageBox.Show(message, title);
                    Application.Exit();
                
            }
            
          




                
          
            
            
            
            
            /*else  if (PictureBoxes[antx, anty].Name == "joueur"  && PictureBoxes[antx, anty].AccessibleDescription == "J")
            {



                if (nextx == antx || nexty == anty)
                {
                    blocagepion_ennemi_joueur_VH = Checking_if_we_have_not_ennemi_in_vertical_Horizontal_road(nextx, nexty, antx, anty, blocagepion_ennemi_joueur_VH);
                    if (blocagepion_ennemi_joueur_VH == false)
                    {

                        PictureBoxes[nextx, nexty].BackColor = Color.Black;
                        PictureBoxes[nextx, nexty].AccessibleDescription = "J";




                        PictureBoxes[antx, anty].BackColor = Color.Transparent;
                        PictureBoxes[antx, anty].AccessibleDescription = null;

                        //attention la case à laquelle on veut faire la passe doit etre à true ,et la position antécedente de la case aussi
                        caseoccupe[nextx, nexty] = true;
                        caseoccupe[antx, anty] = true;


                        ballpassingOneforplayer = true;
                        ballpassingOneforennemi = true;


                        if (nextx == 6)
                        {
                            string message = "Vous avez gagné la partie";
                            string title = "Winner";
                            MessageBox.Show(message, title);
                            Application.Exit();
                        }



                        
                    }
                }
                else
                {
                    //cette condition : c'est pour effectuer des mouvements en diagonales

                    // pour effectuer des mouvement diagonaux avec la balle on verifie si la difference en x est la meme que la difference en y
                    // cela permet de trouver rapidement si le joueur à qui on veut donner la passe est en diagonales ou pas

                    int diffx = nextx - antx;
                    int diffy = nexty - anty;

                    if (diffx < 0)
                    {
                        diffx = Math.Abs(diffx);
                    }
                    if (diffy < 0)
                    {
                        diffy = Math.Abs(diffy);
                    }

                    if (diffx == diffy)
                    {


                        blocagepionennemi = Checking_if_we_have_not_ennemi_in_the_diagonal_road(nextx, nexty, antx, anty, blocagepionennemi);
                        if (blocagepionennemi == false)
                        {
                            PictureBoxes[nextx, nexty].BackColor = Color.Black;
                            PictureBoxes[nextx, nexty].AccessibleDescription = "J";




                            PictureBoxes[antx, anty].BackColor = Color.Transparent;
                            PictureBoxes[antx, anty].AccessibleDescription = null;


                            caseoccupe[nextx, nexty] = true;
                            caseoccupe[antx, anty] = true;


                          

                            if (nextx == 6)
                            {
                                string message = "Vous avez gagné la partie";
                                string title = "Winner";
                                MessageBox.Show(message, title);
                                Application.Exit();
                            }

                            //pour le nombre de tour
                            
                            // ballpassingOnceforplayer = true;
                            //
                        }


                        ballpassingOneforplayer = true;
                        ballpassingOneforennemi = true;
                    }
                }
            }*/


        }

        private void panel_Click(object sender, EventArgs e)
        {
           // label1.Text = "checking";
            System.Windows.Forms.MouseEventArgs a=  (System.Windows.Forms.MouseEventArgs)e;
           // int mouseX = args.X;
            //label1.Text = mouseX.ToString();
        }

        private void makePasse(int antx, int anty, int nextx, int nexty, PictureBox box)
        {
            if (caseoccupe[nextx, nexty] == true)
            {
                //je sais que tous les pions sont à true (que ça soit des joueeurs ou des ennemi)
                //pour faire une passe la condition à respecter est que : 
                //-le pion de destination soit à true et qu'il soit de la meme equipe avec celui qui veut faire la passe 


                if (PictureBoxes[antx, anty].Name == "ennemi" && compteurdetours < 3 && PictureBoxes[antx, anty].AccessibleDescription == "E")
                {



                    //cette condition : c'est pour effectuer des mouvements verticaux et horizontaux 
                    //cad  si joueur qui a la balle est sur la meme ligne ou sur la meme colonne que le joueur qui doit recevoir la passe => on peut bouger
                    if (nextx == antx || nexty == anty)
                    {
                        blocagepion_ennemi_joueur_VH = Checking_if_we_have_not_player_in_vertical_Horizontal_road(nextx, nexty, antx, anty, blocagepion_ennemi_joueur_VH);
                        if (blocagepion_ennemi_joueur_VH == false)
                        {
                            PictureBoxes[nextx, nexty].BackColor = Color.Orange;
                            PictureBoxes[nextx, nexty].AccessibleDescription = "E";




                            PictureBoxes[antx, anty].BackColor = Color.Transparent;
                            PictureBoxes[antx, anty].AccessibleDescription = null;


                            caseoccupe[nextx, nexty] = true;
                            caseoccupe[antx, anty] = true;



                        //probleme
                          Stockvalueballennemi[0] = nextx;
                          Stockvalueballennemi[1] = nexty;





                            ballpassingOneforennemi = true;

                            if (nextx == 0)
                            {
                                string message = "Vous avez gagné la partie";
                                string title = "Winner";
                                MessageBox.Show(message, title);
                                Application.Exit();
                            }

                            //pour le nombre de tour
                            compteurdetours++;
                        }
                    }
                    else
                    {


                        //cette condition : c'est pour effectuer des mouvements en diagonales
                        // pour effectuer des mouvement diagonaux avec la balle on verifie si la difference en x est la meme que la difference en y
                        // cela permet de trouver rapidement si le joueur à qui on veut donner la passe est en diagonales ou pas

                        int diffx = nextx - antx;
                        int diffy = nexty - anty;

                        if (diffx < 0)
                        {
                            diffx = Math.Abs(diffx);
                        }
                        if (diffy < 0)
                        {
                            diffy = Math.Abs(diffy);
                        }

                        if (diffx == diffy)
                        {

                            //blocagepionennemi permet de savoir s'il ny a pas de player dans la diagonale. s'il ya, il est mis à true et du coup, le mouvement n'est pas possible
                            blocagepionennemi = Checking_if_we_have_not_player_in_the_diagonal_road(nextx, nexty, antx, anty, blocagepionennemi);

                            if (blocagepionennemi == false)
                            {
                                PictureBoxes[nextx, nexty].BackColor = Color.Orange;
                                PictureBoxes[nextx, nexty].AccessibleDescription = "E";




                                PictureBoxes[antx, anty].BackColor = Color.Transparent;
                                PictureBoxes[antx, anty].AccessibleDescription = null;


                                caseoccupe[nextx, nexty] = true;
                                caseoccupe[antx, anty] = true;

                               


                                Stockvalueballennemi[0] = nextx;
                               Stockvalueballennemi[1] = nexty;


                                ballpassingOneforennemi = true;
                                //pour le nombre de tour

                                if (nextx == 0)
                                {
                                    string message = "Vous avez gagné la partie";
                                    string title = "Winner";
                                    MessageBox.Show(message, title);
                                    Application.Exit();
                                }

                                compteurdetours++;
                            }
                            //donc c'est à la fin des tours de l'enemi que j'action ce boolean pour dire que le joueur peut faire une passe de nouveau

                            //ballpassingOneforennemi = true;
                        }





                    }



                    /*
               pour faire en sorte que si le joueur voir sur son chemin diagonal un pion ennemi , qu'il n'avance pas
                if((postiojoueuropposéX< nextx && postiojoueuropposéX> antx) || postiojoueuropposéY<nexty && postiojoueuropposéY > anty){

                        int diffx = nextx - antx;
                        int diffy = nexty - anty;

                        if (diffx < 0)
                        {
                            diffx = Math.Abs(diffx);
                        }
                        if (diffy < 0)
                        {
                            diffy = Math.Abs(diffy);
                        }

                        if (diffx == diffy)
                        {
                         //le pion effectue du joueur en question n'effectue auccun mouvement
                    }
              }


               */


                }
                else if (PictureBoxes[antx, anty].Name == "joueur" && (compteurdetours >= 3 && compteurdetours < 6) && PictureBoxes[antx, anty].AccessibleDescription == "J")
                {



                    if (nextx == antx || nexty == anty)
                    {
                        blocagepion_ennemi_joueur_VH = Checking_if_we_have_not_ennemi_in_vertical_Horizontal_road(nextx, nexty, antx, anty, blocagepion_ennemi_joueur_VH);
                        if (blocagepion_ennemi_joueur_VH == false)
                        {

                            PictureBoxes[nextx, nexty].BackColor = Color.Black;
                            PictureBoxes[nextx, nexty].AccessibleDescription = "J";




                            PictureBoxes[antx, anty].BackColor = Color.Transparent;
                            PictureBoxes[antx, anty].AccessibleDescription = null;

                            //attention la case à laquelle on veut faire la passe doit etre à true ,et la position antécedente de la case aussi
                            caseoccupe[nextx, nexty] = true;
                            caseoccupe[antx, anty] = true;


                            ballpassingOneforplayer = true;


                            if (nextx == 6)
                            {
                                string message = "Vous avez gagné la partie";
                                string title = "Winner";
                                MessageBox.Show(message, title);
                                Application.Exit();
                            }



                            //pour le nombre de tour
                            compteurdetours++;
                            // ballpassingOnceforplayer = true;
                        }
                    }
                    else
                    {
                        //cette condition : c'est pour effectuer des mouvements en diagonales

                        // pour effectuer des mouvement diagonaux avec la balle on verifie si la difference en x est la meme que la difference en y
                        // cela permet de trouver rapidement si le joueur à qui on veut donner la passe est en diagonales ou pas

                        int diffx = nextx - antx;
                        int diffy = nexty - anty;

                        if (diffx < 0)
                        {
                            diffx = Math.Abs(diffx);
                        }
                        if (diffy < 0)
                        {
                            diffy = Math.Abs(diffy);
                        }

                        if (diffx == diffy)
                        {


                            blocagepionennemi = Checking_if_we_have_not_ennemi_in_the_diagonal_road(nextx, nexty, antx, anty, blocagepionennemi);
                            if (blocagepionennemi == false)
                            {
                                PictureBoxes[nextx, nexty].BackColor = Color.Black;
                                PictureBoxes[nextx, nexty].AccessibleDescription = "J";




                                PictureBoxes[antx, anty].BackColor = Color.Transparent;
                                PictureBoxes[antx, anty].AccessibleDescription = null;


                                caseoccupe[nextx, nexty] = true;
                                caseoccupe[antx, anty] = true;


                                ballpassingOneforplayer = true;

                                if (nextx == 6)
                                {
                                    string message = "Vous avez gagné la partie";
                                    string title = "Winner";
                                    MessageBox.Show(message, title);
                                    Application.Exit();
                                }

                                //pour le nombre de tour
                                compteurdetours++;
                                // ballpassingOnceforplayer = true;
                                //
                            }
                        }
                    }
                }


                if (compteurdetours == 3)
                {
                    ballpassingOneforplayer = false;
                    label1.Text = "Player";
                    blocagepionennemi = false;
                    blocagepion_ennemi_joueur_VH = false;


                    Playmin_max();
                    compteurdetours++;
                    Playmin_max();
                    compteurdetours++;
                    Playmin_max();
                    compteurdetours++;

                    compteurdetours = 0;

                    ballpassingOneforennemi = false;
                    ballpassingOneforplayer = true;
                    compteurde2deplacementparjoueur = 0;
                    label1.Text = "Player";
                    blocagepionennemi = false;
                    blocagepion_ennemi_joueur_VH = false;
                    compteurde2deplacementparennemi = 0;
                    makepasse_IA = false;

                }
                
                /* if (compteurdetours >= 3 && compteurdetours < 6)
                 {
                     ballpassingOneforplayer = false;
                 }*/

                if (compteurdetours == 6)
                {
                    compteurdetours = 0;

                    ballpassingOneforennemi = false;
                    ballpassingOneforplayer = true;
                    compteurde2deplacementparjoueur = 0;
                    label1.Text = "Ennemi";
                    blocagepionennemi = false;
                    blocagepion_ennemi_joueur_VH = false;
                }
            }

        }
            private void panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        




            

        }

    

        public void check_condition_tours()
        {
            if (compteurdetours == 3)
            {
                ballpassingOneforplayer = false;
                label1.Text = "Player";
                blocagepionennemi = false;
                blocagepion_ennemi_joueur_VH = false;


                Playmin_max();
                compteurdetours++;
                Playmin_max();
                compteurdetours++;
                Playmin_max();
                compteurdetours++;

                compteurdetours = 0;

                ballpassingOneforennemi = false;
                ballpassingOneforplayer = true;
                compteurde2deplacementparjoueur = 0;
                label1.Text = "Ennemi";
                blocagepionennemi = false;
                blocagepion_ennemi_joueur_VH = false;
                compteurde2deplacementparennemi = 0;

            }
        }
    }




}
