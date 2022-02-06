using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiaBalik
{

    class Min_Max
    {
      public   Dictionary<State, int> all_get_score = new Dictionary<State, int>();
        
      // public List<int> stock_four_mvt_ball_s = new List<int>();
        public int scoretest=0;



        public int alpha_beta(State parent_state, int deep, int globaldeep,int alpha, int beta )
        {
          



          
                List<int> all_score = new List<int>();

                scoretest = 0;

                //list qui stocke les differents états
                List<State> allofstate_ = new List<State>();

                //state.getscore();
                //Get_movement() renvoie tout les mouvements possibles
                List<List<int>> stockage = parent_state.Get_movements();
                // state.changeAllposition(stockage);
                foreach (var item in stockage)
                {   //pour chaque nouveau etat cree on se referre de l'etat de base A qui a deja turnplayer = false
                    //c'est pourcela que tout les NEW etat enfant on leurs turnplayer = true
                    State statetest = parent_state.GetNewState(item[0], item[1], item[2], item[3]);
                    //Newchild_allofState_deep_five.Add(statetest)
                    if (deep == 0)
                    {
                        //c'est lorsque deep = 0 qu'on doit recuperer le score
                        all_score.Add(statetest.getscore());

                    }

                    //on a ajoute chaque état qu'on rencontre
                    allofstate_.Add(statetest);

                }

                all_score.Sort();


         






                //int score = 0;
                if (deep == 0)
                {
                    if (parent_state.turnplayer == true)
                    {
                        scoretest = all_score[all_score.Count - 1];
                        //methode max : on prend la valeur la plus grande
                    }
                    else
                    {
                        scoretest = all_score[0];
                        //method min: on prend la valeur la plus petite 

                    }

                }
                else
                {
                    //attention : dans ce foreach on va aller plus en profondeur à chaque état jusqu'à 0 
                    //par exemple le dernier etat (a) qui est à deep =1 a  3 enfant 
                    //lorsqu'on (a) ->va vers l'enfant (1) qui est deep = 0 , il return une valeur qu'on veut stocker au niveau de (a) qui est à deep =1
                    //ensuite on fait la même chose avec  (a) -> vers l'enfant (2) qui est deep =0 ,il return une valeur qu'on veut stocker au niveau de (a) qui est à deep =1
                    //ensuite on fait la même chose avec  (a) -> vers l'enfant (3).

                    //ducoup on les trois valeurs stockés dans la list qui s'initialise à 0 a chaque deep. même si on l'a réutilisé avant lorsque deep =0
                    // on peut l'utiliser lorsque deep =1 on verra que son count =0

                    //tous les enfants du ême noeud
                    foreach (var states in allofstate_)
                    {


                        int score = alpha_beta(states, deep - 1, globaldeep,alpha,beta);
                    //la recursive_method est appelée tant que le deep n'est pas egal = 0 .Ducoup lorsqu'elle est appelé elle ne retourne rien tant que deep !=0
                    // et aussi elle s'arrete là cad que le compilateur ne lie pas all_socre.add() pour l'instant tant que deep !=0
                    //lorsque deep = 0 , un calcull est fait du lorsque la methode retourne une valeur , cette valeur est stocké dans le int


                    //list trié

                    all_score.Add(score);
                    all_score.Sort();


                    if (parent_state.turnplayer == true)
                    {

                        //methode max
                        //on choisi la valeur la plus grande
                        scoretest = all_score[all_score.Count - 1];
                    }
                    else
                    {
                        //on choisi la valeur la plus petite
                        //method min
                        scoretest = all_score[0];

                    }


                    


                    int value = scoretest;
                    //alpha beta : 
                    //pour savoir si c'est un max  (player)
                    //turnplayer == true , cvd que c'est l'ordi  qui joue
                    if(parent_state.turnplayer == true)
                    {
                        //par exemple beta =3, si la valeur qu'on retourne par exemple au niveau de E (5)
                        //on arrete le bouclage et donc alpha = max entre alpha et valeur qu'on retourne (5>3)
                        if( value >= beta)
                        {
                            break;
                        }
                        alpha = Math.Max(alpha, value);
                    }
                    else
                    {
                        if(value <= alpha)
                        {
                            break;
                        }
                        beta = Math.Min(beta, value);
                    }




                    



                        //cela ne va pas être lu tant que le calcul n'a pas commencer à être effectuer
                        //donc lorsque deep =0 que cette condition sera lu ,puis on remonte deep=2

                        //attention , là j'ajoute dans le dico , tous les états lorsque deep =2  et leurs score (lorsque deep =2 , on a tous les nouveau états enfants)
                        //on sait que deep = globaldeep , c'est la dernière couche avant de renvoyer la seule valeur qui serait le max
                        if (deep == globaldeep)
                        {
                            all_get_score.Add(states, score);

                        }

                    }

                   








                   

                }



                return scoretest;

                //return une valeur vers score ;
            





        }



        public int recusive_method(State parent_state, int deep, int globaldeep)
        {
            List<int> all_score = new List<int>();

            scoretest = 0;

            //list qui stocke les differents états
            List<State> allofstate_ = new List<State>();

            //state.getscore();
            List<List<int>> stockage = parent_state.Get_movements();
            // state.changeAllposition(stockage);
            foreach (var item in stockage)
            {   //pour chaque nouveau etat cree on se referre de l'etat de base A qui a deja turnplayer = false
                //c'est pourcela que tout les NEW etat enfant on leurs turnplayer = true
                State statetest = parent_state.GetNewState(item[0], item[1], item[2], item[3]);
                //Newchild_allofState_deep_five.Add(statetest)
                if (deep == 0)
                {
                    //c'est lorsque deep = 0 qu'on doit recuperer le score
                    all_score.Add(statetest.getscore());

                }

                //on a ajoute chaque état qu'on rencontre
                allofstate_.Add(statetest);

            }

            all_score.Sort();



            //int score = 0;
            if (deep == 0)
            {
                if (parent_state.turnplayer == true)
                {
                    scoretest = all_score[all_score.Count - 1];
                    //methode max : on prend la valeur la plus grande
                }
                else
                {
                    scoretest = all_score[0];
                    //method min: on prend la valeur la plus petite 

                   

                }
                
            }
            else
            {
                //attention : dans ce foreach on va aller plus en profondeur à chaque état jusqu'à 0 
                //par exemple le dernier etat (a) qui est à deep =1 a  3 enfant 
                //lorsqu'on (a) ->va vers l'enfant (1) qui est deep = 0 , il return une valeur qu'on veut stocker au niveau de (a) qui est à deep =1
                //ensuite on fait la même chose avec  (a) -> vers l'enfant (2) qui est deep =0 ,il return une valeur qu'on veut stocker au niveau de (a) qui est à deep =1
                //ensuite on fait la même chose avec  (a) -> vers l'enfant (3).

                //ducoup on les trois valeurs stockés dans la list qui s'initialise à 0 a chaque deep. même si on l'a réutilisé avant lorsque deep =0
                // on peut l'utiliser lorsque deep =1 on verra que son count =0

                //tous les enfants du ême noeud
                foreach (var states in allofstate_)
                {


                    int score = recusive_method(states, deep - 1,globaldeep);
                    //la recursive_method est appelée tant que le deep n'est pas egal = 0 .Ducoup lorsqu'elle est appelé elle ne retourne rien tant que deep !=0
                    // et aussi elle s'arrete là cad que le compilateur ne lie pas all_socre.add() pour l'instant tant que deep !=0
                    //lorsque deep = 0 , un calcull est fait du lorsque la methode retourne une valeur , cette valeur est stocké dans le int


                    all_score.Add(score);



                    //cela ne va pas être lu tant que le calcul n'a pas commencer à être effectuer
                    //donc lorsque deep =0 que cette condition sera lu ,puis on remonte deep=2

                    //attention , là j'ajoute dans le dico , tous les états lorsque deep =2  et leurs score (lorsque deep =2 , on a tous les nouveau états enfants)
                    //on sait que deep = globaldeep , c'est la dernière couche avant de renvoyer la seule valeur qui serait le max
                    if (deep == globaldeep)
                    {
                        all_get_score.Add(states, score);

                    }

                }

                all_score.Sort();



   




                if (parent_state.turnplayer == true)
                {

                    //methode max

                    scoretest = all_score[all_score.Count - 1];
                }
                else
                {

                    //method min
                    scoretest = all_score[0];

                }

            }
            


            return scoretest;

            //return une valeur vers score ;
        }

      





        public List<int> get_mvt_we_need(State parent_state, int deep, int scoretest,int compteurde2deplacementparjoueur, bool makepasse_IA, int Stockvalueballballjoueur_I,int Stockvalueballballjoueur_J, PictureBox[,] PictureBoxes)
        {// on veut recuperer les quatres mouvements

            List<List<int>> stockage_stock_ball_next_mvt = new List<List<int>>();
            List<List<int>> stockage_same_score = new List<List<int>>();
            List<int> stockage_Mvt = new List<int>();
            List<List<int>> stock_four_mvt_ball = new List<List<int>>();
            List<List<int>> stock_four_mvt_ball_s = new List<List<int>>();
        
            bool blocagepionennemi_VHS = false;
            

            //on recupere tous les mouvements d'état.
            List<List<int>> stockage = parent_state.Get_movements();


            // attent la liste de stockage dans cette methode est la même que la liste de stockage dans la methode recursive() 
            //cela signifie que les states  au niveau de deep =1 (leur mouvement sont les meme ques les 4 mouvement dans la liste de stockage)
            //leur i sont à la même place dans une liste et dans le dico

            //dans cette premiere boucle , on ajoute dans la liste stockage_same_score
            for (int i = 0; i < all_get_score.Count; i++)
            {
               
                var item = all_get_score.ElementAt(i);
                if (scoretest == item.Value)
                {//On verifie si la valeur socrest est egale au score  d'un élement(etat) qui se trouve dans all_get_Score  
                    var item2 = stockage[i];
                    //comme on sait que dans une boucle , le i de stockage ici == au même i que dans item=all_get_score.ElementAt(i), 
                    stockage_same_score.Add(item2);


                 

                }


              
            }

            //je stocke tous les état de la balle lorsu'elle effectue un mouvement
            for (int j = 0; j < all_get_score.Count; j++)
            {
                var item = stockage.ElementAt(j);
                if (item[0] == Stockvalueballballjoueur_I && item[1] == Stockvalueballballjoueur_J)
                {
                    stock_four_mvt_ball_s.Add(stockage[j]);

                }


            }
            all_get_score.Clear();

            //cette methode permet d'ajouter dans une liste toutes les positions antécedentes qui correspondent a la position de la balle Stockvalueballballjoueur_I et Stockvalueballballjoueur_J
            // stock_four_mvt_ball_s = stockposition_Ball(stock_four_mvt_ball, stockage_same_score, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);

            Random random = new Random();
           // renvoie une valeur (qui correspond au numero de la liste) où j'ai toute les bonnes réponses.
            int number = random.Next(0, stockage_same_score.Count);


           
        


            //compteur de deplacement des joueus:
            if (compteurde2deplacementparjoueur < 2)
            {//avec le count, je verifie si ces deux valeurs ne corresponde pas aux coordonnée de la balle
                int count = 0;
                if (stockage_same_score[number][0] == Stockvalueballballjoueur_I)
                {
                    count++;

                }
                if (stockage_same_score[number][1] == Stockvalueballballjoueur_J)
                {
                    count++;
                }
                //si le count <2  , cvd (je mets toutes les bonnes reponses dans la list stockage_Mvt) je fais correspondre les les  4 positions dans la liste
                if (count < 2)
                {
               

                    stockage_Mvt = stockage_same_score[number];
                    count = 0;
                }
                //si les deux valeurs corresponds, et que ces valeur sont dans la liste stockage_same_score  , je les prends directement et les mettent dans la liste stockage_Mvt
                //makepasse_IA == false , cvd on n'a pas fait encore une passe 
                else if (count == 2 && makepasse_IA == false)
                {
                    //check pour savoir lorsqu'on effectue une passe est qu'il n'y a pas un joueur de l'equipe adverse qui se trouve au milieu  entre deux joueur d'une même équipe

                    number = Checking_Pass_Road(stockage_same_score[number][2], stockage_same_score[number][3], stockage_same_score[number][0], stockage_same_score[number][1], PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, number, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);





                    stockage_Mvt = stockage_same_score[number];
                    
                    //makepasse_IA = true;
                    count = 0;
                }
                //makepasse_IA == true , cvd on a fait  une passe, 
                else if (count == 2 && makepasse_IA == true)
                {//si le booleen makepasse est à true , on veut dorénavement deplacer simplement des joueurs et pas faire une passe
                    Random r = new Random();
                    // renvoie une valeur  où j'ai toute les bonnes réponses.
                    int n = random.Next(0, stockage_same_score.Count);

                    while (n == number )
                    {   //je boucle tant que j'ai pas obtenu une valeur de n differente de number 
                        n = random.Next(0, stockage_same_score.Count);
                    }


                    stockage_Mvt = stockage_same_score[n];

                }
                // stockage_Mvt = stockage_same_score[number];
            }

            //condition pour faire une passe
            else if (stockage_same_score[number][0] == Stockvalueballballjoueur_I && stockage_same_score[number][1] == Stockvalueballballjoueur_J && makepasse_IA == false)
            {

                number = Checking_Pass_Road(stockage_same_score[number][2], stockage_same_score[number][3], stockage_same_score[number][0], stockage_same_score[number][1], PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, number, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);

                stockage_Mvt = stockage_same_score[number];
               
              //  makepasse_IA = true;

            }
            else if (stockage_same_score[number][0] == Stockvalueballballjoueur_I && stockage_same_score[number][1] == Stockvalueballballjoueur_J && makepasse_IA == true)
            {

                Random r = new Random();
                // renvoie une valeur  où j'ai toute les bonnes réponses.
                int n = random.Next(0, stockage_same_score.Count);

                while (n == number)
                {   //je boucle tant que j'ai pas obtenu une valeur de n differente de number 
                    n = random.Next(0, stockage_same_score.Count);
                }

                number = Checking_Pass_Road(stockage_same_score[number][2], stockage_same_score[number][3], stockage_same_score[number][0], stockage_same_score[number][1], PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, number, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);


                stockage_Mvt = stockage_same_score[n];


            }
            //condition pour faire une passe après deux mvt du joueur 
            else if (compteurde2deplacementparjoueur >= 2 && makepasse_IA == false)
            {   //le count permet de verifier si on a une liste qui possede les  positions de la balle
                int count2 = 0;
                if (stockage_same_score[number][0] == Stockvalueballballjoueur_I)
                {
                    count2++;

                }
                if (stockage_same_score[number][1] == Stockvalueballballjoueur_J)
                {
                    count2++;
                }
                if (count2 == 2)
                {
                    number = Checking_Pass_Road(stockage_same_score[number][2], stockage_same_score[number][3], stockage_same_score[number][0], stockage_same_score[number][1], PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, number, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);

                    stockage_Mvt = stockage_same_score[number];
                    count2 = 0;
                }
             else
                {
                    //je verifie dabord si les positions actuel (et la position next) de la balle ne se trouve pas dans la liste stockage_same_score 
                    int count = 0;
                    for (int i = 0; i < stockage_same_score.Count; i++)
                    {

                        var item = stockage_same_score.ElementAt(i);
                        if (item[0] == Stockvalueballballjoueur_I && item[1] == Stockvalueballballjoueur_J)
                        {
                            stockage_Mvt = stockage_same_score[i];
                            count++;
                            break;
                        }

                    }
                    //count =0 , cvd qu'il n'ya pas cette position de la balle dans cette liste
                    if (count == 0)
                    {
                        
                      
                        Random r = new Random();
                        int x = r.Next(0, stock_four_mvt_ball_s.Count);

                        number = Checking_Pass_Road(stock_four_mvt_ball_s[x][2], stock_four_mvt_ball_s[x][3], Stockvalueballballjoueur_I, Stockvalueballballjoueur_J, PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, x, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);
                       

                        //int n =number;

                        // number = Checking_Pass_Road(stockage_same_score[n][2], stockage_same_score[n][3], Stockvalueballballjoueur_I, Stockvalueballballjoueur_J, PictureBoxes, stockage_same_score, stock_four_mvt_ball_s, blocagepionennemi_VHS, n, Stockvalueballballjoueur_I, Stockvalueballballjoueur_J);
                        List<int> stocks_ = new List<int>();
                        stocks_.Add(Stockvalueballballjoueur_I);
                        stocks_.Add(Stockvalueballballjoueur_J);
                        stocks_.Add(stock_four_mvt_ball_s[number][2]);
                        stocks_.Add(stock_four_mvt_ball_s[number][3]);
                        stockage_Mvt = stocks_;
                       
                       // makepasse_IA = true;
                        



                    }

                }
            }
           
               
            return stockage_Mvt;

        }



        public int Checking_Pass_Road(int nextx, int nexty, int antx, int anty, PictureBox[,] PictureBoxes, List<List<int>> stockage_same_score, List<List<int>> stock_four_mvt_ball_s, bool blocagepionennemi_VHS, int number,int Stockvalueballballjoueur_I, int Stockvalueballballjoueur_J)
        {//cette methode permet de verifier s'il n'y a pas un joueur entre les ddeux joueurs d'une même équipe.


            // on verfie si la postion suivante de la balle est égale à la position antécédante
            if (antx == nextx || anty == nexty)
            {//pass vertical ou horizontal
                blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_vertical_Horizontal_road(nextx, nexty, antx, anty, PictureBoxes);

            }
            else
            {//pass oblique

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

                    blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_the_diagonal_road(nextx, nexty, antx, anty, PictureBoxes);
                }
                else
                {//sinon on recommence en choisissant stock_four_mvt_ball_s[k][2] et stock_four_mvt_ball_s[k][3] (qui sont nextx et nexty )de la  liste stock_four_mvt_ball_s qui contitent tous les mouvements possibles de la balle , 
                    bool check = true;
                    while (check == true)
                    {   //je boucle tant que j'ai pas obtenir une bonne solution pour le mvt de la balle
                        Random rndc = new Random();
                        int k = rndc.Next(0, stock_four_mvt_ball_s.Count);

                        //je fais une condition qui permet de verifier si c'est une passe qu'on effectue  
                        if (antx == stock_four_mvt_ball_s[k][2] || anty == stock_four_mvt_ball_s[k][3] || ((stock_four_mvt_ball_s[k][2] - antx) == (stock_four_mvt_ball_s[k][3] - anty)))
                        {
                            //meme valeur k =k
                            k = k + 0;
                        }
                        else
                        {

                            k = rndc.Next(0, stock_four_mvt_ball_s.Count);
                        }


                        if (antx == stock_four_mvt_ball_s[k][2] || anty == stock_four_mvt_ball_s[k][3])
                            {//pass vertical ou horizontal
                                blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_vertical_Horizontal_road(stock_four_mvt_ball_s[k][2], stock_four_mvt_ball_s[k][3], antx, anty, PictureBoxes);
                                number = k;
                            break;
                            }
                            else
                            {//pass oblique

                                int diffx_ = stock_four_mvt_ball_s[k][2] - antx;
                                int diffy_ = stock_four_mvt_ball_s[k][3] - anty;

                                if (diffx_ < 0)
                                {
                                    diffx_ = Math.Abs(diffx_);
                                }
                                if (diffy_ < 0)
                                {
                                    diffy_ = Math.Abs(diffy_);
                                }

                                if (diffx_ == diffy_)
                                {

                                    blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_the_diagonal_road(stock_four_mvt_ball_s[k][2], stock_four_mvt_ball_s[k][3], antx, anty, PictureBoxes);
                                    number = k;
                                    break;

                                }
                            }



                        
                       
                    }
                       
                    }
                








                if (blocagepionennemi_VHS == true)
                {


                    while (blocagepionennemi_VHS == true)
                    {   //je boucle tant que j'ai pas obtenir une bonne solution pour le mvt de la balle
                        Random rndc = new Random();
                        int k = rndc.Next(0, stock_four_mvt_ball_s.Count);

                        if (antx == stock_four_mvt_ball_s[k][2] || anty == stock_four_mvt_ball_s[k][3] || ((stock_four_mvt_ball_s[k][2] - antx) == (stock_four_mvt_ball_s[k][3] - anty)))
                        {
                            //meme valeur k =k
                            k = k + 0;
                        }
                        else
                        {
                            k = rndc.Next(0, stock_four_mvt_ball_s.Count);
                        }



                        if (antx == stock_four_mvt_ball_s[k][2] || anty == stock_four_mvt_ball_s[k][3])
                            {//pass vertical ou horizontal
                                blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_vertical_Horizontal_road(stock_four_mvt_ball_s[k][2], stock_four_mvt_ball_s[k][3], antx, anty, PictureBoxes);
                                number = k;
                             break;
                            }
                            else
                            {//pass oblique

                                int diffx_ = stock_four_mvt_ball_s[k][2] - antx;
                                int diffy_ = stock_four_mvt_ball_s[k][3] - anty;

                                if (diffx_ < 0)
                                {
                                    diffx_ = Math.Abs(diffx_);
                                }
                                if (diffy_ < 0)
                                {
                                    diffy_ = Math.Abs(diffy_);
                                }

                                if (diffx_ == diffy_)
                                {

                                    blocagepionennemi_VHS = Checking_if_we_have_not_IA_player_PASS_in_the_diagonal_road(stock_four_mvt_ball_s[k][2], stock_four_mvt_ball_s[k][3], antx, anty, PictureBoxes);
                                    number = k;
                                break;
                                }
                            }


                        
                       
                    }
                }

            }

            return number;
        }

        public bool Checking_if_we_have_not_IA_player_PASS_in_the_diagonal_road(int nextx, int nexty, int antx, int anty,PictureBox [,] PictureBoxes)
        {
           bool blocagepionennemi = false;
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
        public bool Checking_if_we_have_not_IA_player_PASS_in_vertical_Horizontal_road(int nextx, int nexty, int antx, int anty, PictureBox[,] PictureBoxes)
        {
            bool blocagepionennemi = false;
            if (nexty == anty && nextx < antx)
            {
                int i = nextx + 1;
                int j = nexty;
                while (i < antx)
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
                while (i < nextx)
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
                while (j < nexty)
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

                        j += 1;
                        break;
                    }
                }
            }
            return blocagepionennemi;
        }






    }
}
