using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaBalik
{
    class State
    {
      

            private bool ballpassingOneforplayer;
            private bool ballpassingOneforennemi;
            //toutes les positions de la balle (pour effectuer une passe)
            public List<int> Stockvalueballennemi;//= new List<int>();
            public List<int> Stockvalueballballjoueur;// = new List<int>();
                                                      //toutes les positions des pions
            public List<List<int>> allpostionjoueurs;//= new List<List<int>>();
            public List<List<int>> allpostionennemi;//= new List<List<int>>();

            private bool blocagepionennemi_VH = false;
            private bool blocagepionennemi_Diagonal = false;

            public bool[,] caseoccupe;
            string[,] caseoccupeInt;
            // private PictureBox[,] PictureBoxes ;
            public bool turnplayer;

            //on cree des nvelle liste

            public bool choice = false;
            public List<List<int>> GetCloneDoubleListes(List<List<int>> listes)
            {//cette methode permet de faire un clone non eulement de chacune des liste que se trouve dans la liste parent
                List<List<int>> new_listes = new List<List<int>>();

                foreach (var item in listes)
                {   //on fait ici la copie de chaque list qui se trouve dans list et on la met dans une nouvelle liste
                    List<int> insidelistes = new List<int>(item);
                    new_listes.Add(insidelistes);
                }

                return new_listes;
            }




            public State(bool turnplayer, string[,] caseoccupeInt, bool[,] caseoccupe, List<List<int>> allpostionennemi, List<List<int>> allpostionjoueurs, List<int> Stockvalueballennemi, List<int> Stockvalueballballjoueur)
            {


                // on initialise tout

                //  this.ballpassingOneforennemi = ballpassingOneforennemi;
                //  this.ballpassingOneforplayer = ballpassingOneforplayer;


                this.Stockvalueballballjoueur = Stockvalueballballjoueur;
                this.Stockvalueballennemi = Stockvalueballennemi;
                //on cree une copie de la liste 

                // this.allpostionjoueurs = allpostionjoueurs;
                //this.allpostionennemi = allpostionennemi;
                this.allpostionjoueurs = GetCloneDoubleListes(allpostionjoueurs);
                this.allpostionennemi = GetCloneDoubleListes(allpostionennemi);

                this.caseoccupe = caseoccupe;
                this.turnplayer = turnplayer;
                this.caseoccupeInt = caseoccupeInt;




                string msg = "";
                if (choice == true)
                {


                    for (int i = 0; i < 7; i++)
                    {
                        msg += "\n";
                        for (int j = 0; j < 7; j++)
                        {

                            msg += this.caseoccupeInt[i, j] + " ";
                        }


                    }
                    Console.WriteLine(msg);
                }




                //  this.PictureBoxes = PictureBoxes;
            }




            //en modifiant la position d'un joueur ,on crée un nouveau état State)
            public State GetNewState(int antx, int anty, int nextx, int nexty)
            {

                //on fait des copies list ou des clones pour ne pas modifié les positions des pions
                //ici on parle juste de nouveau état qui ne vont influencer ou modifier les positions pour l'instant
                //pour des doubles listes , il faut assi cloner ce que la double liste  contient 
                List<List<int>> new_allpositionjoueurs = GetCloneDoubleListes(allpostionjoueurs);
                List<List<int>> new_allpositionennemies = GetCloneDoubleListes(allpostionennemi);
                //on fait une copie de la list entiere Stockvalueballballjoueur , et on la stocke dans la new_Stockvalueballballjoueur
                List<int> new_Stockvalueballballjoueur = new List<int>(Stockvalueballballjoueur);
                List<int> new_Stockvalueballennemi = new List<int>(Stockvalueballennemi);



                //allpostionjoueurs
                List<List<int>> TEAM;
                //on fait une copie de toutes les cases . on peut appeler la methode clone lorsqu'on a un tableau
                bool[,] new_caseoccupe = (bool[,])caseoccupe.Clone();
                string[,] new_caseoccupeInt = (string[,])caseoccupeInt.Clone();

                // si c'est la team A de jouer?
                if (turnplayer == true)
                {
                    TEAM = allpostionjoueurs;

                }
                else
                {
                    TEAM = allpostionennemi;

                }
                //dans cet GetNewState on on change la  position precedente par la position suivante
                if (TEAM == allpostionjoueurs)
                {
                    for (int i = 0; i < allpostionjoueurs.Count; i++)
                    {
                        if ((allpostionjoueurs[i][0] == antx && allpostionjoueurs[i][1] == anty))
                        {
                            //position de la balle 
                            if ((Stockvalueballballjoueur[0] == antx && Stockvalueballballjoueur[1] == anty))
                            {
                                // Stockvalueballballjoueur[0] = nextx;
                                //Stockvalueballballjoueur[1] = nexty;
                                new_Stockvalueballballjoueur[0] = nextx;
                                new_Stockvalueballballjoueur[1] = nexty;

                                new_caseoccupeInt[antx, anty] = "0";
                                new_caseoccupeInt[nextx, nexty] = "X";

                                new_caseoccupe[nextx, nexty] = true;
                                new_caseoccupe[antx, anty] = false;

                                break;
                            }
                            else
                            {
                                new_allpositionjoueurs[i][0] = nextx;



                                new_allpositionjoueurs[i][1] = nexty;

                                new_caseoccupeInt[antx, anty] = "0";
                                new_caseoccupeInt[nextx, nexty] = "X";


                                new_caseoccupe[nextx, nexty] = true;
                                new_caseoccupe[antx, anty] = false;

                                break;
                            }


                        }


                    }


                }

                else if (TEAM == allpostionennemi)
                {

                    //allpositionennemi
                    //dans cet GetNewState on on change la  position precedente par la position suivante
                    for (int i = 0; i < allpostionennemi.Count; i++)
                    {
                        if ((allpostionennemi[i][0] == antx && allpostionennemi[i][1] == anty))
                        {


                            if ((Stockvalueballennemi[0] == antx && Stockvalueballennemi[1] == anty))
                            {
                                // Stockvalueballennemi[0] = nextx;
                                // Stockvalueballennemi[1] = nexty;

                                new_Stockvalueballennemi[0] = nextx;
                                new_Stockvalueballennemi[1] = nexty;


                                new_caseoccupeInt[antx, anty] = "0";
                                new_caseoccupeInt[nextx, nexty] = "Y";

                                new_caseoccupe[nextx, nexty] = true;
                                new_caseoccupe[antx, anty] = false;

                                break;
                            }
                            else
                            {
                                new_allpositionennemies[i][0] = nextx;




                                new_allpositionennemies[i][1] = nexty;

                                new_caseoccupe[nextx, nexty] = true;
                                new_caseoccupe[antx, anty] = false;


                                new_caseoccupeInt[antx, anty] = "0";
                                new_caseoccupeInt[nextx, nexty] = "Y";

                                break;
                            }
                        }


                    }
                }

                


                // Console.WriteLine("SUIVANT");

                // return new State(!turnplayer,caseoccupeInt, caseoccupe, ballpassingOneforplayer, ballpassingOneforennemi, allpostionennemi, allpostionjoueurs, Stockvalueballennemi, Stockvalueballballjoueur);
                // return new State(turnplayer, caseoccupeInt, caseoccupe, ballpassingOneforplayer, ballpassingOneforennemi, allpostionennemi, allpostionjoueurs, Stockvalueballennemi, Stockvalueballballjoueur);
                State tmp = new State(!turnplayer, new_caseoccupeInt, new_caseoccupe, new_allpositionennemies, new_allpositionjoueurs, new_Stockvalueballennemi, new_Stockvalueballballjoueur);
                return tmp;

            }


            ///TO DO 
            ///

            //chaque état correspond a une certaine valeur
            //une idée : on fait la somme de la distance de tous nos joueurs
            //donc si tous nos joueurs sont proches , de la derniere ligne adverse cvd on est proche de gagné sinon on est loin de gagné
            public int getscore()
            {
                //on appelle cette fnction getscore  par rapport à un état .et comme dans un etat toutes les valeurs des pions sont fixes
                //on peut les avoir directement => ligne 12 -19
                //Questions : c'est la position de l'ennemi o du joueur?
                int valuesEnnemi = 0;
                int valuesJoueur = 0;
                //si valuejoueur vaut 10 et valuennemi vaut 0, le score sera positive (+10) . si c'est le contraire on aura (-10)
                //le joueur doit maximiser cette valeur qui est retourné , et les ennemi doit la minimiser
                //on considere ennemi comme l'ordi
                foreach (var item in this.allpostionennemi)
                {
                    //on prend seulement la position i  de chaque pion ennemi
                    // valuesEnnemi += ( item[0]-6);
                    valuesEnnemi += (6 - item[0]);
                }

                foreach (var item in this.allpostionjoueurs)
                {
                    //on prend seulement la position i  de chaque pion ennemi
                    //position final - la poistion initiale
                    valuesJoueur += (item[0]);
                    //Console.WriteLine(item[0]);

                    //on doit maximiser quand c'est l'ennemi et minimiser quand c'est le joueur
                }
                int diff = valuesJoueur - valuesEnnemi;
                //  Console.WriteLine(valuesJoueur);
                //    Console.WriteLine("GetScore = " + diff.ToString());
                return valuesJoueur - valuesEnnemi;
            }




            //permet de passer d'un etat à un autre
            //on cree une fonction qui renvoie tous les mouvements possibles ( deplacement de x y vers newx et newy)
            //on a besoin de quatre entiers pour representer un mouvement 
            public List<List<int>> Get_movements()
            {       // public List<(int antx, int anty, int new_x, int new_y)> Get_movements( )
                    //je dois savoir ou est le joueur que je veux deplacé , quel joueur je veux deplacé , où je veux le deplacé
                    // on a sous list de int qui a quatre mouvement , la list parent va stocké tous les mouvement => donc à 0 -> on aura une liste de 4 ( deplacement de x y vers newx et newy) , à 1 on aura une liste de 4 ( deplacement de x y vers newx et newy), ainsi de suite
                List<List<int>> TEAM;
                // List<List<int>> TEAM_Oppose;
                // si c'est la team A de jouer?
                if (turnplayer == true)
                {
                    TEAM = allpostionjoueurs;
                    // TEAM_Oppose = allpostionennemi;
                }
                else
                {
                    TEAM = allpostionennemi;
                    //  TEAM_Oppose = allpostionjoueurs;
                }


                List<List<int>> stockAllmoovementsPossible = new List<List<int>>();

                //pour chaque element pion de cette team on verifie quelle sera la solution pour se mouvoir ou faire une passe s'il a la balle
                foreach (var item in TEAM)
                {

                    Moovementsimpledespions(item[0], item[1], caseoccupe, stockAllmoovementsPossible);

                }
                return stockAllmoovementsPossible;
            }

            //dans cette methode de recupere la list<list<int>> ,dans la sous list on a les 4 mvt (antx,anty,nextx,nexty)
            //dans la list parent , on a stocké tous les list qui possedait les 4 mvt (et qui sont les solution pour cchaque pion de la team)
            public List<State> changeAllposition(List<List<int>> stockAllmoovementsPossible)
            {
                List<State> listofallstate = new List<State>();

                List<List<int>> TEAM;
                // List<List<int>> TEAM_Oppose;
                // si c'est la team A de jouer?
                if (turnplayer == true)
                {
                    TEAM = allpostionjoueurs;
                    // TEAM_Oppose = allpostionennemi;

                }
                else
                {
                    TEAM = allpostionennemi;
                    //  TEAM_Oppose = allpostionjoueurs;
                }

                foreach (var item in stockAllmoovementsPossible)
                {//GetNewState renvoie un etat et je stocke chaque etat dans cette liste
                    listofallstate.Add(GetNewState(item[0], item[1], item[2], item[3]));
                    /* foreach( var second in TEAM)
                    {
                        if( item[0] == second[0] && item[1] == second[1])
                        {
                            second[0] = item[2];
                            second[1] = item[3];
                            return new State(!turnplayer, caseoccupeInt, caseoccupe, ballpassingOneforplayer, ballpassingOneforennemi, allpostionennemi, allpostionjoueurs, Stockvalueballennemi, Stockvalueballballjoueur);

                            break;
                        }
                    }*/
                }
                //   return   new State(!turnplayer, caseoccupeInt, caseoccupe, ballpassingOneforplayer, ballpassingOneforennemi, allpostionennemi, allpostionjoueurs, Stockvalueballennemi, Stockvalueballballjoueur));

                return listofallstate;
            }




            private void Moovementsimpledespions(int valueI, int valueJ, bool[,] solution, List<List<int>> solutioncases)
            {


                //permet de montrer les solutions proposer au joueur sur lequel on a cliqué dessus
                //on peut avoir jusqu'à 4 mouvements possibles

                //les postion de base du joueur
                //List<List<int>> allsolutions;


                //je verifie si la case ouccupe est vraiment un pion (joueurs) parmis tous les autres joueurs 
                if ((caseoccupe[valueI, valueJ] == true))
                {


                    // on verifie, si le pion qui veut effectuer le mouvement n'a pas la balle, s'il a , dans ce cas il doit faire une passe 
                    if ((Stockvalueballennemi[0] == valueI && Stockvalueballennemi[1] == valueJ) || (Stockvalueballballjoueur[0] == valueI && Stockvalueballballjoueur[1] == valueJ))
                    {
                        moov_With_ball(valueI, valueJ, solution, solutioncases);
                    }
                    else
                    {

                        //pion  qui n'a pas la balle et qui peut bouger
                        moov_Wihtout_ball(valueI, valueJ, solution, solutioncases);
                    }


                }

            }


            public void moov_Wihtout_ball(int valueI, int valueJ, bool[,] solution, List<List<int>> solutioncasesall)
            {   //on aura donc à chaque fois une list parent  qui va stocker les 4 positions dans une list (antx ,anty , nextx,nexty)

                if ((valueI <= 6 && valueI >= 0) && valueJ == 0)
                {
                    if (valueI == 6)
                    {//on ne peut que monter et aller à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {


                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI - 1);
                            solutioncases.Add(valueJ);

                            //les changement vont se faire au niveau du tableau de bool avant qu'on ne trie la list des différents mouvements
                            //      caseoccupe[valueI - 1, valueJ] = true;
                            //     caseoccupe[valueI, valueJ] = false;

                            solutioncasesall.Add(solutioncases);


                        }

                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ + 1);

                            solutioncasesall.Add(solutioncases);

                            //      caseoccupe[valueI, valueJ + 1] = true;
                            //      caseoccupe[valueI, valueJ] = false;

                        }
                    }
                    else if (valueI == 0)
                    {//on ne peut que descendre et aller à droite
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI + 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI + 1, valueJ] = true;
                            //      caseoccupe[valueI, valueJ] = false;
                        }
                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);


                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ + 1);

                            solutioncasesall.Add(solutioncases);

                            //     caseoccupe[valueI, valueJ + 1] = true;
                            //      caseoccupe[valueI, valueJ] = false;

                        }
                    }
                    else if (valueI > 0 && valueI < 6)
                    {  //monter -descendre-aler à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);



                            solutioncases.Add(valueI - 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);


                            //      caseoccupe[valueI - 1, valueJ] = true;
                            //        caseoccupe[valueI, valueJ] = false;
                        }
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI + 1);
                            solutioncases.Add(valueJ);


                            solutioncasesall.Add(solutioncases);

                            //    caseoccupe[valueI + 1, valueJ] = true;
                            //     caseoccupe[valueI, valueJ] = false;


                        }
                        if (caseoccupe[valueI, valueJ + 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);


                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ + 1);

                            solutioncasesall.Add(solutioncases);


                            //      caseoccupe[valueI, valueJ + 1] = true;
                            //   caseoccupe[valueI, valueJ] = false;
                        }
                    }
                }



                else if ((valueI >= 0 && valueI <= 6) && valueJ == 6)
                {
                    if (valueI == 6)
                    {//on ne peut que monter et aller à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI - 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);

                            //      caseoccupe[valueI - 1, valueJ] = true;
                            //     caseoccupe[valueI, valueJ] = false;
                        }

                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ - 1);

                            solutioncasesall.Add(solutioncases);
                            //      caseoccupe[valueI, valueJ - 1] = true;
                            //     caseoccupe[valueI, valueJ] = false;
                        }
                    }
                    else if (valueI == 0)
                    {//on ne peut que descendre et aller à droite
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI + 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI + 1, valueJ] = true;
                            //      caseoccupe[valueI, valueJ] = false;

                        }
                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ - 1);

                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI, valueJ - 1] = true;
                            //        caseoccupe[valueI, valueJ] = false;
                        }
                    }
                    else if (valueI > 0 && valueI < 6)
                    {  //monter -descendre-aler à droite
                        if (caseoccupe[valueI - 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);


                            solutioncases.Add(valueI - 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI - 1, valueJ] = true;
                            //       caseoccupe[valueI, valueJ] = false;
                        }
                        if (caseoccupe[valueI + 1, valueJ] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI + 1);
                            solutioncases.Add(valueJ);

                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI + 1, valueJ] = true;
                            //        caseoccupe[valueI, valueJ] = false;

                        }
                        if (caseoccupe[valueI, valueJ - 1] == false)
                        {
                            List<int> solutioncases = new List<int>();
                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ - 1);
                            solutioncasesall.Add(solutioncases);

                            //       caseoccupe[valueI, valueJ - 1] = true;
                            //        caseoccupe[valueI, valueJ] = false;
                        }
                    }
                }



                else if ((valueJ < 6 && valueJ > 0) && valueI == 0)
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI + 1, valueJ] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI + 1);
                        solutioncases.Add(valueJ);

                        solutioncasesall.Add(solutioncases);


                        //      caseoccupe[valueI + 1, valueJ] = true;
                        //     caseoccupe[valueI, valueJ] = false;
                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ + 1);

                        solutioncasesall.Add(solutioncases);

                        //      caseoccupe[valueI, valueJ + 1] = true;
                        //     caseoccupe[valueI, valueJ] = false;
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ - 1);

                        solutioncasesall.Add(solutioncases);

                        //     caseoccupe[valueI, valueJ - 1] = true;
                        //   caseoccupe[valueI, valueJ] = false;
                    }

                }
                else if ((valueJ < 6 && valueJ > 0) && valueI == 6)
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI - 1, valueJ] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI - 1);
                        solutioncases.Add(valueJ);

                        solutioncasesall.Add(solutioncases);


                        //    caseoccupe[valueI - 1, valueJ] = true;
                        //    caseoccupe[valueI, valueJ] = false;
                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ + 1);

                        solutioncasesall.Add(solutioncases);


                        //    caseoccupe[valueI, valueJ + 1] = true;
                        //caseoccupe[valueI, valueJ] = false;
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ - 1);

                        solutioncasesall.Add(solutioncases);

                        //caseoccupe[valueI, valueJ - 1] = true;
                        //  caseoccupe[valueI, valueJ] = false;
                    }

                }
                else
                {
                    //on ne peut que monter et aller à droite
                    if (caseoccupe[valueI - 1, valueJ] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI - 1);
                        solutioncases.Add(valueJ);

                        solutioncasesall.Add(solutioncases);

                        //   caseoccupe[valueI - 1, valueJ] = true;
                        // caseoccupe[valueI, valueJ] = false;
                    }
                    if (caseoccupe[valueI + 1, valueJ] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI + 1);
                        solutioncases.Add(valueJ);

                        solutioncasesall.Add(solutioncases);

                        //   caseoccupe[valueI + 1, valueJ] = true;
                        //   caseoccupe[valueI, valueJ] = false;

                    }

                    if (caseoccupe[valueI, valueJ + 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ + 1);

                        solutioncasesall.Add(solutioncases);

                        //   caseoccupe[valueI, valueJ + 1] = true;
                        //    caseoccupe[valueI, valueJ] = false;
                    }
                    if (caseoccupe[valueI, valueJ - 1] == false)
                    {
                        List<int> solutioncases = new List<int>();
                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ);

                        solutioncases.Add(valueI);
                        solutioncases.Add(valueJ - 1);

                        solutioncasesall.Add(solutioncases);


                        //    caseoccupe[valueI, valueJ - 1] = true;
                        //    caseoccupe[valueI, valueJ] = false;
                    }

                }



            }

            public void moov_With_ball(int valueI, int valueJ, bool[,] solution, List<List<int>> solutioncasesall)
            { bool blocagepionennemi_VHS = false;
            bool blocagepionennemi_DiagonalS = false;
                List<List<int>> TEAM;
                List<List<int>> TEAM_Oppose;
                // si c'est la team A de jouer?
                if (turnplayer == true)
                {
                    TEAM = allpostionjoueurs;
                    TEAM_Oppose = allpostionennemi;
                }
                else 
                {
                    TEAM = allpostionennemi;
                    TEAM_Oppose = allpostionjoueurs;
                }

                //dans cette boucle on verifie, toutes les solutions: cad est ce que je peux faire une passee à tous mes coequipiers de la TEAM
                for (int i = 0; i < TEAM.Count; i++)
                {
                    if (caseoccupe[TEAM[i][0], TEAM[i][1]] == true)
                    {

                        //je verifie la position suivante du joueur  en ligne (ou colonne) est la meme que la position actuel du joueur en ligne (ou colonne) pour savoir quelle methode utilisé
                        if (TEAM[i][0] == valueI || TEAM[i][1] == valueJ)
                        {
                            blocagepionennemi_VHS = Checking_if_we_have_not_ennemi_in_vertical_Horizontal_road(TEAM[i][0], TEAM[i][1], valueI, valueJ, blocagepionennemi_VHS, TEAM_Oppose);

                            if (blocagepionennemi_VHS == false )
                         {

                            List<int> solutioncases = new List<int>();

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(TEAM[i][0]);
                            solutioncases.Add(TEAM[i][1]);

                            //caseoccupe[TEAM[i][0], TEAM[i][1]] = true;
                            //caseoccupe[valueI, valueJ] = false;



                            solutioncasesall.Add(solutioncases);
                             }
                        }
                        else
                        {

                            blocagepionennemi_DiagonalS = Checking_if_we_have_not_in_the_diagonal_road(TEAM[i][0], TEAM[i][1], valueI, valueJ, blocagepionennemi_DiagonalS, TEAM_Oppose);

                            if ( blocagepionennemi_DiagonalS == false)
                            {

                            List<int> solutioncases = new List<int>();

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(TEAM[i][0]);
                            solutioncases.Add(TEAM[i][1]);

                            //caseoccupe[TEAM[i][0], TEAM[i][1]] = true;
                            //caseoccupe[valueI, valueJ] = false;



                            solutioncasesall.Add(solutioncases);
                             }
                        }

                       /* if (blocagepionennemi_VHS == false || blocagepionennemi_DiagonalS == false)
                        {

                            List<int> solutioncases = new List<int>();

                            solutioncases.Add(valueI);
                            solutioncases.Add(valueJ);

                            solutioncases.Add(TEAM[i][0]);
                            solutioncases.Add(TEAM[i][1]);

                            //caseoccupe[TEAM[i][0], TEAM[i][1]] = true;
                            //caseoccupe[valueI, valueJ] = false;



                            solutioncasesall.Add(solutioncases);
                        }*/
                    }
                blocagepionennemi_VHS = false;
                blocagepionennemi_DiagonalS = false;

                }
            }
            public bool Checking_if_we_have_not_in_the_diagonal_road(int nextx, int nexty, int antx, int anty, bool blocagepionennemi, List<List<int>> TEAM_Oppose)
            {
               // blocagepionennemi = false;
                if (TEAM_Oppose == allpostionjoueurs)
                {
                    if (nexty == anty && nextx < antx)
                    {
                        int i = nextx + 1;
                        int j = nexty;
                        while (i < antx)
                        {
                            // if (caseoccupe[TEAM_Oppose[g][i], TEAM_Oppose[g][j]] != true)
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }


                        }

                    }
                    else if (nexty == anty && nextx > antx)
                    {
                        int j = nexty; int i = antx + 1;
                        while (i < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }

                        }

                    }
                    else if (nexty > anty && nextx == antx)
                    {
                        int j = anty + 1; int i = nextx;
                        while (j < nexty)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }

                    }
                    else if (nexty < anty && nextx == antx)
                    {
                        int j = antx + 1; int i = nextx;
                        while (j < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }
                    }
                }
                if (TEAM_Oppose == allpostionennemi)
                {

                    if (nexty == anty && nextx < antx)
                    {
                        int i = nextx + 1;
                        int j = nexty;
                        while (i < antx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }


                        }

                    }
                    else if (nexty == anty && nextx > antx)
                    {
                        int j = nexty; int i = antx + 1;
                        while (i < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }

                        }

                    }
                    else if (nexty > anty && nextx == antx)
                    {
                        int j = anty + 1; int i = nextx;
                        while (j < nexty)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }

                    }
                    else if (nexty < anty && nextx == antx)
                    {
                        int j = antx + 1; int i = nextx;
                        while (j < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }
                    }
                }
                return blocagepionennemi;
            }


            public bool Checking_if_we_have_not_ennemi_in_vertical_Horizontal_road(int nextx, int nexty, int antx, int anty, bool blocagepionennemi, List<List<int>> TEAM_Oppose)
            {
               
                if (TEAM_Oppose == allpostionjoueurs || TEAM_Oppose == allpostionennemi)
                {
                    if (nexty == anty && nextx < antx)
                    {
                        int i = nextx + 1;
                        int j = nexty;
                        while (i < antx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }


                        }

                    }
                    else if (nexty == anty && nextx > antx)
                    {
                        int j = nexty; int i = antx + 1;
                        while (i < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    i += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                i += 1;
                            }

                        }

                    }
                    else if (nexty > anty && nextx == antx)
                    {
                        int j = anty + 1; int i = nextx;
                        while (j < nexty)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }

                    }
                    else if (nexty < anty && nextx == antx)
                    {
                        int j = antx + 1; int i = nextx;
                        while (j < nextx)
                        {
                            foreach (var item in TEAM_Oppose)
                            {
                                if ((item[0] == i && item[1] == j) && caseoccupe[i, j] == true)
                                {
                                    blocagepionennemi = true;
                                    j += 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (blocagepionennemi == true)
                            {
                                break;
                            }
                            else if (blocagepionennemi == false)
                            {
                                j += 1;
                            }

                        }
                    }
                    //  return blocagepionennemi;
                }

                return blocagepionennemi;

            }
        }
    }




